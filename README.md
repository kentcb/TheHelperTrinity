![Logo](Art/Logo150x150.png "Logo")

# The Helper Trinity

## What?

**The Helper Trinity** is a set of helper classes applicable to most any .NET application. Using the classes in this library, you can much more easily validate arguments, raise events, and throw exceptions.

## Why?

Writing code to check arguments, raise events, and throw exceptions is mundane and error-prone. **The Helper Trinity** alleviates you of the need to write such code over and over again.

## Where?

The easiest way to get **The Helper Trinity** is to install via NuGet:

```
Install-Package HelperTrinity
```

## How?

```C#
public string GetContent(Uri uri, ContentFormat format)
{
    // validate arguments
    uri.AssertNotNull("uri");
    format.AssertEnumMember("format");

    // validate environment
    _exceptionHelper.ResolveAndThrowIf(!ConnectedToInternet, "NotConnected");

    // raise an event
    GettingContent.Raise(this, EventArgs.Empty);

    try
    {
        // get and return the content
        ...
    }
    catch (WebException ex)
    {
        // throw a more appropriate exception
        throw _exceptionHelper.Resolve("InvalidState", ex);
    }
} 

public event EventHandler<EventArgs> GettingContent;

private static readonly ExceptionHelper _exceptionHelper = new ExceptionHelper(typeof(MyType));
```

Please see [the documentation](Doc/overview.md) for more details.

## Who?

**The Helper Trinity** is created and maintained by [Kent Boogaart](http://kent-boogaart.com). Issues and pull requests are welcome.

## Primary Features

* Easily check arguments and throw the appropriate exception type where expectations are not met
* Easily raise events in a thread-safe manner
* Easily throw exceptions and maintain exception messages in a centralized location
* Includes extension methods to make argument checking and event raising even simpler
* Support for checking generic arguments
* Built with performance in mind
* Extensively unit tested
* Portable Class Library that targets many platforms