namespace DewCore.DewDatabase.MySQL
{
    /// <summary>
    /// Indicate if class must open a new connection for every query
    /// </summary>
    public enum OneConnection
    {
        /// <summary>
        /// One connection
        /// </summary>
        Yes,
        /// <summary>
        /// Multiple connections
        /// </summary>
        No
    }
}