using System;
using System.Collections.Generic;
using NUnit.Framework;
using Kent.Boogaart.HelperTrinity;

namespace Kent.Boogaart.HelperTrinity.UnitTest
{
	[TestFixture]
	public sealed class ArgumentHelperTest
	{
		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AssertNotNull_Reference_ThrowWhenNull()
		{
			ArgumentHelper.AssertNotNull((string) null, "test");
		}

		[Test]
		public void AssertNotNull_Reference_DontThrowWhenNotNull()
		{
			ArgumentHelper.AssertNotNull("not null", "test");
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AssertNotNullEnumeration_Reference_ShouldThrowIfEnumerationIsNull()
		{
			ArgumentHelper.AssertNotNull((IEnumerable<string>) null, "test", false);
		}

		[Test]
		public void AssertNotNullEnumeration_Reference_ShouldntThrowIfEnumerationIsNonNullAndThereAreNoItemsInEnumeration()
		{
			ArgumentHelper.AssertNotNull(new List<string>(), "test", true);
		}

		[Test]
		public void AssertNotNullEnumeration_Reference_ShouldntThrowIfItemInEnumerationIsNullButCheckingTurnedOff()
		{
			List<string> list = new List<string>();
			list.Add(string.Empty);
			list.Add(null);
			ArgumentHelper.AssertNotNull(list, "test", false);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException), "An item inside the enumeration was null.\r\nParameter name: test")]
		public void AssertNotNullEnumeration_Reference_ShouldThrowIfItemInEnumerationIsNullAndCheckingIsTurnedOn()
		{
			List<string> list = new List<string>();
			list.Add(string.Empty);
			list.Add(null);
			ArgumentHelper.AssertNotNull(list, "test", true);
		}

