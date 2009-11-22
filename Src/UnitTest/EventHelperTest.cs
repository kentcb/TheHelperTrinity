using System;
using System.Reflection;
using System.Threading;
using Xunit;

namespace Kent.Boogaart.HelperTrinity.UnitTest
{
	public sealed class EventHelperTest
	{
		private object _sender;
		private MockEventArgs _e;
		private int _handlerCount;

		public EventHelperTest()
		{
			_sender = null;
			_e = null;
			_handlerCount = 0;
		}

		[Fact]
		public void BeginRaiseNonGeneric_ShouldntThrowWhenPassedNull()
		{
			//this simulates the case where no handlers are attached to the event
			EventHelper.BeginRaise(null, this, null, null);
			EventHelper.BeginRaise(null, null, null, null);
		}

		[Fact]
		public void RaiseNonGeneric_ShouldntThrowWhenPassedNull()
		{
			//this simulates the case where no handlers are attached to the event
			EventHelper.Raise(null, this);
			EventHelper.Raise(null, null);
		}

		[Fact]
		public void BeginRaiseNonGeneric_SenderNull()
		{
			Assert.Null(_sender);
			EventHandler del = NonGenericHandler;

			using (EventWaiter waiter = new EventWaiter())
			{
				EventHelper.BeginRaise(del, null, waiter.Callback, null);
			}

			Assert.Null(_sender);
			Assert.Equal(1, _handlerCount);
		}

		[Fact]
		public void RaiseNonGeneric_SenderNull()
		{
			Assert.Null(_sender);
			EventHandler del = NonGenericHandler;
			EventHelper.Raise(del, null);
			Assert.Null(_sender);
			Assert.Equal(1, _handlerCount);
		}

		[Fact]
		public void BeginRaiseNonGeneric_SingleHandlerShouldBeCalled()
		{
			EventHandler del = NonGenericHandler;

			using (EventWaiter waiter = new EventWaiter())
			{
				EventHelper.BeginRaise(del, this, waiter.Callback, null);
			}

			Assert.Equal(this, _sender);
			Assert.Equal(1, _handlerCount);
		}

		[Fact]
		public void RaiseNonGeneric_SingleHandlerShouldBeCalled()
		{
			EventHandler del = NonGenericHandler;
			EventHelper.Raise(del, this);
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
				EventHelper.BeginRaise(del, this, waiter.Callback, null);
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

			EventHelper.Raise(del, this);
			Assert.Equal(this, _sender);
			Assert.Equal(51, _handlerCount);
		}

		[Fact]
		public void BeginRaiseNonGenericWithEventArgs_ShouldntThrowWhenAllNull()
		{
			//it shouldn't matter that all arguments are null - this is quite possible in regular usage
			EventHelper.BeginRaise(null, null, null, null, null);
		}

		[Fact]
		public void RaiseNonGenericWithEventArgs_ShouldntThrowWhenAllNull()
		{
			//it shouldn't matter that all arguments are null - this is quite possible in regular usage
			EventHelper.Raise(null, null, null);
		}

		[Fact]
		public void RaiseNonGenericWithEventArgs_DelegateMismatchShouldThrow()
		{
			MockDelegateInvalid1 del = delegate
			{
			};

			Assert.Throws<TargetParameterCountException>(() => EventHelper.Raise(del, null, null));
		}

		[Fact]
		public void RaiseNonGenericWithEventArgs_DelegateMismatchShouldThrow2()
		{
			MockDelegateInvalid2 del = delegate
			{
			};

			Assert.Throws<TargetParameterCountException>(() => EventHelper.Raise(del, null, null));
		}

