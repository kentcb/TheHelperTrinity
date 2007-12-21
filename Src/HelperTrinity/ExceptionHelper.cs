//the original source for this code is an online article available at http://www.codeproject.com/csharp/thehelpertrinity.asp

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Xml;

namespace Kent.Boogaart.HelperTrinity
{
	/// <summary>
	/// Provides helper methods for raising exceptions.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The <c>ExceptionHelper</c> class provides a centralised mechanism for throwing exceptions. This helps to keep exception
	/// messages and types consistent.
	/// </para>
	/// <para>
	/// Exception information is stored in an embedded resource called <c>ExceptionHelper.xml</c>, which must reside in the
	/// <c>Properties</c> namespace for the calling assembly. For example, if the root namespace for an assembly is
	/// <c>Company.Product</c> then the exception information must be stored in a resource called
	/// <c>Company.Product.Properties.ExceptionHelper.xml</c>
	/// </para>
	/// <para>
	/// The format for the exception information XML includes a grouping mechanism such that exception keys are scoped to the
	/// type throwing the exception. Thus, different types can use the same exception key because they have different scopes in
	/// the XML structure. An example of the format for the exception XML can be seen below.
	/// </para>
	/// <note type="implementation">
	/// This class is designed to be efficient in the common case (ie. no exception thrown) but is quite inefficient if an
	/// exception is actually thrown. This is not considered a problem, however, since an exception usually indicates that
	/// execution cannot reliably continue.
	/// </note>
	/// </remarks>
	/// <example>
	/// The following example shows how an exception can be conditionally thrown:
	/// <code>
	/// ExceptionHelper.ThrowIf(foo == null, "myKey", "hello");
	/// </code>
	/// Assuming this code resides in a class called <c>Foo.Bar</c>, the XML configuration might look like this:
	/// <code>
	/// <![CDATA[
	/// <?xml version="1.0" encoding="utf-8" ?> 
	/// 
	/// <exceptionHelper>
	/// 	<exceptionGroup type="Foo.Bar">
	/// 		<exception key="myKey" type="System.NullReferenceException">
	/// 			Foo is null but I'll say '{0}' anyway.
	/// 		</exception>
	/// 	</exceptionGroup>
	/// </exceptionHelper>
	/// ]]>
	/// </code>
	/// With this configuration, a <see cref="NullReferenceException"/> will be thrown if <c>foo</c> is <see langword="null"/>.
	/// The exception message will be "Foo is null but I'll say 'hello' anyway.".
	/// </example>
	public static class ExceptionHelper
	{
		/// <summary>
		/// Caches exception information for each participating assembly.
		/// </summary>
		private static IDictionary<Assembly, XmlDocument> _exceptionInfos = new Dictionary<Assembly, XmlDocument>();

		/// <summary>
		/// Synchronizes access to <see cref="_exceptionInfos"/>.
		/// </summary>
		private static object _exceptionInfosLock = new object();

		/// <summary>
		/// The name of the attribute that holds the exception type.
		/// </summary>
		private const string _typeAttributeName = "type";

		/// <summary>
		/// Conditionally throws an exception.
		/// </summary>
		/// <param name="condition">
		/// The condition.
		/// </param>
		/// <param name="exceptionKey">
		/// The exception key.
		/// </param>
		/// <param name="messageArgs">
		/// Arguments to the exception message.
		/// </param>
		[DebuggerHidden]
		public static void ThrowIf(bool condition, string exceptionKey, params object[] messageArgs)
		{
			ThrowIf(condition, exceptionKey, null, null, messageArgs);
		}

		/// <summary>
		/// Conditionally throws an exception.
		/// </summary>
		/// <param name="condition">
		/// The condition.
		/// </param>
		/// <param name="exceptionKey">
		/// The exception key.
		/// </param>
		/// <param name="innerException">
		/// The inner exception - the cause of the new exception.
		/// </param>
		/// <param name="messageArgs">
		/// Arguments to the exception message.
		/// </param>
		[DebuggerHidden]
		public static void ThrowIf(bool condition, string exceptionKey, Exception innerException, params object[] messageArgs)
		{
			ThrowIf(condition, exceptionKey, null, innerException, messageArgs);
		}

