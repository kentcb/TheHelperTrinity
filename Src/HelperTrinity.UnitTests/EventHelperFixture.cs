namespace HelperTrinity.UnitTests
{
    using System;
    using System.Threading;
    using Xunit;

    public sealed class EventHelperFixture
    {
        [Fact]
        public void raise_does_not_throw_if_event_handler_is_null()
        {
            EventHelper.Raise(null, this);
        }

        [Fact]
        public void raise_does_not_throw_if_sender_is_null()
        {
            EventHelper.Raise((EventHandler)((s, e) => { }), null);
        }

        [Fact]
        public void raise_invokes_event_handler()
        {
            var raised = false;
            EventHelper.Raise((EventHandler)((s, e) => raised = true), this);

            Assert.True(raised);
        }

        [Fact]
        public void raise_invokes_all_event_handlers_in_multicast_delegate()
        {
            var invoked = 0;
            var handler = (EventHandler)((s, e) => Interlocked.Increment(ref invoked));
            handler += (s, e) => Interlocked.Increment(ref invoked);
            handler += (s, e) => Interlocked.Increment(ref invoked);

            EventHelper.Raise(handler, this);
            Assert.Equal(3, invoked);
        }

        [Fact]
        public void raise_passes_through_sender()
        {
            object sender = null;
            EventHelper.Raise((EventHandler)((s, e) => sender = s), this);

            Assert.NotNull(sender);
            Assert.Same(this, sender);
        }

        [Fact]
        public void raise_passes_through_empty_event_args_by_default()
        {
            object eventArgs = null;
            EventHelper.Raise((EventHandler)((s, e) => eventArgs = e), this);

            Assert.NotNull(eventArgs);
            Assert.Same(EventArgs.Empty, eventArgs);
        }

        [Fact]
        public void raise_generic_does_not_throw_if_event_handler_is_null()
        {
            EventHelper.Raise(null, this, EventArgs.Empty);
        }

        [Fact]
        public void raise_generic_does_not_throw_if_sender_is_null()
        {
            EventHelper.Raise((s, e) => { }, null, EventArgs.Empty);
        }

        [Fact]
        public void raise_generic_does_not_throw_if_event_args_is_null()
        {
            EventHelper.Raise((s, e) => { }, this, (EventArgs)null);
        }

        [Fact]
        public void raise_generic_invokes_event_handler()
        {
            var raised = false;
            EventHelper.Raise((s, e) => raised = true, this, EventArgs.Empty);

            Assert.True(raised);
        }

        [Fact]
        public void raise_generic_invokes_all_event_handlers_in_multicast_delegate()
        {
            var invoked = 0;
            var handler = (EventHandler<EventArgs>)((s, e) => Interlocked.Increment(ref invoked));
            handler += (s, e) => Interlocked.Increment(ref invoked);
            handler += (s, e) => Interlocked.Increment(ref invoked);

            EventHelper.Raise(handler, this, EventArgs.Empty);
            Assert.Equal(3, invoked);
        }

        [Fact]
        public void raise_generic_passes_through_sender()
        {
            object sender = null;
            EventHelper.Raise((s, e) => sender = s, this, EventArgs.Empty);

            Assert.NotNull(sender);
            Assert.Same(this, sender);
        }

        [Fact]
        public void raise_generic_passes_through_event_args()
        {
            var eventArgs = new EventArgs();
            EventArgs receivedEventArgs = null;
            EventHelper.Raise((s, e) => receivedEventArgs = e, this, eventArgs);

            Assert.NotNull(receivedEventArgs);
            Assert.Same(eventArgs, receivedEventArgs);
        }

        [Fact]
        public void raise_generic_with_lazy_event_args_creation_does_not_throw_if_event_handler_is_null()
        {
            EventHelper.Raise(null, this, () => EventArgs.Empty);
        }

        [Fact]
        public void raise_generic_with_lazy_event_args_creation_does_not_throw_if_sender_is_null()
        {
            EventHelper.Raise((s, e) => { }, null, () => EventArgs.Empty);
        }

        [Fact]
        public void raise_generic_with_lazy_event_args_creation_throws_if_creation_delegate_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => EventHelper.Raise<EventArgs>((s, e) => { }, this, (Func<EventArgs>)null));
        }

        [Fact]
        public void raise_generic_with_lazy_event_args_creation_invokes_event_handler()
        {
            var raised = false;
            EventHelper.Raise((s, e) => raised = true, this, () => EventArgs.Empty);

            Assert.True(raised);
        }

        [Fact]
        public void raise_generic_with_lazy_event_args_creation_invokes_all_event_handlers_in_multicast_delegate()
        {
            var invoked = 0;
            var handler = (EventHandler<EventArgs>)((s, e) => Interlocked.Increment(ref invoked));
            handler += (s, e) => Interlocked.Increment(ref invoked);
            handler += (s, e) => Interlocked.Increment(ref invoked);

            EventHelper.Raise(handler, this, () => EventArgs.Empty);
            Assert.Equal(3, invoked);
        }

        [Fact]
        public void raise_generic_with_lazy_event_args_creation_passes_through_sender()
        {
            object sender = null;
            EventHelper.Raise((s, e) => sender = s, this, () => EventArgs.Empty);

            Assert.NotNull(sender);
            Assert.Same(this, sender);
        }

        [Fact]
        public void raise_generic_with_lazy_event_args_creation_passes_through_event_args()
        {
            var eventArgs = new EventArgs();
            EventArgs receivedEventArgs = null;
            EventHelper.Raise((s, e) => receivedEventArgs = e, this, () => eventArgs);

            Assert.NotNull(receivedEventArgs);
            Assert.Same(eventArgs, receivedEventArgs);
        }

        [Fact]
        public void raise_generic_with_lazy_event_args_does_not_invoke_creation_delegate_if_there_is_no_handler()
        {
            var called = false;

            EventHelper.Raise(
                null,
                this,
                () =>
                {
                    called = true;
                    return EventArgs.Empty;
                });

            Assert.False(called);
        }

        [Fact]
        public void begin_raise_does_not_throw_if_event_handler_is_null()
        {
            EventHelper.BeginRaise(null, this, _ => { }, new object());
        }

        [Fact]
        public void begin_raise_does_not_throw_if_sender_is_null()
        {
            EventHelper.BeginRaise((EventHandler)((s, e) => { }), null, _ => { }, new object());
        }

        [Fact]
        public void begin_raise_does_not_throw_if_callback_is_null()
        {
            EventHelper.BeginRaise((EventHandler)((s, e) => { }), null, null, new object());
        }

        [Fact]
        public void begin_raise_does_not_throw_if_async_state_is_null()
        {
            EventHelper.BeginRaise((EventHandler)((s, e) => { }), null, _ => { }, null);
        }

        [Fact]
        public void begin_raise_invokes_event_handler()
        {
            var waitHandle = new ManualResetEventSlim(false);
            EventHelper.BeginRaise((EventHandler)((s, e) => waitHandle.Set()), this, null, null);

            Assert.True(waitHandle.Wait(TimeSpan.FromSeconds(1)));
        }

        [Fact]
        public void begin_raise_invokes_all_event_handlers_in_multicast_delegate()
        {
            var waitHandle1 = new ManualResetEventSlim(false);
            var waitHandle2 = new ManualResetEventSlim(false);
            var waitHandle3 = new ManualResetEventSlim(false);

            var handler = (EventHandler)((s, e) => waitHandle1.Set());
            handler += (s, e) => waitHandle2.Set();
            handler += (s, e) => waitHandle3.Set();

            EventHelper.BeginRaise(handler, this, null, null);

            Assert.True(waitHandle1.Wait(TimeSpan.FromSeconds(1)));
            Assert.True(waitHandle2.Wait(TimeSpan.FromSeconds(1)));
            Assert.True(waitHandle3.Wait(TimeSpan.FromSeconds(1)));
        }

        [Fact]
        public void begin_raise_invokes_callback_after_handler_has_been_called()
        {
            var handlerCalled = new ManualResetEventSlim(false);
            var callbackCalled = new ManualResetEventSlim(false);
            var wasHandlerCalled = false;
            EventHelper.BeginRaise(
                (EventHandler)((s, e) => handlerCalled.Set()),
                this,
                _ =>
                {
                    wasHandlerCalled = handlerCalled.IsSet;
                    callbackCalled.Set();
                },
                null);

            Assert.True(callbackCalled.Wait(TimeSpan.FromSeconds(1)));
            Assert.True(wasHandlerCalled);
        }

        [Fact]
        public void begin_raise_passes_through_sender()
        {
            var waitHandle = new ManualResetEventSlim(false);
            object sender = null;
            EventHelper.BeginRaise(
                (EventHandler)((s, e) =>
                    {
                        sender = s;
                        waitHandle.Set();
                    }),
                this,
                null,
                null);

            Assert.True(waitHandle.Wait(TimeSpan.FromSeconds(1)));
            Assert.NotNull(sender);
            Assert.Same(this, sender);
        }

        [Fact]
        public void begin_raise_passes_through_empty_event_args_by_default()
        {
            var waitHandle = new ManualResetEventSlim(false);
            object eventArgs = null;
            EventHelper.BeginRaise(
                (EventHandler)((s, e) =>
                {
                    eventArgs = e;
                    waitHandle.Set();
                }),
                this,
                null,
                null);

            Assert.True(waitHandle.Wait(TimeSpan.FromSeconds(1)));
            Assert.NotNull(eventArgs);
            Assert.Same(EventArgs.Empty, eventArgs);
        }

        [Fact]
        public void begin_raise_passes_through_async_state_to_callback()
        {
            var waitHandle = new ManualResetEventSlim(false);
            var asyncState = new object();
            object receivedAsyncState = null;
            EventHelper.BeginRaise(
                (EventHandler)((s, e) => { }),
                this,
                ar =>
                {
                    receivedAsyncState = ar.AsyncState;
                    waitHandle.Set();
                },
                asyncState);

            Assert.True(waitHandle.Wait(TimeSpan.FromSeconds(1)));
            Assert.NotNull(receivedAsyncState);
            Assert.Same(asyncState, receivedAsyncState);
        }

        [Fact]
        public void begin_raise_generic_does_not_throw_if_event_handler_is_null()
        {
            EventHelper.BeginRaise(null, this, EventArgs.Empty, _ => { }, new object());
        }

        [Fact]
        public void begin_raise_generic_does_not_throw_if_sender_is_null()
        {
            EventHelper.BeginRaise((s, e) => { }, null, EventArgs.Empty, _ => { }, new object());
        }

        [Fact]
        public void begin_raise_generic_does_not_throw_if_event_args_is_null()
        {
            EventHelper.BeginRaise((s, e) => { }, null, (EventArgs)null, _ => { }, new object());
        }

        [Fact]
        public void begin_raise_generic_does_not_throw_if_callback_is_null()
        {
            EventHelper.BeginRaise((s, e) => { }, null, EventArgs.Empty, null, new object());
        }

        [Fact]
        public void begin_raise_generic_does_not_throw_if_async_state_is_null()
        {
            EventHelper.BeginRaise((s, e) => { }, null, EventArgs.Empty, _ => { }, null);
        }

        [Fact]
        public void begin_raise_generic_invokes_event_handler()
        {
            var waitHandle = new ManualResetEventSlim(false);
            EventHelper.BeginRaise((s, e) => waitHandle.Set(), this, EventArgs.Empty, null, null);

            Assert.True(waitHandle.Wait(TimeSpan.FromSeconds(1)));
        }

        [Fact]
        public void begin_raise_generic_invokes_event_handler_on_different_thread()
        {
            var waitHandle = new ManualResetEventSlim(false);
            var threadId = -1;
            EventHelper.BeginRaise(
                (s, e) =>
                {
                    threadId = Thread.CurrentThread.ManagedThreadId;
                    waitHandle.Set();
                },
                this,
                EventArgs.Empty,
                null,
                null);

            Assert.True(waitHandle.Wait(TimeSpan.FromSeconds(1)));
            Assert.NotEqual(Thread.CurrentThread.ManagedThreadId, threadId);
        }

        [Fact]
        public void begin_raise_generic_invokes_all_event_handlers_in_multicast_delegate()
        {
            var waitHandle1 = new ManualResetEventSlim(false);
            var waitHandle2 = new ManualResetEventSlim(false);
            var waitHandle3 = new ManualResetEventSlim(false);

            var handler = (EventHandler<EventArgs>)((s, e) => waitHandle1.Set());
            handler += (s, e) => waitHandle2.Set();
            handler += (s, e) => waitHandle3.Set();

            EventHelper.BeginRaise(handler, this, EventArgs.Empty, null, null);

            Assert.True(waitHandle1.Wait(TimeSpan.FromSeconds(1)));
            Assert.True(waitHandle2.Wait(TimeSpan.FromSeconds(1)));
            Assert.True(waitHandle3.Wait(TimeSpan.FromSeconds(1)));
        }

        [Fact]
        public void begin_raise_generic_invokes_callback_after_handler_has_been_called()
        {
            var handlerCalled = new ManualResetEventSlim(false);
            var callbackCalled = new ManualResetEventSlim(false);
            var wasHandlerCalled = false;
            EventHelper.BeginRaise(
                (s, e) => handlerCalled.Set(),
                this,
                EventArgs.Empty,
                _ =>
                {
                    wasHandlerCalled = handlerCalled.IsSet;
                    callbackCalled.Set();
                },
                null);

            Assert.True(callbackCalled.Wait(TimeSpan.FromSeconds(1)));
            Assert.True(wasHandlerCalled);
        }

        [Fact]
        public void begin_raise_generic_passes_through_sender()
        {
            var waitHandle = new ManualResetEventSlim(false);
            object sender = null;
            EventHelper.BeginRaise(
                (s, e) =>
                {
                    sender = s;
                    waitHandle.Set();
                },
                this,
                EventArgs.Empty,
                null,
                null);

            Assert.True(waitHandle.Wait(TimeSpan.FromSeconds(1)));
            Assert.NotNull(sender);
            Assert.Same(this, sender);
        }

        [Fact]
        public void begin_raise_generic_passes_through_event_args()
        {
            var waitHandle = new ManualResetEventSlim(false);
            var eventArgs = new EventArgs();
            EventArgs receivedEventArgs = null;
            EventHelper.BeginRaise(
                (s, e) =>
                {
                    receivedEventArgs = e;
                    waitHandle.Set();
                },
                this,
                eventArgs,
                null,
                null);

            Assert.True(waitHandle.Wait(TimeSpan.FromSeconds(1)));
            Assert.NotNull(receivedEventArgs);
            Assert.Same(eventArgs, receivedEventArgs);
        }

        [Fact]
        public void begin_raise_generic_passes_through_async_state_to_callback()
        {
            var waitHandle = new ManualResetEventSlim(false);
            var asyncState = new object();
            object receivedAsyncState = null;
            EventHelper.BeginRaise(
                (s, e) => { },
                this,
                EventArgs.Empty,
                ar =>
                {
                    receivedAsyncState = ar.AsyncState;
                    waitHandle.Set();
                },
                asyncState);

            Assert.True(waitHandle.Wait(TimeSpan.FromSeconds(1)));
            Assert.NotNull(receivedAsyncState);
            Assert.Same(asyncState, receivedAsyncState);
        }

        [Fact]
        public void begin_raise_generic_with_lazy_event_args_creation_does_not_throw_if_event_handler_is_null()
        {
            EventHelper.BeginRaise(null, this, () => EventArgs.Empty, _ => { }, new object());
        }

        [Fact]
        public void begin_raise_generic_with_lazy_event_args_creation_does_not_throw_if_sender_is_null()
        {
            EventHelper.BeginRaise((s, e) => { }, null, () => EventArgs.Empty, _ => { }, new object());
        }

        [Fact]
        public void begin_raise_generic_with_lazy_event_args_creation_throws_if_creation_delegate_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => EventHelper.BeginRaise<EventArgs>((s, e) => { }, this, (Func<EventArgs>)null, _ => { }, new object()));
        }

        [Fact]
        public void begin_raise_generic_with_lazy_event_args_creation_does_not_throw_if_callback_is_null()
        {
            EventHelper.BeginRaise((s, e) => { }, this, () => EventArgs.Empty, null, new object());
        }

        [Fact]
        public void begin_raise_generic_with_lazy_event_args_creation_does_not_throw_if_async_state_is_null()
        {
            EventHelper.BeginRaise((s, e) => { }, this, () => EventArgs.Empty, _ => { }, null);
        }

        [Fact]
        public void begin_raise_generic_with_lazy_event_args_creation_invokes_event_handler()
        {
            var waitHandle = new ManualResetEventSlim(false);
            EventHelper.BeginRaise((s, e) => waitHandle.Set(), this, () => EventArgs.Empty, null, null);

            Assert.True(waitHandle.Wait(TimeSpan.FromSeconds(1)));
        }

        [Fact]
        public void begin_raise_generic_with_lazy_event_args_creation_invokes_event_handler_on_different_thread()
        {
            var waitHandle = new ManualResetEventSlim(false);
            var threadId = -1;
            EventHelper.BeginRaise((s, e) =>
                {
                    threadId = Thread.CurrentThread.ManagedThreadId;
                    waitHandle.Set();
                },
                this,
                () => EventArgs.Empty,
                null,
                null);

            Assert.True(waitHandle.Wait(TimeSpan.FromSeconds(1)));
            Assert.NotEqual(Thread.CurrentThread.ManagedThreadId, threadId);
        }

        [Fact]
        public void begin_raise_generic_with_lazy_event_args_creation_invokes_all_event_handlers_in_multicast_delegate()
        {
            var waitHandle1 = new ManualResetEventSlim(false);
            var waitHandle2 = new ManualResetEventSlim(false);
            var waitHandle3 = new ManualResetEventSlim(false);

            var handler = (EventHandler<EventArgs>)((s, e) => waitHandle1.Set());
            handler += (s, e) => waitHandle2.Set();
            handler += (s, e) => waitHandle3.Set();

            EventHelper.BeginRaise(handler, this, () => EventArgs.Empty, null, null);

            Assert.True(waitHandle1.Wait(TimeSpan.FromSeconds(1)));
            Assert.True(waitHandle2.Wait(TimeSpan.FromSeconds(1)));
            Assert.True(waitHandle3.Wait(TimeSpan.FromSeconds(1)));
        }

        [Fact]
        public void begin_raise_generic_with_lazy_event_args_creation_invokes_callback_after_handler_has_been_called()
        {
            var handlerCalled = new ManualResetEventSlim(false);
            var callbackCalled = new ManualResetEventSlim(false);
            var wasHandlerCalled = false;
            EventHelper.BeginRaise(
                (s, e) => handlerCalled.Set(),
                this,
                () => EventArgs.Empty,
                _ =>
                {
                    wasHandlerCalled = handlerCalled.IsSet;
                    callbackCalled.Set();
                },
                null);

            Assert.True(callbackCalled.Wait(TimeSpan.FromSeconds(1)));
            Assert.True(wasHandlerCalled);
        }

        [Fact]
        public void begin_raise_generic_with_lazy_event_args_creation_passes_through_sender()
        {
            var waitHandle = new ManualResetEventSlim(false);
            object sender = null;
            EventHelper.BeginRaise(
                (s, e) =>
                {
                    sender = s;
                    waitHandle.Set();
                },
                this,
                () => EventArgs.Empty,
                null,
                null);

            Assert.True(waitHandle.Wait(TimeSpan.FromSeconds(1)));
            Assert.NotNull(sender);
            Assert.Same(this, sender);
        }

        [Fact]
        public void begin_raise_generic_with_lazy_event_args_creation_passes_through_event_args()
        {
            var waitHandle = new ManualResetEventSlim(false);
            var eventArgs = new EventArgs();
            EventArgs receivedEventArgs = null;
            EventHelper.BeginRaise(
                (s, e) =>
                {
                    receivedEventArgs = e;
                    waitHandle.Set();
                },
                this,
                () => eventArgs,
                null,
                null);

            Assert.True(waitHandle.Wait(TimeSpan.FromSeconds(1)));
            Assert.NotNull(receivedEventArgs);
            Assert.Same(eventArgs, receivedEventArgs);
        }

        [Fact]
        public void begin_raise_generic_with_lazy_event_args_does_not_invoke_creation_delegate_if_there_is_no_handler()
        {
            var called = false;

            EventHelper.BeginRaise(
                null,
                this,
                () =>
                {
                    called = true;
                    return EventArgs.Empty;
                },
                null,
                null);

            Assert.False(called);
        }

        [Fact]
        public void begin_raise_generic_with_lazy_event_args_passes_through_async_state_to_callback()
        {
            var waitHandle = new ManualResetEventSlim(false);
            var asyncState = new object();
            object receivedAsyncState = null;
            EventHelper.BeginRaise(
                (s, e) => { },
                this,
                () => EventArgs.Empty,
                ar =>
                {
                    receivedAsyncState = ar.AsyncState;
                    waitHandle.Set();
                },
                asyncState);

            Assert.True(waitHandle.Wait(TimeSpan.FromSeconds(1)));
            Assert.NotNull(receivedAsyncState);
            Assert.Same(asyncState, receivedAsyncState);
        }
    }
}