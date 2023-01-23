
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BlazorWASMCustomAuth.Security.Infrastructure;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.EntityFrameworkCore;
using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using BlazorWASMCustomAuth.SecurityEF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<SecurityTablesTestContext>(options =>
{
	options.UseSqlServer("Server=.;Database=SecurityTablesTest;Trusted_Connection=True;Trust Server Certificate=true");
});

builder.Services.AddScoped<SecurityService>();
builder.Services.AddScoped<SecurityServiceEF>();

builder.Services.Configure<TokenSettingsModel>(builder.Configuration.GetSection("TokenSettings"));

var IssuerSigningKey = builder.Configuration.GetSection("TokenSettings").GetValue<string>("Key") ?? "";
if(IssuerSigningKey == "")
{
	return; //Exit app if IssuerSigningKey is not found
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidIssuer = builder.Configuration.GetSection("TokenSettings").GetValue<string>("Issuer"),
		ValidateIssuer = true,
		ValidAudience = builder.Configuration.GetSection("TokenSettings").GetValue<string>("Audience"),
		ValidateAudience = true,
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(IssuerSigningKey)),
		ValidateIssuerSigningKey = true,
		ValidateLifetime = true,
	};
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "myOrigins",
                      builder =>
                      {
                          builder.AllowAnyOrigin();
                          builder.AllowAnyHeader();
                          builder.AllowAnyMethod();
                      });

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}



app.UseHttpsRedirection();
app.UseCors("myOrigins");
app.UseAuthorization();

app.MapControllers();

app.Run();