		/// <summary>
		/// Conditionally throws an exception.
		/// </summary>
		/// <param name="condition">
		/// The condition.
		/// </param>
		/// <param name="exceptionKey">
		/// The exception key.
		/// </param>
		/// <param name="constructorArgs">
		/// Additional arguments for the exception constructor.
		/// </param>
		/// <param name="innerException">
		/// The inner exception - the cause of the new exception.
		/// </param>
		[DebuggerHidden]
		public static void ThrowIf(bool condition, string exceptionKey, object[] constructorArgs, Exception innerException)
		{
			ThrowIf(condition, exceptionKey, constructorArgs, innerException, (object[]) null);
		}

		/// <summary>
		/// Conditionally throws an exception.
		/// </summary>
		/// <param name="condition">
		/// The condition.
		/// </param>
		/// <param name="exceptionKey">
		/// The exception key.
		/// </param>
		/// <param name="constructorArgs">
		/// Additional arguments for the exception constructor.
		/// </param>
		/// <param name="messageArgs">
		/// Arguments to the exception message.
		/// </param>
		[DebuggerHidden]
		public static void ThrowIf(bool condition, string exceptionKey, object[] constructorArgs, params object[] messageArgs)
		{
			ThrowIf(condition, exceptionKey, constructorArgs, null, messageArgs);
		}

		/// <summary>
		/// Conditionally throws an exception.
		/// </summary>
		/// <param name="condition">
		/// The condition.
		/// </param>
		/// <param name="exceptionKey">
		/// The exception key.
		/// </param>
		/// <param name="constructorArgs">
		/// Additional arguments for the exception constructor.
		/// </param>
		/// <param name="messageArgs">
		/// Arguments to the exception message.
		/// </param>
		/// <param name="innerException">
		/// The inner exception - the cause of the new exception.
		/// </param>
		[DebuggerHidden]
		public static void ThrowIf(bool condition, string exceptionKey, object[] constructorArgs, Exception innerException, params object[] messageArgs)
		{
			if (condition)
			{
				Throw(exceptionKey, constructorArgs, innerException, messageArgs);
			}
		}

		/// <summary>
		/// Throws an exception.
		/// </summary>
		/// <param name="exceptionKey">
		/// The exception key.
		/// </param>
		/// <param name="messageArgs">
		/// Arguments to the exception message.
		/// </param>
		[DebuggerHidden]
		public static void Throw(string exceptionKey, params object[] messageArgs)
		{
			Throw(exceptionKey, null, null, messageArgs);
		}

		/// <summary>
		/// Throws an exception.
		/// </summary>
		/// <param name="exceptionKey">
		/// The exception key.
		/// </param>
		/// <param name="messageArgs">
		/// Arguments to the exception message.
		/// </param>
		/// <param name="innerException">
		/// The inner exception - the cause of the new exception.
		/// </param>
		[DebuggerHidden]
		public static void Throw(string exceptionKey, Exception innerException, params object[] messageArgs)
		{
			Throw(exceptionKey, null, innerException, messageArgs);
		}

		/// <summary>
		/// Throws an exception.
		/// </summary>
		/// <param name="exceptionKey">
		/// The exception key.
		/// </param>
		/// <param name="constructorArgs">
		/// Additional arguments for the exception constructor.
		/// </param>
		/// <param name="innerException">
		/// The inner exception - the cause of the new exception.
		/// </param>
		[DebuggerHidden]
		public static void Throw(string exceptionKey, object[] constructorArgs, Exception innerException)
		{
			Throw(exceptionKey, constructorArgs, innerException, null);
		}

		/// <summary>
		/// Throws an exception.
		/// </summary>
		/// <param name="exceptionKey">
		/// The exception key.
		/// </param>
		/// <param name="constructorArgs">
		/// Additional arguments for the exception constructor.
		/// </param>
		/// <param name="messageArgs">
		/// Arguments to the exception message.
		/// </param>
		[DebuggerHidden]
		public static void Throw(string exceptionKey, object[] constructorArgs, params object[] messageArgs)
		{
			Throw(exceptionKey, constructorArgs, null, messageArgs);
		}