		[Fact]
		public void BeginRaiseNonGeneric_ValidShouldResultInEventBeingRaised()
		{
			Assert.Null(_sender);
			Assert.Null(_e);
			MockDelegate del = Handler;

			using (EventWaiter waiter = new EventWaiter())
			{
				EventHelper.BeginRaise(del, "sender", new MockEventArgs("data"), waiter.Callback, null);
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
			EventHelper.Raise(del, "sender", new MockEventArgs("data"));
			Assert.Equal("sender", _sender);
			Assert.Equal("data", _e.Data);
		}

		[Fact]
		public void BeginRaiseGeneric_ShouldntThrowIfDelegateNull()
		{
			//simulate the case where there are no event handlers
			EventHelper.BeginRaise(null, this, EventArgs.Empty, null, null);
			EventHelper.BeginRaise(null, null, EventArgs.Empty, null, null);
		}

		[Fact]
		public void RaiseGeneric_ShouldntThrowIfDelegateNull()
		{
			//simulate the case where there are no event handlers
			EventHelper.Raise(null, this, EventArgs.Empty);
			EventHelper.Raise(null, null, EventArgs.Empty);
		}

		[Fact]
		public void BeginRaiseGeneric_SenderNullShouldComeThroughAsNull()
		{
			Assert.Null(_sender);
			Assert.Null(_e);
			EventHandler<MockEventArgs> del = Handler;

			using (EventWaiter waiter = new EventWaiter())
			{
				EventHelper.BeginRaise(del, null, new MockEventArgs("my data"), waiter.Callback, null);
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
			EventHelper.Raise(del, null, new MockEventArgs("my data"));
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
				EventHelper.BeginRaise(del, this, new MockEventArgs("my data"), waiter.Callback, null);
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
			EventHelper.Raise(del, this, new MockEventArgs("my data"));
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
				EventHelper.BeginRaise(del, this, new MockEventArgs("my data"), waiter.Callback, null);
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

			EventHelper.Raise(del, this, new MockEventArgs("my data"));
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
				EventHelper.BeginRaise(del, this, delegate
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
			EventHelper.Raise(del, this, delegate
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

		[Fact]
		public void RaiseGeneric_EnsureEventsAreRaisedInThreadSafeManner()
		{
			Assert.Equal(0, _handlerCount);
			EventHandler<EventArgs> del = null;
			//add a single handler
			del += NonGenericHandler;
			Assert.NotNull(del);

			//these are used to synchronize the two threads
			ManualResetEvent waitForEventRaiseStart = new ManualResetEvent(false);
			ManualResetEvent waitForDelegateRemoval = new ManualResetEvent(false);
			ManualResetEvent waitForEventRaiseEnd = new ManualResetEvent(false);

			//kick off the event raise in a separate thread
			ThreadPool.QueueUserWorkItem(delegate
			{
				Raise(waitForEventRaiseStart, waitForEventRaiseEnd, waitForDelegateRemoval, del, this, EventArgs.Empty);
			});

			//wait for the other thread to begin the Raise() method
			waitForEventRaiseStart.WaitOne();
			//now remove the handler
			del -= NonGenericHandler;
			//del should be null here but not in the Raise() method (that is the whole point)
			Assert.Null(del);
			//tell the other thread the delegate has been removed
			waitForDelegateRemoval.Set();
			//wait for the other thread to finish firing the event
			waitForEventRaiseEnd.WaitOne();
			//event should have been raised because we removed the delegate *after* the Raise() method had started
			Assert.Equal(1, _handlerCount);
		}

		//simulates the work of the EventHelper class but includes wait handles so that we can effectively test the thread
		//safety
		public static void Raise<T>(ManualResetEvent waitForEventRaiseStart, ManualResetEvent waitForEventRaiseEnd, ManualResetEvent waitForDelegateRemoval, EventHandler<T> handler, object sender, T e)
			where T : EventArgs
		{
			//tell the main thread that we've entered the method
			waitForEventRaiseStart.Set();
			Assert.NotNull(handler);

			if (handler != null)
			{
				//wait until the other thread removes the handler - thus the original delegate will be null
				waitForDelegateRemoval.WaitOne();
				//if the code wasn't thread safe, a NullReferenceException would be thrown here
				handler(sender, e);
			}

			//tell the main thread that we're done raising the event
			waitForEventRaiseEnd.Set();
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
