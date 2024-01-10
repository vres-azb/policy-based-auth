using System;
using FakeRepos;
using FluentAssertions;
using PolicyBased.Contracts;
using Xunit;

namespace PolicyBased.Tests
{
    public class PermissionTests
    {
        Permission _permissionToTest;

        // setup
        public PermissionTests()
        {
            _permissionToTest = new Permission();
        }

        [Fact]
        public void Evaluate_Should_Require_Role()
        {
            // arrange
            string[] roles = null; // What if roles are empty? must fail

            // act
            Action a = () => _permissionToTest.Evaluate(roles);

            // assert
            a.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Evaluate_Should_Fail_For_Invalid_Role()
        {
            // arrange
            string[] roles = new[] { "Super Admin", "Tenant Admin" };

            // act
            var result = _permissionToTest.Evaluate(roles);

            // assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Evaluate_Should_Pass_For_Valid_Roles()
        {
            // arrange
            string validRole = "User"; // this is an application specific role
            _permissionToTest.Roles.Add(validRole);

            // act
            var result = _permissionToTest.Evaluate(new[] { validRole });

            // assert
            result.Should().BeTrue();
        }
    }
}