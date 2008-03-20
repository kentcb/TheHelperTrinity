#if FX35

using System;
using System.Collections.Generic;
using Kent.Boogaart.HelperTrinity.Extensions;
using NUnit.Framework;

namespace Kent.Boogaart.HelperTrinity.UnitTest
{
	[TestFixture]
	public sealed class ArgumentHelperExtensionsTest
	{
		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AssertNotNull_Reference_ThrowWhenNull()
		{
			string arg = null;
			arg.AssertNotNull("test");
		}

		[Test]
		public void AssertNotNull_Reference_DontThrowWhenNotNull()
		{
			"not null".AssertNotNull("test");
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AssertNotNullEnumeration_Reference_ShouldThrowIfEnumerationIsNull()
		{
			IEnumerable<string> arg = null;
			arg.AssertNotNull("test", false);
		}

		[Test]
		public void AssertNotNullEnumeration_Reference_ShouldntThrowIfEnumerationIsNonNullAndThereAreNoItemsInEnumeration()
		{
			new List<string>().AssertNotNull("test", true);
		}

		[Test]
		public void AssertNotNullEnumeration_Reference_ShouldntThrowIfItemInEnumerationIsNullButCheckingTurnedOff()
		{
			List<string> list = new List<string>();
			list.Add(string.Empty);
			list.Add(null);
			list.AssertNotNull("test", false);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException), "An item inside the enumeration was null.\r\nParameter name: test")]
		public void AssertNotNullEnumeration_Reference_ShouldThrowIfItemInEnumerationIsNullAndCheckingIsTurnedOn()
		{
			List<string> list = new List<string>();
			list.Add(string.Empty);
			list.Add(null);
			list.AssertNotNull("test", true);
		}

