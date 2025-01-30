using Application;
using BackEnd.Middlewares;
using Domain;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace BackEnd
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Verify is exist Important data in UserSecret
            var jwtSection = builder.Configuration.GetSection("JwtData");
            var connectionStringsSection = builder.Configuration.GetSection("ConnectionStrings");
            var emailStringsSection = builder.Configuration.GetSection("EmailStrings");

            var issuer = jwtSection["Issuer"];
            var audience = jwtSection["Audience"];
            var secret = jwtSection["Secret"];
            var connectionString = connectionStringsSection["DbString"];
            var emailString = emailStringsSection["Email"];
            var passwordString = emailStringsSection["Password"];

            if (string.IsNullOrWhiteSpace(issuer))
            {
                throw new NotImplementedException(Messages.NotConfiguredIssuer);
            }
            if (string.IsNullOrWhiteSpace(audience))
            {
                throw new NotImplementedException(Messages.NotConfiguredAudience);
            }
            if (string.IsNullOrWhiteSpace(secret))
            {
                throw new NotImplementedException(Messages.NotConfiguredSecret);
            }
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new NotImplementedException(Messages.NotConfiguredSecret);
            }
            if (string.IsNullOrWhiteSpace(emailString))
            {
                throw new NotImplementedException("Not configured Email");
            }
            if (string.IsNullOrWhiteSpace(passwordString))
            {
                throw new NotImplementedException("Not configured Email");
            }

            // Add services to the container.
            //Injected IConfiguration from this Project
            builder.Services.AddMemoryCache();
            builder.Services.DomainConfiguration(builder.Configuration);
            builder.Services.ApplicationConfiguration(builder.Configuration);
            builder.Services.InfrastructureConfiguration(builder.Configuration);


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Pleese enter 'Bearer [jwt]'",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                });

                var scheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement { { scheme, Array.Empty<string>() } });
            });

            //Add Authentication JWT
            builder.Services
                .AddAuthentication(opt =>
            {
                //Creating Default Scheme [We can use in different Controllers Differnt Scheme]
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(opt =>
            {
                opt.SaveToken = true;
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true, //ClockSkew
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,

                    ClockSkew = TimeSpan.Zero, //Allowed Expired Tokens,ex. TimeSpan.FromMinutes(1)
                    ValidIssuer = issuer, //Who Gives Token
                    ValidAudience = audience, //For Who Given Token
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)), //Secret
                };

                //Returns info about Expired Token
                opt.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Append("Token-expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            var app = builder.Build();
            //React Configuration
            app.UseCors(x => x
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(origin => true) // allow any origin
                    .AllowCredentials());

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseGlobalExceptionHandler();

            app.UseHttpsRedirection();
            app.UseAuthenticationWerifier();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
