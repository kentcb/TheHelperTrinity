//the original source for this code is an online article available at http://www.codeproject.com/csharp/thehelpertrinity.asp

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using NUnit.Framework;
using Kent.Boogaart.HelperTrinity;

namespace Kent.Boogaart.HelperTrinity.UnitTest
{
	[TestFixture]
	public sealed class EventHelperTest
	{
		private object _sender;
		private MockEventArgs _e;
		private int _handlerCount;

		[SetUp]
		public void SetUp()
		{
			_sender = null;
			_e = null;
			_handlerCount = 0;
		}

		[Test]
		public void BeginRaiseNonGeneric_ShouldntThrowWhenPassedNull()
		{
			//this simulates the case where no handlers are attached to the event
			EventHelper.BeginRaise(null, this, null, null);
			EventHelper.BeginRaise(null, null, null, null);
		}

		[Test]
		public void RaiseNonGeneric_ShouldntThrowWhenPassedNull()
		{
			//this simulates the case where no handlers are attached to the event
			EventHelper.Raise(null, this);
			EventHelper.Raise(null, null);
		}

		[Test]
		public void BeginRaiseNonGeneric_SenderNull()
		{
			Assert.IsNull(_sender);
			EventHandler del = new EventHandler(NonGenericHandler);

			using (EventWaiter waiter = new EventWaiter())
			{
				EventHelper.BeginRaise(del, null, waiter.Callback, null);
			}

			Assert.IsNull(_sender);
			Assert.AreEqual(1, _handlerCount);
		}

		[Test]
		public void RaiseNonGeneric_SenderNull()
		{
			Assert.IsNull(_sender);
			EventHandler del = new EventHandler(NonGenericHandler);
			EventHelper.Raise(del, null);
			Assert.IsNull(_sender);
			Assert.AreEqual(1, _handlerCount);
		}

		[Test]
		public void BeginRaiseNonGeneric_SingleHandlerShouldBeCalled()
		{
			EventHandler del = new EventHandler(NonGenericHandler);

			using (EventWaiter waiter = new EventWaiter())
			{
				EventHelper.BeginRaise(del, this, waiter.Callback, null);
			}

			Assert.AreEqual(this, _sender);
			Assert.AreEqual(1, _handlerCount);
		}

		[Test]
		public void RaiseNonGeneric_SingleHandlerShouldBeCalled()
		{
			EventHandler del = new EventHandler(NonGenericHandler);
			EventHelper.Raise(del, this);
			Assert.AreEqual(this, _sender);
			Assert.AreEqual(1, _handlerCount);
		}

		[Test]
		public void BeginRaiseNonGeneric_MultipleHandlersShouldEachBeCalled()
		{
			EventHandler del = new EventHandler(NonGenericHandler);

			for (int i = 0; i < 50; ++i)
			{
				del += new EventHandler(NonGenericHandler);
			}

			using (EventWaiter waiter = new EventWaiter())
			{
				EventHelper.BeginRaise(del, this, waiter.Callback, null);
			}

			Assert.AreEqual(this, _sender);
			Assert.AreEqual(51, _handlerCount);
		}

		[Test]
		public void RaiseNonGeneric_MultipleHandlersShouldEachBeCalled()
		{
			EventHandler del = new EventHandler(NonGenericHandler);

			for (int i = 0; i < 50; ++i)
			{
				del += new EventHandler(NonGenericHandler);
			}

			EventHelper.Raise(del, this);
			Assert.AreEqual(this, _sender);
			Assert.AreEqual(51, _handlerCount);
		}

		[Test]
		public void BeginRaiseNonGenericWithEventArgs_ShouldntThrowWhenAllNull()
		{
			//it shouldn't matter that all arguments are null - this is quite possible in regular usage
			EventHelper.BeginRaise(null, null, null, null, null);
		}

		[Test]
		public void RaiseNonGenericWithEventArgs_ShouldntThrowWhenAllNull()
		{
			//it shouldn't matter that all arguments are null - this is quite possible in regular usage
			EventHelper.Raise(null, null, null);
		}

