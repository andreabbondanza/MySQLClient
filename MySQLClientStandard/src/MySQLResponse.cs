using DewCore.Abstract.Database;
using DewCore.Abstract.Database.MySQL;

namespace DewCore.Database.MySQL
{
    /// <summary>
    /// MySQLClient response object (for update,insert,delete)
    /// </summary>
    public class MySQLResponse : IMySQLResponse
    {
        private bool _success = true;
        private readonly DatabaseError _error = null;
        /// <summary>
        /// Last insert id for the query
        /// </summary>
        private readonly long _lastInsertId;
        /// <summary>
        /// Number of rows affected for the query
        /// </summary>
        private readonly long _rowsAffected;
        /// <summary>
        /// Number of rows affected for the query
        /// </summary>
        private readonly int _fieldCount;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="lastInsertId"></param>
        /// <param name="rowsAffected"></param>
        public MySQLResponse(long lastInsertId, long rowsAffected, int fieldCount, DatabaseError err = null)
        {
            _lastInsertId = lastInsertId;
            _rowsAffected = rowsAffected;
            _fieldCount = fieldCount;
            _error = err;
            if (err != null)
                _success = false;
        }
        /// <summary>
        /// Return database error
        /// </summary>
        /// <returns></returns>
        public DatabaseError GetError()
        {
            return _error;
        }

        public int GetFieldCount()
        {
            return _fieldCount;
        }

        public long GetLastInsertedId()
        {
            return _lastInsertId;
        }

        public long GetRowsAffected()
        {
            return _rowsAffected;
        }

        public bool IsSuccess()
        {
            return _success;
        }
    }
}
