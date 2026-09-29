
using Common.Extensions;
using Common.Services.UserXRole;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using User.Api.Repositories.RefreshToken;
using User.Api.Services.Authentication;

namespace User.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerGeneratorOptions.SwaggerDocs.Clear();

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "User API",
                    Version = "v1",
                    Description = "Web Api do tokenów",
                });
                // Dodanie opcji autoryzacji w Swaggerze
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                string docFolder = "Doc";

                if (Directory.Exists(docFolder))
                {
                    var docFiles = Directory.GetFiles(docFolder, "*.xml", SearchOption.TopDirectoryOnly);

                    foreach (var doc in docFiles)
                    {
                        options.IncludeXmlComments(doc);
                        options.EnableAnnotations(); // W³¹cza obs³ugê adnotacji
                    }
                }

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            builder.Services.AddMroczwareRegisterDependencyInjections(Assembly.GetCallingAssembly());
            builder.Services.AddMroczwareDatabaseService();
            builder.Services.AddMemoryCache();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(15);
                options.Cookie.HttpOnly = true;
                options.Cookie.Name = "SessionCookie";
            });
            builder.Services.AddMroczwareEmailService();
            builder.Services.AddMroczwareUserService(builder.Configuration);
            builder.Services.AddScoped<IUserXRoleService, UserXRoleService>();
            builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

            builder.Services.AddMassTransit(busConfigurator =>
            {
                busConfigurator.AddConsumers(Assembly.GetExecutingAssembly());
                busConfigurator.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(prefix: "user", includeNamespace: false));
                busConfigurator.UsingRabbitMq((context, busFactoryConfiguration) =>
                {
                    busFactoryConfiguration.Host(builder.Configuration["RabbitMqSettings:Uri"]);
                    busFactoryConfiguration.ConfigureEndpoints(context);
                });
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                        {
                            // using var rsa = RSA.Create();

                            string publicKeyBase64 = builder.Configuration["JWT:JwtPublicKey"] ?? throw new Exception("JWT Private key not configured");

                            string publicKeyPem =
                            "-----BEGIN RSA PUBLIC KEY-----\n" +
                            FormatBase64String(publicKeyBase64) +
                            "\n-----END RSA PUBLIC KEY-----";

                            // Konwertuj klucz do RSA i wypisz jego parametry
                            RSA rsa = CreateRsaFromPkcs1(publicKeyPem);


                            //rsa.ImportSubjectPublicKeyInfo(publicKeyBytes, out _);             
                            options.SaveToken = true;
                            options.RequireHttpsMetadata = false;
                            options.TokenValidationParameters = new TokenValidationParameters
                            {
                                ValidateAudience = false,
                                ValidAudience = builder.Configuration["JWT:Audience"],
                                ValidateIssuer = true,
                                ValidIssuers = new[] { builder.Configuration["JWT:Issuer"] },
                                ValidateIssuerSigningKey = true,
                                IssuerSigningKey = new RsaSecurityKey(rsa),
                                RequireExpirationTime = true,
                                ValidateLifetime = true,
                                RequireSignedTokens = true,
                            };
                        });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseHsts();
            }

            //app.UseHttpsRedirection();

            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
                Secure = CookieSecurePolicy.Always,
                HttpOnly = HttpOnlyPolicy.Always
            });

            app.UseSession();

            app.UseCors(x => x
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithOrigins("http://task.api:80") // tylko ten adres jest dozwolony
                    .AllowCredentials()); // allow credentials

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        static string FormatBase64String(string base64)
        {
            // Dodaj ³amanie linii co 64 znaki, aby sformatowaæ zgodnie z wymogami PEM
            var sb = new StringBuilder();
            for (int i = 0; i < base64.Length; i += 64)
            {
                sb.AppendLine(base64.Substring(i, Math.Min(64, base64.Length - i)));
            }
            return sb.ToString();
        }

        static RSA CreateRsaFromPkcs1(string publicKeyPem)
        {
            // Usuñ nag³ówki i stopki PEM
            string publicKeyBase64 = publicKeyPem
                .Replace("-----BEGIN RSA PUBLIC KEY-----", "")
                .Replace("-----END RSA PUBLIC KEY-----", "")
                .Replace("\n", "")
                .Replace("\r", "");

            // Dekoduj Base64
            byte[] pkcs1Bytes = Convert.FromBase64String(publicKeyBase64);

            // Przekonwertuj PKCS#1 do PKCS#8
            byte[] pkcs8Bytes = ConvertPkcs1ToPkcs8(pkcs1Bytes);

            // Wczytaj klucz w formacie PKCS#8
            RSA rsa = RSA.Create();
            rsa.ImportSubjectPublicKeyInfo(pkcs8Bytes, out _);
            return rsa;
        }

        static byte[] ConvertPkcs1ToPkcs8(byte[] pkcs1Bytes)
        {
            using (var rsa = RSA.Create())
            {
                // Importuj klucz w formacie PKCS#1
                rsa.ImportRSAPublicKey(pkcs1Bytes, out _);

                // Eksportuj klucz w formacie PKCS#8
                return rsa.ExportSubjectPublicKeyInfo();
            }
        }
    }
}
