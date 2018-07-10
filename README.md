# MySQLClient
A .net standard client for MySQL based on __MySQLConnector__

# Changelog

##### V 2.8.0

- Added Query Composer

# How to use
Library examples

## MySqlClient

### Correct use
__NOTE:__ You can also pass to MySQLClient constructor the default connectino string

```csharp
public async Task ExampleNormal()
{
	MySQLConnectionString cs = new MySQLConnectionString("myHost", "3306", "schema", "user", "pass");
	using (var client = new MySQLClient(cs))
	{
		// execute query here
	}
}
```

### Transactions

```csharp
public async Task ExampleTransaction()
{
	bool condition = true;
	MySQLConnectionString cs = new MySQLConnectionString("myHost", "3306", "schema", "user", "pass");
	using (var client = new MySQLClient(cs))
	{
		await client.BeginTransactionAsync();
		// do stuff
		if (condition)
			await client.CommitAsync();
		else
			await client.RoolbackAsync();
	}
}
```

### Close connection

MySQLClient close the connection when the object is disposed. However you can force the connection close calling the __CloseConnection__ method.
However the connection is opened automatically when you start a transaction or perform a query.

## Query types
We have three type of query for the library:
- ICollection<T>: return a Collection of T where T is a class with default constructor
- ICollection<Array>: return a Collection of an array of object
- IMySqlResponse: return an object used to get informations about insert\delete\update
### List\<T>
In this way you can get an ICollection of __table__ Users (where Users is a __class__ with the SAME properties)
```csharp
public async Task ExampleQueryList()
{
	MySQLConnectionString cs = new MySQLConnectionString("myHost", "3306", "schema", "user", "pass");
	using (var client = new MySQLClient(cs))
	{
		var result = client.QueryAsync<User>("SELECT * FROM Users");
		 Console.WriteLine(result.Count > 0 ? result.First().Name : "empty");
	}
}
```
### Array
In this way you can get an ICollection of array where every column is an index of the listed array
```csharp
public async Task ExampleQueryArray()
{
	MySQLConnectionString cs = new MySQLConnectionString("myHost", "3306", "schema", "user", "pass");
	using (var client = new MySQLClient(cs))
	{
		var result = await client.QueryArrayAsync("SELECT * FROM Users");
		Console.WriteLine(result.Count > 0 ? result.First()[0] : "empty");
	}
}
```
### Dictionary
In this way you can get an ICollection of array where every column is a collection of dictionary
```csharp
public async Task ExampleQueryArray()
{
	MySQLConnectionString cs = new MySQLConnectionString("myHost", "3306", "schema", "user", "pass");
	using (var client = new MySQLClient(cs))
	{
		var result = await client.QueryArrayAsync("SELECT * FROM Users");
		Console.WriteLine(result.Count > 0 ? result.First().First(x => x.Key == "Surname").Value : "empty");
		//or
		Console.WriteLine(result.Count > 0 ? result.First()["Surname"] : "empty");
	}
}
```
### IMySQLResponse 
Here we have an example of a query that doesn't return a resultset but an IMySQLResponse object

__NOTE:__ In this case we use the prepare statement with an args. See "Query with args" section.
```csharp
public async Task ExampleQueryWithResponse()
{
	long toDelete = 10;
	MySQLConnectionString cs = new MySQLConnectionString("myHost", "3306", "schema", "user", "pass");
	using (var client = new MySQLClient(cs))
	{
		var result = await client.QueryAsync($"DELETE FROM Users WHERE Id=@id", new List<DbParameter>() {
												new MySqlParameter(){ DbType = System.Data.DbType.Int64, Value=toDelete, ParameterName="@id"  }
											});
		Console.WriteLine(result.GetRowsAffected() > 0 ? "deleted" : "not deleted");
	}
}
```
### Query with args
When you get some args from web or other sources, you should prepare the statement before execute the query. With this library you can do it this way.

You just need to pass a __List__ of __DbParameter__, in specific a list of __MySqlParameter__ where in it you need to set at least:

- __Value__: the value of parameter
- __ParameterName__: the placeholder you have used in the query, ex. "@id"
- __DbType__: The database type (better for quickly cast)
```csharp
public async Task ExampleQueryWithArgs()
{
	MySQLConnectionString cs = new MySQLConnectionString("myHost", "3306", "schema", "user", "pass");
	using (var client = new MySQLClient(cs))
	{
		var name = "Andrew";
		var alive = true;
		var result1 = await client.QueryAsync<User>($"SELECT * Users WHERE Name=@name AND IsAlive=@alive", new List<DbParameter>() {
										new MySqlParameter(){ DbType = System.Data.DbType.String, Value=name, ParameterName="@name"  },
										new MySqlParameter(){ DbType = System.Data.DbType.Boolean, Value=alive, ParameterName="@alive"  }
									});
	}
}
```
## Simplified queries
When the query is simple, like a single insert or a simple select on one table, you can use the simplified queries
### Insert
An insert example

