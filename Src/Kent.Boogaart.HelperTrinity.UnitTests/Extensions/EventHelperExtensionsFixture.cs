namespace Kent.Boogaart.HelperTrinity.UnitTests.Extensions
{
    using Kent.Boogaart.HelperTrinity.Extensions;
    using System;
    using System.Threading;
    using Xunit;

    public sealed class EventHelperExtensionsFixture
    {
        [Fact]
        public void raise_does_not_throw_if_event_handler_is_null()
        {
            EventHandler handler = null;
            handler.Raise(this);
        }

        [Fact]
        public void raise_does_not_throw_if_sender_is_null()
        {
            EventHandler handler = (s, e) => { };
            handler.Raise(null);
        }

        [Fact]
        public void raise_invokes_event_handler()
        {
            var raised = false;
            EventHandler handler = (s, e) => raised = true;
            handler.Raise(this);

            Assert.True(raised);
        }

        [Fact]
        public void raise_invokes_all_event_handlers_in_multicast_delegate()
        {
            var invoked = 0;
            EventHandler handler = (s, e) => Interlocked.Increment(ref invoked);
            handler += (s, e) => Interlocked.Increment(ref invoked);
            handler += (s, e) => Interlocked.Increment(ref invoked);

            handler.Raise(this);
            Assert.Equal(3, invoked);
        }

        [Fact]
        public void raise_passes_through_sender()
        {
            object sender = null;
            EventHandler handler = (s, e) => sender = s;
            handler.Raise(this);

            Assert.NotNull(sender);
            Assert.Same(this, sender);
        }

        [Fact]
        public void raise_passes_through_empty_event_args_by_default()
        {
            object eventArgs = null;
            EventHandler handler = (s, e) => eventArgs = e;
            handler.Raise(this);

            Assert.NotNull(eventArgs);
            Assert.Same(EventArgs.Empty, eventArgs);
        }

        [Fact]
        public void raise_generic_does_not_throw_if_event_handler_is_null()
        {
            EventHandler<EventArgs> handler = null;
            handler.Raise(this, EventArgs.Empty);
        }

        [Fact]
        public void raise_generic_does_not_throw_if_sender_is_null()
        {
            EventHandler<EventArgs> handler = (s, e) => { };
            handler.Raise(null, EventArgs.Empty);
        }

        [Fact]
        public void raise_generic_does_not_throw_if_event_args_is_null()
        {
            EventHandler<EventArgs> handler = (s, e) => { };
            handler.Raise(this, (EventArgs)null);
        }

        [Fact]
        public void raise_generic_invokes_event_handler()
        {
            var raised = false;
            EventHandler<EventArgs> handler = (s, e) => raised = true;
            handler.Raise(this, EventArgs.Empty);

            Assert.True(raised);
        }

        [Fact]
        public void raise_generic_invokes_all_event_handlers_in_multicast_delegate()
        {
            var invoked = 0;
            var handler = (EventHandler<EventArgs>)((s, e) => Interlocked.Increment(ref invoked));
            handler += (s, e) => Interlocked.Increment(ref invoked);
            handler += (s, e) => Interlocked.Increment(ref invoked);

            handler.Raise(this, EventArgs.Empty);
            Assert.Equal(3, invoked);
        }

        [Fact]
        public void raise_generic_passes_through_sender()
        {
            object sender = null;
            EventHandler<EventArgs> handler = (s, e) => sender = s;
            handler.Raise(this, EventArgs.Empty);

            Assert.NotNull(sender);
            Assert.Same(this, sender);
        }

        [Fact]
        public void raise_generic_passes_through_event_args()
        {
            var eventArgs = new EventArgs();
            EventArgs receivedEventArgs = null;
            EventHandler<EventArgs> handler = (s, e) => receivedEventArgs = e;
            handler.Raise(this, eventArgs);

            Assert.NotNull(receivedEventArgs);
            Assert.Same(eventArgs, receivedEventArgs);
        }

        [Fact]
        public void raise_generic_with_lazy_event_args_creation_does_not_throw_if_event_handler_is_null()
        {
            EventHandler<EventArgs> handler = null;
            handler.Raise(this, () => EventArgs.Empty);
        }

        [Fact]
        public void raise_generic_with_lazy_event_args_creation_does_not_throw_if_sender_is_null()
        {
            EventHandler<EventArgs> handler = (s, e) => { };
            handler.Raise(null, () => EventArgs.Empty);
        }

        [Fact]
        public void raise_generic_with_lazy_event_args_creation_throws_if_creation_delegate_is_null()
        {
            EventHandler<EventArgs> handler = (s, e) => { };
            Assert.Throws<ArgumentNullException>(() => handler.Raise<EventArgs>(this, (Func<EventArgs>)null));
        }

        [Fact]
        public void raise_generic_with_lazy_event_args_creation_invokes_event_handler()
        {
            var raised = false;
            EventHandler<EventArgs> handler = (s, e) => raised = true;
            handler.Raise(this, () => EventArgs.Empty);

            Assert.True(raised);
        }

        [Fact]
        public void raise_generic_with_lazy_event_args_creation_invokes_all_event_handlers_in_multicast_delegate()
        {
            var invoked = 0;
            EventHandler<EventArgs> handler = (s, e) => Interlocked.Increment(ref invoked);
            handler += (s, e) => Interlocked.Increment(ref invoked);
            handler += (s, e) => Interlocked.Increment(ref invoked);

            handler.Raise(this, () => EventArgs.Empty);
            Assert.Equal(3, invoked);
        }

        [Fact]
        public void raise_generic_with_lazy_event_args_creation_passes_through_sender()
        {
            object sender = null;
            EventHandler<EventArgs> handler = (s, e) => sender = s;
            handler.Raise(this, () => EventArgs.Empty);

            Assert.NotNull(sender);
            Assert.Same(this, sender);
        }

        [Fact]
        public void raise_generic_with_lazy_event_args_creation_passes_through_event_args()
        {
            var eventArgs = new EventArgs();
            EventArgs receivedEventArgs = null;
            EventHandler<EventArgs> handler = (s, e) => receivedEventArgs = e;
            handler.Raise(this, () => eventArgs);

            Assert.NotNull(receivedEventArgs);
            Assert.Same(eventArgs, receivedEventArgs);
        }

        [Fact]
        public void raise_generic_with_lazy_event_args_does_not_invoke_creation_delegate_if_there_is_no_handler()
        {
            var called = false;
            EventHandler<EventArgs> handler = null;

            handler.Raise(
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
            EventHandler handler = null;
            handler.BeginRaise(this, _ => { }, new object());
        }

        [Fact]
        public void begin_raise_does_not_throw_if_sender_is_null()
        {
            EventHandler handler = (s, e) => { };
            handler.BeginRaise(null, _ => { }, new object());
        }

        [Fact]
        public void begin_raise_does_not_throw_if_callback_is_null()
        {
            EventHandler handler = (s, e) => { };
            handler.BeginRaise(null, null, new object());
        }

        [Fact]
        public void begin_raise_does_not_throw_if_async_state_is_null()
        {
            EventHandler handler = (s, e) => { };
            handler.BeginRaise(null, _ => { }, null);
        }

        [Fact]
        public void begin_raise_invokes_event_handler()
        {
            var waitHandle = new ManualResetEventSlim(false);
            EventHandler handler = (s, e) => waitHandle.Set();
            handler.BeginRaise(this, null, null);

            Assert.True(waitHandle.Wait(TimeSpan.FromSeconds(1)));
        }

        [Fact]
        public void begin_raise_invokes_all_event_handlers_in_multicast_delegate()
        {
            var waitHandle1 = new ManualResetEventSlim(false);
            var waitHandle2 = new ManualResetEventSlim(false);
            var waitHandle3 = new ManualResetEventSlim(false);

            EventHandler handler = ((s, e) => waitHandle1.Set());
            handler += (s, e) => waitHandle2.Set();
            handler += (s, e) => waitHandle3.Set();

            handler.BeginRaise(this, null, null);

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
            EventHandler handler = ((s, e) => handlerCalled.Set());
            handler.BeginRaise(
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
            EventHandler handler = (s, e) =>
                {
                    sender = s;
                    waitHandle.Set();
                };
            handler.BeginRaise(
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
            EventHandler handler = (s, e) =>
            {
                eventArgs = e;
                waitHandle.Set();
            };
            handler.BeginRaise(
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
            EventHandler handler = (s, e) => { };
            handler.BeginRaise(
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
            EventHandler<EventArgs> handler = null;
            handler.BeginRaise(this, EventArgs.Empty, _ => { }, new object());
        }

        [Fact]
        public void begin_raise_generic_does_not_throw_if_sender_is_null()
        {
            EventHandler<EventArgs> handler = (s, e) => { };
            handler.BeginRaise(null, EventArgs.Empty, _ => { }, new object());
        }

        [Fact]
        public void begin_raise_generic_does_not_throw_if_event_args_is_null()
        {
            EventHandler<EventArgs> handler = (s, e) => { };
            handler.BeginRaise(null, (EventArgs)null, _ => { }, new object());
        }

        [Fact]
        public void begin_raise_generic_does_not_throw_if_callback_is_null()
        {
            EventHandler<EventArgs> handler = (s, e) => { };
            handler.BeginRaise(null, EventArgs.Empty, null, new object());
        }

        [Fact]
        public void begin_raise_generic_does_not_throw_if_async_state_is_null()
        {
            EventHandler<EventArgs> handler = (s, e) => { };
            handler.BeginRaise(null, EventArgs.Empty, _ => { }, null);
        }

        [Fact]
        public void begin_raise_generic_invokes_event_handler()
        {
            var waitHandle = new ManualResetEventSlim(false);
            EventHandler<EventArgs> handler = (s, e) => waitHandle.Set();
            handler.BeginRaise(this, EventArgs.Empty, null, null);

            Assert.True(waitHandle.Wait(TimeSpan.FromSeconds(1)));
        }

        [Fact]
        public void begin_raise_generic_invokes_all_event_handlers_in_multicast_delegate()
        {
            var waitHandle1 = new ManualResetEventSlim(false);
            var waitHandle2 = new ManualResetEventSlim(false);
            var waitHandle3 = new ManualResetEventSlim(false);

            EventHandler<EventArgs> handler = (s, e) => waitHandle1.Set();
            handler += (s, e) => waitHandle2.Set();
            handler += (s, e) => waitHandle3.Set();

            handler.BeginRaise(this, EventArgs.Empty, null, null);

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
            EventHandler<EventArgs> handler = (s, e) => handlerCalled.Set();
            handler.BeginRaise(
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
            EventHandler<EventArgs> handler = (s, e) =>
                {
                    sender = s;
                    waitHandle.Set();
                };
            handler.BeginRaise(
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
            EventHandler<EventArgs> handler = (s, e) =>
            {
                receivedEventArgs = e;
                waitHandle.Set();
            };
            handler.BeginRaise(
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
            EventHandler<EventArgs> handler = (s, e) => { };
            handler.BeginRaise(
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
            EventHandler<EventArgs> handler = null;
            handler.BeginRaise(this, () => EventArgs.Empty, _ => { }, new object());
        }

        [Fact]
        public void begin_raise_generic_with_lazy_event_args_creation_does_not_throw_if_sender_is_null()
        {
            EventHandler<EventArgs> handler = (s, e) => { };
            handler.BeginRaise(null, () => EventArgs.Empty, _ => { }, new object());
        }

        [Fact]
        public void begin_raise_generic_with_lazy_event_args_creation_throws_if_creation_delegate_is_null()
        {
            EventHandler<EventArgs> handler = (s, e) => { };
            Assert.Throws<ArgumentNullException>(() => handler.BeginRaise<EventArgs>(this, (Func<EventArgs>)null, _ => { }, new object()));
        }

        [Fact]
        public void begin_raise_generic_with_lazy_event_args_creation_does_not_throw_if_callback_is_null()
        {
            EventHandler<EventArgs> handler = (s, e) => { };
            handler.BeginRaise(this, () => EventArgs.Empty, null, new object());
        }

        [Fact]
        public void begin_raise_generic_with_lazy_event_args_creation_does_not_throw_if_async_state_is_null()
        {
            EventHandler<EventArgs> handler = (s, e) => { };
            handler.BeginRaise(this, () => EventArgs.Empty, _ => { }, null);
        }

        [Fact]
        public void begin_raise_generic_with_lazy_event_args_creation_invokes_event_handler()
        {
            var waitHandle = new ManualResetEventSlim(false);
            EventHandler<EventArgs> handler = (s, e) => waitHandle.Set();
            handler.BeginRaise(this, () => EventArgs.Empty, null, null);

            Assert.True(waitHandle.Wait(TimeSpan.FromSeconds(1)));
        }

        [Fact]
        public void begin_raise_generic_with_lazy_event_args_creation_invokes_all_event_handlers_in_multicast_delegate()
        {
            var waitHandle1 = new ManualResetEventSlim(false);
            var waitHandle2 = new ManualResetEventSlim(false);
            var waitHandle3 = new ManualResetEventSlim(false);

            EventHandler<EventArgs> handler = (s, e) => waitHandle1.Set();
            handler += (s, e) => waitHandle2.Set();
            handler += (s, e) => waitHandle3.Set();

            handler.BeginRaise(this, () => EventArgs.Empty, null, null);

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
            EventHandler<EventArgs> handler = (s, e) => handlerCalled.Set();
            handler.BeginRaise(
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
            EventHandler<EventArgs> handler = (s, e) =>
                {
                    sender = s;
                    waitHandle.Set();
                };
            handler.BeginRaise(
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
            EventHandler<EventArgs> handler = (s, e) =>
            {
                receivedEventArgs = e;
                waitHandle.Set();
            };
            handler.BeginRaise(
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
            EventHandler<EventArgs> handler = null;

            handler.BeginRaise(
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
            EventHandler<EventArgs> handler = (s, e) => { };
            handler.BeginRaise(
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