using System;
using DewInterfaces.DewDatabase.MySQL;

namespace DewCore.DewDatabase.MySQL
{
    /// <summary>
    /// MySQLClient response object (for update,insert,delete)
    /// </summary>
    public class MySQLResponse : IMySQLResponse
    {
        /// <summary>
        /// Last insert id for the query
        /// </summary>
        private readonly long LastInsertId;
        /// <summary>
        /// Number of rows affected for the query
        /// </summary>
        private readonly long RowsAffected;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="lastInsertId"></param>
        /// <param name="rowsAffected"></param>
        public MySQLResponse(long lastInsertId, long rowsAffected)
        {
            LastInsertId = lastInsertId;
            RowsAffected = rowsAffected;
        }

        public long GetLastInsertedId()
        {
            return this.LastInsertId;
        }

        public long GetRowsAffected()
        {
            return this.RowsAffected;
        }
    }
}
