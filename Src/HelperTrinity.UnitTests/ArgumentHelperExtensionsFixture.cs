namespace HelperTrinity.UnitTests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using Xunit;

    public sealed class ArgumentHelperExtensionsFixture
    {
        [Fact]
        public void assert_not_null_throws_if_reference_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => ((string)null).AssertNotNull("test"));
        }

        [Fact]
        public void assert_not_null_does_not_throw_if_reference_is_not_null()
        {
            "not null".AssertNotNull("test");
        }

        [Fact]
        public void assert_not_null_throws_if_enumerable_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => ((IEnumerable<string>)null).AssertNotNull("test", false));
        }

        [Fact]
        public void assert_not_null_does_not_throw_if_enumerable_is_empty()
        {
            (Enumerable.Empty<string>()).AssertNotNull("test", true);
        }

        [Fact]
        public void assert_not_null_does_not_throw_if_enumerable_contains_null_but_checking_for_null_items_is_disabled()
        {
            var list = new List<string>
            {
                string.Empty,
                null
            };
            list.AssertNotNull("test", false);
        }

        [Fact]
        public void assert_not_null_throws_if_enumerable_contains_null_and_checking_for_null_items_is_enabled()
        {
            var listOfClassType = new List<string>
            {
                string.Empty,
                null
            };
            var ex = Assert.Throws<ArgumentException>(() => listOfClassType.AssertNotNull("test", true));
            Assert.Equal("An item inside the enumeration was null." + Environment.NewLine + "Parameter name: test", ex.Message);

            var listOfInterfaceType = new List<IDisposable>
            {
                new StringReader(""),
                null
            };
            ex = Assert.Throws<ArgumentException>(() => listOfInterfaceType.AssertNotNull("test", true));
            Assert.Equal("An item inside the enumeration was null." + Environment.NewLine + "Parameter name: test", ex.Message);
        }

        [Fact]
        public void assert_not_null_does_not_throw_if_nullable_has_a_value()
        {
            ((int?)1).AssertNotNull("test");
        }

        [Fact]
        public void assert_not_null_throws_if_nullable_has_no_value()
        {
            Assert.Throws<ArgumentNullException>(() => ((int?)null).AssertNotNull("test"));
        }

        [Fact]
        public void assert_generic_argument_not_null_throws_if_reference_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => ((string)null).AssertGenericArgumentNotNull("test"));
        }

        [Fact]
        public void assert_generic_argument_not_null_throws_if_interface_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => ((IComparable)null).AssertGenericArgumentNotNull("test"));
        }

        [Fact]
        public void assert_generic_argument_not_null_does_not_throw_if_reference_is_not_null()
        {
            "test".AssertGenericArgumentNotNull("test");
        }

        [Fact]
        public void assert_generic_argument_not_null_does_not_throw_if_interface_is_not_null()
        {
            ((IComparable)"test").AssertGenericArgumentNotNull("test");
        }

        [Fact]
        public void assert_generic_argument_not_null_throws_if_nullable_has_no_value()
        {
            Assert.Throws<ArgumentNullException>(() => ((int?)null).AssertGenericArgumentNotNull("test"));
        }

        [Fact]
        public void assert_generic_argument_not_null_does_not_throw_if_nullable_has_a_value()
        {
            ((int?)13).AssertGenericArgumentNotNull("test");
        }

        [Fact]
        public void assert_generic_argument_not_null_does_not_throw_if_given_a_value_type()
        {
            1.AssertGenericArgumentNotNull("test");
            1.37D.AssertGenericArgumentNotNull("test");
            TimeSpan.Zero.AssertGenericArgumentNotNull("test");
            DateTime.Now.AssertGenericArgumentNotNull("test");
        }

        [Fact]
        public void assert_not_null_or_empty_throws_if_string_is_null()
        {
            Assert.Throws<ArgumentException>(() => ((string)null).AssertNotNullOrEmpty("test"));
        }

        [Fact]
        public void assert_not_null_or_empty_throws_if_string_is_empty()
        {
            Assert.Throws<ArgumentException>(() => string.Empty.AssertNotNullOrEmpty("test"));
        }

        [Fact]
        public void assert_not_null_or_empty_does_not_throw_if_string_is_not_null()
        {
            "test".AssertNotNullOrEmpty("test");
        }

        [Fact]
        public void assert_not_null_or_empty_throws_if_enumerable_is_null()
        {
            Assert.Throws<ArgumentException>(() => ((IEnumerable)null).AssertNotNullOrEmpty("test"));
        }

        [Fact]
        public void assert_not_null_or_empty_throws_if_enumerable_is_empty()
        {
            Assert.Throws<ArgumentException>(() => Enumerable.Empty<string>().AssertNotNullOrEmpty("test"));
        }

        [Fact]
        public void assert_not_null_or_empty_does_not_throw_if_enumerable_is_not_empty()
        {
            Enumerable.Range(0, 10).AssertNotNullOrEmpty("test");
        }

        [Fact]
        public void assert_not_null_or_empty_throws_if_collection_is_null()
        {
            Assert.Throws<ArgumentException>(() => ((ICollection)null).AssertNotNullOrEmpty("test"));
        }

        [Fact]
        public void assert_not_null_or_empty_throws_if_collection_is_empty()
        {
            Assert.Throws<ArgumentException>(() => new List<string>().AssertNotNullOrEmpty("test"));
        }

        [Fact]
        public void assert_not_null_or_empty_does_not_throw_if_collection_is_not_empty()
        {
            new List<string>(new string[] { "First", "Second" }).AssertNotNullOrEmpty("test");
        }

        [Fact]
        public void assert_not_null_or_white_space_throws_if_string_is_null()
        {
            Assert.Throws<ArgumentException>(() => ((string)null).AssertNotNullOrWhiteSpace("test"));
        }

        [Fact]
        public void assert_not_null_or_white_space_throws_if_string_is_empty()
        {
            Assert.Throws<ArgumentException>(() => string.Empty.AssertNotNullOrWhiteSpace("test"));
        }

        [Fact]
        public void assert_not_null_or_white_space_throws_if_string_contains_only_white_space()
        {
            Assert.Throws<ArgumentException>(() => "  \t     \r \n  ".AssertNotNullOrWhiteSpace("test"));
        }

        [Fact]
        public void assert_not_null_or_white_space_does_not_throw_if_string_is_not_entirely_white_space()
        {
            "  a ".AssertNotNullOrWhiteSpace("test");
        }

        [Fact]
        public void assert_enum_member_throws_if_given_invalid_flag_enumeration_value()
        {
            var ex = Assert.Throws<ArgumentException>(() => ((FlagsEnum)68).AssertEnumMember("test"));
            Assert.Equal(string.Format(CultureInfo.InvariantCulture, "Enum value '68' is not valid for flags enumeration 'HelperTrinity.UnitTests.ArgumentHelperExtensionsFixture+FlagsEnum'.{0}Parameter name: test", Environment.NewLine), ex.Message);
        }

        [Fact]
        public void assert_enum_member_throws_if_given_invalid_zero_flag_enumeration_value()
        {
            var ex = Assert.Throws<ArgumentException>(() => ((FlagsEnumNoNone)0).AssertEnumMember("test"));
            Assert.Equal(string.Format(CultureInfo.InvariantCulture, "Enum value '0' is not valid for flags enumeration 'HelperTrinity.UnitTests.ArgumentHelperExtensionsFixture+FlagsEnumNoNone'.{0}Parameter name: test", Environment.NewLine), ex.Message);
        }

        [Fact]
        public void assert_enum_member_does_not_throw_if_enumeration_flags_are_valid()
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
        public void assert_enum_member_throws_if_enumeration_value_is_invalid()
        {
            var ex = Assert.Throws<ArgumentException>(() => ((DayOfWeek)69).AssertEnumMember("test"));
            Assert.Equal(string.Format(CultureInfo.InvariantCulture, "Enum value '69' is not defined for enumeration 'System.DayOfWeek'.{0}Parameter name: test", Environment.NewLine), ex.Message);
        }

        [Fact]
        public void assert_enum_member_does_not_throw_if_enumeration_values_are_valid()
        {
            DayOfWeek.Monday.AssertEnumMember("test");
            ((DayOfWeek)3).AssertEnumMember("test");
        }

        [Fact]
        public void assert_enum_member_works_for_byte_flags_enumeration()
        {
            (ByteFlagsEnum.One | ByteFlagsEnum.Three).AssertEnumMember("test");
            ByteFlagsEnum.None.AssertEnumMember("test");
            (ByteFlagsEnum.One | ByteFlagsEnum.Two | ByteFlagsEnum.Three).AssertEnumMember("test");

            var ex = Assert.Throws<ArgumentException>(() => ((ByteFlagsEnum)80).AssertEnumMember("test"));
            Assert.Equal(string.Format(CultureInfo.InvariantCulture, "Enum value '80' is not valid for flags enumeration 'HelperTrinity.UnitTests.ArgumentHelperExtensionsFixture+ByteFlagsEnum'.{0}Parameter name: test", Environment.NewLine), ex.Message);
        }

        [Fact]
        public void assert_enum_member_works_for_byte_enumeration()
        {
            ByteEnum.One.AssertEnumMember("test");
            ByteEnum.Two.AssertEnumMember("test");
            ByteEnum.Three.AssertEnumMember("test");

            var ex = Assert.Throws<ArgumentException>(() => ((ByteEnum)10).AssertEnumMember("test"));
            Assert.Equal(string.Format(CultureInfo.InvariantCulture, "Enum value '10' is not defined for enumeration 'HelperTrinity.UnitTests.ArgumentHelperExtensionsFixture+ByteEnum'.{0}Parameter name: test", Environment.NewLine), ex.Message);
        }

        [Fact]
        public void assert_enum_member_throws_if_no_valid_values_are_provided()
        {
            Assert.Throws<ArgumentNullException>(() => DayOfWeek.Monday.AssertEnumMember("test", null));
        }

        [Fact]
        public void assert_enum_member_throws_if_flag_value_is_not_a_valid_value()
        {
            var ex = Assert.Throws<ArgumentException>(() => FlagsEnum.Three.AssertEnumMember("test", FlagsEnum.One, FlagsEnum.Two, FlagsEnum.Four));
            Assert.Equal(string.Format(CultureInfo.InvariantCulture, "Enum value 'Three' is not allowed for flags enumeration 'HelperTrinity.UnitTests.ArgumentHelperExtensionsFixture+FlagsEnum'.{0}Parameter name: test", Environment.NewLine), ex.Message);
        }

        [Fact]
        public void assert_enum_member_if_valid_flag_values_are_provided_but_the_enumeration_value_is_not_a_valid_flag_enumeration_value()
        {
            var ex = Assert.Throws<ArgumentException>(() => ((FlagsEnum)68).AssertEnumMember("test", FlagsEnum.One, FlagsEnum.Two, FlagsEnum.Four));
            Assert.Equal(string.Format(CultureInfo.InvariantCulture, "Enum value '68' is not allowed for flags enumeration 'HelperTrinity.UnitTests.ArgumentHelperExtensionsFixture+FlagsEnum'.{0}Parameter name: test", Environment.NewLine), ex.Message);
        }

        [Fact]
        public void assert_enum_member_throws_if_invalid_zero_flag_enumeration_value_is_passed_in_but_it_is_not_a_valid_value()
        {
            var ex = Assert.Throws<ArgumentException>(() => ((FlagsEnumNoNone)0).AssertEnumMember("test", FlagsEnumNoNone.One));
            Assert.Equal(string.Format(CultureInfo.InvariantCulture, "Enum value '0' is not allowed for flags enumeration 'HelperTrinity.UnitTests.ArgumentHelperExtensionsFixture+FlagsEnumNoNone'.{0}Parameter name: test", Environment.NewLine), ex.Message);
        }

        [Fact]
        public void assert_enum_member_throws_if_zero_flag_enumeration_value_is_passed_in_but_it_is_not_a_valid_value()
        {
            var ex = Assert.Throws<ArgumentException>(() => FlagsEnum.None.AssertEnumMember("test", FlagsEnum.One));
            Assert.Equal(string.Format(CultureInfo.InvariantCulture, "Enum value 'None' is not allowed for flags enumeration 'HelperTrinity.UnitTests.ArgumentHelperExtensionsFixture+FlagsEnum'.{0}Parameter name: test", Environment.NewLine), ex.Message);
        }

        [Fact]
        public void assert_enum_member_does_not_throw_if_flag_enumeration_values_are_valid()
        {
            var validValues = new FlagsEnum[] { FlagsEnum.One, FlagsEnum.Two, FlagsEnum.Four, FlagsEnum.None };
            FlagsEnum.None.AssertEnumMember("test", validValues);
            FlagsEnum.One.AssertEnumMember("test", validValues);
            FlagsEnum.Two.AssertEnumMember("test", validValues);
            FlagsEnum.Four.AssertEnumMember("test", validValues);
            (FlagsEnum.One | FlagsEnum.Two).AssertEnumMember("test", validValues);
            (FlagsEnum.One | FlagsEnum.Four).AssertEnumMember("test", validValues);
            (FlagsEnum.One | FlagsEnum.Two | FlagsEnum.Four).AssertEnumMember("test", validValues);
        }

        [Fact]
        public void assert_enum_member_throws_if_value_is_not_a_valid_value()
        {
            var ex = Assert.Throws<ArgumentException>(() => DayOfWeek.Monday.AssertEnumMember("test", DayOfWeek.Friday, DayOfWeek.Sunday));
            Assert.Equal(string.Format(CultureInfo.InvariantCulture, "Enum value 'Monday' is defined for enumeration 'System.DayOfWeek' but it is not permitted in this context.{0}Parameter name: test", Environment.NewLine), ex.Message);
        }

        [Fact]
        public void assert_enum_member_if_valid_values_are_provided_but_the_enumeration_value_is_not_a_valid_enumeration_value()
        {
            var ex = Assert.Throws<ArgumentException>(() => ((DayOfWeek)69).AssertEnumMember("test", DayOfWeek.Friday, DayOfWeek.Sunday));
            Assert.Equal(string.Format(CultureInfo.InvariantCulture, "Enum value '69' is not defined for enumeration 'System.DayOfWeek'.{0}Parameter name: test", Environment.NewLine), ex.Message);
        }

        [Fact]
        public void assert_enum_member_does_not_throw_if_valid_values_are_specified_and_enumeration_values_are_valid()
        {
            var validValues = new DayOfWeek[] { DayOfWeek.Friday, DayOfWeek.Sunday, DayOfWeek.Saturday };
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