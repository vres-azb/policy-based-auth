using System;
using System.Security.Claims;
using PolicyBased.Contracts;
using FluentAssertions;
using Xunit;
using FakeRepos;

namespace PolicyBased.Tests
{
    public class RoleTests
    {
        Role _roleToTest;
        IFakeRepository _fakeRepository;

        // setup
        public RoleTests()
        {
            _roleToTest = new Role();
            _fakeRepository = new FakeRepository();
        }

        [Fact]
        public void Evaluate_Should_Require_User()
        {
            // arrange
            ClaimsPrincipal userClaims = null;

            // act
            Action a = () => _roleToTest.Evaluate(userClaims);

            // assert
            a.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Evaluate_Should_Fail_For_Invalid_UserRole()
        {
            // arrange
            string sub = "1"; // subject identifier per OIDC specs
            var user = _fakeRepository.CreateUser(sub); // 

            // act
            var result = _roleToTest.Evaluate(user);

            // assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Evaluate_Should_Succeed_For_User_with_Role()
        {
            // arrange
            string userSubId = "1";
            _roleToTest.Subjects.Add(userSubId);

            // act
            var user = _fakeRepository.CreateUser(userSubId);
            var result = _roleToTest.Evaluate(user);

            // assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Evaluate_Should_Fail_For_Invalid_Role()
        {
            // arrange
            string userSubId = "1";
            var user = _fakeRepository.CreateUser(userSubId);

            // act
            var result = _roleToTest.Evaluate(user);

            // assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Evaluate_Should_Succeed_For_Valid_Role()
        {
            // arrange
            string userSubId = "1";
            string applicationSpecificRole = "Appraiser";
            _roleToTest.IdentityRoles.Add(applicationSpecificRole);

            // act
            var user = _fakeRepository.CreateUser(userSubId, roles: new[] { applicationSpecificRole });
            var result = _roleToTest.Evaluate(user);

            // assert
            result.Should().BeTrue();
        }
    }
}