using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace DewCore.DewDatabase.MySQL.Interfaces
{
    /// <summary>
    /// MySQL Response object for INSERT\DELETE\UPDATE
    /// </summary>
    public interface IMySQLResponse
    {
        /// <summary>
        /// Get last inserted row
        /// </summary>
        /// <returns></returns>
        long GetLastInsertedId();
        /// <summary>
        /// Get affected rows
        /// </summary>
        /// <returns></returns>
        long GetRowsAffected();
        /// <summary>
        /// Get the number of columns
        /// </summary>
        /// <returns></returns>
        int GetFieldCount();
    }
    /// <summary>
    /// MySQLClient dew interface
    /// </summary>
    public interface IMySQLClient
    {
        /// <summary>
        /// Perform a query and return result in a ICollection of array
        /// </summary>
        /// <typeparam name="T">Result type object</typeparam>
        /// <param name="query">Query</param>
        /// <param name="values">ICollection of binded values</param>
        /// <returns>ICollection of array objects (rows)</returns>
        Task<ICollection<object[]>> QueryArrayAsync(string query, ICollection<DbParameter> values);
        /// <summary>
        /// Perform a query (good for SELECT) : 
        /// </summary>
        /// <typeparam name="T">Result type object</typeparam>
        /// <param name="query">Query</param>
        /// <param name="values">ICollection of binded values</param>
        /// <returns>ICollection of T objects (rows)</returns>
        Task<ICollection<T>> QueryAsync<T>(string query, ICollection<DbParameter> values) where T : class, new();
        /// <summary>
        /// Select directly in LINQ. NOTE: T name must be the Table Name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="tablePrefix"></param>
        /// <returns></returns>
        Task<ICollection<T>> Select<T>(Func<T, bool> predicate, string tablePrefix) where T : class, new();
        /// <summary>
        /// Select directly in LINQ. NOTE: T name must be the Table Name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tablePrefix"></param>
        /// <returns></returns>
        Task<ICollection<T>> Select<T>(string tablePrefix) where T : class, new();
        /// <summary>
        /// Perform a query (good for insert, update, delete)
        /// </summary>
        /// <param name="query"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        Task<IMySQLResponse> QueryAsync(string query, ICollection<DbParameter> values);
        /// <summary>
        /// Update a row into the T table.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="toFind">The searched row</param>
        /// <param name="toUpdate">the row</param>
        /// <param name="tablePrefix">a table prefix if exists</param>
        /// <returns></returns>
        Task<IMySQLResponse> UpdateAsync<T>(T toFind, T toUpdate, string tablePrefix = null) where T : class;
        /// <summary>
        /// Delete a row into the T table. Works only with "=" assertions
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="toDelete">The data</param>
        /// <param name="tablePrefix">a table prefix if exists</param>
        /// <returns></returns>
        Task<IMySQLResponse> DeleteAsync<T>(T toDelete, string tablePrefix = null) where T : class;
        /// <summary>
        /// Insert a row into the T table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newRow">The data</param>
        /// <param name="tablePrefix">a table prefix if exists</param>
        /// <returns></returns>
        Task<IMySQLResponse> InsertAsync<T>(T newRow, string tablePrefix = null) where T : class;
        /// <summary>
        /// Commit a transaction
        /// </summary>
        Task CommitAsync();
        /// <summary>
        /// Rollback a transaction
        /// </summary>
        Task RollbackAsync();
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
}
