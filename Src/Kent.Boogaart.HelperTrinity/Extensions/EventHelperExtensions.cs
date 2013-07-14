namespace Kent.Boogaart.HelperTrinity.Extensions
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Defines extension methods for the <see cref="EventHelper"/> class.
    /// </summary>
    /// <remarks>
    /// This class defines extensions methods for the <see cref="EventHelper"/>. All extension methods simply delegate to the
    /// appropriate member of the <see cref="EventHelper"/> class.
    /// </remarks>
    public static class EventHelperExtensions
    {
        /// <include file='../EventHelper.doc.xml' path='doc/member[@name="Raise(EventHandler,object)"]/*' />
        [DebuggerHidden]
        public static void Raise(this EventHandler handler, object sender)
        {
            EventHelper.Raise(handler, sender);
        }

        /// <include file='../EventHelper.doc.xml' path='doc/member[@name="Raise{T}(EventHandler{T},object,T)"]/*' />
        [DebuggerHidden]
        public static void Raise<T>(this EventHandler<T> handler, object sender, T e)
            where T : EventArgs
        {
            EventHelper.Raise(handler, sender, e);
        }

        /// <include file='../EventHelper.doc.xml' path='doc/member[@name="Raise{T}(EventHandler{T},object,Func{T})"]/*' />
        [DebuggerHidden]
        public static void Raise<T>(this EventHandler<T> handler, object sender, Func<T> createEventArguments)
            where T : EventArgs
        {
            EventHelper.Raise(handler, sender, createEventArguments);
        }

        /// <include file='../EventHelper.doc.xml' path='doc/member[@name="Raise(Delegate,object,EventArgs)"]/*' />
        [DebuggerHidden]
        public static void Raise(this Delegate handler, object sender, EventArgs e)
        {
            EventHelper.Raise(handler, sender, e);
        }

        /// <include file='../EventHelper.doc.xml' path='doc/member[@name="BeginRaise(EventHandler,object,AsyncCallback,object)"]/*' />
        [DebuggerHidden]
        public static void BeginRaise(this EventHandler handler, object sender, AsyncCallback callback, object asyncState)
        {
            EventHelper.BeginRaise(handler, sender, callback, asyncState);
        }

        /// <include file='../EventHelper.doc.xml' path='doc/member[@name="BeginRaise{T}(EventHandler{T},object,T,AsyncCallback,object)"]/*' />
        [DebuggerHidden]
        public static void BeginRaise<T>(this EventHandler<T> handler, object sender, T e, AsyncCallback callback, object asyncState)
            where T : EventArgs
        {
            EventHelper.BeginRaise(handler, sender, e, callback, asyncState);
        }

        /// <include file='../EventHelper.doc.xml' path='doc/member[@name="BeginRaise{T}(EventHandler{T},object,Func{T},AsyncCallback,object)"]/*' />
        [DebuggerHidden]
        public static void BeginRaise<T>(this EventHandler<T> handler, object sender, Func<T> createEventArguments, AsyncCallback callback, object asyncState)
            where T : EventArgs
        {
            EventHelper.BeginRaise(handler, sender, createEventArguments, callback, asyncState);
        }

        /// <include file='../EventHelper.doc.xml' path='doc/member[@name="BeginRaise(Delegate,object,EventArgs,AsyncCallback,object)"]/*' />
        [DebuggerHidden]
        public static void BeginRaise(this Delegate handler, object sender, EventArgs e, AsyncCallback callback, object asyncState)
        {
            EventHelper.BeginRaise(handler, sender, e, callback, asyncState);
        }
    }
}