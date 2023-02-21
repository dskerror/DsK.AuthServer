using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BlazorWASMCustomAuth.Security.Infrastructure;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.EntityFrameworkCore;
using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//builder.Services.AddHttpLogging(logging =>
//{
//    // Customize HTTP logging here.
//    logging.LoggingFields = HttpLoggingFields.All;
//    logging.RequestHeaders.Add("sec-ch-ua");
//    logging.RequestHeaders.Add("Access-Control-Request-Headers");
//    logging.RequestHeaders.Add("Access-Control-Request-Method");
//    logging.RequestHeaders.Add("Origin");
//    logging.RequestHeaders.Add("Referer");
//    logging.RequestHeaders.Add("sec-fetch-mode");
//    logging.RequestHeaders.Add("sec-fetch-site");
//    logging.RequestHeaders.Add("sec-fetch-dest");
//    logging.RequestHeaders.Add("Authorization");
//    logging.ResponseHeaders.Add("Access-Control-Allow-Headers");
//    logging.ResponseHeaders.Add("Access-Control-Allow-Methods");
//    logging.ResponseHeaders.Add("Access-Control-Allow-Origin");
//    logging.ResponseHeaders.Add("my-response-header");
//    logging.MediaTypeOptions.AddText("application/javascript");
//    logging.RequestBodyLogLimit = 4096;
//    logging.ResponseBodyLogLimit = 4096;
//});


builder.Services.AddDbContext<SecurityTablesTestContext>(options =>
{
	options.UseSqlServer("Server=.;Database=SecurityTablesTest;Trusted_Connection=True;Trust Server Certificate=true");
});

builder.Services.AddScoped<SecurityService>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

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
	//app.UseSwagger();
	//app.UseSwaggerUI();
}


//app.UseHttpLogging();

app.UseHttpsRedirection();
app.UseCors("myOrigins");
app.UseAuthorization();

app.UseLoggingMiddleware();

app.MapControllers();

app.Run();
