using PethouseAPI.Data;
using PethouseAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;
using Microsoft.AspNetCore.Identity;
using PethouseAPI.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<PethouseDbContext>(options => 
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
        .UseAsyncSeeding(async (context, _, cancellationToken) =>
        {
            
            var userAdmin = await context.Set<User>().FirstOrDefaultAsync(b => b.Role == "ADMIN", cancellationToken);
            if (userAdmin == null)
            {
                var userToAdd = new User();
                var hashPassword = new PasswordHasher<User>().HashPassword(userToAdd, "Admin..");
                userToAdd.Username = "ADMIN";
                userToAdd.Name = "Juan Brito";
                userToAdd.Role = "ADMIN";
                userToAdd.PasswordHash = hashPassword;
                
                var admin = context.Set<User>().Add(userToAdd);
                context.Set<BreedSize>().AddRange(new BreedSize 
                {
                    Id = 1,
                    Name = "Small",
                    Label = "0-10kg",
                    PricePeakSeason = 100.00m,
                    PriceLowSeason = 120.00m
            
                },
                new BreedSize
                {
                    Id = 2,
                    Name = "Medium",
                    Label = "11-25kg",
                    PricePeakSeason = 150.00m,
                    PriceLowSeason = 170.00m
                },
                new BreedSize
                {
                    Id = 3,
                    Name = "Large",
                    Label = "26-40kg",
                    PricePeakSeason = 200.00m,
                    PriceLowSeason = 220.00m
                },
                new BreedSize
                {
                    Id = 4,
                    Name = "Extra Large",
                    Label = "41-60kg",
                    PricePeakSeason = 250.00m,
                    PriceLowSeason = 270.00m
                });
                context.Set<Pet>().AddRange(new Pet
                    {
                        Id = 1,
                        Name = "Pocha",
                        DateOfBirth = new DateOnly(2022, 10, 10),
                        BreedName = "Chihuahua",
                        IsMedicated = false,
                        Notes = "None",
                        BreedSizeId = 1,
                        UserId = admin.Entity.Id
                    },
                    new Pet
                    {
                        Id = 2,
                        Name = "Luna",
                        DateOfBirth = new DateOnly(2020, 10, 10),
                        BreedName = "Border",
                        IsMedicated = false,
                        Notes = "None",
                        BreedSizeId = 2,
                        UserId = admin.Entity.Id
                    },
                    new Pet
                    {
                        Id = 3,
                        Name = "Coco",
                        DateOfBirth = new DateOnly(2018, 10, 10),
                        BreedName = "Labrador",
                        IsMedicated = false,
                        Notes = "None",
                        BreedSizeId = 3,
                        UserId = admin.Entity.Id
                    });

                context.Set<Appointment>().Add(new Appointment
                {
                    Id = 1,
                    AppointmentType = AppointmentType.Hospedaje,
                    StartDate = new DateOnly(2022, 10, 10),
                    EndDate = new DateOnly(2022, 12, 10),
                    IsTosAppointmentDocumentSigned = true,
                    MedicalChecked = true,
                    CarnetCheked = true
            
                });
                await context.SaveChangesAsync(cancellationToken);
            }
        })
);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["AppSettings:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["AppSettings:Audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"]!)),
            ValidateIssuerSigningKey = true
        };
    });

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRepository<BreedSize>,BreedSizeRepository>();
builder.Services.AddScoped<IRepository<Appointment>,AppointmentRepository>();
builder.Services.AddScoped<IRepository<Pet>,PetRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

await using (var serviceScope = app.Services.CreateAsyncScope())
await using (var dbcontext = serviceScope.ServiceProvider.GetRequiredService<PethouseDbContext>())
{
    await dbcontext.Database.EnsureCreatedAsync();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
