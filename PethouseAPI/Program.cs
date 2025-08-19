using PethouseAPI.Data;
using PethouseAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;
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
            var userAdmin = await context.Set<User>().FirstOrDefaultAsync(b => 
                b.Role == builder.Configuration["SuperUser:Role"]!.ToUpper(), cancellationToken);
            
            if (userAdmin is null)
            {
                await SeedData.Seed(context, cancellationToken, builder.Configuration);
            }
        })
        .UseSeeding((context, _) =>
        {
            var userAdmin = context.Set<User>().FirstOrDefault(b =>
                b.Role == builder.Configuration["SuperUser:Role"]!.ToUpper());

            if (userAdmin is null)
            {
                // Blocking call to async method
                SeedData.Seed(context, CancellationToken.None, builder.Configuration).GetAwaiter().GetResult();
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
builder.Services.AddScoped<IRepository<BreedSize>, BreedSizeRepository>();
builder.Services.AddScoped<IRepository<Appointment>, AppointmentRepository>();
builder.Services.AddScoped<IRepository<Pet>, PetRepository>();
builder.Services.AddScoped<IRepository<PeakSeason>, PeakSeasonRepository>();
builder.Services.AddScoped<IRepository<PetAppointment>, PetAppointmentRepository>();
builder.Services.AddScoped<IRepository<User>, UserRepository>();

var localCors = "LocalOrigin";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: localCors,
        policy  =>
        {
            policy.WithOrigins("http://localhost:4200","http://localhost:5173").AllowAnyHeader()
                .AllowAnyMethod();;
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

await using (var serviceScope = app.Services.CreateAsyncScope())
await using (var dbContext = serviceScope.ServiceProvider.GetRequiredService<PethouseDbContext>())
{
    await dbContext.Database.EnsureCreatedAsync();
}

app.UseHttpsRedirection();

app.UseCors(localCors);

app.UseAuthorization();

app.MapControllers();

app.Run();
