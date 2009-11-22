using System;
using System.Collections.Generic;
using System.Globalization;
using Kent.Boogaart.HelperTrinity.Extensions;
using Xunit;

namespace Kent.Boogaart.HelperTrinity.UnitTest
{
	public sealed class ArgumentHelperExtensionsTest
	{
		[Fact]
		public void AssertNotNull_Reference_ThrowWhenNull()
		{
			string arg = null;
			Assert.Throws<ArgumentNullException>(() => arg.AssertNotNull("test"));
		}

		[Fact]
		public void AssertNotNull_Reference_DontThrowWhenNotNull()
		{
			"not null".AssertNotNull("test");
		}

		[Fact]
		public void AssertNotNullEnumeration_Reference_ShouldThrowIfEnumerationIsNull()
		{
			IEnumerable<string> arg = null;
			Assert.Throws<ArgumentNullException>(() => arg.AssertNotNull("test", false));
		}

		[Fact]
		public void AssertNotNullEnumeration_Reference_ShouldntThrowIfEnumerationIsNonNullAndThereAreNoItemsInEnumeration()
		{
			new List<string>().AssertNotNull("test", true);
		}

		[Fact]
		public void AssertNotNullEnumeration_Reference_ShouldntThrowIfItemInEnumerationIsNullButCheckingTurnedOff()
		{
			List<string> list = new List<string>();
			list.Add(string.Empty);
			list.Add(null);
			list.AssertNotNull("test", false);
		}

		[Fact]
		public void AssertNotNullEnumeration_Reference_ShouldThrowIfItemInEnumerationIsNullAndCheckingIsTurnedOn()
		{
			List<string> list = new List<string>();
			list.Add(string.Empty);
			list.Add(null);
			var ex = Assert.Throws<ArgumentException>(() => list.AssertNotNull("test", true));
			Assert.Equal(string.Format(CultureInfo.InvariantCulture, "An item inside the enumeration was null.{0}Parameter name: test", Environment.NewLine), ex.Message);
		}

		[Fact]
		public void AssertNotNull_Nullable_ShouldNotThrowIfHasValue()
		{
			int? i = 1;
			i.AssertNotNull("test");
		}

		[Fact]
		public void AssertNotNull_Nullable_ShouldThrowWhenNull()
		{
			int? i = null;
			Assert.Throws<ArgumentNullException>(() => i.AssertNotNull("test"));
		}

		[Fact]
		public void AssertGenericArgumentNotNull_ShouldThrowIfReferenceTypeNull()
		{
			string arg = null;
			Assert.Throws<ArgumentNullException>(() => arg.AssertGenericArgumentNotNull("test"));
		}

		[Fact]
		public void AssertGenericArgumentNotNull_ShouldThrowIfInterfaceTypeNull()
		{
			IComparable arg = null;
			Assert.Throws<ArgumentNullException>(() => arg.AssertGenericArgumentNotNull("test"));
		}

		[Fact]
		public void AssertGenericArgumentNotNull_ShouldNotThrowIfReferenceTypeIsNotNull()
		{
			"test".AssertGenericArgumentNotNull("test");
			("test" as IComparable).AssertGenericArgumentNotNull("test");
		}

		[Fact]
		public void AssertGenericArgumentNotNull_ShouldThrowIfNullableTypeNull()
		{
			int? i = null;
			Assert.Throws<ArgumentNullException>(() => i.AssertGenericArgumentNotNull("test"));
		}

		[Fact]
		public void AssertGenericArgumentNotNull_ShouldNotThrowIfNullableTypeIsNotNull()
		{
			int? i = 13;
			i.AssertGenericArgumentNotNull("test");
		}

		[Fact]
		public void AssertGenericArgumentNotNull_ShouldNotThrowIfValueType()
		{
			1.AssertGenericArgumentNotNull("test");
			1.37D.AssertGenericArgumentNotNull("test");
			TimeSpan.Zero.AssertGenericArgumentNotNull("test");
			DateTime.Now.AssertGenericArgumentNotNull("test");
		}

		[Fact]
		public void AssertNotNullOrEmpty_ThrowWhenNull()
		{
			string arg = null;
			Assert.Throws<ArgumentException>(() => arg.AssertNotNullOrEmpty("test"));
		}

		[Fact]
		public void AssertNotNullOrEmpty_ThrowWhenEmpty()
		{
			Assert.Throws<ArgumentException>(() => string.Empty.AssertNotNullOrEmpty("test"));
		}

		[Fact]
		public void AssertNotNullOrEmpty_ThrowWhenBlankAndTrimmed()
		{
			Assert.Throws<ArgumentException>(() => "   ".AssertNotNullOrEmpty("test", true));
		}

		[Fact]
		public void AssertNotNullOrEmpty_DontThrowWhenNotNullOrBlank()
		{
			"test".AssertNotNullOrEmpty("test");
			"  a ".AssertNotNullOrEmpty("test", true);
			"  ".AssertNotNullOrEmpty("test", false);
		}