		[Test]
		public void AssertNotNull_Nullable_ShouldNotThrowIfHasValue()
		{
			int? i = 1;
			i.AssertNotNull("test");
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AssertNotNull_Nullable_ShouldThrowWhenNull()
		{
			int? i = null;
			i.AssertNotNull("test");
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AssertGenericArgumentNotNull_ShouldThrowIfReferenceTypeNull()
		{
			string arg = null;
			arg.AssertGenericArgumentNotNull("test");
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AssertGenericArgumentNotNull_ShouldThrowIfInterfaceTypeNull()
		{
			IComparable arg = null;
			arg.AssertGenericArgumentNotNull("test");
		}

		[Test]
		public void AssertGenericArgumentNotNull_ShouldNotThrowIfReferenceTypeIsNotNull()
		{
			"test".AssertGenericArgumentNotNull("test");
			("test" as IComparable).AssertGenericArgumentNotNull("test");
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AssertGenericArgumentNotNull_ShouldThrowIfNullableTypeNull()
		{
			int? i = null;
			i.AssertGenericArgumentNotNull("test");
		}

		[Test]
		public void AssertGenericArgumentNotNull_ShouldNotThrowIfNullableTypeIsNotNull()
		{
			int? i = 13;
			i.AssertGenericArgumentNotNull("test");
		}

		[Test]
		public void AssertGenericArgumentNotNull_ShouldNotThrowIfValueType()
		{
			1.AssertGenericArgumentNotNull("test");
			1.37D.AssertGenericArgumentNotNull("test");
			TimeSpan.Zero.AssertGenericArgumentNotNull("test");
			DateTime.Now.AssertGenericArgumentNotNull("test");
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void AssertNotNullOrEmpty_ThrowWhenNull()
		{
			string arg = null;
			arg.AssertNotNullOrEmpty("test");
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void AssertNotNullOrEmpty_ThrowWhenEmpty()
		{
			string.Empty.AssertNotNullOrEmpty("test");
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void AssertNotNullOrEmpty_ThrowWhenBlankAndTrimmed()
		{
			"   ".AssertNotNullOrEmpty("test", true);
		}

		[Test]
		public void AssertNotNullOrEmpty_DontThrowWhenNotNullOrBlank()
		{
			"test".AssertNotNullOrEmpty("test");
			"  a ".AssertNotNullOrEmpty("test", true);
			"  ".AssertNotNullOrEmpty("test", false);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException), "Enum value '68' is not valid for flags enumeration 'Kent.Boogaart.HelperTrinity.UnitTest.ArgumentHelperExtensionsTest+FlagsEnum'.\r\nParameter name: test")]
		public void AssertEnumMember_ThrowWhenInvalidFlags()
		{
			((FlagsEnum)68).AssertEnumMember("test");
		}

		[Test]
		[ExpectedException(typeof(ArgumentException), "Enum value '0' is not valid for flags enumeration 'Kent.Boogaart.HelperTrinity.UnitTest.ArgumentHelperExtensionsTest+FlagsEnumNoNone'.\r\nParameter name: test")]
		public void AssertEnumMember_ThrowWhenInvalidFlagsNoZeroValue()
		{
			((FlagsEnumNoNone)0).AssertEnumMember("test");
		}

		[Test]
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

		[Test]
		[ExpectedException(typeof(ArgumentException), "Enum value '69' is not defined for enumeration 'System.DayOfWeek'.\r\nParameter name: test")]
		public void AssertEnumMember_ThrowWhenInvalid()
		{
			((DayOfWeek)69).AssertEnumMember("test");
		}

		[Test]
		public void AssertEnumMember_DontThrowWhenValid()
		{
			DayOfWeek.Monday.AssertEnumMember("test");
			((DayOfWeek)3).AssertEnumMember("test");
		}

		[Test]
		public void AssertEnumMember_DifferentBaseTypeFlags()
		{
			(ByteFlagsEnum.One | ByteFlagsEnum.Three).AssertEnumMember("test");
			ByteFlagsEnum.None.AssertEnumMember("test");
			(ByteFlagsEnum.One | ByteFlagsEnum.Two | ByteFlagsEnum.Three).AssertEnumMember("test");

			//just try one invalid case to be safe
			try
			{
				((ByteFlagsEnum)80).AssertEnumMember("test");
				Assert.Fail("Expected an ArgumentException.");
			}
			catch (ArgumentException ex)
			{
				Assert.AreEqual("Enum value '80' is not valid for flags enumeration 'Kent.Boogaart.HelperTrinity.UnitTest.ArgumentHelperExtensionsTest+ByteFlagsEnum'.\r\nParameter name: test", ex.Message);
			}
		}

		[Test]
		public void AssertEnumMember_DifferentBaseType()
		{
			ByteEnum.One.AssertEnumMember("test");
			ByteEnum.Two.AssertEnumMember("test");
			ByteEnum.Three.AssertEnumMember("test");

			//just try one invalid case to be safe
			try
			{
				((ByteEnum)10).AssertEnumMember("test");
				Assert.Fail("Expected an ArgumentException.");
			}
			catch (ArgumentException ex)
			{
				Assert.AreEqual("Enum value '10' is not defined for enumeration 'Kent.Boogaart.HelperTrinity.UnitTest.ArgumentHelperExtensionsTest+ByteEnum'.\r\nParameter name: test", ex.Message);
			}
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AssertEnumMember_ThrowsIfValidValuesNull()
		{
			DayOfWeek.Monday.AssertEnumMember("test", null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException), "Enum value 'Three' is not allowed for flags enumeration 'Kent.Boogaart.HelperTrinity.UnitTest.ArgumentHelperExtensionsTest+FlagsEnum'.\r\nParameter name: test")]
		public void AssertEnumMember_ThrowsIfNotAllowedFlag()
		{
			FlagsEnum.Three.AssertEnumMember("test", FlagsEnum.One, FlagsEnum.Two, FlagsEnum.Four);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException), "Enum value '68' is not allowed for flags enumeration 'Kent.Boogaart.HelperTrinity.UnitTest.ArgumentHelperExtensionsTest+FlagsEnum'.\r\nParameter name: test")]
		public void AssertEnumMember_ThrowsIfInvalidFlag()
		{
			((FlagsEnum)68).AssertEnumMember("test", FlagsEnum.One, FlagsEnum.Two, FlagsEnum.Four);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException), "Enum value '0' is not allowed for flags enumeration 'Kent.Boogaart.HelperTrinity.UnitTest.ArgumentHelperExtensionsTest+FlagsEnumNoNone'.\r\nParameter name: test")]
		public void AssertEnumMember_ThrowsIfNoNoneButNonePassedIn()
		{
			((FlagsEnumNoNone)0).AssertEnumMember("test", FlagsEnumNoNone.One);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException), "Enum value 'None' is not allowed for flags enumeration 'Kent.Boogaart.HelperTrinity.UnitTest.ArgumentHelperExtensionsTest+FlagsEnum'.\r\nParameter name: test")]
		public void AssertEnumMember_ThrowsIfNoneNotAllowedButNonePassedIn()
		{
			FlagsEnum.None.AssertEnumMember("test", FlagsEnum.One);
		}

		[Test]
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

		[Test]
		[ExpectedException(typeof(ArgumentException), "Enum value 'Monday' is defined for enumeration 'System.DayOfWeek' but it is not permitted in this context.\r\nParameter name: test")]
		public void AssertEnumMember_ThrowsIfNotAllowed()
		{
			DayOfWeek.Monday.AssertEnumMember("test", DayOfWeek.Friday, DayOfWeek.Sunday);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException), "Enum value '69' is not defined for enumeration 'System.DayOfWeek'.\r\nParameter name: test")]
		public void AssertEnumMember_ThrowsIfInvalid()
		{
			((DayOfWeek)69).AssertEnumMember("test", DayOfWeek.Friday, DayOfWeek.Sunday);
		}

		[Test]
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

#endif