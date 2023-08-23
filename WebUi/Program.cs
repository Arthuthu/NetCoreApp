using WebUi.Configuration;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services
	.AddApplicationDependencyInjection()
	.AddApplicationDbContext(config)
	.AddAuthenticationAndAuthorization(config)
	.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
