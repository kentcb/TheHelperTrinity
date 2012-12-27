using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Xunit;

namespace Kent.Boogaart.HelperTrinity.UnitTest
{
	public sealed class ArgumentHelperTest
	{
		[Fact]
		public void AssertNotNull_Reference_ThrowWhenNull()
		{
			Assert.Throws<ArgumentNullException>(() => ArgumentHelper.AssertNotNull((string) null, "test"));
		}

		[Fact]
		public void AssertNotNull_Reference_DontThrowWhenNotNull()
		{
			ArgumentHelper.AssertNotNull("not null", "test");
		}

		[Fact]
		public void AssertNotNullEnumeration_Reference_ShouldThrowIfEnumerationIsNull()
		{
			Assert.Throws<ArgumentNullException>(() => ArgumentHelper.AssertNotNull((IEnumerable<string>) null, "test", false));
		}

		[Fact]
		public void AssertNotNullEnumeration_Reference_ShouldntThrowIfEnumerationIsNonNullAndThereAreNoItemsInEnumeration()
		{
			ArgumentHelper.AssertNotNull(new List<string>(), "test", true);
		}

		[Fact]
		public void AssertNotNullEnumeration_Reference_ShouldntThrowIfItemInEnumerationIsNullButCheckingTurnedOff()
		{
			List<string> list = new List<string>();
			list.Add(string.Empty);
			list.Add(null);
			ArgumentHelper.AssertNotNull(list, "test", false);
		}

		[Fact]
		public void AssertNotNullEnumeration_Reference_ShouldThrowIfItemInEnumerationIsNullAndCheckingIsTurnedOn()
		{
			List<string> list = new List<string>();
			list.Add(string.Empty);
			list.Add(null);
			var ex = Assert.Throws<ArgumentException>(() => ArgumentHelper.AssertNotNull(list, "test", true));
			Assert.Equal("An item inside the enumeration was null." + Environment.NewLine + "Parameter name: test", ex.Message);
		}

		[Fact]
		public void AssertNotNull_Nullable_ShouldNotThrowIfHasValue()
		{
			int? i = 1;
			ArgumentHelper.AssertNotNull(i, "test");
		}

		[Fact]
		public void AssertNotNull_Nullable_ShouldThrowWhenNull()
		{
			int? i = null;
			var ex = Assert.Throws<ArgumentNullException>(() => ArgumentHelper.AssertNotNull(i, "test"));
		}

		[Fact]
		public void AssertGenericArgumentNotNull_ShouldThrowIfReferenceTypeNull()
		{
			var ex = Assert.Throws<ArgumentNullException>(() => ArgumentHelper.AssertGenericArgumentNotNull((string) null, "test"));
		}

		[Fact]
		public void AssertGenericArgumentNotNull_ShouldThrowIfInterfaceTypeNull()
		{
			var ex = Assert.Throws<ArgumentNullException>(() => ArgumentHelper.AssertGenericArgumentNotNull<IComparable>(null, "test"));
		}

		[Fact]
		public void AssertGenericArgumentNotNull_ShouldNotThrowIfReferenceTypeIsNotNull()
		{
			ArgumentHelper.AssertGenericArgumentNotNull("test", "test");
			ArgumentHelper.AssertGenericArgumentNotNull((IComparable)"test", "test");
		}

		[Fact]
		public void AssertGenericArgumentNotNull_ShouldThrowIfNullableTypeNull()
		{
			int? i = null;
			var ex = Assert.Throws<ArgumentNullException>(() => ArgumentHelper.AssertGenericArgumentNotNull(i, "test"));
		}

		[Fact]
		public void AssertGenericArgumentNotNull_ShouldNotThrowIfNullableTypeIsNotNull()
		{
			int? i = 13;
			ArgumentHelper.AssertGenericArgumentNotNull(i, "test");
		}

		[Fact]
		public void AssertGenericArgumentNotNull_ShouldNotThrowIfValueType()
		{
			ArgumentHelper.AssertGenericArgumentNotNull(1, "test");
			ArgumentHelper.AssertGenericArgumentNotNull(1.37D, "test");
			ArgumentHelper.AssertGenericArgumentNotNull(TimeSpan.Zero, "test");
			ArgumentHelper.AssertGenericArgumentNotNull(DateTime.Now, "test");
		}

