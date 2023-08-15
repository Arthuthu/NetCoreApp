using Domain.Models;
using WebUi.Dto;

namespace WebUi.Mapper;

public static class DtoToDomain
{
	public static UserModel MapDtoToDomain(this UserRequest userDto)
	{
		return new UserModel
		{
			Username = userDto.Username,
			Password = userDto.Password,
		};
	}
}
