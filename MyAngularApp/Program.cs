using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyAngularApp.Data;
using MyAngularApp.Helpers;
using MyAngularApp.Interfaces;
using MyAngularApp.Middleware;
using MyAngularApp.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ITokenService,TokenService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    
});



    builder.Services.AddCors((setup) => {
        setup.AddPolicy("Defaults", options => {
            options.AllowAnyMethod().AllowAnyHeader().WithOrigins("https://localhost:4200");
        });
    });

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => 
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetConnectionString("TokenKey"))),
            ValidateIssuer = false,
            ValidAudience = null,

        };
    });

var app = builder.Build();


//for migrating automatically
//var app=builder.Build();

try
{
    using (var scope = app.Services.CreateScope())
    {
        var applicationDBContext = scope.ServiceProvider.GetRequiredService<DataContext>();
        await applicationDBContext.Database.MigrateAsync();
        await Seed.SeedUsers(applicationDBContext);
    }
}
catch (Exception ex)
{

    var logger = app.Services.GetService<ILogger<Program>>();
    logger.LogError(ex, "An exception occurred during migration");
}


app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("Defaults");


//app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
