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
        /// Number of rows affected for the query
        /// </summary>
        private readonly int FieldCount;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="lastInsertId"></param>
        /// <param name="rowsAffected"></param>
        public MySQLResponse(long lastInsertId, long rowsAffected, int fieldCount)
        {
            LastInsertId = lastInsertId;
            RowsAffected = rowsAffected;
            FieldCount = fieldCount;
        }

        public int GetFieldCount()
        {
            return this.FieldCount;
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
