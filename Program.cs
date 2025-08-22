using Firewall.Repositories;
using Firewall.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Persistence;

DotNetEnv.Env.Load(); 

var builder = WebApplication.CreateBuilder(args);

// mongodb
var mongoSettings = builder.Configuration.GetSection("MongoDbSettings");
var mongoConnectionString = mongoSettings["ConnectionString"];
var mongoDatabaseName = mongoSettings["DatabaseName"];

builder.Services.AddSingleton<MongoDbContext>(sp =>
    new MongoDbContext(mongoConnectionString, mongoDatabaseName));

// env variables
var tenantId = Environment.GetEnvironmentVariable("AZURE_AD_TENANT_ID");
var clientId = Environment.GetEnvironmentVariable("AZURE_AD_CLIENT_ID");
var apiToken = Environment.GetEnvironmentVariable("HETZNER_API_TOKEN")
    ?? throw new Exception("Hetzner API token not configured");

// cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontEnd", builder =>
    {
        builder.WithOrigins("http://localhost:3050")
               .AllowAnyHeader()
               .AllowAnyMethod();
        //.AllowCredentials(); 
    });
});

// hetzner
builder.Services.AddHttpClient<IFirewallsRepository, FirewallsRepository>(client =>
{
    client.BaseAddress = new Uri("https://api.hetzner.cloud/v1/");
    client.DefaultRequestHeaders.Authorization =
        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiToken);
});

// repos
builder.Services.AddScoped<IFirewallRequestRepository, FirewallRequestRepository>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();

// azure ad
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"https://login.microsoftonline.com/{tenantId}";
        options.Audience = clientId;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = $"https://sts.windows.net/{tenantId}/",
            ValidateAudience = true,
            ValidAudience = clientId,
            ValidateLifetime = true
        };
    });

// controllers and swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.WebHost.UseUrls("http://0.0.0.0:5000");

var app = builder.Build();
app.MapSubscribeHandler(); 


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("AllowFrontEnd");

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();