		[Fact]
		public void AssertNotNullOrEmpty_String_ThrowWhenNull()
		{
			Assert.Throws<ArgumentException>(() => ArgumentHelper.AssertNotNullOrEmpty((string)null, "test"));
		}

		[Fact]
        public void AssertNotNullOrEmpty_String_ThrowWhenEmpty()
		{
			Assert.Throws<ArgumentException>(() => ArgumentHelper.AssertNotNullOrEmpty(string.Empty, "test"));
		}

		[Fact]
        public void AssertNotNullOrEmpty_String_ThrowWhenBlankAndTrimmed()
		{
			Assert.Throws<ArgumentException>(() => ArgumentHelper.AssertNotNullOrEmpty("  ", "test", true));
		}

		[Fact]
        public void AssertNotNullOrEmpty_String_DontThrowWhenNotNullOrBlank()
		{
			ArgumentHelper.AssertNotNullOrEmpty("test", "test");
			ArgumentHelper.AssertNotNullOrEmpty("  a ", "test", true);
			ArgumentHelper.AssertNotNullOrEmpty("  ", "test", false);
		}

        [Fact]
        public void AssertNotNullOrEmpty_Collection_ThrowWhenNull()
        {
            Assert.Throws<ArgumentException>(() => ArgumentHelper.AssertNotNullOrEmpty((ICollection)null, "test"));
        }

        [Fact]
        public void AssertNotNullOrEmpty_Collection_ThrowWhenEmpty()
        {
            Assert.Throws<ArgumentException>(() => ArgumentHelper.AssertNotNullOrEmpty(new List<string>(), "test"));
        }

        [Fact]
        public void AssertNotNullOrEmpty_Collection_DontThrowWhenNotNullOrEmpty()
        {
            ArgumentHelper.AssertNotNullOrEmpty(new List<string>(new string[] { "First", "Second" }), "test");
        }

		[Fact]
		public void AssertEnumMember_ThrowWhenInvalidFlags()
		{
			var ex = Assert.Throws<ArgumentException>(() => ArgumentHelper.AssertEnumMember((FlagsEnum) 68, "test"));
			Assert.Equal(string.Format(CultureInfo.InvariantCulture, "Enum value '68' is not valid for flags enumeration 'Kent.Boogaart.HelperTrinity.UnitTest.ArgumentHelperTest+FlagsEnum'.{0}Parameter name: test", Environment.NewLine), ex.Message);
		}

		[Fact]
		public void AssertEnumMember_ThrowWhenInvalidFlagsNoZeroValue()
		{
			var ex = Assert.Throws<ArgumentException>(() => ArgumentHelper.AssertEnumMember((FlagsEnumNoNone) 0, "test"));
			Assert.Equal(string.Format(CultureInfo.InvariantCulture, "Enum value '0' is not valid for flags enumeration 'Kent.Boogaart.HelperTrinity.UnitTest.ArgumentHelperTest+FlagsEnumNoNone'.{0}Parameter name: test", Environment.NewLine), ex.Message);
		}

		[Fact]
		public void AssertEnumMember_DontThrowWhenValidFlags()
		{
			ArgumentHelper.AssertEnumMember(FlagsEnum.None, "test");
			ArgumentHelper.AssertEnumMember(FlagsEnum.One, "test");
			ArgumentHelper.AssertEnumMember(FlagsEnum.Two, "test");
			ArgumentHelper.AssertEnumMember(FlagsEnum.Three, "test");
			ArgumentHelper.AssertEnumMember(FlagsEnum.Four, "test");
			ArgumentHelper.AssertEnumMember(FlagsEnum.One | FlagsEnum.Two, "test");
			ArgumentHelper.AssertEnumMember(FlagsEnum.Two | FlagsEnum.Four, "test");
			ArgumentHelper.AssertEnumMember(FlagsEnum.One | FlagsEnum.Two | FlagsEnum.Three, "test");
			ArgumentHelper.AssertEnumMember(FlagsEnum.One | FlagsEnum.Two | FlagsEnum.Three | FlagsEnum.Four, "test");
		}

		[Fact]
		public void AssertEnumMember_ThrowWhenInvalid()
		{
			var ex = Assert.Throws<ArgumentException>(() => ArgumentHelper.AssertEnumMember((DayOfWeek) 69, "test"));
			Assert.Equal(string.Format(CultureInfo.InvariantCulture, "Enum value '69' is not defined for enumeration 'System.DayOfWeek'.{0}Parameter name: test", Environment.NewLine), ex.Message);
		}

