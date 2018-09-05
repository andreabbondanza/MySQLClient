using DewCore.Abstract.Database.MySQL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using DewCore.Abstract.Database;
using DewCore.Extensions.Strings;

namespace DewCore.Database.MySQL
{
    /// <summary>
    /// Query composer
    /// </summary>
    public class MySQLSimpleQueryComposer : IMySQLSimpleQueryComposer
    {
        /// <summary>
        /// Current composed query
        /// </summary>
        public string CurrentQuery { get; private set; } = string.Empty;
        /// <summary>
        /// And composer
        /// </summary>
        /// <param name="column"></param>
        /// <param name="op"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ISimpleQueryComposer And(string column, string op, string value)
        {
            CurrentQuery += $" AND {column} {op} {value} ";
            return this;
        }
        /// <summary>
        /// Single And composer
        /// </summary>
        /// <returns></returns>
        public ISimpleQueryComposer And()
        {
            CurrentQuery += $" AND ";
            return this;
        }
        /// <summary>
        /// Between Composer
        /// </summary>
        /// <param name="column"></param>
        /// <param name="before"></param>
        /// <param name="after"></param>
        /// <returns></returns>
        public ISimpleQueryComposer Between(string column, string before, string after)
        {
            CurrentQuery += $" {column} BETWEEN {before} AND {after}";
            return this;
        }

        public ISimpleQueryComposer Column(string column)
        {
            CurrentQuery += $" {column} ";
            return this;
        }

        /// <summary>
        /// Brackets composer
        /// </summary>
        /// <param name="innerComposer"></param>
        /// <returns></returns>
        public ISimpleQueryComposer Brackets(ISimpleQueryComposer innerComposer)
        {
            CurrentQuery += $" ({innerComposer.ComposedQuery()}) ";
            return this;
        }
        /// <summary>
        /// Composed query
        /// </summary>
        /// <returns></returns>
        public string ComposedQuery()
        {
            return CurrentQuery;
        }
        /// <summary>
        /// Condition composer
        /// </summary>
        /// <param name="column"></param>
        /// <param name="op"></param>
        /// <param name="value"></param>
        /// <param name="comma"></param>
        /// <returns></returns>
        public ISimpleQueryComposer Condition(string column, string op, string value, string comma = "")
        {
            CurrentQuery += $" {column} {op} {value} {comma}";
            return this;
        }
        /// <summary>
        /// Delete composer
        /// </summary>
        /// <returns></returns>
        public ISimpleQueryComposer Delete()
        {
            CurrentQuery += $" DELETE ";
            return this;
        }

