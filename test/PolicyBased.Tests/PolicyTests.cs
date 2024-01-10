using System;
using System.Security.Claims;
using System.Threading.Tasks;
using FakeRepos;
using FluentAssertions;
using PolicyBased.Contracts;
using Xunit;

namespace PolicyBased.Tests
{
    public class PolicyTests
    {
        Policy _policyToTest;
        IFakeRepository _fakeRepository;

        // these are the "sub" claim, as defined in the OIDC spec
        string _subjectIdentifier1;
        string _subjectIdentifier2;
        string _subjectIdentifier3;

        // app specific roles
        string _appSecificRole1;
        string _appSecificRole2;
        string _appSecificRole3;

        // app specific permissions
        string _appPermission1;
        string _appPermission2;
        string _appPermission3;

        // setup
        public PolicyTests()
        {
            _policyToTest = new Policy();
            _fakeRepository = new FakeRepository();

            _subjectIdentifier1 = "1";
            _subjectIdentifier2 = "2";
            _subjectIdentifier3 = "3";

            _appSecificRole1 = "Tenant1 User role";
            _appSecificRole2 = "Tenant1 Team lead role";
            _appSecificRole3 = "Tenant1 Admin role";

            _appPermission1 = "CanViewOrders";
            _appPermission2 = "CanDeleteOrders";
            _appPermission3 = "CanExportOrderToPDF";
        }

        [Fact]
        public void Evaluate_Should_Require_User()
        {
            // arrange
            ClaimsPrincipal userClaims = null; // What if claims are empty? must fail

            // act
            Func<Task> a = () => _policyToTest.EvaluateAsync(userClaims);

            // assert
            a.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task Evaluate_Should_Return_Matched_Roles()
        {
            // arrange
            _policyToTest.Roles.AddRange(new[] {
                new Role{ Name = _appSecificRole1, Subjects = { _subjectIdentifier1 } },
                new Role{ Name = _appSecificRole2, Subjects = { _subjectIdentifier1 } },
                new Role{ Name = _appSecificRole3, Subjects = { _subjectIdentifier2 } },
            });

            var user = _fakeRepository.CreateUser(_subjectIdentifier1);

            // act
            var result = await _policyToTest.EvaluateAsync(user);

            // assert
            // user 1 must have 2 app roles r1 and r2
            result.Roles.Should().BeEquivalentTo(new[] { _appSecificRole1, _appSecificRole2 });
        }

        [Fact]
        public async Task Evaluate_Should_Not_Return_Unmatched_Roles()
        {
            // arrange
            _policyToTest.Roles.AddRange(new[] {
                new Role{ Name = _appSecificRole3, Subjects = { _subjectIdentifier2 } },
                new Role{ Name = _appSecificRole1, Subjects = { _subjectIdentifier3 } },
                new Role{ Name = _appSecificRole2, Subjects = { _subjectIdentifier2 } },
            });

            var user = _fakeRepository.CreateUser(_subjectIdentifier1);

            // act
            var result = await _policyToTest.EvaluateAsync(user);

            // assert
            // user 1 does not have any roles
            result.Roles.Should().BeEmpty();
        }

        [Fact]
        public async Task Evaluate_Should_Return_Remove_Duplicate_Roles()
        {
            // arrange
            _policyToTest.Roles.AddRange(new[] {
                new Role{ Name = _appSecificRole1, Subjects = { _subjectIdentifier1 } },
                new Role{ Name = _appSecificRole1, Subjects = { _subjectIdentifier1 } },
            });

            var user = _fakeRepository.CreateUser(_subjectIdentifier1);

            // act
            var result = await _policyToTest.EvaluateAsync(user);

            // assert
            result.Roles.Should().BeEquivalentTo(new[] { _appSecificRole1 });
        }

        [Fact]
        public async Task Evaluate_Should_Return_Matched_Permissions()
        {
            // arrange
            _policyToTest.Roles.AddRange(new[] {
                new Role{ Name = _appSecificRole1, Subjects = { _subjectIdentifier1 } },
                new Role{ Name = _appPermission1, Subjects = { _subjectIdentifier1 } },
            });

            _policyToTest.Permissions.AddRange(new[] {
                new Permission{ Name = _appPermission1, Roles = { _appSecificRole1 } },
                new Permission{ Name = _appPermission3, Roles = { _appSecificRole1 } },
                new Permission{ Name = _appPermission2, Roles = { _appSecificRole2 } },
            });

            var user = _fakeRepository.CreateUser(_subjectIdentifier1);

            // act
            var result = await _policyToTest.EvaluateAsync(user);

            // assert
            result.Permissions.Should().BeEquivalentTo(new[] { _appPermission1, _appPermission3 });
        }

        [Fact]
        public async Task Evaluate_Should_Not_Return_Unmatched_Permissions()
        {
            // arrange
            _policyToTest.Roles.AddRange(new[] {
                new Role{ Name = _appSecificRole2, Subjects = { _subjectIdentifier2 } },
            });
            _policyToTest.Permissions.AddRange(new[] {
                new Permission{ Name = _appPermission1, Roles = { "CanInviteMembers" } },
                new Permission{ Name = _appPermission3, Roles = { "CanInviteMembers" } },
                new Permission{ Name = _appPermission2, Roles = { "CanInviteMembers" } },
            });

            var user = _fakeRepository.CreateUser(_subjectIdentifier2);

            // act
            var result = await _policyToTest.EvaluateAsync(user);

            // assert
            result.Permissions.Should().BeEmpty();
        }

        [Fact]
        public async Task Evaluate_Should_Remove_Duplicate_Permissions()
        {
            // arrange
            _policyToTest.Roles.AddRange(new[] {
                new Role{ Name = _appSecificRole1, Subjects = { _subjectIdentifier3} },
            });
            _policyToTest.Permissions.AddRange(new[] {
                new Permission{ Name = _appPermission1, Roles = { _appSecificRole1 } },
                new Permission{ Name = _appPermission1, Roles = { _appSecificRole1 } },
            });

            var user = _fakeRepository.CreateUser(_subjectIdentifier3);

            // act
            var result = await _policyToTest.EvaluateAsync(user);

            // assert
            result.Permissions.Should().BeEquivalentTo(new[] { _appPermission1 });
        }

        [Fact]
        public async Task Evaluate_Should_Not_Allow_Identity_Roles_To_Match_Permissions()
        {
            // arrange
            _policyToTest.Permissions.AddRange(new[] {
                new Permission{ Name = _appPermission1, Roles = { _appSecificRole1 } },
            });

            var user = _fakeRepository.CreateUser(_subjectIdentifier1, roles: new[] { _appSecificRole1 });

            // act
            var result = await _policyToTest.EvaluateAsync(user);

            // assert
            result.Permissions.Should().BeEmpty();
        }
    }
}