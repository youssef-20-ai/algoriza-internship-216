using AlgorizaProject.DAL.DbContext;
using AlgorizaProject.DAL.Entities;
using AlgorizaProject.Dtos;
using AlgorizaProject.Helper;
using APIDemo.BLL.Interface;
using APIDemo.BLL.Reposatories;
using APIDemo.BLL.Services;
using APIDemo.DAL.Interface;
using APIDemo.DAL.Reposatories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

namespace AlgorizaProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<VezeetaDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            }).AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<VezeetaDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<ITokenService, TokenService>();

            builder.Services.AddAutoMapper(typeof(MapperProfiler));

            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
             .AddJwtBearer(options =>
             {
                 options.SaveToken = true;
                 options.TokenValidationParameters = new TokenValidationParameters()
                 {
                     ValidateAudience = true,
                     ValidAudience = builder.Configuration["JWT:ValidAudience"],
                     ValidateIssuer = true,
                     ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
                     ValidateLifetime = true,
                     ClockSkew = TimeSpan.Zero
                 };
             });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo() { Title = "Talabat.API", Version = "v1" });
                var securityScheme = new OpenApiSecurityScheme
                {
                    Description = "JWT Auth Bearer Scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                c.AddSecurityDefinition("Bearer", securityScheme);

                var securityrequirement = new OpenApiSecurityRequirement { { securityScheme, new[] { "Bearer" } } };

                c.AddSecurityRequirement(securityrequirement);
            });

            builder.Services.AddMvc()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });



            var app = builder.Build();

            // Configure the HTTP request pipeline.
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
        }
    }
}