        public ISimpleQueryComposer From(string table)
        {
            CurrentQuery += $" FROM {table} ";
            return this;
        }
        /// <summary>
        /// Group by
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public ISimpleQueryComposer GroupBy(params string[] columns)
        {
            CurrentQuery += $" GROUP BY {columns.Aggregate((curr, next) => curr + "," + next)}";
            return this;
        }
        /// <summary>
        /// In composer
        /// </summary>
        /// <param name="innerComposer"></param>
        /// <returns></returns>
        public ISimpleQueryComposer In(ISimpleQueryComposer innerComposer)
        {
            CurrentQuery += $" IN ({innerComposer.ComposedQuery()}) ";
            return this;
        }
        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="table"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public ISimpleQueryComposer Insert(string table, params string[] columns)
        {
            CurrentQuery += $" INSERT INTO {table} ({columns.Aggregate((curr, next) => curr + "," + next)}) ";
            return this;
        }
        /// <summary>
        /// Join
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public ISimpleQueryComposer Join(string table)
        {
            CurrentQuery += $" INNER JOIN {table} ";
            return this;
        }
        /// <summary>
        /// Ljoin
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public ISimpleQueryComposer LJoin(string table)
        {
            CurrentQuery += $" LEFT JOIN {table} ";
            return this;
        }
        /// <summary>
        /// Not composer
        /// </summary>
        /// <param name="column"></param>
        /// <param name="op"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ISimpleQueryComposer Not(string column, string op, string value)
        {
            CurrentQuery += $" NOT {column} {op} {value} ";
            return this;
        }
        /// <summary>
        /// Single not composer
        /// </summary>
        /// <returns></returns>
        public ISimpleQueryComposer Not()
        {
            CurrentQuery += " NOT ";
            return this;
        }
        /// <summary>
        /// On composer
        /// </summary>
        /// <param name="columnA"></param>
        /// <param name="columnB"></param>
        /// <returns></returns>
        public ISimpleQueryComposer On(string columnA, string columnB)
        {
            CurrentQuery += $" ON {columnA} = {columnB}";
            return this;
        }
        /// <summary>
        /// Or composer
        /// </summary>
        /// <param name="column"></param>
        /// <param name="op"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ISimpleQueryComposer Or(string column, string op, string value)
        {
            CurrentQuery += $" OR {column} {op} {value} ";
            return this;
        }
        /// <summary>
        /// Single or composer
        /// </summary>
        /// <returns></returns>
        public ISimpleQueryComposer Or()
        {
            CurrentQuery += $" OR ";
            return this;
        }
        /// <summary>
        /// Order by composer
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public ISimpleQueryComposer OrderBy(params string[] columns)
        {
            CurrentQuery += $" ORDER BY {columns.Aggregate((curr, next) => curr + "," + next)} ";
            return this;
        }
        /// <summary>
        /// Order by desc composer
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public ISimpleQueryComposer OrderByDesc(params string[] columns)
        {
            CurrentQuery += $" ORDER BY {columns.Aggregate((curr, next) => curr + "," + next)} DESC";
            return this;
        }
        /// <summary>
        /// Reset the current query
        /// </summary>
        /// <returns></returns>
        public ISimpleQueryComposer Reset()
        {
            CurrentQuery = string.Empty;
            return this;
        }
        /// <summary>
        /// R join composer
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public ISimpleQueryComposer RJoin(string table)
        {
            CurrentQuery += $" RIGHT JOIN {table} ";
            return this;
        }
        /// <summary>
        /// Select composer
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public ISimpleQueryComposer Select(params string[] columns)
        {
            string cols = "*";
            cols = columns.Length <= 0 ? "*" : columns.Aggregate((curr, next) => curr + "," + next);
            CurrentQuery += $"SELECT {cols} ";
            return this;
        }
        /// <summary>
        /// Select avg
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public ISimpleQueryComposer SelectAvg(string column)
        {
            CurrentQuery += $"SELECT AVG({column}) ";
            return this;
        }
        /// <summary>
        /// Select count
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public ISimpleQueryComposer SelectCount(params string[] columns)
        {
            string cols = "*";
            cols = columns.Length <= 0 ? "*" : columns.Aggregate((curr, next) => curr + "," + next);
            CurrentQuery += $"SELECT COUNT({cols}) ";
            return this;
        }
        /// <summary>
        /// Select distinct
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public ISimpleQueryComposer SelectDistinct(params string[] columns)
        {
            string cols = null;
            cols = columns.Length <= 0 ? "*" : columns.Aggregate((curr, next) => curr + "," + next);
            CurrentQuery += $"SELECT DISTINCT {cols} ";
            return this;
        }
        /// <summary>
        /// Select max
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public ISimpleQueryComposer SelectMax(string column)
        {
            CurrentQuery += $"SELECT MAX({column}) ";
            return this;
        }
        /// <summary>
        /// Select Min
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public ISimpleQueryComposer SelectMin(string column)
        {
            CurrentQuery += $"SELECT MIN({column}) ";
            return this;
        }
        /// <summary>
        /// Select sum composer
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public ISimpleQueryComposer SelectSum(string column)
        {
            CurrentQuery += $"SELECT SUM({column}) ";
            return this;
        }
        /// <summary>
        /// Update composer
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public ISimpleQueryComposer Update(string table)
        {
            CurrentQuery += $"UPDATE {table} SET ";
            return this;
        }
        /// <summary>
        /// Values composer
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public ISimpleQueryComposer Values(params string[] columns)
        {
            CurrentQuery += $" VALUES ({columns.Aggregate((curr, next) => curr + "," + next)}) ";
            return this;
        }
        /// <summary>
        /// Where composer
        /// </summary>
        /// <param name="column"></param>
        /// <param name="op"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ISimpleQueryComposer Where(string column, string op, string value)
        {
            CurrentQuery += $" WHERE {column} {op} {value} ";
            return this;

        }
        /// <summary>
        /// Single where composer
        /// </summary>
        /// <returns></returns>
        public ISimpleQueryComposer Where()
        {
            CurrentQuery += $" WHERE ";
            return this;
        }

        /// <summary>
        /// Implicit cast to string
        /// </summary>
        /// <param name="composer"></param>
        public static implicit operator string(MySQLSimpleQueryComposer composer) => composer.CurrentQuery;
    }

