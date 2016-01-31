namespace HelperTrinity.UnitTests
{
    using System;
    using Xunit;

    public sealed class ExceptionHelperFixture
    {
        [Fact]
        public void ctor_throws_if_type_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => new ExceptionHelper(null));
        }

        [Fact]
        public void ctor_throws_if_resource_name_is_null()
        {
            Assert.Throws<ArgumentException>(() => new ExceptionHelper(GetType(), null));
        }

        [Fact]
        public void ctor_throws_if_resource_name_is_empty()
        {
            Assert.Throws<ArgumentException>(() => new ExceptionHelper(GetType(), "   "));
        }

        [Fact]
        public void resolve_throws_if_key_is_not_found()
        {
            var exceptionHelper = new ExceptionHelper(typeof(ExceptionHelperFixture));
            var ex = Assert.Throws<InvalidOperationException>(() => exceptionHelper.Resolve("invalidKey"));
            Assert.Equal("The exception details for key 'invalidKey' could not be found at /exceptionHelper/exceptionGroup[@type'HelperTrinity.UnitTests.ExceptionHelperFixture']/exception[@key='invalidKey'].", ex.Message);
        }

        [Fact]
        public void resolve_throws_if_type_attribute_is_not_found()
        {
            var exceptionHelper = new ExceptionHelper(typeof(ExceptionHelperFixture));
            var ex = Assert.Throws<InvalidOperationException>(() => exceptionHelper.Resolve("noTypeAttribute"));
            Assert.Equal("The 'type' attribute could not be found for exception with key 'noTypeAttribute'", ex.Message);
        }

        [Fact]
        public void resolve_throws_if_type_could_not_be_loaded()
        {
            var exceptionHelper = new ExceptionHelper(typeof(ExceptionHelperFixture));
            var ex = Assert.Throws<InvalidOperationException>(() => exceptionHelper.Resolve("typeCouldNotBeLoaded"));
            Assert.Equal("Type 'Foo.Bar.Wont.Load, Anywhere' could not be loaded for exception with key 'typeCouldNotBeLoaded'", ex.Message);
        }

        [Fact]
        public void resolve_throws_if_type_is_not_an_exception()
        {
            var exceptionHelper = new ExceptionHelper(typeof(ExceptionHelperFixture));
            var ex = Assert.Throws<InvalidOperationException>(() => exceptionHelper.Resolve("typeNotException"));
            Assert.Equal("Type 'System.DateTime' for exception with key 'typeNotException' does not inherit from 'System.Exception'", ex.Message);
        }

        [Fact]
        public void resolve_throws_if_no_constructor_could_be_found()
        {
            var exceptionHelper = new ExceptionHelper(typeof(ExceptionHelperFixture));
            var ex = Assert.Throws<InvalidOperationException>(() => exceptionHelper.Resolve("noConstructorFound"));
            Assert.Equal("An appropriate constructor could not be found for exception type 'HelperTrinity.UnitTests.ExceptionHelperFixture+TestException, for exception with key 'noConstructorFound'", ex.Message);
        }

        [Fact]
        public void resolve_returns_exception()
        {
            var exceptionHelper = new ExceptionHelper(typeof(ExceptionHelperFixture));
            var ex = exceptionHelper.Resolve("valid");
            Assert.True(ex is InvalidOperationException);
            Assert.Equal("Here is the message.", ex.Message);
        }

        [Fact]
        public void resolve_allows_formatting_of_exception_message()
        {
            var exceptionHelper = new ExceptionHelper(typeof(ExceptionHelperFixture));
            var ex = exceptionHelper.Resolve("withMessageArgs", "hello", 12);
            Assert.Equal("Here is the message with argument (hello) or two (12).", ex.Message);
        }

        [Fact]
        public void resolve_allows_inner_exception_to_be_provided()
        {
            var exceptionHelper = new ExceptionHelper(typeof(ExceptionHelperFixture));
            var inner = new ArgumentException();
            var ex = exceptionHelper.Resolve("valid", inner);
            Assert.NotNull(ex.InnerException);
            Assert.Same(inner, ex.InnerException);
        }

        [Fact]
        public void resolve_allows_custom_constructor_to_be_called()
        {
            var exceptionHelper = new ExceptionHelper(typeof(ExceptionHelperFixture));
            var ex = exceptionHelper.Resolve("withConstructorArgs", new object[] { 1, 2, "more info" }, (Exception)null) as TestException;
            Assert.NotNull(ex);
            Assert.Equal("A message.", ex.Message);
            Assert.Equal(1, ex.Num1);
            Assert.Equal(2, ex.Num2);
            Assert.Equal("more info", ex.Info);
        }

        [Fact]
        public void resolve_allows_custom_constructor_and_message_formatting_in_tandem()
        {
            var exceptionHelper = new ExceptionHelper(typeof(ExceptionHelperFixture));
            var ex = exceptionHelper.Resolve("withConstructorAndMessageArgs", new object[] { 1, 2, "more info" }, "param1") as TestException;
            Assert.NotNull(ex);
            Assert.Equal("My message with a parameter: 'param1'", ex.Message);
            Assert.Equal(1, ex.Num1);
            Assert.Equal(2, ex.Num2);
            Assert.Equal("more info", ex.Info);
        }

        [Fact]
        public void exception_helper_resource_can_be_in_custom_location()
        {
            var exceptionHelper = new ExceptionHelper(typeof(ExceptionHelperFixture), "HelperTrinity.UnitTests.ExceptionHelper.Subfolder.CustomExceptionHelperResource.xml");
            var ex = Assert.Throws<InvalidOperationException>(() => exceptionHelper.ResolveAndThrowIf(true, "anException"));
            Assert.Equal("Here is the message.", ex.Message);
        }

        #region Supporting Types

        public sealed class TestException : Exception
        {
            private readonly int num1;
            private readonly int num2;
            private readonly string info;

            public int Num1
            {
                get { return num1; }
            }

            public int Num2
            {
                get { return num2; }
            }

            public string Info
            {
                get { return info; }
            }

            public TestException(string message, int num1, int num2, string info)
                : this(message, num1, num2, info, null)
            {
            }

            public TestException(string message, int num1, int num2, string info, Exception inner)
                : base(message, inner)
            {
                this.num1 = num1;
                this.num2 = num2;
                this.info = info;
            }
        }

        #endregion
    }
}
