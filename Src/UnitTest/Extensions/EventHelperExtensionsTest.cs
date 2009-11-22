using System;
using System.Reflection;
using System.Threading;
using Kent.Boogaart.HelperTrinity.Extensions;
using Xunit;

namespace Kent.Boogaart.HelperTrinity.UnitTest
{
	public sealed class EventHelperExtensionsTest
	{
		private object _sender;
		private MockEventArgs _e;
		private int _handlerCount;

		public EventHelperExtensionsTest()
		{
			_sender = null;
			_e = null;
			_handlerCount = 0;
		}

		[Fact]
		public void BeginRaiseNonGeneric_ShouldntThrowWhenPassedNull()
		{
			EventHandler handler = null;

			//this simulates the case where no handlers are attached to the event
			handler.BeginRaise(this, null, null);
			handler.BeginRaise(null, null, null);
		}

		[Fact]
		public void RaiseNonGeneric_ShouldntThrowWhenPassedNull()
		{
			EventHandler handler = null;

			//this simulates the case where no handlers are attached to the event
			handler.Raise(this);
			handler.Raise(null);
		}

		[Fact]
		public void BeginRaiseNonGeneric_SenderNull()
		{
			Assert.Null(_sender);
			EventHandler del = NonGenericHandler;

			using (EventWaiter waiter = new EventWaiter())
			{
				del.BeginRaise(null, waiter.Callback, null);
			}

			Assert.Null(_sender);
			Assert.Equal(1, _handlerCount);
		}

		[Fact]
		public void RaiseNonGeneric_SenderNull()
		{
			Assert.Null(_sender);
			EventHandler del = NonGenericHandler;
			del.Raise(null);
			Assert.Null(_sender);
			Assert.Equal(1, _handlerCount);
		}

		[Fact]
		public void BeginRaiseNonGeneric_SingleHandlerShouldBeCalled()
		{
			EventHandler del = NonGenericHandler;

			using (EventWaiter waiter = new EventWaiter())
			{
				del.BeginRaise(this, waiter.Callback, null);
			}

			Assert.Equal(this, _sender);
			Assert.Equal(1, _handlerCount);
		}

		[Fact]
		public void RaiseNonGeneric_SingleHandlerShouldBeCalled()
		{
			EventHandler del = NonGenericHandler;
			del.Raise(this);
			Assert.Equal(this, _sender);
			Assert.Equal(1, _handlerCount);
		}

		[Fact]
		public void BeginRaiseNonGeneric_MultipleHandlersShouldEachBeCalled()
		{
			EventHandler del = NonGenericHandler;

			for (int i = 0; i < 50; ++i)
			{
				del += NonGenericHandler;
			}

			using (EventWaiter waiter = new EventWaiter())
			{
				del.BeginRaise(this, waiter.Callback, null);
			}

			Assert.Equal(this, _sender);
			Assert.Equal(51, _handlerCount);
		}

		[Fact]
		public void RaiseNonGeneric_MultipleHandlersShouldEachBeCalled()
		{
			EventHandler del = NonGenericHandler;

			for (int i = 0; i < 50; ++i)
			{
				del += NonGenericHandler;
			}

			del.Raise(this);
			Assert.Equal(this, _sender);
			Assert.Equal(51, _handlerCount);
		}

		[Fact]
		public void BeginRaiseNonGenericWithEventArgs_ShouldntThrowWhenAllNull()
		{
			EventHandler handler = null;

			//it shouldn't matter that all arguments are null - this is quite possible in regular usage
			handler.BeginRaise(null, null, null, null);
		}

		[Fact]
		public void RaiseNonGenericWithEventArgs_ShouldntThrowWhenAllNull()
		{
			EventHandler handler = null;

			//it shouldn't matter that all arguments are null - this is quite possible in regular usage
			handler.Raise(null, null);
		}

		[Fact]
		public void RaiseNonGenericWithEventArgs_DelegateMismatchShouldThrow()
		{
			MockDelegateInvalid1 del = delegate
			{
			};

			Assert.Throws<TargetParameterCountException>(() => del.Raise(null, null));
		}

		[Fact]
		public void RaiseNonGenericWithEventArgs_DelegateMismatchShouldThrow2()
		{
			MockDelegateInvalid2 del = delegate
			{
			};

			Assert.Throws<TargetParameterCountException>(() => del.Raise(null, null));
		}

		[Fact]
		public void BeginRaiseNonGeneric_ValidShouldResultInEventBeingRaised()
		{
			Assert.Null(_sender);
			Assert.Null(_e);
			MockDelegate del = Handler;

			using (EventWaiter waiter = new EventWaiter())
			{
				del.BeginRaise("sender", new MockEventArgs("data"), waiter.Callback, null);
			}

			Assert.Equal("sender", _sender);
			Assert.Equal("data", _e.Data);
		}

		[Fact]
		public void RaiseNonGeneric_ValidShouldResultInEventBeingRaised()
		{
			Assert.Null(_sender);
			Assert.Null(_e);
			MockDelegate del = Handler;
			del.Raise("sender", new MockEventArgs("data"));
			Assert.Equal("sender", _sender);
			Assert.Equal("data", _e.Data);
		}

		[Fact]
		public void BeginRaiseGeneric_ShouldntThrowIfDelegateNull()
		{
			EventHandler<EventArgs> handler = null;

			//simulate the case where there are no event handlers
			handler.BeginRaise(this, EventArgs.Empty, null, null);
			handler.BeginRaise(null, EventArgs.Empty, null, null);
		}

