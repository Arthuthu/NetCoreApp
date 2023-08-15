using Domain.Models;
using WebUi.Dto;

namespace WebUi.Mapper.Auth;

public static class DtoToDomain
{
	public static UserModel MapDtoToDomain(this LoginRequest loginRequest)
	{
		return new UserModel
		{
			Username = loginRequest.Username,
			Password = loginRequest.Password,
		};
	}
}