		/// <summary>
		/// Throws an exception.
		/// </summary>
		/// <param name="exceptionKey">
		/// The exception key.
		/// </param>
		/// <param name="constructorArgs">
		/// Additional arguments for the exception constructor.
		/// </param>
		/// <param name="messageArgs">
		/// Arguments to the exception message.
		/// </param>
		/// <param name="innerException">
		/// The inner exception - the cause of the new exception.
		/// </param>
		/// <exception cref="InvalidOperationException">
		/// If any problem occurs locating the details of the exception to throw, or in constructing the exception to throw.
		/// </exception>
		/// <exception cref="Exception">
		/// This method always throws an exception. The exact type of the exception depends on the configuration.
		/// </exception>
		[DebuggerHidden]
		public static void Throw(string exceptionKey, object[] constructorArgs, Exception innerException, params object[] messageArgs)
		{
			ArgumentHelper.AssertNotNull(exceptionKey, "exceptionKey");

			//first we need to find the type from which we were invoked - this is used as a grouping mechanism in the XML config file
			Type invokingType = null;
			int skipFrames = 1;

			while (true)
			{
				StackFrame stackFrame = new StackFrame(skipFrames);
				Debug.Assert(stackFrame.GetMethod() != null);

				if (stackFrame.GetMethod().DeclaringType != typeof(ExceptionHelper))
				{
					invokingType = stackFrame.GetMethod().DeclaringType;
					break;
				}

				++skipFrames;
			}

			XmlDocument exceptionInfo = GetExceptionInfo(invokingType.Assembly);

			string xpath = string.Concat("/exceptionHelper/exceptionGroup[@type=\"", invokingType.FullName, "\"]/exception[@key=\"", exceptionKey, "\"]");
			XmlNode exceptionNode = exceptionInfo.SelectSingleNode(xpath);

			if (exceptionNode == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The exception details for key '{0}' could not be found (they should be under '{1}')", exceptionKey, xpath));
			}

			XmlAttribute typeAttribute = exceptionNode.Attributes[_typeAttributeName];

			if (typeAttribute == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The '{0}' attribute could not be found for exception with key '{1}'", _typeAttributeName, exceptionKey));
			}

			Type type = Type.GetType(typeAttribute.Value);

			if (type == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Type '{0}' could not be loaded for exception with key '{1}'", typeAttribute.Value, exceptionKey));
			}

			if (!typeof(Exception).IsAssignableFrom(type))
			{
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Type '{0}' for exception with key '{1}' does not inherit from '{2}'", type.FullName, exceptionKey, typeof(Exception).FullName));
			}

			string message = exceptionNode.InnerText.Trim();

			if ((messageArgs != null) && (messageArgs.Length > 0))
			{
				message = string.Format(CultureInfo.InvariantCulture, message, messageArgs);
			}

			List<object> constructorArgsList = new List<object>();
			//message is always first
			constructorArgsList.Add(message);

			//next, any additional constructor args
			if (constructorArgs != null)
			{
				constructorArgsList.AddRange(constructorArgs);
			}

			//finally, the inner exception, if any
			if (innerException != null)
			{
				constructorArgsList.Add(innerException);
			}

			object[] constructorArgsArr = constructorArgsList.ToArray();
			BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;
			object state;
			ConstructorInfo constructor = null;

			try
			{
				constructor = (ConstructorInfo) Type.DefaultBinder.BindToMethod(bindingFlags, type.GetConstructors(bindingFlags), ref constructorArgsArr, null, null, null, out state);
			}
			catch (MissingMethodException)
			{
				//swallow and deal with below
			}

			if (constructor == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "An appropriate constructor could not be found for exception type '{0}, for exception with key '{1}'", type.FullName, exceptionKey));
			}

			//create the exception instance
			Exception e = (Exception) constructor.Invoke(constructorArgsArr);
			//finally, throw the exception
			throw e;
		}

		/// <summary>
		/// Gets exception information for a specified assembly.
		/// </summary>
		/// <param name="assembly">
		/// The assembly for which exception information should be retrieved.
		/// </param>0
		/// <returns>
		/// The exception information.
		/// </returns>
		[DebuggerHidden]
		private static XmlDocument GetExceptionInfo(Assembly assembly)
		{
			XmlDocument retVal = null;

			lock (_exceptionInfosLock)
			{
				if (_exceptionInfos.ContainsKey(assembly))
				{
					retVal = _exceptionInfos[assembly];
				}
				else
				{
					//if the exception info isn't cached we have to load it from an embedded resource in the calling assembly
					if (retVal == null)
					{
						retVal = new XmlDocument();
						string resourceName = string.Concat(assembly.GetName().Name, ".Properties.ExceptionHelper.xml");

						using (Stream stream = assembly.GetManifestResourceStream(resourceName))
						{
							if (stream == null)
							{
								throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "XML resource file '{0}' could not be found in assembly '{1}'.", resourceName, assembly.FullName));
							}

							retVal.Load(stream);
						}

						_exceptionInfos[assembly] = retVal;
					}
				}
			}

			return retVal;
		}
	}
}
