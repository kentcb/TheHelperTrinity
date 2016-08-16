namespace HelperTrinity
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Defines extension methods for the <see cref="ArgumentHelper"/> class.
    /// </summary>
    /// <remarks>
    /// This class defines extensions methods for the <see cref="ArgumentHelper"/>. All extension methods simply delegate to the
    /// appropriate member of the <see cref="ArgumentHelper"/> class.
    /// </remarks>
    /// <example>
    /// The following code ensures that the <c>name</c> argument is not <see langword="null"/>:
    /// <code>
    /// public void DisplayDetails(string name)
    /// {
    ///     name.AssertNotNull("name");
    ///     //now we know that name is not null
    ///     ...
    /// }
    /// </code>
    /// </example>
    public static class ArgumentHelperExtensions
    {
        /// <include file='ArgumentHelper.doc.xml' path='doc/member[@name="AssertNotNull{T}(T,string)"]/*' />
        [DebuggerHidden]
        public static void AssertNotNull<T>(this T arg, string argName)
            where T : class
        {
            ArgumentHelper.AssertNotNull(arg, argName);
        }

        /// <include file='ArgumentHelper.doc.xml' path='doc/member[@name="AssertNotNull{T}(Nullable{T},string)"]/*' />
        [DebuggerHidden]
        public static void AssertNotNull<T>(this T? arg, string argName)
            where T : struct
        {
            ArgumentHelper.AssertNotNull(arg, argName);
        }

        /// <include file='ArgumentHelper.doc.xml' path='doc/member[@name="AssertGenericArgumentNotNull{T}(T,string)"]/*' />
        [DebuggerHidden]
        public static void AssertGenericArgumentNotNull<T>(this T arg, string argName)
        {
            ArgumentHelper.AssertGenericArgumentNotNull(arg, argName);
        }

        /// <include file='ArgumentHelper.doc.xml' path='doc/member[@name="AssertNotNull{T}(IEnumerable{T},string,bool)"]/*' />
        [DebuggerHidden]
        public static void AssertNotNull<T>(this IEnumerable<T> arg, string argName, bool assertContentsNotNull)
        {
            ArgumentHelper.AssertNotNull(arg, argName, assertContentsNotNull);
        }

        /// <include file='ArgumentHelper.doc.xml' path='doc/member[@name="AssertNotNullOrEmpty(string,string)"]/*' />
        [DebuggerHidden]
        public static void AssertNotNullOrEmpty(this string arg, string argName)
        {
            ArgumentHelper.AssertNotNullOrEmpty(arg, argName);
        }

        /// <include file='ArgumentHelper.doc.xml' path='doc/member[@name="AssertNotNullOrEmpty(IEnumerable,string)"]/*' />
        [DebuggerHidden]
        public static void AssertNotNullOrEmpty(this IEnumerable arg, string argName)
        {
            ArgumentHelper.AssertNotNullOrEmpty(arg, argName);
        }

        /// <include file='ArgumentHelper.doc.xml' path='doc/member[@name="AssertNotNullOrEmpty(ICollection,string)"]/*' />
        [DebuggerHidden]
        public static void AssertNotNullOrEmpty(this ICollection arg, string argName)
        {
            ArgumentHelper.AssertNotNullOrEmpty(arg, argName);
        }

        /// <include file='ArgumentHelper.doc.xml' path='doc/member[@name="AssertNotNullOrWhiteSpace(string,string)"]/*' />
        [DebuggerHidden]
        public static void AssertNotNullOrWhiteSpace(this string arg, string argName)
        {
            ArgumentHelper.AssertNotNullOrWhiteSpace(arg, argName);
        }

        /// <include file='ArgumentHelper.doc.xml' path='doc/member[@name="AssertEnumMember{TEnum}(TEnum,string)"]/*' />
        [DebuggerHidden]
        [CLSCompliant(false)]
        public static void AssertEnumMember<TEnum>(this TEnum enumValue, string argName)
            where TEnum : struct
        {
            ArgumentHelper.AssertEnumMember(enumValue, argName);
        }

        /// <include file='ArgumentHelper.doc.xml' path='doc/member[@name="AssertEnumMember{TEnum}(TEnum,string,TEnum[])"]/*' />
        [DebuggerHidden]
        [CLSCompliant(false)]
        public static void AssertEnumMember<TEnum>(this TEnum enumValue, string argName, params TEnum[] validValues)
            where TEnum : struct
        {
            ArgumentHelper.AssertEnumMember(enumValue, argName, validValues);
        }
    }
}