		[Fact]
		public void AssertEnumMember_DontThrowWhenValid()
		{
			ArgumentHelper.AssertEnumMember(DayOfWeek.Monday, "test");
			ArgumentHelper.AssertEnumMember((DayOfWeek) 3, "test");
		}

		[Fact]
		public void AssertEnumMember_DifferentBaseTypeFlags()
		{
			ArgumentHelper.AssertEnumMember(ByteFlagsEnum.One | ByteFlagsEnum.Three, "test");
			ArgumentHelper.AssertEnumMember(ByteFlagsEnum.None, "test");
			ArgumentHelper.AssertEnumMember(ByteFlagsEnum.One | ByteFlagsEnum.Two | ByteFlagsEnum.Three, "test");

			var ex = Assert.Throws<ArgumentException>(() => ArgumentHelper.AssertEnumMember((ByteFlagsEnum) 80, "test"));
			Assert.Equal(string.Format(CultureInfo.InvariantCulture, "Enum value '80' is not valid for flags enumeration 'Kent.Boogaart.HelperTrinity.UnitTest.ArgumentHelperTest+ByteFlagsEnum'.{0}Parameter name: test", Environment.NewLine), ex.Message);
		}

		[Fact]
		public void AssertEnumMember_DifferentBaseType()
		{
			ArgumentHelper.AssertEnumMember(ByteEnum.One, "test");
			ArgumentHelper.AssertEnumMember(ByteEnum.Two, "test");
			ArgumentHelper.AssertEnumMember(ByteEnum.Three, "test");

			var ex = Assert.Throws<ArgumentException>(() => ArgumentHelper.AssertEnumMember((ByteEnum) 10, "test"));
			Assert.Equal(string.Format(CultureInfo.InvariantCulture, "Enum value '10' is not defined for enumeration 'Kent.Boogaart.HelperTrinity.UnitTest.ArgumentHelperTest+ByteEnum'.{0}Parameter name: test", Environment.NewLine), ex.Message);
		}

		[Fact]
		public void AssertEnumMember_ThrowsIfValidValuesNull()
		{
			Assert.Throws<ArgumentNullException>(() => ArgumentHelper.AssertEnumMember(DayOfWeek.Monday, "test", null));
		}

		[Fact]
		public void AssertEnumMember_ThrowsIfNotAllowedFlag()
		{
			var ex = Assert.Throws<ArgumentException>(() => ArgumentHelper.AssertEnumMember(FlagsEnum.Three, "test", FlagsEnum.One, FlagsEnum.Two, FlagsEnum.Four));
			Assert.Equal(string.Format(CultureInfo.InvariantCulture, "Enum value 'Three' is not allowed for flags enumeration 'Kent.Boogaart.HelperTrinity.UnitTest.ArgumentHelperTest+FlagsEnum'.{0}Parameter name: test", Environment.NewLine), ex.Message);
		}

		[Fact]
		public void AssertEnumMember_ThrowsIfInvalidFlag()
		{
			var ex = Assert.Throws<ArgumentException>(() => ArgumentHelper.AssertEnumMember((FlagsEnum) 68, "test", FlagsEnum.One, FlagsEnum.Two, FlagsEnum.Four));
			Assert.Equal(string.Format(CultureInfo.InvariantCulture, "Enum value '68' is not allowed for flags enumeration 'Kent.Boogaart.HelperTrinity.UnitTest.ArgumentHelperTest+FlagsEnum'.{0}Parameter name: test", Environment.NewLine), ex.Message);
		}

		[Fact]
		public void AssertEnumMember_ThrowsIfNoNoneButNonePassedIn()
		{
			var ex = Assert.Throws<ArgumentException>(() => ArgumentHelper.AssertEnumMember((FlagsEnumNoNone) 0, "test", FlagsEnumNoNone.One));
			Assert.Equal(string.Format(CultureInfo.InvariantCulture, "Enum value '0' is not allowed for flags enumeration 'Kent.Boogaart.HelperTrinity.UnitTest.ArgumentHelperTest+FlagsEnumNoNone'.{0}Parameter name: test", Environment.NewLine), ex.Message);
		}

