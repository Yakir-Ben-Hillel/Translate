using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using Translation.API.Models.Configurations;
using Translation.API.Services;
namespace Translation.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            // Cache previous requests to translation api. 
            builder.Services.AddResponseCaching();
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme.",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });

                c.OperationFilter<SecurityRequirementsOperationFilter>();
            });
            builder.Services.Configure<TranslationApiConfig>(builder.Configuration.GetSection("TranslationApi"));
            builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("Jwt"));
            builder.Services.AddScoped<ITranslationService, TranslationService>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            var translationApiConfig = builder.Configuration.GetSection("TranslationApi").Get<TranslationApiConfig>()
            ?? throw new InvalidOperationException("TranslationApi configuration section is missing");

            var jwtConfig = builder.Configuration.GetSection("Jwt").Get<JwtConfig>()
            ?? throw new InvalidOperationException("Jwt configuration section is missing");

            builder.Services.AddHttpClient(translationApiConfig.HttpClientName, client =>
            {
                client.BaseAddress = new Uri(translationApiConfig.ApiBaseAddress);
                foreach (var header in translationApiConfig.Headers)
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            });
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtConfig.Issuer,
                    ValidAudience = jwtConfig.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SeceretKey))
                };
            });
            builder.Services.AddCors(options =>
            options.AddDefaultPolicy(policy =>
                policy.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod()));
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseCors();
            app.UseResponseCaching();
            app.MapControllers();

            app.Run();
        }
    }
}