    /// <summary>
    /// Composer
    /// </summary>
    public class QueryComposer : IQueryComposer
    {
        /// <summary>
        /// Current composed query
        /// </summary>
        public string CurrentQuery { get; protected set; } = string.Empty;
        /// <summary>
        /// Reset
        /// </summary>
        public void Reset() => CurrentQuery = string.Empty;
        /// <summary>
        /// Get query and clean
        /// </summary>
        public string GetAndCleanQuery { get { var temp = CurrentQuery; CurrentQuery = string.Empty; return temp; } }
        /// <summary>
        /// Get composed string
        /// </summary>
        /// <returns></returns>
        public string ComposedQuery() => CurrentQuery;
        /// <summary>
        /// APpend composers
        /// </summary>
        /// <param name="compose"></param>
        /// <returns></returns>
        public IQueryComposer Compose(IQueryComposer compose)
        {
            CurrentQuery += compose.ComposedQuery();
            return this;
        }
        /// <summary>
        /// Set a query (use when constructor can't be used)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public T SetQuery<T>(string query) where T : class, IQueryComposer
        {
            CurrentQuery += query;
            return this as T;
        }
        /// <summary>
        /// Append column
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="column"></param>
        /// <returns></returns>
        public T Column<T>(string column) where T : class, IQueryComposer
        {
            CurrentQuery += " " + column + " ";
            return this as T;
        }
        protected QueryComposer()
        {

        }
    }
    /// <summary>
    /// Root composer
    /// </summary>
    public class RootComposer : QueryComposer, IRootComposer
    {
        /// <summary>
        /// Delete composer
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public IDeleteComposer Delete(string table)
        {
            CurrentQuery += $"DELETE FROM {table} ";
            return new DeleteComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Insert composer
        /// </summary>
        /// <param name="table"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public IInsertComposer Insert(string table, params string[] columns)
        {
            CurrentQuery += $"INSERT INTO {table} ({columns.Aggregate((curr, next) => curr + "," + next)}) ";
            return new InsertComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Select composer
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public ISelectComposer Select(params string[] columns)
        {
            string cols = "*";
            cols = columns.Length <= 0 ? "*" : columns.Aggregate((curr, next) => curr + "," + next);
            CurrentQuery += $"SELECT {cols} ";
            return new SelectComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Select avg composer
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public ISelectComposer SelectAvg(string column)
        {
            CurrentQuery += $"SELECT AVG({column}) ";
            return new SelectComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Select count composer
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public ISelectComposer SelectCount(params string[] columns)
        {
            string cols = "*";
            cols = columns.Length <= 0 ? "*" : columns.Aggregate((curr, next) => curr + "," + next);
            CurrentQuery += $"SELECT COUNT({cols}) ";
            return new SelectComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Select distinct composer
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public ISelectComposer SelectDistinct(params string[] columns)
        {
            string cols = null;
            cols = columns.Length <= 0 ? "*" : columns.Aggregate((curr, next) => curr + "," + next);
            CurrentQuery += $"SELECT DISTINCT {cols} ";
            return new SelectComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Select max composer
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public ISelectComposer SelectMax(string column)
        {
            CurrentQuery += $"SELECT MAX({column}) ";
            return new SelectComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Select min composer
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public ISelectComposer SelectMin(string column)
        {
            CurrentQuery += $"SELECT MIN({column}) ";
            return new SelectComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Select sum composer
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public ISelectComposer SelectSum(string column)
        {
            CurrentQuery += $"SELECT SUM({column}) ";
            return new SelectComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Update composer
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public IUpdateComposer Update(string table)
        {
            CurrentQuery += $"UPDATE {table} SET ";
            return new UpdateComposer(GetAndCleanQuery);
        }
    }
    /// <summary>
    /// Update composer
    /// </summary>
    public class UpdateComposer : QueryComposer, IUpdateComposer
    {
        public UpdateComposer() { }
        /// <summary>
        /// Composer
        /// </summary>
        /// <param name="query"></param>
        public UpdateComposer(string query) => CurrentQuery += query;
        /// <summary>
        /// Condition 
        /// </summary>
        /// <param name="column"></param>
        /// <param name="op"></param>
        /// <param name="value"></param>
        /// <param name="comma"></param>
        /// <returns></returns>
        public IConditionComposer Condition(string column, string op, string value, string comma = "")
        {
            CurrentQuery += $" {column} {op} {value} {comma}";
            return new ConditionComposer(GetAndCleanQuery);
        }
    }
    /// <summary>
    /// On composer
    /// </summary>
    public class OnComposer : QueryComposer, IOnComposer
    {
        public OnComposer() { }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="query"></param>
        public OnComposer(string query) => CurrentQuery += query;
        /// <summary>
        /// Order by composer
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public IOrderByComposer OrderBy(params string[] columns)
        {
            CurrentQuery += $" ORDER BY {columns.Aggregate((curr, next) => curr + "," + next)} ";
            return new OrderByComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Order by desc composer
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public IOrderByComposer OrderByDesc(params string[] columns)
        {
            CurrentQuery += $" ORDER BY {columns.Aggregate((curr, next) => curr + "," + next)} DESC";
            return new OrderByComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Where composer
        /// </summary>
        /// <param name="column"></param>
        /// <param name="op"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IWhereComposer Where(string column, string op, string value)
        {
            CurrentQuery += $" WHERE {column} {op} {value} ";
            return new WhereComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Single where composer
        /// </summary>
        /// <returns></returns>
        public IWhereComposer Where()
        {
            CurrentQuery += " WHERE ";
            return new WhereComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Brackets composer
        /// </summary>
        /// <typeparam name="T">Target composer</typeparam>
        /// <param name="innerComposer"></param>
        /// <returns></returns>
        public T Brackets<T>(IQueryComposer innerComposer) where T : class, IQueryComposer, new()
        {
            CurrentQuery += $" ({innerComposer.ComposedQuery()}) ";
            return new T().SetQuery<T>(GetAndCleanQuery);
        }
        /// <summary>
        /// Group by composer
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public IGroupByComposer GroupBy(params string[] columns)
        {
            CurrentQuery += $" GROUP BY {columns.Aggregate((curr, next) => curr + "," + next)} DESC";
            return new GroupByComposer(GetAndCleanQuery);
        }
    }
    /// <summary>
    /// Condition composer
    /// </summary>
    public class ConditionComposer : QueryComposer, IConditionComposer
    {
        public ConditionComposer() { }
        /// <summary>
        /// Constructo
        /// </summary>
        /// <param name="query"></param>
        public ConditionComposer(string query) => CurrentQuery += query;
        /// <summary>
        /// Single and composer
        /// </summary>
        /// <returns></returns>
        public IAndComposer And()
        {
            CurrentQuery += " AND ";
            return new AndComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// And composer
        /// </summary>
        /// <param name="column"></param>
        /// <param name="op"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IAndComposer And(string column, string op, string value)
        {
            CurrentQuery += $" AND {column} {op} {value} ";
            return new AndComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Single Or composer
        /// </summary>
        /// <returns></returns>
        public IOrComposer Or()
        {
            CurrentQuery += " OR ";
            return new OrComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Or composer
        /// </summary>
        /// <param name="column"></param>
        /// <param name="op"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IOrComposer Or(string column, string op, string value)
        {
            CurrentQuery += $" OR {column} {op} {value} ";
            return new OrComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Where composer
        /// </summary>
        /// <param name="column"></param>
        /// <param name="op"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IWhereComposer Where(string column, string op, string value)
        {
            CurrentQuery += $" WHERE {column} {op} {value} ";
            return new WhereComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Single Where composer
        /// </summary>
        /// <returns></returns>
        public IWhereComposer Where()
        {
            CurrentQuery += " WHERE ";
            return new WhereComposer(GetAndCleanQuery);
        }
    }
    /// <summary>
    /// Select composer
    /// </summary>
    public class SelectComposer : QueryComposer, ISelectComposer
    {
        public SelectComposer() { }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="query"></param>
        public SelectComposer(string query)
        {
            CurrentQuery += query;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public IFromComposer From(string table, string alias = null)
        {
            if (!alias.IsNullOrEmpty())
                alias = $" AS {alias}";
            CurrentQuery += $" FROM ({table}){alias} ";
            return new FromComposer(GetAndCleanQuery);
        }
    }
    /// <summary>
    /// From composer
    /// </summary>
    public class FromComposer : QueryComposer, IFromComposer
    {
        public FromComposer() { }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="query"></param>
        public FromComposer(string query) => CurrentQuery += query;
        /// <summary>
        /// Brackets
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="innerComposer"></param>
        /// <returns></returns>
        public T Brackets<T>(IQueryComposer innerComposer) where T : class, IQueryComposer, new()
        {
            CurrentQuery += $" ({innerComposer.ComposedQuery()}) ";
            return new T().SetQuery<T>(GetAndCleanQuery);
        }
        /// <summary>
        /// Group by
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public IGroupByComposer GroupBy(params string[] columns)
        {
            CurrentQuery += $" GROUP BY {columns.Aggregate((curr, next) => curr + "," + next)} DESC";
            return new GroupByComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Join composition
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public IJoinComposer Join(string table, string alias = null)
        {
            if (!alias.IsNullOrEmpty())
                alias = $" AS {alias}";
            CurrentQuery += $" INNER JOIN ({table}){alias} ";
            return new JoinComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Left join composition
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public IJoinComposer LJoin(string table, string alias = null)
        {
            if (!alias.IsNullOrEmpty())
                alias = $" AS {alias}";
            CurrentQuery += $" LEFT JOIN ({table}){alias} ";
            return new JoinComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Order by 
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public IOrderByComposer OrderBy(params string[] columns)
        {
            CurrentQuery += $" ORDER BY {columns.Aggregate((curr, next) => curr + "," + next)} ";
            return new OrderByComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Order by desc
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public IOrderByComposer OrderByDesc(params string[] columns)
        {
            CurrentQuery += $" ORDER BY {columns.Aggregate((curr, next) => curr + "," + next)} DESC";
            return new OrderByComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Right join composer
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public IJoinComposer RJoin(string table, string alias = null)
        {
            if (!alias.IsNullOrEmpty())
                alias = $" AS {alias}";
            CurrentQuery += $" RIGHT JOIN ({table}){alias} ";
            return new JoinComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Where composer
        /// </summary>
        /// <param name="column"></param>
        /// <param name="op"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IWhereComposer Where(string column, string op, string value)
        {
            CurrentQuery += $" WHERE {column} {op} {value} ";
            return new WhereComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Single Where composer
        /// </summary>
        /// <returns></returns>
        public IWhereComposer Where()
        {
            CurrentQuery += " WHERE ";
            return new WhereComposer(GetAndCleanQuery);
        }
    }
    /// <summary>
    /// And composer
    /// </summary>
    public class AndComposer : QueryComposer, IAndComposer
    {
        public AndComposer() { }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="query"></param>
        public AndComposer(string query) => CurrentQuery += query;
        /// <summary>
        /// Between composer
        /// </summary>
        /// <param name="column"></param>
        /// <param name="before"></param>
        /// <param name="after"></param>
        /// <returns></returns>
        public IBetweenComposer Between(string column, string before, string after)
        {
            CurrentQuery += $" {column} BETWEEN {before} AND {after} ";
            return new BetweenComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Condition composer
        /// </summary>
        /// <param name="column"></param>
        /// <param name="op"></param>
        /// <param name="value"></param>
        /// <param name="comma"></param>
        /// <returns></returns>
        public IConditionComposer Condition(string column, string op, string value, string comma = "")
        {
            CurrentQuery += $" {column} {op} {value} {comma}";
            return new ConditionComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// In composer
        /// </summary>
        /// <param name="innerComposer"></param>
        /// <returns></returns>
        public IInComposer In(IQueryComposer innerComposer)
        {
            CurrentQuery += $" IN ({innerComposer.ComposedQuery()}) ";
            return new InComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Like composer
        /// </summary>
        /// <param name="column"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public ILikeComposer Like(string column, string pattern)
        {
            CurrentQuery += $" {column} LIKE '{pattern}'";
            return new LikeComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Not composer
        /// </summary>
        /// <returns></returns>
        public INotComposer Not()
        {
            CurrentQuery += " NOT ";
            return new NotComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Not composer
        /// </summary>
        /// <param name="column"></param>
        /// <param name="op"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public INotComposer Not(string column, string op, string value)
        {
            CurrentQuery += $" NOT {column} {op} {value} ";
            return new NotComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Brackets composer
        /// </summary>
        /// <typeparam name="T">Target composer</typeparam>
        /// <param name="innerComposer"></param>
        /// <returns></returns>
        public T Brackets<T>(IQueryComposer innerComposer) where T : class, IQueryComposer, new()
        {
            CurrentQuery += $" ({innerComposer.ComposedQuery()}) ";
            return new T().SetQuery<T>(GetAndCleanQuery);
        }
        /// <summary>
        /// Single or composer
        /// </summary>
        /// <returns></returns>
        public IOrComposer Or()
        {
            CurrentQuery += " OR ";
            return new OrComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Or composer
        /// </summary>
        /// <param name="column"></param>
        /// <param name="op"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IOrComposer Or(string column, string op, string value)
        {
            CurrentQuery += $" OR {column} {op} {value} ";
            return new OrComposer(GetAndCleanQuery);
        }

        /// <summary>
        /// Single and composer
        /// </summary>
        /// <returns></returns>
        public IAndComposer And()
        {
            CurrentQuery += " AND ";
            return new AndComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// And composer
        /// </summary>
        /// <param name="column"></param>
        /// <param name="op"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IAndComposer And(string column, string op, string value)
        {
            CurrentQuery += $" AND {column} {op} {value} ";
            return new AndComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Group by composer
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public IGroupByComposer GroupBy(params string[] columns)
        {
            CurrentQuery += $" GROUP BY {columns.Aggregate((curr, next) => curr + "," + next)} DESC";
            return new GroupByComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Order by composer
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public IOrderByComposer OrderBy(params string[] columns)
        {
            CurrentQuery += $" ORDER BY {columns.Aggregate((curr, next) => curr + "," + next)} ";
            return new OrderByComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Order by composer
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public IOrderByComposer OrderByDesc(params string[] columns)
        {
            CurrentQuery += $" ORDER BY {columns.Aggregate((curr, next) => curr + "," + next)} DESC";
            return new OrderByComposer(GetAndCleanQuery);
        }
    }
    /// <summary>
    /// Between composer
    /// </summary>
    public class BetweenComposer : QueryComposer, IBetweenComposer
    {
        public BetweenComposer() { }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="query"></param>
        public BetweenComposer(string query) => CurrentQuery += query;
        /// <summary>
        /// Single and composer
        /// </summary>
        /// <returns></returns>
        public IAndComposer And()
        {
            CurrentQuery += " AND ";
            return new AndComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// And composer
        /// </summary>
        /// <param name="column"></param>
        /// <param name="op"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IAndComposer And(string column, string op, string value)
        {
            CurrentQuery += $" AND {column} {op} {value} ";
            return new AndComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Single or composer
        /// </summary>
        /// <returns></returns>
        public IOrComposer Or()
        {
            CurrentQuery += " OR ";
            return new OrComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Or composer
        /// </summary>
        /// <param name="column"></param>
        /// <param name="op"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IOrComposer Or(string column, string op, string value)
        {
            CurrentQuery += $" OR {column} {op} {value} ";
            return new OrComposer(GetAndCleanQuery);
        }
    }
    /// <summary>
    /// Where composer
    /// </summary>
    public class WhereComposer : QueryComposer, IWhereComposer
    {
        public WhereComposer() { }
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="query"></param>
        public WhereComposer(string query)
        {
            CurrentQuery += query;
        }
        /// <summary>
        /// Single and composer
        /// </summary>
        /// <returns></returns>
        public IAndComposer And()
        {
            CurrentQuery += " AND ";
            return new AndComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// And composer
        /// </summary>
        /// <param name="column"></param>
        /// <param name="op"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IAndComposer And(string column, string op, string value)
        {
            CurrentQuery += $" AND {column} {op} {value} ";
            return new AndComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Between composer
        /// </summary>
        /// <param name="column"></param>
        /// <param name="before"></param>
        /// <param name="after"></param>
        /// <returns></returns>
        public IBetweenComposer Between(string column, string before, string after)
        {
            CurrentQuery += $" {column} BETWEEN {before} AND {after}";
            return new BetweenComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Brakets composer
        /// </summary>
        /// <typeparam name="T">Target composer</typeparam>
        /// <param name="innerComposer"></param>
        /// <returns></returns>
        public T Brackets<T>(IQueryComposer innerComposer) where T : class, IQueryComposer, new()
        {
            CurrentQuery += $" ({innerComposer.ComposedQuery()}) ";
            return new T().SetQuery<T>(GetAndCleanQuery);
        }
        /// <summary>
        /// Group by composer
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public IGroupByComposer GroupBy(params string[] columns)
        {
            CurrentQuery += $" GROUP BY {columns.Aggregate((curr, next) => curr + "," + next)} DESC";
            return new GroupByComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// In composer
        /// </summary>
        /// <param name="innerComposer"></param>
        /// <returns></returns>
        public IInComposer In(IQueryComposer innerComposer)
        {
            CurrentQuery += $" IN ({innerComposer.ComposedQuery()}) ";
            return new InComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Like composer
        /// </summary>
        /// <param name="column"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public ILikeComposer Like(string column, string pattern)
        {
            CurrentQuery += $" {column} LIKE '{pattern}'";
            return new LikeComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Single not composer
        /// </summary>
        /// <returns></returns>
        public INotComposer Not()
        {
            CurrentQuery += " NOT ";
            return new NotComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Single not composer
        /// </summary>
        /// <param name="column"></param>
        /// <param name="op"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public INotComposer Not(string column, string op, string value)
        {
            CurrentQuery += $" NOT {column} {op} {value} ";
            return new NotComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Single or composer
        /// </summary>
        /// <returns></returns>
        public IOrComposer Or()
        {
            CurrentQuery += " OR ";
            return new OrComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Or composer
        /// </summary>
        /// <param name="column"></param>
        /// <param name="op"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IOrComposer Or(string column, string op, string value)
        {
            CurrentQuery += $" OR {column} {op} {value} ";
            return new OrComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Order by composer
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public IOrderByComposer OrderBy(params string[] columns)
        {
            CurrentQuery += $" ORDER BY {columns.Aggregate((curr, next) => curr + "," + next)} ";
            return new OrderByComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Order by desc composer
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public IOrderByComposer OrderByDesc(params string[] columns)
        {
            CurrentQuery += $" ORDER BY {columns.Aggregate((curr, next) => curr + "," + next)} DESC";
            return new OrderByComposer(GetAndCleanQuery);
        }

    }
    /// <summary>
    /// Or composer
    /// </summary>
    public class OrComposer : QueryComposer, IOrComposer
    {
        public OrComposer() { }
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="query"></param>
        public OrComposer(string query) => CurrentQuery += query;
        /// <summary>
        /// Single and composer
        /// </summary>
        /// <returns></returns>
        public IAndComposer And()
        {
            CurrentQuery += " AND ";
            return new AndComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// And composer
        /// </summary>
        /// <param name="column"></param>
        /// <param name="op"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IAndComposer And(string column, string op, string value)
        {
            CurrentQuery += $" AND {column} {op} {value} ";
            return new AndComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Between composer
        /// </summary>
        /// <param name="column"></param>
        /// <param name="before"></param>
        /// <param name="after"></param>
        /// <returns></returns>
        public IBetweenComposer Between(string column, string before, string after)
        {
            CurrentQuery += $" {column} BETWEEN {before} AND {after}";
            return new BetweenComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Condition composer
        /// </summary>
        /// <param name="column"></param>
        /// <param name="op"></param>
        /// <param name="value"></param>
        /// <param name="comma"></param>
        /// <returns></returns>
        public IConditionComposer Condition(string column, string op, string value, string comma = "")
        {
            CurrentQuery += $" {column} {op} {value} {comma}";
            return new ConditionComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// In composer
        /// </summary>
        /// <param name="innerComposer"></param>
        /// <returns></returns>
        public IInComposer In(IQueryComposer innerComposer)
        {
            CurrentQuery += $" IN ({innerComposer.ComposedQuery()}) ";
            return new InComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Like composer
        /// </summary>
        /// <param name="column"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public ILikeComposer Like(string column, string pattern)
        {
            CurrentQuery += $" {column} LIKE '{pattern}'";
            return new LikeComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Single not composer
        /// </summary>
        /// <returns></returns>
        public INotComposer Not()
        {
            CurrentQuery += " NOT ";
            return new NotComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Not composer
        /// </summary>
        /// <param name="column"></param>
        /// <param name="op"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public INotComposer Not(string column, string op, string value)
        {
            CurrentQuery += $" NOT {column} {op} {value} ";
            return new NotComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Brackets composer
        /// </summary>
        /// <typeparam name="T">Target composer</typeparam>
        /// <param name="innerComposer"></param>
        /// <returns></returns>
        public T Brackets<T>(IQueryComposer innerComposer) where T : class, IQueryComposer, new()
        {
            CurrentQuery += $" ({innerComposer.ComposedQuery()}) ";
            return new T().SetQuery<T>(GetAndCleanQuery);
        }
        /// <summary>
        /// Single or composer
        /// </summary>
        /// <returns></returns>
        public IOrComposer Or()
        {
            CurrentQuery += " OR ";
            return new OrComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Or composer
        /// </summary>
        /// <param name="column"></param>
        /// <param name="op"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IOrComposer Or(string column, string op, string value)
        {
            CurrentQuery += $" OR {column} {op} {value} ";
            return new OrComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Group by composer
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public IGroupByComposer GroupBy(params string[] columns)
        {
            CurrentQuery += $" GROUP BY {columns.Aggregate((curr, next) => curr + "," + next)} DESC";
            return new GroupByComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Order by composer
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public IOrderByComposer OrderBy(params string[] columns)
        {
            CurrentQuery += $" ORDER BY {columns.Aggregate((curr, next) => curr + "," + next)} ";
            return new OrderByComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Order by composer
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public IOrderByComposer OrderByDesc(params string[] columns)
        {
            CurrentQuery += $" ORDER BY {columns.Aggregate((curr, next) => curr + "," + next)} DESC";
            return new OrderByComposer(GetAndCleanQuery);
        }
    }
    /// <summary>
    /// Like composer
    /// </summary>
    public class LikeComposer : QueryComposer, ILikeComposer
    {
        public LikeComposer() { }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="query"></param>
        public LikeComposer(string query) => CurrentQuery += query;
        /// <summary>
        /// And composer
        /// </summary>
        /// <returns></returns>
        public IAndComposer And()
        {
            CurrentQuery += " AND ";
            return new AndComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// And composer
        /// </summary>
        /// <param name="column"></param>
        /// <param name="op"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IAndComposer And(string column, string op, string value)
        {
            CurrentQuery += $" AND {column} {op} {value} ";
            return new AndComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Single or composer
        /// </summary>
        /// <returns></returns>
        public IOrComposer Or()
        {
            CurrentQuery += " OR ";
            return new OrComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Or composer
        /// </summary>
        /// <param name="column"></param>
        /// <param name="op"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IOrComposer Or(string column, string op, string value)
        {
            CurrentQuery += $" OR {column} {op} {value} ";
            return new OrComposer(GetAndCleanQuery);
        }
    }
    /// <summary>
    /// In composer
    /// </summary>
    public class InComposer : QueryComposer, IInComposer
    {
        public InComposer() { }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="query"></param>
        public InComposer(string query) => CurrentQuery += query;
        /// <summary>
        /// Single and composer
        /// </summary>
        /// <returns></returns>
        public IAndComposer And()
        {
            CurrentQuery += " AND ";
            return new AndComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// And composer
        /// </summary>
        /// <param name="column"></param>
        /// <param name="op"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IAndComposer And(string column, string op, string value)
        {
            CurrentQuery += $" AND {column} {op} {value} ";
            return new AndComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Single or composer
        /// </summary>
        /// <returns></returns>
        public IOrComposer Or()
        {
            CurrentQuery += " OR ";
            return new OrComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Or Composer
        /// </summary>
        /// <param name="column"></param>
        /// <param name="op"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IOrComposer Or(string column, string op, string value)
        {
            CurrentQuery += $" OR {column} {op} {value} ";
            return new OrComposer(GetAndCleanQuery);
        }
    }
    /// <summary>
    /// Not composer
    /// </summary>
    public class NotComposer : QueryComposer, INotComposer
    {
        public NotComposer() { }
        /// <summary>
        /// Not composer
        /// </summary>
        /// <param name="query"></param>
        public NotComposer(string query) => CurrentQuery += query;
        /// <summary>
        /// Single and composer
        /// </summary>
        /// <returns></returns>
        public IAndComposer And()
        {
            CurrentQuery += " AND ";
            return new AndComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// And composer
        /// </summary>
        /// <param name="column"></param>
        /// <param name="op"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IAndComposer And(string column, string op, string value)
        {
            CurrentQuery += $" AND {column} {op} {value} ";
            return new AndComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Between composer
        /// </summary>
        /// <param name="column"></param>
        /// <param name="before"></param>
        /// <param name="after"></param>
        /// <returns></returns>
        public IBetweenComposer Between(string column, string before, string after)
        {
            CurrentQuery += $" {column} BETWEEN {before} AND {after} ";
            return new BetweenComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Brackets composer
        /// </summary>
        /// <typeparam name="T">Target composer</typeparam>
        /// <param name="innerComposer"></param>
        /// <returns></returns>
        public T Brackets<T>(IQueryComposer innerComposer) where T : class, IQueryComposer, new()
        {
            CurrentQuery += $" ({innerComposer.ComposedQuery()}) ";
            return new T().SetQuery<T>(GetAndCleanQuery);
        }
        /// <summary>
        /// In composer
        /// </summary>
        /// <param name="innerComposer"></param>
        /// <returns></returns>
        public IInComposer In(IQueryComposer innerComposer)
        {
            CurrentQuery += $" IN ({innerComposer.ComposedQuery()}) ";
            return new InComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Like composer
        /// </summary>
        /// <param name="column"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public ILikeComposer Like(string column, string pattern)
        {
            CurrentQuery += $" {column} LIKE '{pattern}'";
            return new LikeComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Single or composer
        /// </summary>
        /// <returns></returns>
        public IOrComposer Or()
        {
            CurrentQuery += " OR ";
            return new OrComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Or composer
        /// </summary>
        /// <param name="column"></param>
        /// <param name="op"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IOrComposer Or(string column, string op, string value)
        {
            CurrentQuery += $" OR {column} {op} {value} ";
            return new OrComposer(GetAndCleanQuery);
        }
    }
    /// <summary>
    /// Insert composer
    /// </summary>
    public class InsertComposer : QueryComposer, IInsertComposer
    {
        public InsertComposer() { }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="query"></param>
        public InsertComposer(string query) => CurrentQuery += query;
        /// <summary>
        /// Select composer
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public ISelectComposer Select(params string[] columns)
        {
            string cols = "*";
            cols = columns.Length <= 0 ? "*" : columns.Aggregate((curr, next) => curr + "," + next);
            CurrentQuery += $"SELECT {cols} ";
            return new SelectComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Select distinct composer
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public ISelectComposer SelectDistinct(params string[] columns)
        {
            string cols = null;
            cols = columns.Length <= 0 ? "*" : columns.Aggregate((curr, next) => curr + "," + next);
            CurrentQuery += $"SELECT DISTINCT {cols} ";
            return new SelectComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Values composer
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public IValuesComposer Values(params string[] values)
        {
            CurrentQuery += $" VALUES ({values.Aggregate((curr, next) => curr + "," + next)}) ";
            return new ValuesComposer(GetAndCleanQuery);
        }
    }
    /// <summary>
    /// Values composer
    /// </summary>
    public class ValuesComposer : QueryComposer, IValuesComposer
    {
        public ValuesComposer() { }
        /// <summary>
        /// Values composer
        /// </summary>
        /// <param name="query"></param>
        public ValuesComposer(string query) => CurrentQuery += query;
    }
    /// <summary>
    /// OrderByComposer
    /// </summary>
    public class OrderByComposer : QueryComposer, IOrderByComposer
    {
        public OrderByComposer() { }
        /// <summary>
        /// Order by composer
        /// </summary>
        /// <param name="query"></param>
        public OrderByComposer(string query)
        {
            CurrentQuery += query;
        }
    }
    /// <summary>
    /// Group by composer
    /// </summary>
    public class GroupByComposer : QueryComposer, IGroupByComposer
    {
        public GroupByComposer() { }
        /// <summary>
        /// Group by composer
        /// </summary>
        /// <param name="query"></param>
        public GroupByComposer(string query)
        {
            CurrentQuery += query;
        }
        /// <summary>
        /// Order by composer
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public IOrderByComposer OrderBy(params string[] columns)
        {
            CurrentQuery += $" ORDER BY {columns.Aggregate((curr, next) => curr + "," + next)} ";
            return new OrderByComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Order by desc composer
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public IOrderByComposer OrderByDesc(params string[] columns)
        {
            CurrentQuery += $" ORDER BY {columns.Aggregate((curr, next) => curr + "," + next)} DESC";
            return new OrderByComposer(GetAndCleanQuery);
        }
    }
    /// <summary>
    /// Delete composer
    /// </summary>
    public class DeleteComposer : QueryComposer, IDeleteComposer
    {
        public DeleteComposer() { }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="query"></param>
        public DeleteComposer(string query)
        {
            CurrentQuery += query;
        }
        /// <summary>
        /// Where composer
        /// </summary>
        /// <param name="column"></param>
        /// <param name="op"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IWhereComposer Where(string column, string op, string value)
        {
            CurrentQuery += $" WHERE {column} {op} {value} ";
            return new WhereComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Single Where composer
        /// </summary>
        /// <returns></returns>
        public IWhereComposer Where()
        {
            CurrentQuery += " WHERE ";
            return new WhereComposer(GetAndCleanQuery);
        }
    }
    /// <summary>
    /// Join Composer
    /// </summary>
    public class JoinComposer : QueryComposer, IJoinComposer
    {
        public JoinComposer() { }
        /// <summary>
        /// Join composer constructor
        /// </summary>
        /// <param name="query"></param>
        public JoinComposer(string query) => CurrentQuery = query;
        /// <summary>
        /// Group by composer
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public IGroupByComposer GroupBy(params string[] columns)
        {
            CurrentQuery += $" GROUP BY {columns.Aggregate((curr, next) => curr + "," + next)} DESC";
            return new GroupByComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Order by composer
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public IOrderByComposer OrderBy(params string[] columns)
        {
            CurrentQuery += $" ORDER BY {columns.Aggregate((curr, next) => curr + "," + next)} ";
            return new OrderByComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Order by composer
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public IOrderByComposer OrderByDesc(params string[] columns)
        {
            CurrentQuery += $" ORDER BY {columns.Aggregate((curr, next) => curr + "," + next)} DESC";
            return new OrderByComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Where composer
        /// </summary>
        /// <param name="column"></param>
        /// <param name="op"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IWhereComposer Where(string column, string op, string value)
        {
            CurrentQuery += $" WHERE {column} {op} {value} ";
            return new WhereComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// Single where composer
        /// </summary>
        /// <returns></returns>
        public IWhereComposer Where()
        {
            CurrentQuery += " WHERE ";
            return new WhereComposer(GetAndCleanQuery);
        }
        /// <summary>
        /// On brackets
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="innerComposer"></param>
        /// <returns></returns>
        public T Brackets<T>(IQueryComposer innerComposer) where T : class, IQueryComposer, new()
        {
            CurrentQuery += $" ({innerComposer.ComposedQuery()}) ";
            return new T().SetQuery<T>(GetAndCleanQuery);
        }
        /// <summary>
        /// On composer
        /// </summary>
        /// <param name="columnA"></param>
        /// <param name="columnB"></param>
        /// <returns></returns>
        public IOnComposer On(string columnA, string columnB)
        {
            CurrentQuery += $" ON {columnA} = {columnB} ";
            return new OnComposer(GetAndCleanQuery);
        }
    }

}
