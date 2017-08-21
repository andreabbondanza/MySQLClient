﻿using DewCore.Abstract.Database;
using DewCore.Abstract.Database.MySQL;
using DewCore.DewLogger;
using DewLogger.Interfaces;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace DewCore.Database.MySQL
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
        private MySqlTransaction transaction = null;
        ///<summary>
        /// Cancellation token
        ///</summary>
        private CancellationToken cancellationToken = default(CancellationToken);
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
        public MySQLClient(string connectionString, CancellationToken cancellationToken = default(CancellationToken))
        {
            this.Db = new MySqlConnection(connectionString);
            if (DebugOn)
            {
                debugger.WriteLine("Connection to database:" + Db.Database);
                debugger.WriteLine("With connection string:" + connectionString);
            }
            this.cancellationToken = cancellationToken;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString"></param>
        public MySQLClient(MySQLConnectionString connectionString, CancellationToken cancellationToken = default(CancellationToken))
        {
            this.Db = new MySqlConnection(connectionString.GetConnectionString());
            if (DebugOn)
            {
                debugger.WriteLine("Connection to database:" + Db.Database);
                debugger.WriteLine("With user:" + connectionString.User);
                debugger.WriteLine("With password:" + connectionString.Password);
                debugger.WriteLine("With host:" + connectionString.Host);
                debugger.WriteLine("With port:" + connectionString.Port);
            }
            this.cancellationToken = cancellationToken;
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
        /// Begin a transaction
        /// </summary>
        /// <param name="isolationLevel"></param>
        /// <returns></returns>
        public async Task<bool> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        {
            bool result = true;
            try
            {
                await this.OpenConnectionAsync();
                if (this.transaction == null)
                    this.transaction = await Db.BeginTransactionAsync(isolationLevel, cancellationToken == null ? default(CancellationToken) : cancellationToken);
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
            if (transaction == null)
                throw new NoTransactionException();
            else
            {
                await this.transaction.CommitAsync(cancellationToken == null ? default(CancellationToken) : cancellationToken);
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
        /// <param name="values">ICollection of binded values</param
        /// <exception cref="MySqlException">MySqlException</exception>
        /// <returns>ICollection of T objects (rows)</returns>
        public async Task<ICollection<T>> QueryAsync<T>(string query, ICollection<DbParameter> values = null) where T : class, new()
        {
            await this.OpenConnectionAsync();
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
            ICollection<T> result = null;
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
                using (var reader = await cmd.ExecuteReaderAsync(cancellationToken == null ? default(CancellationToken) : cancellationToken))
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
        private async Task<ICollection<T>> SetFields<T>(DbDataReader reader) where T : class, new()
        {
            var columns = reader.FieldCount;
            ICollection<T> result = new List<T>();
            while (await reader.ReadAsync())
            {
                T item = new T();
                var reflection = item.GetType();
                var properties = reflection.GetRuntimeProperties();
                for (int i = 0; i < columns; i++)
                {
                    var colName = reader.GetName(i);
                    var property = properties.FirstOrDefault((x) => { return x.Name == colName; });
                    if (property != null)
                    {
                        var value = reader.GetValue(i).GetType() == typeof(DBNull) ? null : reader.GetValue(i);
                        property.SetValue(item, value);
                    }
                }
                result.Add(item);
            }
            return result;
        }
        /// <summary>
        /// Perform a query
        /// </summary>
        /// <param name="query">Query</param>
        /// <param name="values">ICollection of binded values</param
        /// <exception cref="MySqlException">MySqlException</exception>
        /// <returns>ICollection of array objects (rows)</returns>
        public async Task<ICollection<object[]>> QueryArrayAsync(string query, ICollection<DbParameter> values = null)
        {
            await this.OpenConnectionAsync();
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
            ICollection<object[]> result = new List<object[]>();
            using (var reader = await cmd.ExecuteReaderAsync(cancellationToken == null ? default(CancellationToken) : cancellationToken))
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
        public async Task RollbackAsync()
        {
            if (transaction == null)
                throw new NoTransactionException();
            else
            {
                await this.transaction.RollbackAsync(cancellationToken == null ? default(CancellationToken) : cancellationToken);
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
        public async Task<IMySQLResponse> QueryAsync(string query, ICollection<DbParameter> values = null)
        {
            await this.OpenConnectionAsync();
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
            int fieldcount = 0;
            using (var reader = await cmd.ExecuteReaderAsync(cancellationToken == null ? default(CancellationToken) : cancellationToken))
            {
                affectedRows = reader.RecordsAffected;
                fieldcount = reader.FieldCount;
            }
            return new MySQLResponse(cmd.LastInsertedId, affectedRows, fieldcount);
        }
        /// <summary>
        /// Select directly in LINQ. NOTE: T name must be the Table Name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="tablePrefix"></param>
        /// <returns></returns>
        public async Task<ICollection<T>> Select<T>(Func<T, bool> predicate, string tablePrefix = null) where T : class, new()
        {
            Type t = typeof(T);
            var response = await this.QueryAsync<T>($"SELECT * FROM {tablePrefix}{t.Name}");
            return response.Where(predicate).ToList();
        }
        /// <summary>
        /// Select directly in LINQ. NOTE: T name must be the Table Name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tablePrefix"></param>
        /// <returns></returns>
        public async Task<ICollection<T>> Select<T>(string tablePrefix = null) where T : class, new()
        {
            Type t = typeof(T);
            var response = await this.QueryAsync<T>($"SELECT * FROM {tablePrefix}{t.Name}");
            return response;
        }
        /// <summary>
        /// Insert a row into the T table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newRow">The data</param>
        /// <param name="tablePrefix">a table prefix if exists</param>
        /// <returns></returns>
        public async Task<IMySQLResponse> InsertAsync<T>(T newRow, string tablePrefix = null) where T : class
        {
            Type t = newRow.GetType();
            string name = tablePrefix + t.Name;
            string queryBefore = $"INSERT INTO {name} (";
            string queryAfter = ") VALUES (";
            ICollection<DbParameter> parameters = new List<DbParameter>();
            foreach (var item in t.GetRuntimeProperties())
            {
                IEnumerable<Attribute> attributes = item.GetCustomAttributes();
                if (attributes.FirstOrDefault(x => x.GetType() == typeof(IgnoreInsert)) == default(Attribute) && attributes.FirstOrDefault(x => x.GetType() == typeof(NoColumn)) == default(Attribute))
                {
                    queryBefore += item.Name + ",";
                    queryAfter += $"@{item.Name.ToLower()},";
                    parameters.Add(new MySqlParameter() { ParameterName = $"@{item.Name.ToLower()}", Value = item.GetValue(newRow) });
                }
            }
            queryBefore = queryBefore.Substring(0, queryBefore.Length - 1);
            queryAfter = queryAfter.Substring(0, queryAfter.Length - 1) + ")";
            string query = queryBefore + queryAfter;
            return await QueryAsync(query, parameters);
        }
        /// <summary>
        /// Delete a row into the T table. Works only with "=" assertions
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="toDelete">The data</param>
        /// <param name="tablePrefix">a table prefix if exists</param>
        /// <returns></returns>
        public async Task<IMySQLResponse> DeleteAsync<T>(T toDelete, string tablePrefix = null) where T : class
        {
            Type t = toDelete.GetType();
            string name = tablePrefix + t.Name;
            string query = $"DELETE FROM {name} WHERE ";
            ICollection<DbParameter> parameters = new List<DbParameter>();
            foreach (var item in t.GetRuntimeProperties())
            {
                IEnumerable<Attribute> attributes = item.GetCustomAttributes();
                if (attributes.FirstOrDefault(x => x.GetType() == typeof(CheckDelete)) != default(Attribute))
                {
                    query += item.Name + $"=@{item.Name.ToLower()} AND  ";
                    parameters.Add(new MySqlParameter() { ParameterName = $"@{item.Name.ToLower()}", Value = item.GetValue(toDelete) });
                }
            }
            query = query.Substring(0, query.Length - 6);
            return await QueryAsync(query, parameters);
        }
        /// <summary>
        /// Update a row into the T table.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="toFind">The searched row</param>
        /// <param name="toUpdate">the row</param>
        /// <param name="tablePrefix">a table prefix if exists</param>
        /// <returns></returns>
        public async Task<IMySQLResponse> UpdateAsync<T1>(T1 toFind, T1 toUpdate, string tablePrefix = null) where T1 : class
        {
            Type t = toUpdate.GetType();
            string name = tablePrefix + t.Name;
            string query = $"UPDATE {name} SET ";
            ICollection<DbParameter> parameters = new List<DbParameter>();
            foreach (var item in t.GetRuntimeProperties())
            {
                IEnumerable<Attribute> attributes = item.GetCustomAttributes();
                if (attributes.FirstOrDefault(x => x.GetType() == typeof(IgnoreUpdate)) == default(Attribute) && attributes.FirstOrDefault(x => x.GetType() == typeof(NoColumn)) == default(Attribute))
                {
                    query += item.Name + $"=@{item.Name.ToLower()},";
                    parameters.Add(new MySqlParameter() { ParameterName = $"@{item.Name.ToLower()}", Value = item.GetValue(toUpdate) });
                }
            }
            query = query.Substring(0, query.Length - 1) + " WHERE ";
            foreach (var item in t.GetRuntimeProperties())
            {
                IEnumerable<Attribute> attributes = item.GetCustomAttributes();
                if (attributes.FirstOrDefault(x => x.GetType() == typeof(CheckUpdate)) != default(Attribute))
                {
                    query += item.Name + $"=@u{item.Name.ToLower()} AND  ";
                    parameters.Add(new MySqlParameter() { ParameterName = $"@u{item.Name.ToLower()}", Value = item.GetValue(toFind) });
                }
            }
            query = query.Substring(0, query.Length - 6);
            return await QueryAsync(query, parameters);
        }

        public async Task OpenConnectionAsync()
        {
            try
            {
                if (this.Db.State == ConnectionState.Closed || this.Db.State == ConnectionState.Broken)
                {
                    await this.Db.OpenAsync(this.cancellationToken);
                }
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
        /// You should use OpenConnectionAsync
        /// </summary>
        public void OpenConnection()
        {
            this.OpenConnectionAsync().Wait();
        }
        /// <summary>
        /// Not implemented
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        /// <returns></returns>
        public Task CloseConnectionAsync()
        {
            throw new NotImplementedException();
        }
    }
}