		[Test]
		[ExpectedException(typeof(TargetParameterCountException))]
		public void RaiseNonGenericWithEventArgs_DelegateMismatchShouldThrow()
		{
			MockDelegateInvalid1 del = new MockDelegateInvalid1(delegate
			{
			});

			EventHelper.Raise(del, null, null);
		}

		[Test]
		[ExpectedException(typeof(TargetParameterCountException))]
		public void RaiseNonGenericWithEventArgs_DelegateMismatchShouldThrow2()
		{
			MockDelegateInvalid2 del = new MockDelegateInvalid2(delegate
			{
			});

			EventHelper.Raise(del, null, null);
		}

		[Test]
		public void BeginRaiseNonGeneric_ValidShouldResultInEventBeingRaised()
		{
			Assert.IsNull(_sender);
			Assert.IsNull(_e);
			MockDelegate del = new MockDelegate(Handler);

			using (EventWaiter waiter = new EventWaiter())
			{
				EventHelper.BeginRaise(del, "sender", new MockEventArgs("data"), waiter.Callback, null);
			}

			Assert.AreEqual("sender", _sender);
			Assert.AreEqual("data", _e.Data);
		}

		[Test]
		public void RaiseNonGeneric_ValidShouldResultInEventBeingRaised()
		{
			Assert.IsNull(_sender);
			Assert.IsNull(_e);
			MockDelegate del = new MockDelegate(Handler);
			EventHelper.Raise(del, "sender", new MockEventArgs("data"));
			Assert.AreEqual("sender", _sender);
			Assert.AreEqual("data", _e.Data);
		}

		[Test]
		public void BeginRaiseGeneric_ShouldntThrowIfDelegateNull()
		{
			//simulate the case where there are no event handlers
			EventHelper.BeginRaise<EventArgs>(null, this, EventArgs.Empty, null, null);
			EventHelper.BeginRaise<EventArgs>(null, null, EventArgs.Empty, null, null);
		}

		[Test]
		public void RaiseGeneric_ShouldntThrowIfDelegateNull()
		{
			//simulate the case where there are no event handlers
			EventHelper.Raise<EventArgs>(null, this, EventArgs.Empty);
			EventHelper.Raise<EventArgs>(null, null, EventArgs.Empty);
		}

		[Test]
		public void BeginRaiseGeneric_SenderNullShouldComeThroughAsNull()
		{
			Assert.IsNull(_sender);
			Assert.IsNull(_e);
			EventHandler<MockEventArgs> del = new EventHandler<MockEventArgs>(Handler);

			using (EventWaiter waiter = new EventWaiter())
			{
				EventHelper.BeginRaise(del, null, new MockEventArgs("my data"), waiter.Callback, null);
			}

			Assert.IsNull(_sender);
			Assert.IsNotNull(_e);
			Assert.AreEqual("my data", _e.Data);
			Assert.AreEqual(1, _handlerCount);
		}

		[Test]
		public void RaiseGeneric_SenderNullShouldComeThroughAsNull()
		{
			Assert.IsNull(_sender);
			Assert.IsNull(_e);
			EventHandler<MockEventArgs> del = new EventHandler<MockEventArgs>(Handler);
			EventHelper.Raise(del, null, new MockEventArgs("my data"));
			Assert.IsNull(_sender);
			Assert.IsNotNull(_e);
			Assert.AreEqual("my data", _e.Data);
			Assert.AreEqual(1, _handlerCount);
		}

		[Test]
		public void BeginRaiseGeneric_SingleHandlerShouldBeCalled()
		{
			Assert.IsNull(_e);
			EventHandler<MockEventArgs> del = new EventHandler<MockEventArgs>(Handler);

			using (EventWaiter waiter = new EventWaiter())
			{
				EventHelper.BeginRaise(del, this, new MockEventArgs("my data"), waiter.Callback, null);
			}

			Assert.AreEqual(this, _sender);
			Assert.IsNotNull(_e);
			Assert.AreEqual("my data", _e.Data);
			Assert.AreEqual(1, _handlerCount);
		}

		[Test]
		public void RaiseGeneric_SingleHandlerShouldBeCalled()
		{
			Assert.IsNull(_e);
			EventHandler<MockEventArgs> del = new EventHandler<MockEventArgs>(Handler);
			EventHelper.Raise(del, this, new MockEventArgs("my data"));
			Assert.AreEqual(this, _sender);
			Assert.IsNotNull(_e);
			Assert.AreEqual("my data", _e.Data);
			Assert.AreEqual(1, _handlerCount);
		}

