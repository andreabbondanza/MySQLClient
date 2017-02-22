using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DewCore.MySQLClient
{
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
        Task<List<object[]>> QueryArrayAsync<T>(string query, List<MySqlParameter> values);
        /// <summary>
        /// Perform a query
        /// </summary>
        /// <typeparam name="T">Result type object</typeparam>
        /// <param name="query">Query</param>
        /// <param name="values">List of binded values</param>
        /// <returns>List of T objects (rows)</returns>
        Task<List<T>> QueryAsync<T>(string query, List<MySqlParameter> values);
        /// <summary>
        /// Perform a select on a table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selector"></param>
        /// <returns></returns>
        Task<List<T>> SelectAsync<T>(Func<Task, bool> selector);
        /// <summary>
        /// Perform a delete on a table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selector"></param>
        /// <returns></returns>
        Task<List<T>> DeleteAsync<T>(Func<Task, bool> selector);
        /// <summary>
        /// Perform an update on a table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selector"></param>
        /// <param name="updated"></param>
        /// <returns></returns>
        Task<List<T>> UpdateAsync<T>(Func<Task, bool> selector, T updated);
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
        /// Constructor
        /// </summary>
        /// <param name="connectionString"></param>
        public MySQLClient(string connectionString)
        {
            this.Db = new MySqlConnection(connectionString);            
        }

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

        public async Task CommitAsync()
        {
            if (transiction == null)
                throw new NoTransactionException();
            else
            {
                await this.transiction.CommitAsync();
            }
        }

        public Task<List<T>> DeleteAsync<T>(Func<Task, bool> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Dispose client
        /// </summary>
        public void Dispose()
        {
            if(Db != null && Db.State != System.Data.ConnectionState.Closed)
                Db.Close();
        }
        /// <summary>
        /// Perform a query
        /// </summary>
        /// <typeparam name="T">Result type object</typeparam>
        /// <param name="query">Query</param>
        /// <param name="values">List of binded values</param>
        /// <returns>List of T objects (rows)</returns>
        public List<T> PerformQueryAsync<T>(string query, List<MySqlParameter> values)
        {
            var cmd = Db.CreateCommand() as MySqlCommand;
            
            return null;

        }

        public Task<List<object[]>> QueryArrayAsync<T>(string query, List<MySqlParameter> values)
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> QueryAsync<T>(string query, List<MySqlParameter> values)
        {
            throw new NotImplementedException();
        }

        public async Task RoolbackAsync()
        {
            if (transiction == null)
                throw new NoTransactionException();
            else
            {
                await this.transiction.RollbackAsync();
            }
        }
        public void p(object s,System.EventArgs a)
        { }
        public Task<List<T>> SelectAsync<T>(Func<Task, bool> selector)
        {
            
            throw new NotImplementedException();
        }

        public Task<List<T>> UpdateAsync<T>(Func<Task, bool> selector, T updated)
        {
            throw new NotImplementedException();
        }
    }
}