__NOTE:__ in this case the class name must be the same of the table, if your tables use a prefix, you can pass it like second argument to Insert
```csharp
public async Task ExampleInsert()
{
	MySQLConnectionString cs = new MySQLConnectionString("myHost", "3306", "schema", "user", "pass");
	using (var client = new MySQLClient(cs))
	{
		var result = client.InsertAsync<User>(new User() { Name = "Andrew", IsAlive = true });
	}
}
```
__IMPORTANT:__ You can also ignore fields for insert, like the id that many time is an autoincrement field.
 
You can do that with the "__Attribute mapping__". Check the section.
### Delete
A delete example

__NOTE:__ in this case the class name must be the same of the table, if your tables use a prefix, you can pass it like second argument to Delete
```csharp
public async Task ExampleDelete()
{
	MySQLConnectionString cs = new MySQLConnectionString("myHost", "3306", "schema", "user", "pass");
	using (var client = new MySQLClient(cs))
	{
		var result = client.DeleteAsync<User>(new User() { Id = 4});
	}
}
```
__IMPORTANT:__ You must set at least a "__CheckDelete__" property in the class definition.

You can do that with the "__Attribute mapping__". Check the section.
### Update
An Update example

__NOTE:__ in this case the class name must be the same of the table, if your tables use a prefix, you can pass it like second argument to Update
```csharp
public async Task ExampleUpdate()
{
	MySQLConnectionString cs = new MySQLConnectionString("myHost", "3306", "schema", "user", "pass");
	using (var client = new MySQLClient(cs))
	{
		var result = client.UpdateAsync<User>(new User() { Id = 5 }, new User() { Name = "Andrew", IsAlive = false });
	}
}
```
__IMPORTANT:__ You can ignore properties for update setting (for example the row id) and you need to set at least a property like __CheckUpdate__ to target the row\rows to update.

You can do that with the "__Attribute mapping__". Check the section.

### Select
There are two way to make simplified selects

__NOTE:__ in this case the class name must be the same of the table, if your tables use a prefix, you can pass it like second argument to Select
```csharp
public async Task ExampleUpdate()
{
	MySQLConnectionString cs = new MySQLConnectionString("myHost", "3306", "schema", "user", "pass");
	using (var client = new MySQLClient(cs))
	{
		var result = client.Select<User>();
	}
}
```
Or you can use a predicate to filter
```csharp
public async Task ExampleUpdate()
{
	MySQLConnectionString cs = new MySQLConnectionString("myHost", "3306", "schema", "user", "pass");
	using (var client = new MySQLClient(cs))
	{
		var result = client.Select<User>(x=> x.Name == "Andrew");
	}
}
```
## Other 
Other stuff

### Debug
You can enable debug (for example you can enable\disable it for development\production environment)
```csharp
public async Task ExampleDebug()
{
	if (env.IsDevelopment())
		MySQLClient.DebugOn = true;
	MySQLConnectionString cs = new MySQLConnectionString("myHost", "3306", "schema", "user", "pass");
	using (var client = new MySQLClient(cs))
	{
		var result = client.Select<User>(x=> x.Name == "Andrew");
	}
}
```
You can also set the log way. The library use the IDewLogger interface. 
If you don't want implement a new logger, you can use one of the built in logger:

- DewConsole: Print into a console (for console applications)
- DewDebug: Print into the visual studio debug window
- DewFileLog: Print the log into a file
- DewServerLog: Send the log like get request to a server

You can set the logger this way

```csharp
public async Task ExampleDebug()
{
	if (env.IsDevelopment())
	{
		MySQLClient.DebugOn = true;
		MySQLClient.SetDebugger(new DewConsole());
	}
	MySQLConnectionString cs = new MySQLConnectionString("myHost", "3306", "schema", "user", "pass");
	using (var client = new MySQLClient(cs))
	{
		var result = client.Select<User>(x => x.Name == "Andrew");
	}
}
```


### Exceptions

- NoTransactionException: when you commit\rollback without begin transaction

### Attributes for mapping
When you define a class to use the query with T object, you can also map the properties to use the calss with simplified queries.
Here an example:

```csharp

public class User
{
	[CheckDelete] //This property will be used to target the row in Delete function
	[IgnoreInsert]//This property will be ignored in the Insert function
	[CheckUpdate] //This property will be used to target the row in Update function
	public long Id { get; set; }
	public string Name { get; set; }
	public bool IsAlive { get; set; }
	[IgnoreInsert]//This property will be ignored in the Insert function
	[IgnoreUpdate]//This property will be ignored in the Update function
	public long IdTriggered { get; set; }
        [NoColumn] //This property will be always ignored
        public long FriendlyName {get => Name + (IsAlive ? "alive" : "dead"); }
}

```

