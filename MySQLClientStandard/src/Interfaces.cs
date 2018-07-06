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

    public interface IMySQLQueryComposer : IQueryComposer
    {

    }

    /// <summary>
    /// ------------------ temp interfaces for DEWDATABASE ABSTRACT
    /// </summary>
    public interface IQueryComposer
    {
        IQueryComposer Select(params string[] columns);
        IQueryComposer SelectDistinct(params string[] columns);
        IQueryComposer SelectCount(params string[] columns);
        IQueryComposer SelectAvg(string column);
        IQueryComposer SelectSum(string column);
        IQueryComposer SelectMin(string column);
        IQueryComposer SelectMax(string column);
        IQueryComposer Update(string table);
        IQueryComposer Delete();
        IQueryComposer Insert(string table, params string[] columns);
        IQueryComposer Values(params string[] columns);
        IQueryComposer From(string table);
        IQueryComposer Join(string table);
        IQueryComposer LJoin(string table);
        IQueryComposer RJoin(string table);
        IQueryComposer Where(string column, string op, string value);
        IQueryComposer OrderBy(params string[] columns);
        IQueryComposer OrderByDesc(params string[] columns);
        IQueryComposer GroupBy(params string[] columns);
        IQueryComposer AND(string column, string op, string value);
        IQueryComposer AND();
        IQueryComposer Condition(string column, string op, string value, string comma);
        IQueryComposer OR();
        IQueryComposer IN(IQueryComposer innerComposer);
        IQueryComposer OR(string column, string op, string value);
        IQueryComposer NOT(string column, string op, string value);
        IQueryComposer NOT();
        IQueryComposer Between(string column, string before, string after);
        IQueryComposer Brackets(IQueryComposer innerComposer);
        IQueryComposer ON(string column, string op, string value);
        IQueryComposer Reset();
        string ComposedQuery();
    }


    public interface IIQueryComposer
    {
        void Reset();
        string ComposedQuery();
        IIQueryComposer Compose(IIQueryComposer compose);

    }

    public interface IRootComposer
    {
        ISelectComposer Select(params string[] columns);
        ISelectComposer SelectDistinct(params string[] columns);
        ISelectComposer SelectCount(params string[] columns);
        ISelectComposer SelectAvg(string column);
        ISelectComposer SelectSum(string column);
        ISelectComposer SelectMin(string column);
        ISelectComposer SelectMax(string column);
        ISelectComposer Update(string table);
        IDeleteComposer Delete(string table);
        IInsertComposer Insert(string table, params string[] columns);
    }

    public interface ISelectComposer : IIQueryComposer
    {
        IFromComposer From(string table);
    }

    public interface IDeleteComposer : IIQueryComposer
    {
        IWhereComposer Where(string column, string op, string value);
    }
    public interface IInsertComposer : IIQueryComposer
    {
        IValuesComposer Values(params string[] values);
        ISelectComposer Select(params string[] columns);
        ISelectComposer SelectDistinct(params string[] columns);
    }

    public interface IValuesComposer : IIQueryComposer
    {

    }
    public interface IFromComposer : IIQueryComposer
    {
        IJoinComposer Join(string table);
        IJoinComposer LJoin(string table);
        IJoinComposer RJoin(string table);
        IWhereComposer Where(string column, string op, string value);
        IGroupByComposer GroupBy(params string[] columns);
        IOrderByComposer OrderBy(params string[] columns);
        IOrderByComposer OrderByDesc(params string[] columns);
    }



    public interface IGroupByComposer : IIQueryComposer
    {
        IOrderByComposer OrderBy(params string[] columns);
        IOrderByComposer OrderByDesc(params string[] columns);
    }
    public interface IOrderByComposer : IIQueryComposer
    {

    }

    public interface IWhereComposer : IIQueryComposer
    {

    }

    public interface IJoinComposer : IIQueryComposer
    {

    }


}
