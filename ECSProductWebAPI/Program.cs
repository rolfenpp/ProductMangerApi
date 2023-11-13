using ECSProductWebAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;



var builder = WebApplication.CreateBuilder(args);


// Maskineriet för att kunna JwT Tokens, För att tolka en sådan.
builder.Services
.AddAuthentication()
.AddJwtBearer(options =>
      {
        // Här används signeringsnyckeln för att verifiera att den inte
        // manipulerats på vägen (av klienten, eller av någon annan som vill
        // attackera/utnyttja API:et)
        var signingKey = Convert.FromBase64String(builder.Configuration["JWT:SigningSecret"]);
        
        options.TokenValidationParameters = new TokenValidationParameters
        {
           ValidateIssuer = false,
           ValidateAudience = false,
           IssuerSigningKey = new SymmetricSecurityKey(signingKey)
        };
    });

// Add services to the container (Dependency Injection Container = DI container).
builder.Services.AddDbContext<ApplicationDbContext>
    (options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("Default")));

builder.Services.AddCors();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.MapControllers();

app.Run();

// Skapa kontroller
// Laggt till fakedata
// Dtoer
// Installerade EF core paket
// Connection string
// fixa Program.cs
// Migrera

// Dotnet ef migrations add
// 