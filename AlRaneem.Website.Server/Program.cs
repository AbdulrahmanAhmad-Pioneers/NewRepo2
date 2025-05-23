using AlRaneem.Website.DataAccess;
using AlRaneem.Website.DataAccess.Contexts;
using AlRaneem.Website.DataAccess.Enums;
using AlRaneem.Website.DataAccess.Models;
using AlRaneem.Website.DataAccess.Models.SupportSystemModels;
using AlRaneem.Website.Server.handlers;
using AlRaneem.Website.Server.Middlewars;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.AddDataAccessRegistration();

//builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);

//builder.Services.AddAuthentication()
//    .AddBearerToken(IdentityConstants.BearerScheme, options =>
//    {
//        options.BearerTokenExpiration = TimeSpan.FromDays(7); // Set expiry to 7 days
//    });
var azureAd = builder.Configuration
    .GetSection("AzureAd")
    .Get<AzureConfig>();

builder.Services.Configure<AzureConfig>(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://login.microsoftonline.com/" + azureAd?.TenantId;
        options.Audience = "api://"+ azureAd?.ClientId;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true
        };
    });
builder.Services.AddScoped<IAuthorizationHandler, RoleAuthorizationHandler>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.Requirements.Add(new RoleRequirement(UserRoles.Admin)));
});


builder.Services.AddIdentityCore<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddApiEndpoints();



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    await context.Database.MigrateAsync();
    await Seeder.Seed(services);
}
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseCors("AllowAll");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
//app.UseMiddleware<ResponseTransformationMiddleware>();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.MapIdentityApi<ApplicationUser>();
app.MapFallbackToFile("/index.html");

app.Run();
