using System.Security.Cryptography;
using DotNetEnv;
using MarketingAlliance.App.Service;
using MarketingAlliance.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services);

var app = builder.Build();

ApplyMigrations(app);
ConfigureMiddleware(app);

app.Run();

void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();
    ConfigureAuthentication(services);

    ConfigureDatabase(services);
    ConfigureRedis(services);
    ConfigureMail(services);

    services.AddSingleton<RetailsService>();
    services.AddSingleton<CacheService>();
    ConfigureSwagger(services);




}

void ConfigureMail(IServiceCollection services)
{
    var mailServer = GetEnvVar("MAIL_SERVER");
    var mailPort = int.Parse(GetEnvVar("MAIL_PORT"));
    services.AddSingleton<MailService>(sp => new MailService(mailServer, mailPort));
}


void ConfigureRedis(IServiceCollection services)
{
    services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = GetEnvVar("REDIS_CONNECTION_STRING");
        options.InstanceName = GetEnvVar("REDIS_INSTANCE_NAME");
    });
}

void ApplyMigrations(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<MarketingAllianceContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)

    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

void ConfigureAuthentication(IServiceCollection services)
{
    RSA rsa = RSA.Create();
    var publicKey = GetEnvVar("JWT_ASYMMETRIC_PUBLIC_KEY");
    var jwtIssuer = GetEnvVar("JWT_ISSUER");
    var jwtAudience = GetEnvVar("JWT_AUDIENCE");

    rsa.ImportRSAPublicKey(
        source: Convert.FromBase64String(publicKey),
        bytesRead: out int _);

    var rsaSecurityKey = new RsaSecurityKey(rsa);
    services.AddSingleton(rsaSecurityKey);

    services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = rsaSecurityKey,
                ValidAudience = jwtAudience,
                ValidIssuer = jwtIssuer,
                RequireSignedTokens = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                RequireExpirationTime = false,
                ValidateLifetime = true
            };
        });
}

void ConfigureDatabase(IServiceCollection services)
{
    var connectionString = GetEnvVar("CONNECTION_STRING");
    services.AddDbContext<MarketingAllianceContext>(options =>
        options.UseNpgsql(connectionString)
    );
}

void ConfigureMiddleware(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
}

string GetEnvVar(string name) => Environment.GetEnvironmentVariable(name) ?? throw new Exception($"{name} is not set");

void ConfigureSwagger(IServiceCollection services)
{
    services.AddSwaggerExamples();

    services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "Marketing Alliance Api",
            Description = "Api",
        });

        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "Bearer auth scheme",
            In = ParameterLocation.Header,
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        });

        options.ExampleFilters();
        options.OperationFilter<SecurityRequirementsOperationFilter>();
        options.EnableAnnotations();
    });
}

