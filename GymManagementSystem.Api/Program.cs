
using GymManagementSystem.Core.Entities;
using GymManagementSystem.Core.Enums;

using GymManagementSystem.Api.Models;
using GymManagementSystem.Infrastructure;
using GymManagementSystem.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    o.AddSecurityRequirement(new OpenApiSecurityRequirement
    {{new OpenApiSecurityScheme

    {Reference = new OpenApiReference
    {
    Type = ReferenceType.SecurityScheme, Id ="Bearer"}}, Array.Empty<string>() }});
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));


builder.Services.AddScoped<CheckInService>();
builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddScoped<PasswordHasher>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o => o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"]!))
    });


builder.Services.AddScoped<MemberService>();

builder.Services.AddScoped<PlanService>();

builder.Services.AddScoped<SubscriptionService>();

builder.Services.AddScoped<PaymentService>();

builder.Services.AddAuthorization();





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
app.UseAuthorization();

app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var hasher = scope.ServiceProvider.GetRequiredService<PasswordHasher>();
    var pass = builder.Configuration["Seed:OwnerPassword"];
    if (!string.IsNullOrEmpty(pass) && !db.Users.Any(u => u.UserName == "owner"))

    {
        db.Users.Add(new User { UserName = "owner", PasswordHash = hasher.Hash(pass), Role = StaffRole.Owner });
        db.SaveChanges();

    }
}


app.Run();
