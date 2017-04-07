using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using DewCore.DewLogger;
using DewInterfaces.DewLogger;
using DewInterfaces.DewDatabase.MySQL;
using System.Data.Common;

namespace DewCore.DewDatabase.MySQL
{    
    /// <summary>
    /// Client
    /// </summary>
    public class MySQLClient : IMySQLClient, IDisposable
    {
        /// <summary>
        /// Enable debug
        /// </summary>
        public static bool DebugOn = false;
        /// <summary>
        /// debugger
        /// </summary>
        private static IDewLogger debugger = new DewDebug();
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
        /// Set logger
        /// </summary>
        /// <param name="debugger"></param>
        public static void SetDebugger(IDewLogger debugger)
        {
            MySQLClient.debugger = debugger;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="oneConnection"></param>
        public MySQLClient(string connectionString, OneConnection oneConnection = OneConnection.No)
        {
            this.Db = new MySqlConnection(connectionString);
            this.oneConnection = oneConnection;
            if (DebugOn)
            {
                debugger.WriteLine("Connection to database:" + Db.Database);
                debugger.WriteLine("With connection string:" + connectionString);
                debugger.WriteLine("And One connection set to:" + this.oneConnection);
            }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="oneConnection"></param>
        public MySQLClient(MySQLConnectionString connectionString, OneConnection oneConnection = OneConnection.No)
        {
            this.Db = new MySqlConnection(connectionString.GetConnectionString());
            this.oneConnection = oneConnection;
            if (DebugOn)
            {
                debugger.WriteLine("Connection to database:" + Db.Database);
                debugger.WriteLine("With user:" + connectionString.User);
                debugger.WriteLine("With password:" + connectionString.Password);
                debugger.WriteLine("With host:" + connectionString.Host);
                debugger.WriteLine("With port:" + connectionString.Port);
                debugger.WriteLine("And One connection set to:" + this.oneConnection);
            }
        }
        /// <summary>
        /// Open a connectino
        /// </summary>
        /// <returns></returns>
        private async Task OpenConnection()
        {
            try
            {
                if (this.Db.State == ConnectionState.Closed || this.Db.State == ConnectionState.Broken)
                    await this.Db.OpenAsync();
                if (DebugOn)
                {
                    debugger.WriteLine("Connection opened with:" + Db.Database);
                }
            }
            catch (Exception exc)
            {
                if (DebugOn)
                    debugger.WriteLine("Exception with open connection:" + exc.Message);
            }
        }
        /// <summary>
        /// Close Connection
        /// </summary>
        public void CloseConnection()
        {
            try
            {
                if (this.Db.State != ConnectionState.Closed)
                    this.Db.Close();
                if (DebugOn)
                {
                    debugger.WriteLine("Connection closed with:" + Db.Database);
                }
            }
            catch (Exception exc)
            {

                if (DebugOn)
                    debugger.WriteLine("Exception with close connection:" + exc.Message);
            }
        }
        /// <summary>
        /// Set the connection state based from onConnection attribute
        /// </summary>
        /// <returns></returns>
        private async Task SetConnectionState()
        {
            if (this.oneConnection == OneConnection.No && this.Db.State != ConnectionState.Open)
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
                await this.OpenConnection();
                if (this.transiction == null)
                    this.transiction = await Db.BeginTransactionAsync(isolationLevel);
                if (DebugOn)
                    debugger.WriteLine("Transactino started on:" + Db.Database);

            }
            catch (Exception exc)
            {
                if (DebugOn)
                    debugger.WriteLine("Exception with begin transactino connection:" + exc.Message);
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
                if (DebugOn)
                    debugger.WriteLine("Transaction commited on:" + Db.Database);
            }
        }
        /// <summary>
        /// Dispose client
        /// </summary>
        public void Dispose()
        {
            if (Db != null && Db.State != ConnectionState.Closed)
                Db.Close();
            if (DebugOn)
                debugger.WriteLine("MySQLClient object disposed");
        }
        /// <summary>
        /// Perform a query
        /// </summary>
        /// <typeparam name="T">Result type object</typeparam>
        /// <param name="query">Query</param>
        /// <param name="values">List of binded values</param
        /// <exception cref="MySqlException">MySqlException</exception>
        /// <returns>List of T objects (rows)</returns>
        public async Task<List<T>> QueryAsync<T>(string query, List<DbParameter> values = null) where T : class, new()
        {
            await this.SetConnectionState();
            if (DebugOn)
            {
                debugger.WriteLine("Executing query: " + query);
                debugger.WriteLine("With params: ");
                if (values != null)
                    foreach (var item in values)
                    {
                        debugger.WriteLine("Type:{0}, value:{1}, paramName:{2} | ", new object[] { item.DbType, item.Value, item.ParameterName });
                    }
            }
            List<T> result = null;
            using (var cmd = Db.CreateCommand() as MySqlCommand)
            {

                cmd.CommandText = query;
                if (values != null)
                {
                    foreach (var item in values)
                    {
                        cmd.Parameters.Add(item);
                    }
                }
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    result = await this.SetFields<T>(reader);
                }

            }
            return result;
        }
        /// <summary>
        /// Set the fields of a Type T from a reader of a T table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        private async Task<List<T>> SetFields<T>(DbDataReader reader) where T : class, new()
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
                    var value = reader.GetValue(i).GetType() == typeof(DBNull) ? null : reader.GetValue(i);
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
        public async Task<List<object[]>> QueryArrayAsync(string query, List<DbParameter> values = null)
        {
            await this.SetConnectionState();
            if (DebugOn)
            {
                debugger.WriteLine("Executing query: " + query);
                debugger.WriteLine("With params: ");
                if (values != null)
                    foreach (var item in values)
                    {
                        debugger.WriteLine("Type:{0}, value:{1}, paramName:{2} | ", new object[] { item.DbType, item.Value, item.ParameterName });
                    }
            }
            var cmd = Db.CreateCommand() as MySqlCommand;
            cmd.CommandText = query;
            if (values != null)
            {
                foreach (var item in values)
                {
                    cmd.Parameters.Add(item);
                }
            }
            List<object[]> result = new List<object[]>();
            using (var reader = await cmd.ExecuteReaderAsync())
            {                
                while (await reader.ReadAsync())
                {
                    object[] item = new object[reader.FieldCount];
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        item[i] = reader.GetValue(i);
                    }
                    result.Add(item);
                }
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
                if (DebugOn)
                    debugger.WriteLine("Transaction rollbacked on:" + Db.Database);
            }
        }
        /// <summary>
        /// Perform a query (good for UPDATE,DELETE,INSERT)
        /// </summary>
        /// <param name="query"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public async Task<IMySQLResponse> QueryAsync(string query, List<DbParameter> values = null)
        {
            await this.SetConnectionState();
            if (DebugOn)
            {
                debugger.WriteLine("Executing query: " + query);
                debugger.WriteLine("With params: ");
                if (values != null)
                    foreach (var item in values)
                    {
                        debugger.WriteLine("Type:{0}, value:{1}, paramName:{2} | ", new object[] { item.DbType, item.Value, item.ParameterName });
                    }
            }
            var cmd = Db.CreateCommand() as MySqlCommand;
            cmd.CommandText = query;
            if (values != null)
            {
                foreach (var item in values)
                {
                    cmd.Parameters.Add(item);
                }
            }
            int affectedRows = 0;
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                affectedRows = reader.RecordsAffected;
            }
            return new MySQLResponse(cmd.LastInsertedId, affectedRows);
        }
        /// <summary>
        /// Select directly in LINQ. NOTE: T name must be the Table Name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="tablePrefix"></param>
        /// <returns></returns>
        public async Task<List<T>> Select<T>(Func<T, bool> predicate, string tablePrefix = null) where T : class, new()
        {
            Type t = typeof(T);
            var response = await this.QueryAsync<T>($"SELECT * FROM {tablePrefix}{t.Name}");
            return response.Where(predicate).ToList<T>();
        }
        /// <summary>
        /// Select directly in LINQ. NOTE: T name must be the Table Name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tablePrefix"></param>
        /// <returns></returns>
        public async Task<List<T>> Select<T>(string tablePrefix = null) where T : class, new()
        {
            Type t = typeof(T);
            var response = await this.QueryAsync<T>($"SELECT * FROM {tablePrefix}{t.Name}");
            return response;
        }
    }
}
