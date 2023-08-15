using Domain.Models;
using WebUi.Dto;

namespace WebUi.Mapper;

public static class DtoToDomain
{
	public static User MapDtoToDomain(this UserRequest userDto)
	{
		return new User
		{
			Username = userDto.Username,
			Password = userDto.Password,
		};
	}
}
