using System;
using NUnit.Framework;
using Kent.Boogaart.HelperTrinity;

namespace Kent.Boogaart.HelperTrinity.UnitTest
{
	[TestFixture]
	public sealed class ExceptionHelperTest
	{
		[Test]
		[ExpectedException(typeof(InvalidOperationException), "The exception details for key 'invalidKey' could not be found (they should be under '/exceptionHelper/exceptionGroup[@type=\"Kent.Boogaart.HelperTrinity.UnitTest.ExceptionHelperTest\"]/exception[@key=\"invalidKey\"]')")]
		public void Throw_ShouldThrowIfKeyNotFound()
		{
			ExceptionHelper.Throw("invalidKey");
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException), "The 'type' attribute could not be found for exception with key 'noTypeAttribute'")]
		public void Throw_ShouldThrowIfTypeAttributeNotFound()
		{
			ExceptionHelper.Throw("noTypeAttribute");
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException), "Type 'Foo.Bar.Wont.Load, Anywhere' could not be loaded for exception with key 'typeCouldNotBeLoaded'")]
		public void Throw_ShouldThrowIfTypeCouldNotBeLoaded()
		{
			ExceptionHelper.Throw("typeCouldNotBeLoaded");
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException), "Type 'System.DateTime' for exception with key 'typeNotException' does not inherit from 'System.Exception'")]
		public void Throw_ShouldThrowIfTheTypeIsNotAnException()
		{
			ExceptionHelper.Throw("typeNotException");
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException), "An appropriate constructor could not be found for exception type 'Kent.Boogaart.HelperTrinity.UnitTest.ExceptionHelperTest+TestException, for exception with key 'noConstructorFound'")]
		public void Throw_ShouldThrowIfNoConstructorWasFound()
		{
			ExceptionHelper.Throw("noConstructorFound");
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException), "Here is the message.")]
		public void Throw_ShouldThrowCorrectExceptionIfAllSetupIsCorrect()
		{
			ExceptionHelper.Throw("valid");
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException), "Here is the message with argument (hello) or two (12).")]
		public void Throw_ShouldAllowFormattingOfExceptionMessage()
		{
			ExceptionHelper.Throw("withMessageArgs", "hello", 12);
		}

		[Test]
		public void Throw_ShouldAllowInnerExceptionToBeSpecified()
		{
			Exception inner = new ArgumentException();

			try
			{
				ExceptionHelper.Throw("valid", inner);
			}
			catch (InvalidOperationException e)
			{
				Assert.IsNotNull(e.InnerException);
				Assert.IsTrue(e.InnerException.GetType() == typeof(ArgumentException));
			}
		}

		[Test]
		public void Throw_ShouldAllowCustomConstructorToBeCalled()
		{
			try
			{
				ExceptionHelper.Throw("withConstructorArgs", new object[] { 1, 2, "more info" }, (Exception) null);
			}
			catch (TestException e)
			{
				Assert.AreEqual("A message.", e.Message);
				Assert.AreEqual(1, e.Num1);
				Assert.AreEqual(2, e.Num2);
				Assert.AreEqual("more info", e.Info);
			}
		}

		[Test]
		public void Throw_ShouldAllowCustomConstructorAndMessageFormattingInTandem()
		{
			try
			{
				ExceptionHelper.Throw("withConstructorAndMessageArgs", new object[] { 1, 2, "more info" }, "param1");
			}
			catch (TestException e)
			{
				Assert.AreEqual("My message with a parameter: 'param1'", e.Message);
				Assert.AreEqual(1, e.Num1);
				Assert.AreEqual(2, e.Num2);
				Assert.AreEqual("more info", e.Info);
			}
		}

		[Test]
		public void ThrowIf_ShouldntThrowIfConditionIsFalse()
		{
			ExceptionHelper.ThrowIf(false, "valid");
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException), "Here is the message.")]
		public void ThrowIf_ShouldThrowIfConditionIsTrue()
		{
			ExceptionHelper.ThrowIf(true, "valid");
		}

		[Test]
		public void ThrowIf_ShouldAllowCustomConstructorAndMessageFormattingInTandem()
		{
			ExceptionHelper.ThrowIf(false, "withConstructorAndMessageArgs", new object[] { 1, 2, "more info" }, "param1");

			try
			{
				ExceptionHelper.ThrowIf(true, "withConstructorAndMessageArgs", new object[] { 1, 2, "more info" }, "param1");
			}
			catch (TestException e)
			{
				Assert.AreEqual("My message with a parameter: 'param1'", e.Message);
				Assert.AreEqual(1, e.Num1);
				Assert.AreEqual(2, e.Num2);
				Assert.AreEqual("more info", e.Info);
			}
		}

		[Test]
		public void ThrowIf_ShouldAllowCustomConstructorAndInnerExceptionInTandem()
		{
			Exception inner = new ArgumentException();
			ExceptionHelper.ThrowIf(false, "withConstructorArgs", new object[] { 1, 2, "more info" }, inner);

			try
			{
				ExceptionHelper.ThrowIf(true, "withConstructorArgs", new object[] { 1, 2, "more info" }, inner);
			}
			catch (TestException e)
			{
				Assert.AreEqual("A message.", e.Message);
				Assert.AreEqual(1, e.Num1);
				Assert.AreEqual(2, e.Num2);
				Assert.AreEqual("more info", e.Info);
				Assert.AreSame(inner, e.InnerException);
			}
		}

		[Test]
		public void ThrowIf_ShouldAllowMessageFormattingAndInnerExceptionInTandem()
		{
			Exception inner = new ArgumentException();
			ExceptionHelper.ThrowIf(false, "withMessageArgs", inner, 1, "two");

			try
			{
				ExceptionHelper.ThrowIf(true, "withMessageArgs", inner, 1, "two");
			}
			catch (InvalidOperationException e)
			{
				Assert.AreEqual("Here is the message with argument (1) or two (two).", e.Message);
				Assert.AreSame(inner, e.InnerException);
			}
		}

		#region Supporting Types

		public sealed class TestException : Exception
		{
			private readonly int _num1;
			private readonly int _num2;
			private readonly string _info;

			public int Num1
			{
				get
				{
					return _num1;
				}
			}

			public int Num2
			{
				get
				{
					return _num2;
				}
			}

			public string Info
			{
				get
				{
					return _info;
				}
			}

			public TestException(string message, int num1, int num2, string info)
				: this(message, num1, num2, info, null)
			{
			}

			public TestException(string message, int num1, int num2, string info, Exception inner)
				: base(message, inner)
			{
				_num1 = num1;
				_num2 = num2;
				_info = info;
			}
		}

		#endregion
	}
}
