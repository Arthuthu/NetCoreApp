using Application.Services.User;
using AutoFixture;
using Domain.Repositories.User;
using NSubstitute;

namespace NetCoreTests.Application.Services;

public class UserServiceTests
{
    private readonly UserService _sut;
    private readonly UserRepository _userRepository = Substitute.For<UserRepository>();
    private readonly Fixture _fixture = new Fixture();