		[Fact]
		public void RaiseGeneric_ShouldntThrowIfDelegateNull()
		{
			EventHandler<EventArgs> handler = null;

			//simulate the case where there are no event handlers
			handler.Raise(this, EventArgs.Empty);
			handler.Raise(null, EventArgs.Empty);
		}

		[Fact]
		public void BeginRaiseGeneric_SenderNullShouldComeThroughAsNull()
		{
			Assert.Null(_sender);
			Assert.Null(_e);
			EventHandler<MockEventArgs> del = Handler;

			using (EventWaiter waiter = new EventWaiter())
			{
				del.BeginRaise(null, new MockEventArgs("my data"), waiter.Callback, null);
			}

			Assert.Null(_sender);
			Assert.NotNull(_e);
			Assert.Equal("my data", _e.Data);
			Assert.Equal(1, _handlerCount);
		}

		[Fact]
		public void RaiseGeneric_SenderNullShouldComeThroughAsNull()
		{
			Assert.Null(_sender);
			Assert.Null(_e);
			EventHandler<MockEventArgs> del = Handler;
			del.Raise(null, new MockEventArgs("my data"));
			Assert.Null(_sender);
			Assert.NotNull(_e);
			Assert.Equal("my data", _e.Data);
			Assert.Equal(1, _handlerCount);
		}

		[Fact]
		public void BeginRaiseGeneric_SingleHandlerShouldBeCalled()
		{
			Assert.Null(_e);
			EventHandler<MockEventArgs> del = Handler;

			using (EventWaiter waiter = new EventWaiter())
			{
				del.BeginRaise(this, new MockEventArgs("my data"), waiter.Callback, null);
			}

			Assert.Equal(this, _sender);
			Assert.NotNull(_e);
			Assert.Equal("my data", _e.Data);
			Assert.Equal(1, _handlerCount);
		}

		[Fact]
		public void RaiseGeneric_SingleHandlerShouldBeCalled()
		{
			Assert.Null(_e);
			EventHandler<MockEventArgs> del = Handler;
			del.Raise(this, new MockEventArgs("my data"));
			Assert.Equal(this, _sender);
			Assert.NotNull(_e);
			Assert.Equal("my data", _e.Data);
			Assert.Equal(1, _handlerCount);
		}

		[Fact]
		public void BeginRaiseGeneric_MultipleHandlersShouldEachBeCalled()
		{
			Assert.Null(_e);
			EventHandler<MockEventArgs> del = Handler;

			for (int i = 0; i < 50; ++i)
			{
				del += Handler;
			}

			using (EventWaiter waiter = new EventWaiter())
			{
				del.BeginRaise(this, new MockEventArgs("my data"), waiter.Callback, null);
			}

			Assert.Equal(this, _sender);
			Assert.NotNull(_e);
			Assert.Equal("my data", _e.Data);
			Assert.Equal(51, _handlerCount);
		}

		[Fact]
		public void RaiseGeneric_MultipleHandlersShouldEachBeCalled()
		{
			Assert.Null(_e);
			EventHandler<MockEventArgs> del = Handler;

			for (int i = 0; i < 50; ++i)
			{
				del += Handler;
			}

			del.Raise(this, new MockEventArgs("my data"));
			Assert.Equal(this, _sender);
			Assert.NotNull(_e);
			Assert.Equal("my data", _e.Data);
			Assert.Equal(51, _handlerCount);
		}

		[Fact]
		public void BeginRaiseGenericWithCallback_CallbackShouldBeCalledToCreateEventArgs()
		{
			bool callback = false;
			EventHandler<MockEventArgs> del = Handler;

			using (EventWaiter waiter = new EventWaiter())
			{
				del.BeginRaise(this, delegate
				{
					callback = true;
					return new MockEventArgs("test");
				}, waiter.Callback, null);
			}

			Assert.True(callback);
			Assert.Equal(this, _sender);
			Assert.NotNull(_e);
			Assert.Equal("test", _e.Data);
			Assert.Equal(1, _handlerCount);
		}

		[Fact]
		public void RaiseGenericWithCallback_CallbackShouldBeCalledToCreateEventArgs()
		{
			bool callback = false;
			EventHandler<MockEventArgs> del = Handler;
			del.Raise(this, delegate
			{
				callback = true;
				return new MockEventArgs("test");
			});
			Assert.True(callback);
			Assert.Equal(this, _sender);
			Assert.NotNull(_e);
			Assert.Equal("test", _e.Data);
			Assert.Equal(1, _handlerCount);
		}

		private void Handler(object sender, MockEventArgs e)
		{
			_sender = sender;
			_e = e;
			++_handlerCount;
		}

		private void NonGenericHandler(object sender, EventArgs e)
		{
			_sender = sender;
			++_handlerCount;
		}

		#region Supporting Types

		private sealed class EventWaiter : IDisposable
		{
			private readonly ManualResetEvent _waitHandle = new ManualResetEvent(false);

			public AsyncCallback Callback
			{
				get
				{
					return AsyncCallback;
				}
			}

			void IDisposable.Dispose()
			{
				_waitHandle.WaitOne();
			}

			private void AsyncCallback(IAsyncResult result)
			{
				_waitHandle.Set();
			}
		}

		private sealed class MockEventArgs : EventArgs
		{
			private readonly string _data;

			public string Data
			{
				get
				{
					return _data;
				}
			}

			internal MockEventArgs(string data)
			{
				_data = data;
			}
		}

		private delegate void MockDelegate(object sender, MockEventArgs e);
		private delegate void MockDelegateInvalid1(object sender, MockEventArgs e, string someOtherData);
		private delegate void MockDelegateInvalid2(MockEventArgs e);

		#endregion
	}
}