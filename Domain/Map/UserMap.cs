using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Map;

public class UserMap : BaseMap<UserModel>
{
	public UserMap() : base("Users")
	{
	}

	public override void Configure(EntityTypeBuilder<UserModel> builder)
	{
		base.Configure(builder);

		builder.HasKey(u => u.Id);
		builder.Property(u => u.Username).HasColumnType("varchar(100)").IsRequired();
		builder.Property(u => u.Password).HasColumnType("nvarchar(20)").IsRequired();
		builder.Property(u => u.Role).HasColumnType("nvarchar(50)");
	}
}