		[Test]
		public void BeginRaiseGeneric_MultipleHandlersShouldEachBeCalled()
		{
			Assert.IsNull(_e);
			EventHandler<MockEventArgs> del = new EventHandler<MockEventArgs>(Handler);

			for (int i = 0; i < 50; ++i)
			{
				del += new EventHandler<MockEventArgs>(Handler);
			}

			using (EventWaiter waiter = new EventWaiter())
			{
				EventHelper.BeginRaise(del, this, new MockEventArgs("my data"), waiter.Callback, null);
			}

			Assert.AreEqual(this, _sender);
			Assert.IsNotNull(_e);
			Assert.AreEqual("my data", _e.Data);
			Assert.AreEqual(51, _handlerCount);
		}

		[Test]
		public void RaiseGeneric_MultipleHandlersShouldEachBeCalled()
		{
			Assert.IsNull(_e);
			EventHandler<MockEventArgs> del = new EventHandler<MockEventArgs>(Handler);

			for (int i = 0; i < 50; ++i)
			{
				del += new EventHandler<MockEventArgs>(Handler);
			}

			EventHelper.Raise(del, this, new MockEventArgs("my data"));
			Assert.AreEqual(this, _sender);
			Assert.IsNotNull(_e);
			Assert.AreEqual("my data", _e.Data);
			Assert.AreEqual(51, _handlerCount);
		}

		[Test]
		public void BeginRaiseGenericWithCallback_CallbackShouldBeCalledToCreateEventArgs()
		{
			bool callback = false;
			EventHandler<MockEventArgs> del = new EventHandler<MockEventArgs>(Handler);

			using (EventWaiter waiter = new EventWaiter())
			{
				EventHelper.BeginRaise(del, this, delegate
				{
					callback = true;
					return new MockEventArgs("test");
				}, waiter.Callback, null);
			}

			Assert.IsTrue(callback);
			Assert.AreEqual(this, _sender);
			Assert.IsNotNull(_e);
			Assert.AreEqual("test", _e.Data);
			Assert.AreEqual(1, _handlerCount);
		}

		[Test]
		public void RaiseGenericWithCallback_CallbackShouldBeCalledToCreateEventArgs()
		{
			bool callback = false;
			EventHandler<MockEventArgs> del = new EventHandler<MockEventArgs>(Handler);
			EventHelper.Raise(del, this, delegate
			{
				callback = true;
				return new MockEventArgs("test");
			});
			Assert.IsTrue(callback);
			Assert.AreEqual(this, _sender);
			Assert.IsNotNull(_e);
			Assert.AreEqual("test", _e.Data);
			Assert.AreEqual(1, _handlerCount);
		}

		[Test]
		public void RaiseGeneric_EnsureEventsAreRaisedInThreadSafeManner()
		{
			Assert.AreEqual(0, _handlerCount);
			EventHandler<EventArgs> del = null;
			//add a single handler
			del += new EventHandler<EventArgs>(NonGenericHandler);
			Assert.IsNotNull(del);

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
			del -= new EventHandler<EventArgs>(NonGenericHandler);
			//del should be null here but not in the Raise() method (that is the whole point)
			Assert.IsNull(del);
			//tell the other thread the delegate has been removed
			waitForDelegateRemoval.Set();
			//wait for the other thread to finish firing the event
			waitForEventRaiseEnd.WaitOne();
			//event should have been raised because we removed the delegate *after* the Raise() method had started
			Assert.AreEqual(1, _handlerCount);
		}

		//simulates the work of the EventHelper class but includes wait handles so that we can effectively test the thread
		//safety
		public void Raise<T>(ManualResetEvent waitForEventRaiseStart, ManualResetEvent waitForEventRaiseEnd, ManualResetEvent waitForDelegateRemoval, EventHandler<T> handler, object sender, T e)
			where T : EventArgs
		{
			//tell the main thread that we've entered the method
			waitForEventRaiseStart.Set();
			Assert.IsNotNull(handler);

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
					return new AsyncCallback(AsyncCallback);
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
			private string _data;

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