## Query Composer

With 2.8.0 I've added the Query Composer, a way to compose a query without (or we trying :) ) syntax errors.

We have two type of it:

- SimpleQueryComposer
- ComplexQueryComposer

#### Simple query composer

The simple query composer let you compose the query with intellisense but without any type of control. For example you can call two times a select.

```csharp
public void Example()
{
    var c1 = new MySQLSimpleQueryComposer();
    string query = c1.Select().From("ExampleTable").OrderBy("IdExample").ComposedQuery();
    c1.Reset();
    string query1 = c1.Select("Name", "Surname").From("Users").Where("Age", ">", "18").And("Sex", "=", "M").OrderByDesc("Age").ComposedQuery();
    c1.Reset();
    string queryArgs = c1.Select("Name", "Surname").From("Users").Where("Age", ">", "@age").And("Sex", "=", "@sex").OrderByDesc("Age").ComposedQuery();
    c1.Reset();
    var c2 = new MySQLSimpleQueryComposer().Select("ExampleCode").From("ExampleCodesTable");
    string queryComposed = c1.Select().From("ExampleTable as A").Join("ExampleTable1 as B").On("A.Id", "B.IdExample").Where().Column("ExampleCode").Not().In(c2).ComposedQuery();
    c1.Reset();
    c1.From("Table").Select().And("Age", ">", "18");
    string wrongQuery = c1;
    Console.WriteLine(query);
    Console.WriteLine(query1);
    Console.WriteLine(queryArgs);
    Console.WriteLine(queryComposed);
    Console.WriteLine(wrongQuery);
}
```
The output is:
```sql
SELECT *  FROM ExampleTable  ORDER BY IdExample
SELECT Name,Surname  FROM Users  WHERE Age > 18  AND Sex = M  ORDER BY Age DESC
SELECT Name,Surname  FROM Users  WHERE Age > @age  AND Sex = @sex  ORDER BY Age DESC
SELECT *  FROM ExampleTable as A  INNER JOIN ExampleTable1 as B  ON A.Id = B.IdExample WHERE  ExampleCode  NOT  IN (SELECT ExampleCode  FROM ExampleCodesTable )
FROM Table SELECT *  AND Age > 18
```
As you can see, with __simplequerycomposer__ you don't have any type of control with __SQL__ syntax.

#### Complex query composer

The complex query composer will help you to prevent syntax errors because it use intellisense to lead you through the query composition.

Example:

```csharp
public void Example()
{
    RootComposer c = new RootComposer();
    string val = c.Select().From("dev_barb").Where().Column<WhereComposer>("idBarb").Not().In(new RootComposer().Select("idBarb").From("dev_payed").Where("Pay", ">", "0")).And("State", "=", "1").ComposedQuery();
    Console.WriteLine(val);
    val = c.Select().From("dev_barb as T").OrderByDesc("name", "surname").ComposedQuery();
    Console.WriteLine(val);
    val = c.Select().From("A as X").Join("B as Y").OrderBy("puop").ComposedQuery();
    Console.WriteLine(val);
    val = c.Select().From("A as X").Join("B as Y").On("X.IdUser", "Y.IdUtente").OrderBy("puop").ComposedQuery();
    Console.WriteLine(val);
}
```
Give output:
```sql
SELECT *  FROM dev_barb  WHERE idBarb NOT  IN (SELECT idBarb  FROM dev_payed  WHERE Pay > 0 )  AND State = 1
SELECT *  FROM dev_barb as T  ORDER BY name,surname DESC
SELECT *  FROM A as X  INNER JOIN B as Y  ORDER BY puop
SELECT *  FROM A as X  INNER JOIN B as Y  ON X.IdUser = Y.IdUtente  ORDER BY puop
```

The main difference is that you need to start from __RootComposer__ that let you to choose between starts object like _SELECT_,_INSERT_ etc.
Now the intellisense will lead you, for example the __SELECT__ composer object can only go to the __FROM__ composer object.

__NOTE__: This will not prevent at 100% the sql syntax errors, for example the __where__ goes in __groupby__, but if you use a __groupby__ after a __where__ in a __delete__ you'll get an error.

## NuGet
You can find it on nuget with the name [DewMySQLClient](https://www.nuget.org/packages/DewMySQLClient/)

## About
[Andrea Vincenzo Abbondanza](http://www.andrewdev.eu)

## Donate
[Help me to grow up, if you want](https://payPal.me/andreabbondanza)
