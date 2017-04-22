# MySQLClient
A .net standard client for MySQL based on __MySQLConnector__

# How to use
Library examples

## MySqlConnectionString

## MySqlClient

### Correct use

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
public async Task ExampleDebug()
{
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
    }
}
```

## NuGet
You can find it on nuget with the name [DewMySQLClient](https://www.nuget.org/packages/DewMySQLClient/)

## About
[Andrea Vincenzo Abbondanza](http://www.andrewdev.eu)

## Donate
[Help me to grow up, if you want](https://payPal.me/andreabbondanza)
