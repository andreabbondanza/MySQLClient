using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DewCore.MySQLClient
{
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
        /// Perform a query
        /// </summary>
        /// <typeparam name="T">Result type object</typeparam>
        /// <param name="query">Query</param>
        /// <param name="values">List of binded values</param>
        /// <returns>List of T objects (rows)</returns>
        List<T> Query<T>(string query, List<MySqlParameter> values);
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

        Task<bool> CommitAsync();
        Task<bool> RoolbackAsync();

        Task<bool> BeginTransactionAsync();

    }
    public class MySQLClient : IMySQLClient, IDisposable
    {
        /// <summary>
        /// Database connection
        /// </summary>
        public readonly MySqlConnection Db = null;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString"></param>
        public MySQLClient(string connectionString)
        {
            this.Db = new MySqlConnection(connectionString);
            
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
    }
}
