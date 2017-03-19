using System;

namespace DewCore.DewDatabase.MySQL
{
    /// <summary>
    /// Exception for null reference in transaction
    /// </summary>
    public class NoTransactionException : Exception
    {
        /// <summary>
        /// To string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "No transaction initalized. Missing begin transaction";
        }
    }
}
