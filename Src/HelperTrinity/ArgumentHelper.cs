using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace Kent.Boogaart.HelperTrinity
{
	/// <summary>
	/// Provides helper methods for asserting arguments.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This class provides helper methods for asserting the validity of arguments. It can be used to reduce the number of
	/// laborious <c>if</c>, <c>throw</c> sequences in your code.
	/// </para>
	/// <para>
	/// The <see cref="AssertNotNull"/> methods can be used to ensure that arguments are not <see langword="null"/>. The
	/// <see cref="AssertNotNullOrEmpty"/> overloads can be used to ensure that strings are not <see langword="null"/> or empty.
	/// The <see cref="AssertEnumMember"/> overloads can be used to assert the validity of enumeration arguments.
	/// </para>
	/// </remarks>
	/// <example>
	/// The following code ensures that the <c>name</c> argument is not <see langword="null"/>:
	/// <code>
	/// public void DisplayDetails(string name)
	/// {
	///		ArgumentHelper.AssertNotNull(name, "name");
	///		//now we know that name is not null
	///		...
	/// }
	/// </code>
	/// </example>
	/// <example>
	/// The following code ensures that the <c>name</c> argument is not <see langword="null"/> or an empty <c>string</c>:
	/// <code>
	/// public void DisplayDetails(string name)
	/// {
	///		ArgumentHelper.AssertNotNullOrEmpty(name, "name", true);
	///		//now we know that name is not null and is not an empty string (or blank)
	///		...
	/// }
	/// </code>
	/// </example>
	/// <example>
	/// The following code ensures that the <c>day</c> parameter is a valid member of its enumeration:
	/// <code>
	/// public void DisplayInformation(DayOfWeek day)
	/// {
	///		ArgumentHelper.AssertEnumMember(day);
	///		//now we know that day is a valid member of DayOfWeek
	///		...
	/// }
	/// </code>
	/// </example>
	/// <example>
	/// The following code ensures that the <c>day</c> parameter is either DayOfWeek.Monday or DayOfWeek.Thursday:
	/// <code>
	/// public void DisplayInformation(DayOfWeek day)
	/// {
	///		ArgumentHelper.AssertEnumMember(day, DayOfWeek.Monday, DayOfWeek.Thursday);
	///		//now we know that day is either Monday or Thursday
	///		...
	/// }
	/// </code>
	/// </example>
	/// <example>
	/// The following code ensures that the <c>bindingFlags</c> parameter is either BindingFlags.Public, BindingFlags.NonPublic
	/// or both:
	/// <code>
	/// public void GetInformation(BindingFlags bindingFlags)
	/// {
	///		ArgumentHelper.AssertEnumMember(bindingFlags, BindingFlags.Public, BindingFlags.NonPublic);
	///		//now we know that bindingFlags is either Public, NonPublic or both
	///		...
	/// }
	/// </code>
	/// </example>
	/// <example>
	/// The following code ensures that the <c>bindingFlags</c> parameter is either BindingFlags.Public, BindingFlags.NonPublic,
	/// both or neither (BindingFlags.None):
	/// <code>
	/// public void GetInformation(BindingFlags bindingFlags)
	/// {
	///		ArgumentHelper.AssertEnumMember(bindingFlags, BindingFlags.Public, BindingFlags.NonPublic, BindingFlags.None);
	///		//now we know that bindingFlags is either Public, NonPublic, both or neither
	///		...
	/// }
	/// </code>
	/// </example>
	public static class ArgumentHelper
	{
		/// <summary>
		/// Ensures that <paramref name="arg"/> is not <see langword="null"/>. If it is, an <see cref="ArgumentNullException"/>
		/// is thrown.
		/// </summary>
		/// <typeparam name="T">
		/// The type of the argument.
		/// </typeparam>
		/// <param name="arg">
		/// The argument to check for <see langword="null"/>.
		/// </param>
		/// <param name="argName">
		/// The name of the argument.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="arg"/> is <see langword="null"/>.
		/// </exception>
		[DebuggerHidden]
		public static void AssertNotNull<T>(T arg, string argName)
			where T : class
		{
			if (arg == null)
			{
				throw new ArgumentNullException(argName);
			}
		}

		/// <summary>
		/// Ensures that <paramref name="arg"/> is not <see langword="null"/>. If it is, an <see cref="ArgumentNullException"/>
		/// is thrown.
		/// </summary>
		/// <typeparam name="T">
		/// The type of the nullable argument.
		/// </typeparam>
		/// <param name="arg">
		/// The nullable argument.
		/// </param>
		/// <param name="argName">
		/// The name of the argument.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="arg"/> is <see langword="null"/>.
		/// </exception>
		[DebuggerHidden]
		public static void AssertNotNull<T>(Nullable<T> arg, string argName)
			where T : struct
		{
			if (!arg.HasValue)
			{
				throw new ArgumentNullException(argName);
			}
		}

		/// <summary>
		/// Ensures that <paramref name="arg"/> is not <see langword="null"/>. If it is, an <see cref="ArgumentNullException"/>
		/// is thrown.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This method can be used instead of one of the <see cref="AssertNotNull"/> overloads in the case where the argument
		/// being checked is generic. It will ensure that <paramref name="arg"/> is not <see langword="null"/> if it is a
		/// reference type or if it is an instance of <see cref="Nullable{T}"/>.
		/// </para>
		/// </remarks>
		/// <typeparam name="T">
		/// The type of the argument.
		/// </typeparam>
		/// <param name="arg">
		/// The argument.
		/// </param>
		/// <param name="argName">
		/// The name of the argument.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="arg"/> is <see langword="null"/>.
		/// </exception>
		/// <example>
		/// The following code ensures that the <c>name</c> argument is not <see langword="null"/> or an empty <c>string</c>:
		/// <code>
		/// public void SomeMethod&lt;T&gt;(T arg)
		/// {
		///		ArgumentHelper.AssertGenericArgumentNotNull(arg, "arg");
		///		//now we know that arg is not null, regardless of whether it is a reference type or a Nullable type
		///		...
		/// }
		/// </code>
		/// </example>
		[DebuggerHidden]
		public static void AssertGenericArgumentNotNull<T>(T arg, string argName)
		{
			Type type = typeof(T);

			if (!type.IsValueType || (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Nullable<>))))
			{
				AssertNotNull((object) arg, argName);
			}
		}

		/// <summary>
		/// Ensures that <paramref name="arg"/> is not <see langword="null"/>, optionally checking each item in it for
		/// <see langword="null"/>. If any checked items are <see langword="null"/>, an exception is thrown.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This method throws an <see cref="ArgumentNullException"/> if <paramref name="arg"/> is <see langword="null"/>. If
		/// <paramref name="assertContentsNotNull"/> is <see langword="true"/> and one of the items in <paramref name="arg"/>
		/// is found to be <see langword="null"/>, an <see cref="ArgumentException"/> is thrown.
		/// </para>
		/// </remarks>
		/// <typeparam name="T">
		/// The type of the items in the <paramref name="arg"/> enumeration.
		/// </typeparam>
		/// <param name="arg">
		/// The argument to check for <see langword="null"/>.
		/// </param>
		/// <param name="argName">
		/// The name of the argument.
		/// </param>
		/// <param name="assertContentsNotNull">
		/// If <see langword="true"/>, each item inside the <paramref name="arg"/> enumeration is also checked for
		/// <see langword="null"/>. If <see langword="false"/>, only <paramref name="arg"/> itself is checked for
		/// <see langword="null"/>.
		/// </param>
		/// <exception cref="ArgumentException">
		/// If <paramref name="assertContentsNotNull"/> is <see langword="true"/> and one of the items in <paramref name="arg"/>
		/// is <see langword="null"/>.
		/// </exception>
		[DebuggerHidden]
		public static void AssertNotNull<T>(IEnumerable<T> arg, string argName, bool assertContentsNotNull)
		{
			//make sure the enumerable item itself isn't null
			AssertNotNull(arg, argName);

			if (assertContentsNotNull && typeof(T).IsClass)
			{
				//make sure each item in the enumeration isn't null
				foreach (T item in arg)
				{
					if (item == null)
					{
						throw new ArgumentException("An item inside the enumeration was null.", argName);
					}
				}
			}
		}

		/// <summary>
		/// Ensures that <paramref name="arg"/> is not <see langword="null"/> or an empty <c>string</c>. If it is, an
		/// <see cref="ArgumentException"/> is thrown.
		/// </summary>
		/// <param name="arg">
		/// The argument to check for <see langword="null"/> or an empty <c>string</c>.
		/// </param>
		/// <param name="argName">
		/// The name of the argument.
		/// </param>
		[DebuggerHidden]
		public static void AssertNotNullOrEmpty(string arg, string argName)
		{
			AssertNotNullOrEmpty(arg, argName, false);
		}

		/// <summary>
		/// Ensures that <paramref name="arg"/> is not <see langword="null"/> or an empty <c>string</c>, optionally trimming
		/// <paramref name="arg"/> first. If it is, an <see cref="ArgumentException"/> is thrown.
		/// </summary>
		/// <param name="arg">
		/// The argument to check for <see langword="null"/> or an empty <c>string</c>.
		/// </param>
		/// <param name="argName">
		/// The name of the argument.
		/// </param>
		/// <param name="trim">
		/// If <see langword="true"/> and <paramref name="arg"/> is not <see langword="null"/> or an empty <c>string</c>, it is
		/// trimmed and re-tested for being empty.
		/// </param>
		/// <exception cref="ArgumentException">
		/// If <paramref name="arg"/> is <see langword="null"/> or an empty <c>string</c>, or if it is a blank <c>string</c> and
		/// <paramref name="trim"/> is <see langword="true"/>.
		/// </exception>
		[DebuggerHidden]
		public static void AssertNotNullOrEmpty(string arg, string argName, bool trim)
		{
			if (string.IsNullOrEmpty(arg) || (trim && IsOnlyWhitespace(arg)))
			{
				throw new ArgumentException("Cannot be null or empty.", argName);
			}
		}

		/// <summary>
		/// Ensures that <paramref name="enumValue"/> is a valid member of the <typeparamref name="TEnum"/> enumeration. If it
		/// is not, an <see cref="ArgumentException"/> is thrown.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This method can be used to validate all publicly-supplied enumeration values. Without such an assertion, it is
		/// possible to cast any <c>int</c> value to the enumeration type and pass it in.
		/// </para>
		/// <para>
		/// This method works for both flags and non-flags enumerations. In the case of a flags enumeration, any combination of
		/// values in the enumeration is accepted. In the case of a non-flags enumeration, <paramref name="enumValue"/> must
		/// be equal to one of the values in the enumeration.
		/// </para>
		/// <para>
		/// This method is generic and quite slow as a result. You should prefer using the
		/// <see cref="AssertEnumMember{TEnum}(TEnum, TEnum[])"/> overload where possible. That overload is both faster and
		/// safer. Faster because it does not incur reflection costs, and safer because you are able to specify the exact
		/// values accepted by your method.
		/// </para>
		/// </remarks>
		/// <typeparam name="TEnum">
		/// The enumeration type.
		/// </typeparam>
		/// <param name="enumValue">
		/// The value of the enumeration.
		/// </param>
		/// <param name="argName">
		/// The name of the argument.
		/// </param>
		/// <exception cref="ArgumentException">
		/// If <paramref name="enumValue"/> is not a valid member of the <typeparamref name="TEnum"/> enumeration.
		/// </exception>
		[DebuggerHidden]
		[CLSCompliant(false)]
		public static void AssertEnumMember<TEnum>(TEnum enumValue, string argName)
				where TEnum : struct, IConvertible
		{
			if (Attribute.IsDefined(typeof(TEnum), typeof(FlagsAttribute), false))
			{
				//flag enumeration - we can only get here if TEnum is a valid enumeration type, since the FlagsAttribute can
				//only be applied to enumerations
				bool throwEx;
				long longValue = enumValue.ToInt64(CultureInfo.InvariantCulture);

				if (longValue == 0)
				{
					//only throw if zero isn't defined in the enum - we have to convert zero to the underlying type of the enum
					throwEx = !Enum.IsDefined(typeof(TEnum), ((IConvertible) 0).ToType(Enum.GetUnderlyingType(typeof(TEnum)), CultureInfo.InvariantCulture));
				}
				else
				{
					foreach (TEnum value in Enum.GetValues(typeof(TEnum)))
					{
						longValue &= ~value.ToInt64(CultureInfo.InvariantCulture);
					}

					//throw if there is a value left over after removing all valid values
					throwEx = (longValue != 0);
				}

				if (throwEx)
				{
					throw new ArgumentException(string.Format(CultureInfo.InvariantCulture,
						"Enum value '{0}' is not valid for flags enumeration '{1}'.",
						enumValue, typeof(TEnum).FullName), argName);
				}
			}
			else
			{
				//not a flag enumeration
				if (!Enum.IsDefined(typeof(TEnum), enumValue))
				{
					throw new ArgumentException(string.Format(CultureInfo.InvariantCulture,
							"Enum value '{0}' is not defined for enumeration '{1}'.",
							enumValue, typeof(TEnum).FullName), argName);
				}
			}
		}

		/// <summary>
		/// Ensures that <paramref name="enumValue"/> is included in the values specified by <paramref name="validValues"/>. If
		/// it is not, an <see cref="ArgumentException"/> is thrown.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This method can be used to ensure that an enumeration argument is valid for the context of the method. It works for
		/// both flags and non-flags enumerations. For flags enumerations, <paramref name="enumValue"/> must be any combination
		/// of values specified by <paramref name="validValues"/>. For non-flags enumerations, <paramref name="enumValue"/>
		/// must be one of the values specified by <paramref name="validValues"/>.
		/// </para>
		/// <para>
		/// This method is much faster than the <see cref="AssertEnumMember{TEnum}(TEnum)"/> overload. This is because it does
		/// not use reflection to determine the values defined by the enumeration. For this reason you should prefer this method
		/// when validating enumeration arguments.
		/// </para>
		/// <para>
		/// Another reason why this method is prefered is because it allows you to explicitly specify the values that your code
		/// handles. If you use the <see cref="AssertEnumMember{TEnum}(TEnum)"/> overload and a new value is later added to the
		/// enumeration, the assertion will not fail but your code probably will.
		/// </para>
		/// </remarks>
		/// <typeparam name="TEnum">
		/// The enumeration type.
		/// </typeparam>
		/// <param name="enumValue">
		/// The value of the enumeration.
		/// </param>
		/// <param name="argName">
		/// The name of the argument.
		/// </param>
		/// <param name="validValues">
		/// An array of all valid values.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="validValues"/> is <see langword="null"/>.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// If <paramref name="enumValue"/> is not present in <paramref name="validValues"/>, or (for flag enumerations) if
		/// <paramref name="enumValue"/> is not some combination of values specified in <paramref name="validValues"/>.
		/// </exception>
		[DebuggerHidden]
		[CLSCompliant(false)]
		public static void AssertEnumMember<TEnum>(TEnum enumValue, string argName, params TEnum[] validValues)
			where TEnum : struct, IConvertible
		{
			AssertNotNull(validValues, "validValues");

			if (Attribute.IsDefined(typeof(TEnum), typeof(FlagsAttribute), false))
			{
				//flag enumeration
				bool throwEx;
				long longValue = enumValue.ToInt64(CultureInfo.InvariantCulture);

				if (longValue == 0)
				{
					//only throw if zero isn't permitted by the valid values
					throwEx = true;

					foreach (TEnum value in validValues)
					{
						if (value.ToInt64(CultureInfo.InvariantCulture) == 0)
						{
							throwEx = false;
							break;
						}
					}
				}
				else
				{
					foreach (TEnum value in validValues)
					{
						longValue &= ~value.ToInt64(CultureInfo.InvariantCulture);
					}

					//throw if there is a value left over after removing all valid values
					throwEx = (longValue != 0);
				}

				if (throwEx)
				{
					throw new ArgumentException(string.Format(CultureInfo.InvariantCulture,
						"Enum value '{0}' is not allowed for flags enumeration '{1}'.",
						enumValue, typeof(TEnum).FullName), argName);
				}
			}
			else
			{
				//not a flag enumeration
				foreach (TEnum value in validValues)
				{
					if (enumValue.Equals(value))
					{
						return;
					}
				}

				//at this point we know an exception is required - however, we want to tailor the message based on whether the
				//specified value is undefined or simply not allowed
				if (!Enum.IsDefined(typeof(TEnum), enumValue))
				{
					throw new ArgumentException(string.Format(CultureInfo.InvariantCulture,
							"Enum value '{0}' is not defined for enumeration '{1}'.",
							enumValue, typeof(TEnum).FullName), argName);
				}
				else
				{
					throw new ArgumentException(string.Format(CultureInfo.InvariantCulture,
							"Enum value '{0}' is defined for enumeration '{1}' but it is not permitted in this context.",
							enumValue, typeof(TEnum).FullName), argName);
				}
			}
		}

		private static bool IsOnlyWhitespace(string arg)
		{
			Debug.Assert(arg != null);

			foreach (char c in arg)
			{
				if (!char.IsWhiteSpace(c))
				{
					return false;
				}
			}

			return true;
		}
	}
}
