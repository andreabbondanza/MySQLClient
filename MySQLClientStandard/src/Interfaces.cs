using DewCore.Abstract.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace DewCore.Abstract.Database.MySQL
{
    /// <summary>
    /// MySQL Response object for INSERT\DELETE\UPDATE
    /// </summary>
    public interface IMySQLResponse : IDatabaseResponse
    {

    }
    /// <summary>
    /// MySQLClient dew interface
    /// </summary>
    public interface IMySQLClient : IDatabaseClient<IMySQLResponse>
    {

    }/// <summary>
     /// MySQL Table interface
     /// </summary>
    public interface IMySQLTable : IDatabaseTable
    {

    }
}
