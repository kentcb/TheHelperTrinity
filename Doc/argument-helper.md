# `ArgumentHelper`

The `ArgumentHelper` class provides a number of methods for checking arguments. Any time an assertion fails, an `ArgumentNullException` or `ArgumentException` with an appropriate message will be thrown. 

## `AssertNotNull()`

The `AssertNotNull()` methods allow you to make sure an argument is non-`null`. It works for both reference and nullable types: 

```C#
public class MyClass
{
    public void MyMethod(string arg1, int? arg2)
    {
        ArgumentHelper.AssertNotNull(arg1, "arg1");
        ArgumentHelper.AssertNotNull(arg2, "arg2");
    }
}
```

There is also an overload of `AssertNotNull` that allows you to assert that any enumerable argument is non-`null`, and optionally check each item in the enumeration for `null`: 

```C#
public class MyClass
{
    public void MyMethod(IList<>string> arg)
    {
        // this will throw if arg is null, or if any item in the enumeration is null
        ArgumentHelper.AssertNotNull(arg, "arg", true);
    }
}
```

## `AssertGenericArgumentNotNull()`

The `AssertGenericArgumentNotNull()` method can be used in the special case where you have an unconstrained generic parameter that you want to ensure is non-`null`. Since the developer can not know whether the generic parameter will be a reference, value, or nullable type, this method is very useful:

```C#
public class MyClass<T>
{
    public void MyMethod<T>(T arg)
    {
        // this will work regardless of type T
        ArgumentHelper.AssertGenericArgumentNotNull(arg, "arg");
    }
}
```

In the case where type `T` above is a reference or nullable type, the call will work exactly the same as the `AssertNotNull()` equivalents. In the case where `T` is a value type, the call will be a no-op.

## `AssertNotNullOrEmpty()`

The `AssertNotNullOrEmpty()` methods can be used against strings, enumerables, or collections. They allow you to ensure that the argument is non-`null` and that it is not empty. 

```C#
public class MyClass
{
    public void MyMethod(string arg1, IList<int> arg2)
    {
        // this will throw if arg1 is null or "" but not if it is " "
        ArgumentHelper.AssertNotNullOrEmpty(arg1, "arg1");

        // this will throw if arg2 is null or does not contain at least one integer
        ArgumentHelper.AssertNotNullOrEmpty(arg2, "arg2");
    }
}
```

## `AssertNotNullOrWhiteSpace()`

The `AssertNotNullOrWhiteSpace()` method can be used against strings only. It allows you to ensure that a `string` is non-`null`, non-empty, and does not consist entirely of white-space characters.

```C#
public class MyClass
{
    public void MyMethod(string arg1)
    {
        // this will throw if arg1 is null, "", "  ", or "  \t\r\n", but not if it's "  \t\r\n foo"
        ArgumentHelper.AssertNotNullOrWhiteSpace(arg1, "arg1");
    }
}
```

## `AssertEnumMember()`

The `AssertEnumMember()` methods allow you to check enumeration arguments for correctness. They work with both flags and non-flags enumerations. The basic usage is as follows: 

```C#
public enum MyEnum
{
    One,
    Two,
    Three
}

public class MyClass
{
    public void MyMethod(MyEnum arg)
    {
        // this will ensure that arg is a valid member of the MyEnum enumeration
        ArgumentHelper.AssertEnumMember(arg, "arg");
    }
}
```

This is useful because calling `MyMethod()` like this will fail at runtime: 

```C#
new MyClass().MyMethod((MyEnum)3);
```

Without the call to `AssertEnumMember()`, `MyMethod()` would attempt to use the invalid value of `3`. 

The above call to `AssertEnumMember()` uses `Enum.GetValues()` to determine the valid values for the enumeration. This may not be desirable for various reasons, including efficiency and correctness. `Enum.GetValue()` does have a performance penalty that may be significant in tight code loops, for example. And it might not make sense for your API to accept any value in the enumeration - you may want to only accept some values. To solve these problems, there is an overload of `AssertEnumMember()` that allows you to provide specific enumeration values that should be accepted: 

```C#
public enum MyEnum
{
    One,
    Two,
    Three
}

public class MyClass
{
    private static readonly MyEnum[] _validValues = new MyEnum[] { MyEnum.One, MyEnum.Two };

    public void MyMethod(MyEnum arg)
    {
        // this will ensure that arg is either MyEnum.One or MyEnum.Two 
        // any other value (including MyEnum.Three) will result in an exception
        ArgumentHelper.AssertEnumMember(arg, "arg", _validValues);
    }
}
```

Using flag enumerations with these methods is also very simple and intuitive. Any valid combination of the flags you provide will pass the assertion.

```C#
[Flags]
public enum MyFlags
{
    One,
    Two,
    Three
}

public class MyClass
{
    private static readonly MyFlags[] _validValues = new MyFlags[] { MyFlags.One, MyFlags.Two };

    public void MyMethod(MyFlags arg)
    {
        // this will throw for MyFlags.Three or (MyFlags.One | MyFlags.Three) 
        // but it will work for MyFlags.One or (MyFlags.One | MyFlags.Two)
        ArgumentHelper.AssertEnumMember(arg, "arg");
    }
}
```

## Extension Methods

There are several extension methods defined to make argument checking even simpler and more intuitive: 

```C#
using Kent.Boogaart.HelperTrinity;

public class MyClass
{
    public void MyMethod(User arg1, string arg2)
    {
        arg1.AssertNotNull("arg1");
        arg2.AssertNotNullOrEmpty("arg2", true);
    }
}
```

All the capabilities of the `ArgumentHelper` class are exposed via extension methods.