		[Test]
		public void AssertNotNull_Nullable_ShouldNotThrowIfHasValue()
		{
			int? i = 1;
			ArgumentHelper.AssertNotNull(i, "test");
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AssertNotNull_Nullable_ShouldThrowWhenNull()
		{
			int? i = null;
			ArgumentHelper.AssertNotNull(i, "test");
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AssertGenericArgumentNotNull_ShouldThrowIfReferenceTypeNull()
		{
			ArgumentHelper.AssertGenericArgumentNotNull((string) null, "test");
		}

		[Test]
		public void AssertGenericArgumentNotNull_ShouldNotThrowIfReferenceTypeIsNotNull()
		{
			ArgumentHelper.AssertGenericArgumentNotNull("test", "test");
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AssertGenericArgumentNotNull_ShouldThrowIfNullableTypeNull()
		{
			int? i = null;
			ArgumentHelper.AssertGenericArgumentNotNull(i, "test");
		}

		[Test]
		public void AssertGenericArgumentNotNull_ShouldNotThrowIfNullableTypeIsNotNull()
		{
			int? i = 13;
			ArgumentHelper.AssertGenericArgumentNotNull(i, "test");
		}

		[Test]
		public void AssertGenericArgumentNotNull_ShouldNotThrowIfValueType()
		{
			ArgumentHelper.AssertGenericArgumentNotNull(1, "test");
			ArgumentHelper.AssertGenericArgumentNotNull(1.37D, "test");
			ArgumentHelper.AssertGenericArgumentNotNull(TimeSpan.Zero, "test");
			ArgumentHelper.AssertGenericArgumentNotNull(DateTime.Now, "test");
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void AssertNotNullOrEmpty_ThrowWhenNull()
		{
			ArgumentHelper.AssertNotNullOrEmpty(null, "test");
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void AssertNotNullOrEmpty_ThrowWhenEmpty()
		{
			ArgumentHelper.AssertNotNullOrEmpty(string.Empty, "test");
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void AssertNotNullOrEmpty_ThrowWhenBlankAndTrimmed()
		{
			ArgumentHelper.AssertNotNullOrEmpty("  ", "test", true);
		}

		[Test]
		public void AssertNotNullOrEmpty_DontThrowWhenNotNullOrBlank()
		{
			ArgumentHelper.AssertNotNullOrEmpty("test", "test");
			ArgumentHelper.AssertNotNullOrEmpty("  a ", "test", true);
			ArgumentHelper.AssertNotNullOrEmpty("  ", "test", false);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException), "Enum value '68' is not valid for flags enumeration 'Kent.Boogaart.HelperTrinity.UnitTest.ArgumentHelperTest+FlagsEnum'.\r\nParameter name: test")]
		public void AssertEnumMember_ThrowWhenInvalidFlags()
		{
			ArgumentHelper.AssertEnumMember((FlagsEnum) 68, "test");
		}

		[Test]
		[ExpectedException(typeof(ArgumentException), "Enum value '0' is not valid for flags enumeration 'Kent.Boogaart.HelperTrinity.UnitTest.ArgumentHelperTest+FlagsEnumNoNone'.\r\nParameter name: test")]
		public void AssertEnumMember_ThrowWhenInvalidFlagsNoZeroValue()
		{
			ArgumentHelper.AssertEnumMember((FlagsEnumNoNone) 0, "test");
		}

		[Test]
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

		[Test]
		[ExpectedException(typeof(ArgumentException), "Enum value '69' is not defined for enumeration 'System.DayOfWeek'.\r\nParameter name: test")]
		public void AssertEnumMember_ThrowWhenInvalid()
		{
			ArgumentHelper.AssertEnumMember((DayOfWeek) 69, "test");
		}

		[Test]
		public void AssertEnumMember_DontThrowWhenValid()
		{
			ArgumentHelper.AssertEnumMember(DayOfWeek.Monday, "test");
			ArgumentHelper.AssertEnumMember((DayOfWeek) 3, "test");
		}

		[Test]
		public void AssertEnumMember_DifferentBaseTypeFlags()
		{
			ArgumentHelper.AssertEnumMember(ByteFlagsEnum.One | ByteFlagsEnum.Three, "test");
			ArgumentHelper.AssertEnumMember(ByteFlagsEnum.None, "test");
			ArgumentHelper.AssertEnumMember(ByteFlagsEnum.One | ByteFlagsEnum.Two | ByteFlagsEnum.Three, "test");

			//just try one invalid case to be safe
			try
			{
				ArgumentHelper.AssertEnumMember((ByteFlagsEnum) 80, "test");
				Assert.Fail("Expected an ArgumentException.");
			}
			catch (ArgumentException ex)
			{
				Assert.AreEqual("Enum value '80' is not valid for flags enumeration 'Kent.Boogaart.HelperTrinity.UnitTest.ArgumentHelperTest+ByteFlagsEnum'.\r\nParameter name: test", ex.Message);
			}
		}

		[Test]
		public void AssertEnumMember_DifferentBaseType()
		{
			ArgumentHelper.AssertEnumMember(ByteEnum.One, "test");
			ArgumentHelper.AssertEnumMember(ByteEnum.Two, "test");
			ArgumentHelper.AssertEnumMember(ByteEnum.Three, "test");

			//just try one invalid case to be safe
			try
			{
				ArgumentHelper.AssertEnumMember((ByteEnum) 10, "test");
				Assert.Fail("Expected an ArgumentException.");
			}
			catch (ArgumentException ex)
			{
				Assert.AreEqual("Enum value '10' is not defined for enumeration 'Kent.Boogaart.HelperTrinity.UnitTest.ArgumentHelperTest+ByteEnum'.\r\nParameter name: test", ex.Message);
			}
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AssertEnumMember_ThrowsIfValidValuesNull()
		{
			ArgumentHelper.AssertEnumMember(DayOfWeek.Monday, "test", null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException), "Enum value 'Three' is not allowed for flags enumeration 'Kent.Boogaart.HelperTrinity.UnitTest.ArgumentHelperTest+FlagsEnum'.\r\nParameter name: test")]
		public void AssertEnumMember_ThrowsIfNotAllowedFlag()
		{
			ArgumentHelper.AssertEnumMember(FlagsEnum.Three, "test", FlagsEnum.One, FlagsEnum.Two, FlagsEnum.Four);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException), "Enum value '68' is not allowed for flags enumeration 'Kent.Boogaart.HelperTrinity.UnitTest.ArgumentHelperTest+FlagsEnum'.\r\nParameter name: test")]
		public void AssertEnumMember_ThrowsIfInvalidFlag()
		{
			ArgumentHelper.AssertEnumMember((FlagsEnum) 68, "test", FlagsEnum.One, FlagsEnum.Two, FlagsEnum.Four);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException), "Enum value '0' is not allowed for flags enumeration 'Kent.Boogaart.HelperTrinity.UnitTest.ArgumentHelperTest+FlagsEnumNoNone'.\r\nParameter name: test")]
		public void AssertEnumMember_ThrowsIfNoNoneButNonePassedIn()
		{
			ArgumentHelper.AssertEnumMember((FlagsEnumNoNone) 0, "test", FlagsEnumNoNone.One);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException), "Enum value 'None' is not allowed for flags enumeration 'Kent.Boogaart.HelperTrinity.UnitTest.ArgumentHelperTest+FlagsEnum'.\r\nParameter name: test")]
		public void AssertEnumMember_ThrowsIfNoneNotAllowedButNonePassedIn()
		{
			ArgumentHelper.AssertEnumMember(FlagsEnum.None, "test", FlagsEnum.One);
		}

		[Test]
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

		[Test]
		[ExpectedException(typeof(ArgumentException), "Enum value 'Monday' is defined for enumeration 'System.DayOfWeek' but it is not permitted in this context.\r\nParameter name: test")]
		public void AssertEnumMember_ThrowsIfNotAllowed()
		{
			ArgumentHelper.AssertEnumMember(DayOfWeek.Monday, "test", DayOfWeek.Friday, DayOfWeek.Sunday);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException), "Enum value '69' is not defined for enumeration 'System.DayOfWeek'.\r\nParameter name: test")]
		public void AssertEnumMember_ThrowsIfInvalid()
		{
			ArgumentHelper.AssertEnumMember((DayOfWeek) 69, "test", DayOfWeek.Friday, DayOfWeek.Sunday);
		}

		[Test]
		public void AssertEnumMember_DoesntThrowIfValid()
		{
			DayOfWeek[] validValues = new DayOfWeek[] { DayOfWeek.Friday, DayOfWeek.Sunday, DayOfWeek.Saturday };
			ArgumentHelper.AssertEnumMember(DayOfWeek.Friday, "test", validValues);
			ArgumentHelper.AssertEnumMember(DayOfWeek.Sunday, "test", validValues);
			ArgumentHelper.AssertEnumMember(DayOfWeek.Saturday, "test", validValues);
		}

		//[Test]
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
