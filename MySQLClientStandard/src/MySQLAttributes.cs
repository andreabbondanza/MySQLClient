using System;

namespace DewCore.DewDatabase.MySQL
{
    /// <summary>
    /// Ignore this attribute when you are in the insert query
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class IgnoreInsert : Attribute
    {

    }
    /// <summary>
    /// This attribute must be checked in conditional phase for delete
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class CheckDelete : Attribute
    {

    }
    /// <summary>
    /// Ignore this property when you are in the set phase in update query
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class IgnoreUpdate : Attribute
    {

    }
    /// <summary>
    /// Ignore this property when you are in the conditional phase in update query
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class CheckUpdate : Attribute
    {

    }
    /// <summary>
    /// This property isn't on database table
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class NoColumn : Attribute
    {

    }
}
