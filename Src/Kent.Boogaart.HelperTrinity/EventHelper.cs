namespace Kent.Boogaart.HelperTrinity
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Provides helper methods for raising events.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <c>EventHelper</c> class provides methods that can be used to raise events. It avoids the need for explicitly checking event sinks for <see langword="null"/> before raising the event.
    /// </para>
    /// <para>
    /// The <see cref="Raise"/> overloads raise an event synchronously. All handlers will be invoked one after the other on the calling thread. The <see cref="BeginRaise"/> overloads raise an event asynchronously.
    /// All handlers will be invoked one after the other on a background thread.
    /// </para>
    /// </remarks>
    /// <example>
    /// The following example shows how a non-generic event can be raised:
    /// <code>
    /// public event EventHandler Changed;
    /// 
    /// protected void OnChanged()
    /// {
    ///        EventHelper.Raise(Changed, this);
    /// }
    /// </code>
    /// </example>
    /// <example>
    /// The following example shows how a non-generic event can be raised where the event type requires a specific
    /// <c>EventArgs</c> subclass:
    /// <code>
    /// public event PropertyChangedEventHandler PropertyChanged;
    /// 
    /// protected void OnPropertyChanged(PropertyChangedEventArgs e)
    /// {
    ///        EventHelper.Raise(PropertyChanged, this, e);
    /// }
    /// </code>
    /// </example>
    /// <example>
    /// The following example shows how a generic event can be raised:
    /// <code>
    /// public event EventHandler&lt;EventArgs&gt; Changed;
    /// 
    /// protected void OnChanged()
    /// {
    ///        EventHelper.Raise(Changed, this, EventArgs.Empty);
    /// }
    /// </code>
    /// </example>
    /// <example>
    /// The following example shows how a generic event with custom event arguments can be raised:
    /// <code>
    /// public event EventHandler&lt;MyEventArgs&gt; MyEvent;
    /// 
    /// protected void OnMyEvent(MyEventArgs e)
    /// {
    ///        EventHelper.Raise(MyEventArgs, this, e);
    /// }
    /// </code>
    /// </example>
    /// <example>
    /// The following example raises a generic event, but does not create the event arguments unless there is at least one
    /// handler for the event:
    /// <code>
    /// public event EventHandler&lt;MyEventArgs&gt; MyEvent;
    /// 
    /// protected void OnMyEvent(int someData)
    /// {
    ///        EventHelper.Raise(MyEvent, this, delegate
    ///        {
    ///            return new MyEventArgs(someData);
    ///        });
    /// }
    /// </code>
    /// </example>
    public static class EventHelper
    {
        /// <include file='EventHelper.doc.xml' path='doc/member[@name="Raise(EventHandler,object)"]/*' />
        [SuppressMessage("Microsoft.Design", "CA1030", Justification = "False positive - the Raise method overloads are supposed to raise an event on behalf of a client, not on behalf of its declaring class.")]
        [DebuggerHidden]
        public static void Raise(EventHandler handler, object sender)
        {
            if (handler != null)
            {
                handler(sender, EventArgs.Empty);
            }
        }

        /// <include file='EventHelper.doc.xml' path='doc/member[@name="Raise{T}(EventHandler{T},object,T)"]/*' />
        [SuppressMessage("Microsoft.Design", "CA1030", Justification = "False positive - the Raise method overloads are supposed to raise an event on behalf of a client, not on behalf of its declaring class.")]
        [DebuggerHidden]
        public static void Raise<T>(EventHandler<T> handler, object sender, T e)
            where T : EventArgs
        {
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        /// <include file='EventHelper.doc.xml' path='doc/member[@name="Raise{T}(EventHandler{T},object,CreateEventArguments{T})"]/*' />
        [SuppressMessage("Microsoft.Design", "CA1030", Justification = "False positive - the Raise method overloads are supposed to raise an event on behalf of a client, not on behalf of its declaring class.")]
        [DebuggerHidden]
        public static void Raise<T>(EventHandler<T> handler, object sender, CreateEventArguments<T> createEventArguments)
            where T : EventArgs
        {
            ArgumentHelper.AssertNotNull(createEventArguments, "createEventArguments");

            if (handler != null)
            {
                handler(sender, createEventArguments());
            }
        }

        /// <include file='EventHelper.doc.xml' path='doc/member[@name="Raise(Delegate,object,EventArgs)"]/*' />
        [SuppressMessage("Microsoft.Design", "CA1030", Justification = "False positive - the Raise method overloads are supposed to raise an event on behalf of a client, not on behalf of its declaring class.")]
        [DebuggerHidden]
        public static void Raise(Delegate handler, object sender, EventArgs e)
        {
            if (handler != null)
            {
                handler.DynamicInvoke(sender, e);
            }
        }

        /// <include file='EventHelper.doc.xml' path='doc/member[@name="BeginRaise(EventHandler,object,AsyncCallback,object)"]/*' />
        [SuppressMessage("Microsoft.Design", "CA1030", Justification = "False positive - the Raise method overloads are supposed to raise an event on behalf of a client, not on behalf of its declaring class.")]
        [DebuggerHidden]
        public static void BeginRaise(EventHandler handler, object sender, AsyncCallback callback, object asyncState)
        {
            if (handler != null)
            {
                ((Action<EventHandler, object>)Raise).BeginInvoke(handler, sender, callback, asyncState);
            }
        }

        /// <include file='EventHelper.doc.xml' path='doc/member[@name="BeginRaise{T}(EventHandler{T},object,T,AsyncCallback,object)"]/*' />
        [SuppressMessage("Microsoft.Design", "CA1030", Justification = "False positive - the Raise method overloads are supposed to raise an event on behalf of a client, not on behalf of its declaring class.")]
        [DebuggerHidden]
        public static void BeginRaise<T>(EventHandler<T> handler, object sender, T e, AsyncCallback callback, object asyncState)
            where T : EventArgs
        {
            if (handler != null)
            {
                ((Action<EventHandler<T>, object, T>)Raise).BeginInvoke(handler, sender, e, callback, asyncState);
            }
        }

        /// <include file='EventHelper.doc.xml' path='doc/member[@name="BeginRaise{T}(EventHandler{T},object,CreateEventArguments{T},AsyncCallback,object)"]/*' />
        [SuppressMessage("Microsoft.Design", "CA1030", Justification = "False positive - the Raise method overloads are supposed to raise an event on behalf of a client, not on behalf of its declaring class.")]
        [DebuggerHidden]
        public static void BeginRaise<T>(EventHandler<T> handler, object sender, CreateEventArguments<T> createEventArguments, AsyncCallback callback, object asyncState)
            where T : EventArgs
        {
            ArgumentHelper.AssertNotNull(createEventArguments, "createEventArguments");

            if (handler != null)
            {
                ((Action<EventHandler<T>, object, CreateEventArguments<T>>)Raise).BeginInvoke(handler, sender, createEventArguments, callback, asyncState);
            }
        }

        /// <include file='EventHelper.doc.xml' path='doc/member[@name="BeginRaise(Delegate,object,EventArgs,AsyncCallback,object)"]/*' />
        [SuppressMessage("Microsoft.Design", "CA1030", Justification = "False positive - the Raise method overloads are supposed to raise an event on behalf of a client, not on behalf of its declaring class.")]
        [DebuggerHidden]
        public static void BeginRaise(Delegate handler, object sender, EventArgs e, AsyncCallback callback, object asyncState)
        {
            if (handler != null)
            {
                ((Action<Delegate, object, EventArgs>)Raise).BeginInvoke(handler, sender, e, callback, asyncState);
            }
        }

        /// <summary>
        /// A handler used to create an event arguments instance for the <see cref="Raise{T}(EventHandler{T}, object, CreateEventArguments{T})"/> method.
        /// </summary>
        /// <remarks>
        /// This delegate is invoked by the <see cref="Raise{T}(EventHandler{T}, object, CreateEventArguments{T})"/> method to create the event arguments instance.
        /// The handler should create the instance and return it.
        /// </remarks>
        /// <typeparam name="T">
        /// The event arguments type.
        /// </typeparam>
        /// <returns>
        /// The event arguments instance.
        /// </returns>
        public delegate T CreateEventArguments<T>()
            where T : EventArgs;
    }
}
