# `EventHelper`

The `EventHelper` class provides methods for raising events in a safe and simple manner. Both generic and non-generic event delegates are supported.

## `Raise()`

The `Raise()` methods allow you to synchronously raise an event. The usage is simple:

```C#
public class MyClass
{
    public event EventHandler MyEvent;

    protected virtual void OnMyEvent(EventArgs e)
    {
        EventHelper.Raise(MyEvent, this, e);
    }
}
```

The call to `Raise()` raises the event in a thread-safe manner. If there are no listeners, the event will not be raised. Events that leverage the generic `EventHandler<T>` delegate are also supported: 

```C#
public class MyClass
{
    public event EventHandler<MyEventArgs> MyEvent;

    protected virtual void OnMyEvent(MyEventArgs e)
    {
        EventHelper.Raise(MyEvent, this, e);
    }
}
```

## `BeginRaise()`

The `BeginRaise()` methods can be used to asynchronously raise an event. Its use is very similar to `Raise()` except that some extra parameters may be provided for callback purposes: 

```C#
public class MyClass
{
    public event EventHandler MyEvent;

    protected virtual void OnMyEvent(EventArgs e)
    {
        EventHelper.BeginRaise(MyEvent, this, e, MyCallback, "state");
    }

    private void MyCallback(IAsyncResult result)
    {
        // this will display "state"
        MessageBox.Show(result.AsyncState);
    }
}
```

The callback is entirely optional. If you don't need to be notified when the event has been raised, just pass in `null` for the `callback` and `state` parameters:

```C#
protected virtual void OnMyEvent(EventArgs e)
{
    EventHelper.BeginRaise(MyEvent, this, e, null, null);
}
```

Just like with `Raise()` methods, there are `BeginRaise()` overloads to support the generic `EventHandler<T>` delegate.

## Lazy Event Argument Creation

Both the `Raise()` and `BeginRaise()` methods support overloads that take an instance of the `Func<T>` delegate instead of an instance of `EventArgs`. These overloads are intended for an esoteric use case. If all of the following is true, you should consider using these overloads when raising your event: 

* Your event is raised very frequently
* Your event rarely has listeners
* Your event data is a custom `EventArgs` instance
* The instance of the `Func<T>` delegate does not need to be recreated every time you raise the event

Even if all these points hold true, you should measure performance to prove that the extra complexity in your code base is warranted. For a complete example that that uses lazy event argument creation please see [here](lazy-event-creation-example.md).

## Extension Methods

There are several extension methods defined to make raising events even simpler and more intuitive: 

```C#
using Kent.Boogaart.HelperTrinity;

public class MyClass
{
    public event EventHandler<MyEventArgs> MyEvent;

    protected virtual void OnMyEvent(MyEventArgs e)
    {
        MyEvent.Raise(this, e);
    }
}
```

All the capabilities of the `EventHelper` class are exposed via extension methods.