using DewCore.Abstract.Database.MySQL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DewCore.Database.MySQL
{
    /// <summary>
    /// Query composer
    /// </summary>
    public class MySQLQueryComposer : IMySQLQueryComposer
    {
        /// <summary>
        /// Current composed query
        /// </summary>
        public string CurrentQuery { get; private set; } = string.Empty;

        public IQueryComposer AND(string column, string op, string value)
        {
            CurrentQuery += $" AND {column} {op} {value} ";
            return this;
        }

        public IQueryComposer AND()
        {
            CurrentQuery += $" AND ";
            return this;
        }

        public IQueryComposer Between(string column, string before, string after)
        {
            CurrentQuery += $" {column} BETWEEN {before} AND {after}";
            return this;
        }

        public IQueryComposer Brackets(IQueryComposer innerComposer)
        {
            CurrentQuery += $" ({innerComposer.ComposedQuery()}) ";
            return this;
        }

        public string ComposedQuery()
        {
            return CurrentQuery;
        }

        public IQueryComposer Condition(string column, string op, string value, string comma = "")
        {
            CurrentQuery += $" {column} {op} {value} {comma}";
            return this;
        }


        public IQueryComposer Delete()
        {
            CurrentQuery += $" DELETE ";
            return this;
        }

        public IQueryComposer From(string table)
        {
            CurrentQuery += $" FROM {table} ";
            return this;
        }

        public IQueryComposer GroupBy(params string[] columns)
        {
            CurrentQuery += $" GROUP BY {columns.Aggregate((curr, next) => curr + "," + next)} DESC";
            return this;
        }

        public IQueryComposer IN(IQueryComposer innerComposer)
        {
            CurrentQuery += $" IN ({innerComposer.ComposedQuery()}) ";
            return this;
        }

        public IQueryComposer Insert(string table, params string[] columns)
        {
            CurrentQuery += $" INSERT INTO {table} ({columns.Aggregate((curr, next) => curr + "," + next)}) ";
            return this;
        }

        public IQueryComposer Join(string table)
        {
            CurrentQuery += $" INNER JOIN {table} ";
            return this;
        }

        public IQueryComposer LJoin(string table)
        {
            CurrentQuery += $" LEFT JOIN {table} ";
            return this;
        }

        public IQueryComposer NOT(string column, string op, string value)
        {
            CurrentQuery += $" NOT {column} {op} {value} ";
            return this;
        }

        public IQueryComposer NOT()
        {
            CurrentQuery += " NOT ";
            return this;
        }

        public IQueryComposer ON(string column, string op, string value)
        {
            CurrentQuery += $"ON {column} {op} {value}";
            return this;
        }

        public IQueryComposer OR(string column, string op, string value)
        {
            CurrentQuery += $" OR {column} {op} {value} ";
            return this;
        }

        public IQueryComposer OR()
        {
            CurrentQuery += $" OR ";
            return this;
        }

        public IQueryComposer OrderBy(params string[] columns)
        {
            CurrentQuery += $" ORDER BY {columns.Aggregate((curr, next) => curr + "," + next)} ";
            return this;
        }

        public IQueryComposer OrderByDesc(params string[] columns)
        {
            CurrentQuery += $" ORDER BY {columns.Aggregate((curr, next) => curr + "," + next)} DESC";
            return this;
        }

        public IQueryComposer Reset()
        {
            CurrentQuery = string.Empty;
            return this;
        }

        public IQueryComposer RJoin(string table)
        {
            CurrentQuery += $" RIGHT JOIN {table} ";
            return this;
        }
        /// <summary>
        /// Select composer
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public IQueryComposer Select(params string[] columns)
        {
            string cols = "*";
            cols = columns.Length <= 0 ? "*" : columns.Aggregate((curr, next) => curr + "," + next);
            CurrentQuery += $"SELECT {cols} ";
            return this;
        }

        public IQueryComposer SelectAvg(string column)
        {
            CurrentQuery += $"SELECT AVG({column}) ";
            return this;
        }

        public IQueryComposer SelectCount(params string[] columns)
        {
            string cols = "*";
            cols = columns.Length <= 0 ? "*" : columns.Aggregate((curr, next) => curr + "," + next);
            CurrentQuery += $"SELECT COUNT({cols}) ";
            return this;
        }

        public IQueryComposer SelectDistinct(params string[] columns)
        {
            string cols = null;
            cols = columns.Length <= 0 ? "*" : columns.Aggregate((curr, next) => curr + "," + next);
            CurrentQuery += $"SELECT DISTINCT {cols} ";
            return this;
        }

        public IQueryComposer SelectMax(string column)
        {
            CurrentQuery += $"SELECT MAX({column}) ";
            return this;
        }

        public IQueryComposer SelectMin(string column)
        {
            CurrentQuery += $"SELECT MIN({column}) ";
            return this;
        }

        public IQueryComposer SelectSum(string column)
        {
            CurrentQuery += $"SELECT SUM({column}) ";
            return this;
        }

        public IQueryComposer Update(string table)
        {
            CurrentQuery += $"UPDATE {table} SET ";
            return this;
        }

        public IQueryComposer Values(params string[] columns)
        {
            CurrentQuery += $" VALUES ({columns.Aggregate((curr, next) => curr + "," + next)}) ";
            return this;
        }

        public IQueryComposer Where(string column, string op, string value)
        {
            CurrentQuery += $" WHERE {column} {op} {value} ";
            return this;

        }

        public static implicit operator string(MySQLQueryComposer composer) => composer.CurrentQuery;
    }

    /// <summary>
    /// Composer
    /// </summary>
    public class QueryComposer : IIQueryComposer
    {
        /// <summary>
        /// Current composed query
        /// </summary>
        public string CurrentQuery { get; protected set; } = string.Empty;
        /// <summary>
        /// Reset
        /// </summary>
        public void Reset() => CurrentQuery = string.Empty;

        public string GetAndCleanQuery { get { var temp = CurrentQuery; CurrentQuery = string.Empty; return temp; } }
        /// <summary>
        /// Get composed string
        /// </summary>
        /// <returns></returns>
        public string ComposedQuery() => CurrentQuery;
        public IIQueryComposer Compose(IIQueryComposer compose)
        {
            CurrentQuery += compose.ComposedQuery();
            return this;
        }
    }
    public class RootComposer : QueryComposer, IRootComposer
    {
        public IDeleteComposer Delete(string table)
        {
            CurrentQuery += $"DELETE FROM {table} ";
            return new DeleteComposer(GetAndCleanQuery);
        }

        public IInsertComposer Insert(string table, params string[] columns)
        {
            CurrentQuery += $"INSERT INTO {table} ({columns.Aggregate((curr, next) => curr + "," + next)}) ";
            return new InsertComposer(GetAndCleanQuery);
        }

        public ISelectComposer Select(params string[] columns)
        {
            string cols = "*";
            cols = columns.Length <= 0 ? "*" : columns.Aggregate((curr, next) => curr + "," + next);
            CurrentQuery += $"SELECT {cols} ";
            return new SelectComposer(GetAndCleanQuery);
        }

        public ISelectComposer SelectAvg(string column)
        {
            CurrentQuery += $"SELECT AVG({column}) ";
            return new SelectComposer(GetAndCleanQuery);
        }

        public ISelectComposer SelectCount(params string[] columns)
        {
            string cols = "*";
            cols = columns.Length <= 0 ? "*" : columns.Aggregate((curr, next) => curr + "," + next);
            CurrentQuery += $"SELECT COUNT({cols}) ";
            return new SelectComposer(GetAndCleanQuery);
        }

        public ISelectComposer SelectDistinct(params string[] columns)
        {
            string cols = null;
            cols = columns.Length <= 0 ? "*" : columns.Aggregate((curr, next) => curr + "," + next);
            CurrentQuery += $"SELECT DISTINCT {cols} ";
            return new SelectComposer(GetAndCleanQuery);
        }

        public ISelectComposer SelectMax(string column)
        {
            CurrentQuery += $"SELECT MAX({column}) ";
            return new SelectComposer(GetAndCleanQuery);
        }

        public ISelectComposer SelectMin(string column)
        {
            CurrentQuery += $"SELECT MIN({column}) ";
            return new SelectComposer(GetAndCleanQuery);
        }

        public ISelectComposer SelectSum(string column)
        {
            CurrentQuery += $"SELECT MIN({column}) ";
            return new SelectComposer(GetAndCleanQuery);
        }

        public ISelectComposer Update(string table)
        {
            throw new NotImplementedException();
        }
    }
    public class SelectComposer : QueryComposer, ISelectComposer
    {
        public SelectComposer(string query)
        {
            CurrentQuery += query;
        }
        public IFromComposer From(string table)
        {
            CurrentQuery += $" FROM {table} ";
            return new FromComposer(GetAndCleanQuery);
        }
    }

    public class FromComposer : QueryComposer, IFromComposer
    {
        public FromComposer(string query)
        {
            CurrentQuery += query;
        }

        public IGroupByComposer GroupBy(params string[] columns)
        {
            throw new NotImplementedException();
        }

        public IJoinComposer Join(string table)
        {
            throw new NotImplementedException();
        }

        public IJoinComposer LJoin(string table)
        {
            throw new NotImplementedException();
        }

        public IOrderByComposer OrderBy(params string[] columns)
        {
            CurrentQuery += $" ORDER BY {columns.Aggregate((curr, next) => curr + "," + next)} ";
            return new OrderByComposer(GetAndCleanQuery);
        }

        public IOrderByComposer OrderByDesc(params string[] columns)
        {
            CurrentQuery += $" ORDER BY {columns.Aggregate((curr, next) => curr + "," + next)} DESC";
            return new OrderByComposer(GetAndCleanQuery);
        }

        public IJoinComposer RJoin(string table)
        {
            throw new NotImplementedException();
        }

        public IWhereComposer Where(string column, string op, string value)
        {
            throw new NotImplementedException();
        }
    }

    public class InsertComposer : QueryComposer, IInsertComposer
    {
        public InsertComposer(string query) => CurrentQuery += query;
        public ISelectComposer Select(params string[] columns)
        {
            string cols = "*";
            cols = columns.Length <= 0 ? "*" : columns.Aggregate((curr, next) => curr + "," + next);
            CurrentQuery += $"SELECT {cols} ";
            return new SelectComposer(GetAndCleanQuery);
        }
        public ISelectComposer SelectDistinct(params string[] columns)
        {
            string cols = null;
            cols = columns.Length <= 0 ? "*" : columns.Aggregate((curr, next) => curr + "," + next);
            CurrentQuery += $"SELECT DISTINCT {cols} ";
            return new SelectComposer(GetAndCleanQuery);
        }

        public IValuesComposer Values(params string[] values)
        {
            CurrentQuery += $" VALUES ({values.Aggregate((curr, next) => curr + "," + next)}) ";
            return new ValuesComposer(GetAndCleanQuery);
        }
    }
    public class ValuesComposer : QueryComposer, IValuesComposer
    {
        public ValuesComposer(string query) => CurrentQuery += query;
    }
    public class OrderByComposer : QueryComposer, IOrderByComposer
    {
        public OrderByComposer(string query)
        {
            CurrentQuery += query;
        }
    }

    public class GroupByComposer : QueryComposer, IGroupByComposer
    {
        public GroupByComposer(string query)
        {
            CurrentQuery += query;
        }

        public IOrderByComposer OrderBy(params string[] columns)
        {
            CurrentQuery += $" ORDER BY {columns.Aggregate((curr, next) => curr + "," + next)} ";
            return new OrderByComposer(GetAndCleanQuery);
        }

        public IOrderByComposer OrderByDesc(params string[] columns)
        {
            CurrentQuery += $" ORDER BY {columns.Aggregate((curr, next) => curr + "," + next)} DESC";
            return new OrderByComposer(GetAndCleanQuery);
        }
    }

    public class DeleteComposer : QueryComposer, IDeleteComposer
    {
        public DeleteComposer(string query)
        {
            CurrentQuery += query;
        }
        public IWhereComposer Where(string column, string op, string value)
        {
            throw new NotImplementedException();
        }
    }

}
