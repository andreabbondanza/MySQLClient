using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DewCore.MySQLClient
{
    public class MySQLConnectionString
    {
        /// <summary>
        /// server address
        /// </summary>
        public readonly string Host;
        /// <summary>
        /// server port
        /// </summary>
        public readonly string Port;
        /// <summary>
        /// database name
        /// </summary>
        public readonly string Database;
        /// <summary>
        /// username
        /// </summary>
        public readonly string User;
        /// <summary>
        /// password
        /// </summary>
        public readonly string Password;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="host">server address</param>
        /// <param name="port">server port</param>
        /// <param name="database">database name</param>
        /// <param name="user">username</param>
        /// <param name="password">password</param>
        public MySQLConnectionString(string host, string port, string database, string user, string password)
        {
            this.Host = host;
            this.Port = port;
            this.Database = database;
            this.User = user;
            this.Password = password;
        }
        /// <summary>
        /// Return the current parameters connection string
        /// </summary>
        /// <returns></returns>
        public string GetConnectionString()
        {
            return @"host=" + this.Host + ";port=" + this.Port + ";user id=" + this.User + ";password=" + this.Password + ";database=" + this.Database + ";";
        }
    }
    /// <summary>
    /// Indicate if class must open a new connection for every query
    /// </summary>
    public enum OneConnection
    {
        Yes,
        No
    }
    /// <summary>
    /// Exception for null reference in transaction
    /// </summary>
    public class NoTransactionException : Exception
    {
        public override string ToString()
        {
            return "No transaction initalized. Missing begin transaction";
        }
    }
    public interface IMySQLClient
    {
        /// <summary>
        /// Perform a query and return result in a list of array
        /// </summary>
        /// <typeparam name="T">Result type object</typeparam>
        /// <param name="query">Query</param>
        /// <param name="values">List of binded values</param>
        /// <returns>List of array objects (rows)</returns>
        Task<List<object[]>> QueryArrayAsync(string query, List<MySqlParameter> values);
        /// <summary>
        /// Perform a query
        /// </summary>
        /// <typeparam name="T">Result type object</typeparam>
        /// <param name="query">Query</param>
        /// <param name="values">List of binded values</param>
        /// <returns>List of T objects (rows)</returns>
        Task<List<T>> QueryAsync<T>(string query, List<MySqlParameter> values) where T : class, new();
        /// <summary>
        /// Commit a transaction
        /// </summary>
        Task CommitAsync();
        /// <summary>
        /// Rollback a transaction
        /// </summary>
        Task RoolbackAsync();
        /// <summary>
        /// Begin a transiction
        /// </summary>
        /// <returns></returns>
        Task<bool> BeginTransactionAsync(IsolationLevel isolationLevel);
        /// <summary>
        /// Close database connection
        /// </summary>
        void CloseConnection();

    }
    public class MySQLClient : IMySQLClient, IDisposable
    {
        /// <summary>
        /// Database connection
        /// </summary>
        public readonly MySqlConnection Db = null;
        /// <summary>
        /// Transaction var
        /// </summary>
        private MySqlTransaction transiction = null;
        /// <summary>
        /// Open connection type
        /// </summary>
        private OneConnection oneConnection = OneConnection.No;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString"></param>
        public MySQLClient(string connectionString, OneConnection oneConnection = OneConnection.No)
        {
            this.Db = new MySqlConnection(connectionString);
            this.oneConnection = oneConnection;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString"></param>
        public MySQLClient(MySQLConnectionString connectionString, OneConnection oneConnection = OneConnection.No)
        {
            this.Db = new MySqlConnection(connectionString.GetConnectionString());
            this.oneConnection = oneConnection;
        }
        /// <summary>
        /// Open a connectino
        /// </summary>
        /// <returns></returns>
        private async Task OpenConnection()
        {
            if (this.Db.State == ConnectionState.Closed || this.Db.State == ConnectionState.Broken)
                await this.Db.OpenAsync();
        }
        /// <summary>
        /// Close Connection
        /// </summary>
        public void CloseConnection()
        {
            if (this.Db.State != ConnectionState.Closed)
                this.Db.Close();
        }
        /// <summary>
        /// Set the connection state based from onConnection attribute
        /// </summary>
        /// <returns></returns>
        private async Task SetConnectionState()
        {
            if (this.oneConnection == OneConnection.No)
            {
                await this.Db.OpenAsync();
            }
            else
            {
                await this.OpenConnection();
            }
        }
        /// <summary>
        /// Begin a transaction
        /// </summary>
        /// <param name="isolationLevel"></param>
        /// <returns></returns>
        public async Task<bool> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        {
            bool result = true;
            try
            {
                if (this.transiction == null)
                    this.transiction = await Db.BeginTransactionAsync(isolationLevel);
            }
            catch
            {
                result = false;
            }
            return result;
        }
        /// <summary>
        /// Execute commit
        /// </summary>
        /// <exception cref="NoTransactionException">No transaction initialized</exception>
        /// <returns></returns>
        public async Task CommitAsync()
        {
            if (transiction == null)
                throw new NoTransactionException();
            else
            {
                await this.transiction.CommitAsync();
            }
        }
        /// <summary>
        /// Dispose client
        /// </summary>
        public void Dispose()
        {
            if (Db != null && Db.State != ConnectionState.Closed)
                Db.Close();
        }
        /// <summary>
        /// Perform a query
        /// </summary>
        /// <typeparam name="T">Result type object</typeparam>
        /// <param name="query">Query</param>
        /// <param name="values">List of binded values</param
        /// <exception cref="MySqlException">MySqlException</exception>
        /// <returns>List of T objects (rows)</returns>
        public async Task<List<T>> QueryAsync<T>(string query, List<MySqlParameter> values = null) where T : class, new()
        {
            await this.SetConnectionState();
            var cmd = Db.CreateCommand() as MySqlCommand;
            cmd.CommandText = query;
            if (values != null)
            {
                foreach (var item in values)
                {
                    cmd.Parameters.Add(item);
                }
            }
            var reader = await cmd.ExecuteReaderAsync();
            var result = await this.SetFields<T>(reader);
            return result;
        }
        /// <summary>
        /// Set the fields of a Type T from a reader of a T table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        private async Task<List<T>> SetFields<T>(System.Data.Common.DbDataReader reader) where T : class, new()
        {
            var columns = reader.FieldCount;
            List<T> result = new List<T>();
            while (await reader.ReadAsync())
            {
                T item = new T();
                var reflection = item.GetType();
                var properties = reflection.GetRuntimeProperties();
                for (int i = 0; i < columns; i++)
                {
                    var colName = reader.GetName(i);
                    var property = properties.First((x) => { return x.Name == colName; });
                    var value = reader.GetValue(i);
                    property.SetValue(item, value);
                }
                result.Add(item);
            }
            return result;
        }
        /// <summary>
        /// Perform a query
        /// </summary>
        /// <param name="query">Query</param>
        /// <param name="values">List of binded values</param
        /// <exception cref="MySqlException">MySqlException</exception>
        /// <returns>List of array objects (rows)</returns>
        public async Task<List<object[]>> QueryArrayAsync(string query, List<MySqlParameter> values = null)
        {
            await this.SetConnectionState();
            var cmd = Db.CreateCommand() as MySqlCommand;
            cmd.CommandText = query;
            if (values != null)
            {
                foreach (var item in values)
                {
                    cmd.Parameters.Add(item);
                }
            }
            var reader = await cmd.ExecuteReaderAsync();
            List<object[]> result = new List<object[]>();
            while (await reader.ReadAsync())
            {
                object[] item = new object[reader.FieldCount];
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    item[i] = reader.GetValue(i);
                }
                result.Add(item);
                
            }
            return result;
        }
        /// <summary>
        /// Execute roolback
        /// </summary>
        /// <exception cref="NoTransactionException">No transaction initialized</exception>
        /// <returns></returns>
        public async Task RoolbackAsync()
        {
            if (transiction == null)
                throw new NoTransactionException();
            else
            {
                await this.transiction.RollbackAsync();
            }
        }
    }
}
