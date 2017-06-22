namespace DewCore.DewDatabase.MySQL
{
    /// <summary>
    /// MySQL connection string
    /// </summary>
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
        /// Other flags
        /// </summary>
        public readonly string OtherFlags;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="host">server address</param>
        /// <param name="port">server port</param>
        /// <param name="database">database name</param>
        /// <param name="user">username</param>
        /// <param name="password">password</param>
        public MySQLConnectionString(string host, string port, string database, string user, string password, string otherFlags = null)
        {
            this.Host = host;
            this.Port = port;
            this.Database = database;
            this.User = user;
            this.Password = password;
            this.OtherFlags = otherFlags;
        }
        /// <summary>
        /// Return the current parameters connection string
        /// </summary>
        /// <returns></returns>
        public string GetConnectionString()
        {
            return @"host=" + this.Host + ";port=" + this.Port + ";user id=" + this.User + ";password=" + this.Password + ";database=" + this.Database + ";" + OtherFlags;
        }
    }
}