		[Fact]
		public void AssertEnumMember_ThrowWhenInvalidFlags()
		{
			var ex = Assert.Throws<ArgumentException>(() => ((FlagsEnum)68).AssertEnumMember("test"));
			Assert.Equal(string.Format(CultureInfo.InvariantCulture, "Enum value '68' is not valid for flags enumeration 'Kent.Boogaart.HelperTrinity.UnitTest.ArgumentHelperExtensionsTest+FlagsEnum'.{0}Parameter name: test", Environment.NewLine), ex.Message);
		}

		[Fact]
		public void AssertEnumMember_ThrowWhenInvalidFlagsNoZeroValue()
		{
			var ex = Assert.Throws<ArgumentException>(() => ((FlagsEnumNoNone)0).AssertEnumMember("test"));
			Assert.Equal(string.Format(CultureInfo.InvariantCulture, "Enum value '0' is not valid for flags enumeration 'Kent.Boogaart.HelperTrinity.UnitTest.ArgumentHelperExtensionsTest+FlagsEnumNoNone'.{0}Parameter name: test", Environment.NewLine), ex.Message);
		}

		[Fact]
		public void AssertEnumMember_DontThrowWhenValidFlags()
		{
			FlagsEnum.None.AssertEnumMember("test");
			FlagsEnum.One.AssertEnumMember("test");
			FlagsEnum.Two.AssertEnumMember("test");
			FlagsEnum.Three.AssertEnumMember("test");
			FlagsEnum.Four.AssertEnumMember("test");
			(FlagsEnum.One | FlagsEnum.Two).AssertEnumMember("test");
			(FlagsEnum.Two | FlagsEnum.Four).AssertEnumMember("test");
			(FlagsEnum.One | FlagsEnum.Two | FlagsEnum.Three).AssertEnumMember("test");
			(FlagsEnum.One | FlagsEnum.Two | FlagsEnum.Three | FlagsEnum.Four).AssertEnumMember("test");
		}

		[Fact]
		public void AssertEnumMember_ThrowWhenInvalid()
		{
			var ex = Assert.Throws<ArgumentException>(() => ((DayOfWeek)69).AssertEnumMember("test"));
			Assert.Equal(string.Format(CultureInfo.InvariantCulture, "Enum value '69' is not defined for enumeration 'System.DayOfWeek'.{0}Parameter name: test", Environment.NewLine), ex.Message);
		}

		[Fact]
		public void AssertEnumMember_DontThrowWhenValid()
		{
			DayOfWeek.Monday.AssertEnumMember("test");
			((DayOfWeek)3).AssertEnumMember("test");
		}

		[Fact]
		public void AssertEnumMember_DifferentBaseTypeFlags()
		{
			(ByteFlagsEnum.One | ByteFlagsEnum.Three).AssertEnumMember("test");
			ByteFlagsEnum.None.AssertEnumMember("test");
			(ByteFlagsEnum.One | ByteFlagsEnum.Two | ByteFlagsEnum.Three).AssertEnumMember("test");

			var ex = Assert.Throws<ArgumentException>(() => ((ByteFlagsEnum)80).AssertEnumMember("test"));
			Assert.Equal(string.Format(CultureInfo.InvariantCulture, "Enum value '80' is not valid for flags enumeration 'Kent.Boogaart.HelperTrinity.UnitTest.ArgumentHelperExtensionsTest+ByteFlagsEnum'.{0}Parameter name: test", Environment.NewLine), ex.Message);
		}

		[Fact]
		public void AssertEnumMember_DifferentBaseType()
		{
			ByteEnum.One.AssertEnumMember("test");
			ByteEnum.Two.AssertEnumMember("test");
			ByteEnum.Three.AssertEnumMember("test");

			var ex = Assert.Throws<ArgumentException>(() => ((ByteEnum)10).AssertEnumMember("test"));
			Assert.Equal(string.Format(CultureInfo.InvariantCulture, "Enum value '10' is not defined for enumeration 'Kent.Boogaart.HelperTrinity.UnitTest.ArgumentHelperExtensionsTest+ByteEnum'.{0}Parameter name: test", Environment.NewLine), ex.Message);
		}

		[Fact]
		public void AssertEnumMember_ThrowsIfValidValuesNull()
		{
			Assert.Throws<ArgumentNullException>(() => DayOfWeek.Monday.AssertEnumMember("test", null));
		}

		[Fact]
		public void AssertEnumMember_ThrowsIfNotAllowedFlag()
		{
			var ex = Assert.Throws<ArgumentException>(() => FlagsEnum.Three.AssertEnumMember("test", FlagsEnum.One, FlagsEnum.Two, FlagsEnum.Four));
			Assert.Equal(string.Format(CultureInfo.InvariantCulture, "Enum value 'Three' is not allowed for flags enumeration 'Kent.Boogaart.HelperTrinity.UnitTest.ArgumentHelperExtensionsTest+FlagsEnum'.{0}Parameter name: test", Environment.NewLine), ex.Message);
		}

