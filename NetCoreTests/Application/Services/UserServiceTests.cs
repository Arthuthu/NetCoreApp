using Application.Services.User;
using AutoFixture;
using Domain.Models;
using Domain.Repositories.User;
using FluentAssertions;
using NSubstitute;

namespace NetCoreTests.Application.Services;

public class UserServiceTests
{
    private readonly UserService _sut;
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
	private readonly Fixture _fixture = new();

    public UserServiceTests()
    {
		_sut = new UserService(_userRepository);
	}

    [Fact]
    public async void GetUserById_ShouldReturnUser_WhenIdIsValid()
    {
        //Arrange
        var expectedUser = _fixture.Build<UserModel>()
            .With(u => u.Id, Guid.NewGuid())
            .With(u => u.Username, "John")
            .Create();

        CancellationToken cancellationToken = new();

        _userRepository.Get(expectedUser.Id, cancellationToken).Returns(expectedUser);

        //Act
        var result = await _sut.Get(expectedUser.Id, cancellationToken);

        //Assert
        result.Should().NotBeNull()
            .And.BeOfType<UserModel>()
            .And.BeSameAs(expectedUser);
    }

	[Fact]
	public async void GetUserById_ShouldReturnNull_WhenIdIsInvalid()
	{
		//Arrange
		var expectedUser = _fixture.Build<UserModel>()
			.With(u => u.Id, Guid.NewGuid())
			.With(u => u.Username, "John")
			.Create();

        UserModel? nullValue = null;

		CancellationToken cancellationToken = new();

		_userRepository.Get(expectedUser.Id, cancellationToken).Returns(nullValue);

		//Act
		var result = await _sut.Get(expectedUser.Id, cancellationToken);

        //Assert
        result.Should().BeNull();
	}
}