		[Fact]
		public void AssertEnumMember_ThrowsIfNoneNotAllowedButNonePassedIn()
		{
			var ex = Assert.Throws<ArgumentException>(() => ArgumentHelper.AssertEnumMember(FlagsEnum.None, "test", FlagsEnum.One));
			Assert.Equal(string.Format(CultureInfo.InvariantCulture, "Enum value 'None' is not allowed for flags enumeration 'Kent.Boogaart.HelperTrinity.UnitTest.ArgumentHelperTest+FlagsEnum'.{0}Parameter name: test", Environment.NewLine), ex.Message);
		}

		[Fact]
		public void AssertEnumMember_DoesntThrowIfValidFlags()
		{
			FlagsEnum[] validValues = new FlagsEnum[] { FlagsEnum.One, FlagsEnum.Two, FlagsEnum.Four, FlagsEnum.None };
			ArgumentHelper.AssertEnumMember(FlagsEnum.None, "test", validValues);
			ArgumentHelper.AssertEnumMember(FlagsEnum.One, "test", validValues);
			ArgumentHelper.AssertEnumMember(FlagsEnum.Two, "test", validValues);
			ArgumentHelper.AssertEnumMember(FlagsEnum.Four, "test", validValues);
			ArgumentHelper.AssertEnumMember(FlagsEnum.One | FlagsEnum.Two, "test", validValues);
			ArgumentHelper.AssertEnumMember(FlagsEnum.One | FlagsEnum.Four, "test", validValues);
			ArgumentHelper.AssertEnumMember(FlagsEnum.One | FlagsEnum.Two | FlagsEnum.Four, "test", validValues);
		}

		[Fact]
		public void AssertEnumMember_ThrowsIfNotAllowed()
		{
			var ex = Assert.Throws<ArgumentException>(() => ArgumentHelper.AssertEnumMember(DayOfWeek.Monday, "test", DayOfWeek.Friday, DayOfWeek.Sunday));
			Assert.Equal(string.Format(CultureInfo.InvariantCulture, "Enum value 'Monday' is defined for enumeration 'System.DayOfWeek' but it is not permitted in this context.{0}Parameter name: test", Environment.NewLine), ex.Message);
		}

		[Fact]
		public void AssertEnumMember_ThrowsIfInvalid()
		{
			var ex = Assert.Throws<ArgumentException>(() => ArgumentHelper.AssertEnumMember((DayOfWeek) 69, "test", DayOfWeek.Friday, DayOfWeek.Sunday));
			Assert.Equal(string.Format(CultureInfo.InvariantCulture, "Enum value '69' is not defined for enumeration 'System.DayOfWeek'.{0}Parameter name: test", Environment.NewLine), ex.Message);
		}

		[Fact]
		public void AssertEnumMember_DoesntThrowIfValid()
		{
			DayOfWeek[] validValues = new DayOfWeek[] { DayOfWeek.Friday, DayOfWeek.Sunday, DayOfWeek.Saturday };
			ArgumentHelper.AssertEnumMember(DayOfWeek.Friday, "test", validValues);
			ArgumentHelper.AssertEnumMember(DayOfWeek.Sunday, "test", validValues);
			ArgumentHelper.AssertEnumMember(DayOfWeek.Saturday, "test", validValues);
		}

		//[Fact]
		public void PerformanceTest_ReflectiveVerusNonReflective()
		{
			System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
			int count = 1000000;

			stopwatch.Start();

			for (int i = 0; i < count; ++i)
			{
				//test the overload that uses Enum.GetValues() behind the scenes
				ArgumentHelper.AssertEnumMember(DayOfWeek.Sunday, "test");
				ArgumentHelper.AssertEnumMember(DayOfWeek.Saturday, "test");
			}

			stopwatch.Stop();
			Console.WriteLine("Reflective: {0}", stopwatch.ElapsedMilliseconds);

			stopwatch.Reset();
			stopwatch.Start();
			DayOfWeek[] validValues = new DayOfWeek[] { DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday };

			for (int i = 0; i < count; ++i)
			{
				//test the overload that allows you to explicitly state acceptable values
				ArgumentHelper.AssertEnumMember(DayOfWeek.Sunday, "test", validValues);
				ArgumentHelper.AssertEnumMember(DayOfWeek.Saturday, "test", validValues);
			}

			stopwatch.Stop();
			Console.WriteLine("Non-reflective: {0}", stopwatch.ElapsedMilliseconds);
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