		[Fact]
		public void AssertEnumMember_ThrowsIfInvalidFlag()
		{
			var ex = Assert.Throws<ArgumentException>(() => ((FlagsEnum)68).AssertEnumMember("test", FlagsEnum.One, FlagsEnum.Two, FlagsEnum.Four));
			Assert.Equal(string.Format(CultureInfo.InvariantCulture, "Enum value '68' is not allowed for flags enumeration 'Kent.Boogaart.HelperTrinity.UnitTest.ArgumentHelperExtensionsTest+FlagsEnum'.{0}Parameter name: test", Environment.NewLine), ex.Message);
		}

		[Fact]
		public void AssertEnumMember_ThrowsIfNoNoneButNonePassedIn()
		{
			var ex = Assert.Throws<ArgumentException>(() => ((FlagsEnumNoNone)0).AssertEnumMember("test", FlagsEnumNoNone.One));
			Assert.Equal(string.Format(CultureInfo.InvariantCulture, "Enum value '0' is not allowed for flags enumeration 'Kent.Boogaart.HelperTrinity.UnitTest.ArgumentHelperExtensionsTest+FlagsEnumNoNone'.{0}Parameter name: test", Environment.NewLine), ex.Message);
		}

		[Fact]
		public void AssertEnumMember_ThrowsIfNoneNotAllowedButNonePassedIn()
		{
			var ex = Assert.Throws<ArgumentException>(() => FlagsEnum.None.AssertEnumMember("test", FlagsEnum.One));
			Assert.Equal(string.Format(CultureInfo.InvariantCulture, "Enum value 'None' is not allowed for flags enumeration 'Kent.Boogaart.HelperTrinity.UnitTest.ArgumentHelperExtensionsTest+FlagsEnum'.{0}Parameter name: test", Environment.NewLine), ex.Message);
		}

		[Fact]
		public void AssertEnumMember_DoesntThrowIfValidFlags()
		{
			FlagsEnum[] validValues = new FlagsEnum[] { FlagsEnum.One, FlagsEnum.Two, FlagsEnum.Four, FlagsEnum.None };
			FlagsEnum.None.AssertEnumMember("test", validValues);
			FlagsEnum.One.AssertEnumMember("test", validValues);
			FlagsEnum.Two.AssertEnumMember("test", validValues);
			FlagsEnum.Four.AssertEnumMember("test", validValues);
			(FlagsEnum.One | FlagsEnum.Two).AssertEnumMember("test", validValues);
			(FlagsEnum.One | FlagsEnum.Four).AssertEnumMember("test", validValues);
			(FlagsEnum.One | FlagsEnum.Two | FlagsEnum.Four).AssertEnumMember("test", validValues);
		}

		[Fact]
		public void AssertEnumMember_ThrowsIfNotAllowed()
		{
			var ex = Assert.Throws<ArgumentException>(() => DayOfWeek.Monday.AssertEnumMember("test", DayOfWeek.Friday, DayOfWeek.Sunday));
			Assert.Equal(string.Format(CultureInfo.InvariantCulture, "Enum value 'Monday' is defined for enumeration 'System.DayOfWeek' but it is not permitted in this context.{0}Parameter name: test", Environment.NewLine), ex.Message);
		}

		[Fact]
		public void AssertEnumMember_ThrowsIfInvalid()
		{
			var ex = Assert.Throws<ArgumentException>(() => ((DayOfWeek)69).AssertEnumMember("test", DayOfWeek.Friday, DayOfWeek.Sunday));
			Assert.Equal(string.Format(CultureInfo.InvariantCulture, "Enum value '69' is not defined for enumeration 'System.DayOfWeek'.{0}Parameter name: test", Environment.NewLine), ex.Message);
		}

		[Fact]
		public void AssertEnumMember_DoesntThrowIfValid()
		{
			DayOfWeek[] validValues = new DayOfWeek[] { DayOfWeek.Friday, DayOfWeek.Sunday, DayOfWeek.Saturday };
			DayOfWeek.Friday.AssertEnumMember("test", validValues);
			DayOfWeek.Sunday.AssertEnumMember("test", validValues);
			DayOfWeek.Saturday.AssertEnumMember("test", validValues);
		}

		#region Supporting Types

		[Flags]
		private enum FlagsEnum
		{
			None = 0,
			One = 1,
			Two = 2,
			Three = 4,
			Four = 8
		}

		[Flags]
		private enum FlagsEnumNoNone
		{
			One = 1,
		}

		private enum ByteEnum : byte
		{
			One,
			Two,
			Three
		}

		[Flags]
		private enum ByteFlagsEnum : byte
		{
			None = 0,
			One = 1,
			Two = 2,
			Three = 4
		}

		#endregion
	}
}