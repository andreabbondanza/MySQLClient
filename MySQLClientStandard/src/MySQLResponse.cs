namespace DewCore.MySQLClient
{
    /// <summary>
    /// MySQLClient response object (for update,insert,delete)
    /// </summary>
    public class MySQLResponse
    {
        /// <summary>
        /// Last insert id for the query
        /// </summary>
        public readonly long LastInsertId;
        /// <summary>
        /// Number of rows affected for the query
        /// </summary>
        public readonly long RowsAffected;
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
    }
}
