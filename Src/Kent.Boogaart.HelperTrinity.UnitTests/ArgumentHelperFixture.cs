namespace Kent.Boogaart.HelperTrinity.UnitTests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Xunit;

    public sealed class ArgumentHelperFixture
    {
        [Fact]
        public void assert_not_null_throws_if_reference_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => ArgumentHelper.AssertNotNull((string)null, "test"));
        }

        [Fact]
        public void assert_not_null_does_not_throw_if_reference_is_not_null()
        {
            ArgumentHelper.AssertNotNull("not null", "test");
        }

        [Fact]
        public void assert_not_null_throws_if_enumerable_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => ArgumentHelper.AssertNotNull((IEnumerable<string>)null, "test", false));
        }

        [Fact]
        public void assert_not_null_does_not_throw_if_enumerable_is_empty()
        {
            ArgumentHelper.AssertNotNull(Enumerable.Empty<string>(), "test", true);
        }

        [Fact]
        public void assert_not_null_does_not_throw_if_enumerable_contains_null_but_checking_for_null_items_is_disabled()
        {
            var list = new List<string>
            {
                string.Empty,
                null
            };
            ArgumentHelper.AssertNotNull(list, "test", false);
        }

        [Fact]
        public void assert_not_null_throws_if_enumerable_contains_null_and_checking_for_null_items_is_enabled()
        {
            var list = new List<string>
            {
                string.Empty,
                null
            };
            var ex = Assert.Throws<ArgumentException>(() => ArgumentHelper.AssertNotNull(list, "test", true));
            Assert.Equal("An item inside the enumeration was null." + Environment.NewLine + "Parameter name: test", ex.Message);
        }

        [Fact]
        public void assert_not_null_does_not_throw_if_nullable_has_a_value()
        {
            ArgumentHelper.AssertNotNull((int?)1, "test");
        }

        [Fact]
        public void assert_not_null_throws_if_nullable_has_no_value()
        {
            Assert.Throws<ArgumentNullException>(() => ArgumentHelper.AssertNotNull((int?)null, "test"));
        }

        [Fact]
        public void assert_generic_argument_not_null_throws_if_reference_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => ArgumentHelper.AssertGenericArgumentNotNull<string>(null, "test"));
        }

        [Fact]
        public void assert_generic_argument_not_null_throws_if_interface_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => ArgumentHelper.AssertGenericArgumentNotNull<IComparable>(null, "test"));
        }

        [Fact]
        public void assert_generic_argument_not_null_does_not_throw_if_reference_is_not_null()
        {
            ArgumentHelper.AssertGenericArgumentNotNull("test", "test");
        }

        [Fact]
        public void assert_generic_argument_not_null_does_not_throw_if_interface_is_not_null()
        {
            ArgumentHelper.AssertGenericArgumentNotNull((IComparable)"test", "test");
        }

        [Fact]
        public void assert_generic_argument_not_null_throws_if_nullable_has_no_value()
        {
            Assert.Throws<ArgumentNullException>(() => ArgumentHelper.AssertGenericArgumentNotNull((int?)null, "test"));
        }

        [Fact]
        public void assert_generic_argument_not_null_does_not_throw_if_nullable_has_a_value()
        {
            ArgumentHelper.AssertGenericArgumentNotNull((int?)13, "test");
        }

        [Fact]
        public void assert_generic_argument_not_null_does_not_throw_if_given_a_value_type()
        {
            ArgumentHelper.AssertGenericArgumentNotNull(1, "test");
            ArgumentHelper.AssertGenericArgumentNotNull(1.37D, "test");
            ArgumentHelper.AssertGenericArgumentNotNull(TimeSpan.Zero, "test");
            ArgumentHelper.AssertGenericArgumentNotNull(DateTime.Now, "test");
        }

        [Fact]
        public void assert_not_null_or_empty_throws_if_string_is_null()
        {
            Assert.Throws<ArgumentException>(() => ArgumentHelper.AssertNotNullOrEmpty((string)null, "test"));
        }

        [Fact]
        public void assert_not_null_or_empty_throws_if_string_is_empty()
        {
            Assert.Throws<ArgumentException>(() => ArgumentHelper.AssertNotNullOrEmpty(string.Empty, "test"));
        }

        [Fact]
        public void assert_not_null_or_empty_does_not_throw_if_string_is_not_null()
        {
            ArgumentHelper.AssertNotNullOrEmpty("test", "test");
        }

        [Fact]
        public void assert_not_null_or_empty_throws_if_enumerable_is_null()
        {
            Assert.Throws<ArgumentException>(() => ArgumentHelper.AssertNotNullOrEmpty((IEnumerable)null, "test"));
        }

        [Fact]
        public void assert_not_null_or_empty_throws_if_enumerable_is_empty()
        {
            Assert.Throws<ArgumentException>(() => ArgumentHelper.AssertNotNullOrEmpty(Enumerable.Empty<string>(), "test"));
        }

        [Fact]
        public void assert_not_null_or_empty_does_not_throw_if_enumerable_is_not_empty()
        {
            ArgumentHelper.AssertNotNullOrEmpty(Enumerable.Range(0, 10), "test");
        }

        [Fact]
        public void assert_not_null_or_empty_throws_if_collection_is_null()
        {
            Assert.Throws<ArgumentException>(() => ArgumentHelper.AssertNotNullOrEmpty((ICollection)null, "test"));
        }

        [Fact]
        public void assert_not_null_or_empty_throws_if_collection_is_empty()
        {
            Assert.Throws<ArgumentException>(() => ArgumentHelper.AssertNotNullOrEmpty(new List<string>(), "test"));
        }

        [Fact]
        public void assert_not_null_or_empty_does_not_throw_if_collection_is_not_empty()
        {
            ArgumentHelper.AssertNotNullOrEmpty(new List<string>(new string[] { "First", "Second" }), "test");
        }

        [Fact]
        public void assert_not_null_or_white_space_throws_if_string_is_null()
        {
            Assert.Throws<ArgumentException>(() => ArgumentHelper.AssertNotNullOrWhiteSpace((string)null, "test"));
        }

        [Fact]
        public void assert_not_null_or_white_space_throws_if_string_is_empty()
        {
            Assert.Throws<ArgumentException>(() => ArgumentHelper.AssertNotNullOrWhiteSpace(string.Empty, "test"));
        }

        [Fact]
        public void assert_not_null_or_white_space_throws_if_string_contains_only_white_space()
        {
            Assert.Throws<ArgumentException>(() => ArgumentHelper.AssertNotNullOrWhiteSpace("  \t     \r \n  ", "test"));
        }

        [Fact]
        public void assert_not_null_or_white_space_does_not_throw_if_string_is_not_entirely_white_space()
        {
            ArgumentHelper.AssertNotNullOrWhiteSpace("  a ", "test");
        }

        [Fact]
        public void assert_enum_member_throws_if_given_invalid_flag_enumeration_value()
        {
            var ex = Assert.Throws<ArgumentException>(() => ArgumentHelper.AssertEnumMember((FlagsEnum)68, "test"));
            Assert.Equal(string.Format(CultureInfo.InvariantCulture, "Enum value '68' is not valid for flags enumeration 'Kent.Boogaart.HelperTrinity.UnitTests.ArgumentHelperFixture+FlagsEnum'.{0}Parameter name: test", Environment.NewLine), ex.Message);
        }

        [Fact]
        public void assert_enum_member_throws_if_given_invalid_zero_flag_enumeration_value()
        {
            var ex = Assert.Throws<ArgumentException>(() => ArgumentHelper.AssertEnumMember((FlagsEnumNoNone)0, "test"));
            Assert.Equal(string.Format(CultureInfo.InvariantCulture, "Enum value '0' is not valid for flags enumeration 'Kent.Boogaart.HelperTrinity.UnitTests.ArgumentHelperFixture+FlagsEnumNoNone'.{0}Parameter name: test", Environment.NewLine), ex.Message);
        }

        [Fact]
        public void assert_enum_member_does_not_throw_if_enumeration_flags_are_valid()
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
        public void assert_enum_member_throws_if_enumeration_value_is_invalid()
        {
            var ex = Assert.Throws<ArgumentException>(() => ArgumentHelper.AssertEnumMember((DayOfWeek)69, "test"));
            Assert.Equal(string.Format(CultureInfo.InvariantCulture, "Enum value '69' is not defined for enumeration 'System.DayOfWeek'.{0}Parameter name: test", Environment.NewLine), ex.Message);
        }

        [Fact]
        public void assert_enum_member_does_not_throw_if_enumeration_values_are_valid()
        {
            ArgumentHelper.AssertEnumMember(DayOfWeek.Monday, "test");
            ArgumentHelper.AssertEnumMember((DayOfWeek)3, "test");
        }

        [Fact]
        public void assert_enum_member_works_for_byte_flags_enumeration()
        {
            ArgumentHelper.AssertEnumMember(ByteFlagsEnum.One | ByteFlagsEnum.Three, "test");
            ArgumentHelper.AssertEnumMember(ByteFlagsEnum.None, "test");
            ArgumentHelper.AssertEnumMember(ByteFlagsEnum.One | ByteFlagsEnum.Two | ByteFlagsEnum.Three, "test");

            var ex = Assert.Throws<ArgumentException>(() => ArgumentHelper.AssertEnumMember((ByteFlagsEnum)80, "test"));
            Assert.Equal(string.Format(CultureInfo.InvariantCulture, "Enum value '80' is not valid for flags enumeration 'Kent.Boogaart.HelperTrinity.UnitTests.ArgumentHelperFixture+ByteFlagsEnum'.{0}Parameter name: test", Environment.NewLine), ex.Message);
        }

        [Fact]
        public void assert_enum_member_works_for_byte_enumeration()
        {
            ArgumentHelper.AssertEnumMember(ByteEnum.One, "test");
            ArgumentHelper.AssertEnumMember(ByteEnum.Two, "test");
            ArgumentHelper.AssertEnumMember(ByteEnum.Three, "test");

            var ex = Assert.Throws<ArgumentException>(() => ArgumentHelper.AssertEnumMember((ByteEnum)10, "test"));
            Assert.Equal(string.Format(CultureInfo.InvariantCulture, "Enum value '10' is not defined for enumeration 'Kent.Boogaart.HelperTrinity.UnitTests.ArgumentHelperFixture+ByteEnum'.{0}Parameter name: test", Environment.NewLine), ex.Message);
        }

        [Fact]
        public void assert_enum_member_throws_if_no_valid_values_are_provided()
        {
            Assert.Throws<ArgumentNullException>(() => ArgumentHelper.AssertEnumMember(DayOfWeek.Monday, "test", null));
        }

        [Fact]
        public void assert_enum_member_throws_if_flag_value_is_not_a_valid_value()
        {
            var ex = Assert.Throws<ArgumentException>(() => ArgumentHelper.AssertEnumMember(FlagsEnum.Three, "test", FlagsEnum.One, FlagsEnum.Two, FlagsEnum.Four));
            Assert.Equal(string.Format(CultureInfo.InvariantCulture, "Enum value 'Three' is not allowed for flags enumeration 'Kent.Boogaart.HelperTrinity.UnitTests.ArgumentHelperFixture+FlagsEnum'.{0}Parameter name: test", Environment.NewLine), ex.Message);
        }

        [Fact]
        public void assert_enum_member_if_valid_flag_values_are_provided_but_the_enumeration_value_is_not_a_valid_flag_enumeration_value()
        {
            var ex = Assert.Throws<ArgumentException>(() => ArgumentHelper.AssertEnumMember((FlagsEnum)68, "test", FlagsEnum.One, FlagsEnum.Two, FlagsEnum.Four));
            Assert.Equal(string.Format(CultureInfo.InvariantCulture, "Enum value '68' is not allowed for flags enumeration 'Kent.Boogaart.HelperTrinity.UnitTests.ArgumentHelperFixture+FlagsEnum'.{0}Parameter name: test", Environment.NewLine), ex.Message);
        }

        [Fact]
        public void assert_enum_member_throws_if_invalid_zero_flag_enumeration_value_is_passed_in_but_it_is_not_a_valid_value()
        {
            var ex = Assert.Throws<ArgumentException>(() => ArgumentHelper.AssertEnumMember((FlagsEnumNoNone)0, "test", FlagsEnumNoNone.One));
            Assert.Equal(string.Format(CultureInfo.InvariantCulture, "Enum value '0' is not allowed for flags enumeration 'Kent.Boogaart.HelperTrinity.UnitTests.ArgumentHelperFixture+FlagsEnumNoNone'.{0}Parameter name: test", Environment.NewLine), ex.Message);
        }

        [Fact]
        public void assert_enum_member_throws_if_zero_flag_enumeration_value_is_passed_in_but_it_is_not_a_valid_value()
        {
            var ex = Assert.Throws<ArgumentException>(() => ArgumentHelper.AssertEnumMember(FlagsEnum.None, "test", FlagsEnum.One));
            Assert.Equal(string.Format(CultureInfo.InvariantCulture, "Enum value 'None' is not allowed for flags enumeration 'Kent.Boogaart.HelperTrinity.UnitTests.ArgumentHelperFixture+FlagsEnum'.{0}Parameter name: test", Environment.NewLine), ex.Message);
        }

        [Fact]
        public void assert_enum_member_does_not_throw_if_flag_enumeration_values_are_valid()
        {
            var validValues = new FlagsEnum[] { FlagsEnum.One, FlagsEnum.Two, FlagsEnum.Four, FlagsEnum.None };
            ArgumentHelper.AssertEnumMember(FlagsEnum.None, "test", validValues);
            ArgumentHelper.AssertEnumMember(FlagsEnum.One, "test", validValues);
            ArgumentHelper.AssertEnumMember(FlagsEnum.Two, "test", validValues);
            ArgumentHelper.AssertEnumMember(FlagsEnum.Four, "test", validValues);
            ArgumentHelper.AssertEnumMember(FlagsEnum.One | FlagsEnum.Two, "test", validValues);
            ArgumentHelper.AssertEnumMember(FlagsEnum.One | FlagsEnum.Four, "test", validValues);
            ArgumentHelper.AssertEnumMember(FlagsEnum.One | FlagsEnum.Two | FlagsEnum.Four, "test", validValues);
        }

        [Fact]
        public void assert_enum_member_throws_if_value_is_not_a_valid_value()
        {
            var ex = Assert.Throws<ArgumentException>(() => ArgumentHelper.AssertEnumMember(DayOfWeek.Monday, "test", DayOfWeek.Friday, DayOfWeek.Sunday));
            Assert.Equal(string.Format(CultureInfo.InvariantCulture, "Enum value 'Monday' is defined for enumeration 'System.DayOfWeek' but it is not permitted in this context.{0}Parameter name: test", Environment.NewLine), ex.Message);
        }

        [Fact]
        public void assert_enum_member_if_valid_values_are_provided_but_the_enumeration_value_is_not_a_valid_enumeration_value()
        {
            var ex = Assert.Throws<ArgumentException>(() => ArgumentHelper.AssertEnumMember((DayOfWeek)69, "test", DayOfWeek.Friday, DayOfWeek.Sunday));
            Assert.Equal(string.Format(CultureInfo.InvariantCulture, "Enum value '69' is not defined for enumeration 'System.DayOfWeek'.{0}Parameter name: test", Environment.NewLine), ex.Message);
        }

        [Fact]
        public void assert_enum_member_does_not_throw_if_valid_values_are_specified_and_enumeration_values_are_valid()
        {
            var validValues = new DayOfWeek[] { DayOfWeek.Friday, DayOfWeek.Sunday, DayOfWeek.Saturday };
            ArgumentHelper.AssertEnumMember(DayOfWeek.Friday, "test", validValues);
            ArgumentHelper.AssertEnumMember(DayOfWeek.Sunday, "test", validValues);
            ArgumentHelper.AssertEnumMember(DayOfWeek.Saturday, "test", validValues);
        }

        [Fact(Skip = "This is a performance test.")]
        public void performance_test_reflective_versus_non_reflective()
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
