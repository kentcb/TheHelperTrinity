using System;
using Xunit;

namespace Kent.Boogaart.HelperTrinity.UnitTest
{
    public sealed class ExceptionHelperTest
    {
        private readonly ExceptionHelper _exceptionHelper;

        public ExceptionHelperTest()
        {
            _exceptionHelper = new ExceptionHelper(GetType());
        }

        [Fact]
        public void Constructor_ShouldThrowIfTypeIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new ExceptionHelper(null));
        }

        [Fact]
        public void Constructor_ShouldThrowIfResourceNameIsNull()
        {
            Assert.Throws<ArgumentException>(() => new ExceptionHelper(GetType(), null));
        }

        [Fact]
        public void Constructor_ShouldThrowIfResourceNameIsEmpty()
        {
            Assert.Throws<ArgumentException>(() => new ExceptionHelper(GetType(), "   "));
        }

        [Fact]
        public void Resolve_ShouldThrowIfKeyNotFound()
        {
            var ex = Assert.Throws<InvalidOperationException>(() => _exceptionHelper.Resolve("invalidKey"));
            Assert.Equal("The exception details for key 'invalidKey' could not be found at /exceptionHelper/exceptionGroup[@type'Kent.Boogaart.HelperTrinity.UnitTest.ExceptionHelperTest']/exception[@key='invalidKey'].", ex.Message);
        }

        [Fact]
        public void Resolve_ShouldThrowIfTypeAttributeNotFound()
        {
            var ex = Assert.Throws<InvalidOperationException>(() => _exceptionHelper.Resolve("noTypeAttribute"));
            Assert.Equal("The 'type' attribute could not be found for exception with key 'noTypeAttribute'", ex.Message);
        }

        [Fact]
        public void Resolve_ShouldThrowIfTypeCouldNotBeLoaded()
        {
            var ex = Assert.Throws<InvalidOperationException>(() => _exceptionHelper.Resolve("typeCouldNotBeLoaded"));
            Assert.Equal("Type 'Foo.Bar.Wont.Load, Anywhere' could not be loaded for exception with key 'typeCouldNotBeLoaded'", ex.Message);
        }

        [Fact]
        public void Resolve_ShouldThrowIfTheTypeIsNotAnException()
        {
            var ex = Assert.Throws<InvalidOperationException>(() => _exceptionHelper.Resolve("typeNotException"));
            Assert.Equal("Type 'System.DateTime' for exception with key 'typeNotException' does not inherit from 'System.Exception'", ex.Message);
        }

        [Fact]
        public void Resolve_ShouldThrowIfNoConstructorWasFound()
        {
            var ex = Assert.Throws<InvalidOperationException>(() => _exceptionHelper.Resolve("noConstructorFound"));
            Assert.Equal("An appropriate constructor could not be found for exception type 'Kent.Boogaart.HelperTrinity.UnitTest.ExceptionHelperTest+TestException, for exception with key 'noConstructorFound'", ex.Message);
        }

        [Fact]
        public void Resolve_ShouldReturnCorrectExceptionIfAllSetupIsCorrect()
        {
            var ex = _exceptionHelper.Resolve("valid");
            Assert.True(ex is InvalidOperationException);
            Assert.Equal("Here is the message.", ex.Message);
        }

        [Fact]
        public void Resolve_ShouldAllowFormattingOfExceptionMessage()
        {
            var ex = _exceptionHelper.Resolve("withMessageArgs", "hello", 12);
            Assert.Equal("Here is the message with argument (hello) or two (12).", ex.Message);
        }

        [Fact]
        public void Resolve_ShouldAllowInnerExceptionToBeSpecified()
        {
            var inner = new ArgumentException();
            var ex = _exceptionHelper.Resolve("valid", inner);
            Assert.NotNull(ex.InnerException);
            Assert.Same(inner, ex.InnerException);
        }

        [Fact]
        public void Resolve_ShouldAllowCustomConstructorToBeCalled()
        {
            var ex = _exceptionHelper.Resolve("withConstructorArgs", new object[] { 1, 2, "more info" }, (Exception)null) as TestException;
            Assert.NotNull(ex);
            Assert.Equal("A message.", ex.Message);
            Assert.Equal(1, ex.Num1);
            Assert.Equal(2, ex.Num2);
            Assert.Equal("more info", ex.Info);
        }

        [Fact]
        public void Resolve_ShouldAllowCustomConstructorAndMessageFormattingInTandem()
        {
            var ex = _exceptionHelper.Resolve("withConstructorAndMessageArgs", new object[] { 1, 2, "more info" }, "param1") as TestException;
            Assert.NotNull(ex);
            Assert.Equal("My message with a parameter: 'param1'", ex.Message);
            Assert.Equal(1, ex.Num1);
            Assert.Equal(2, ex.Num2);
            Assert.Equal("more info", ex.Info);
        }

        [Fact]
        public void ExceptionHelperResourceCanBeInCustomLocation()
        {
            var exceptionHelper = new ExceptionHelper(GetType(), "Kent.Boogaart.HelperTrinity.UnitTest.ExceptionHelper.Subfolder.CustomExceptionHelperResource.xml");
            var ex = Assert.Throws<InvalidOperationException>(() => exceptionHelper.ResolveAndThrowIf(true, "anException"));
            Assert.Equal("Here is the message.", ex.Message);
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
