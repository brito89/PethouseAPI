using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PethouseAPI.Entities;

namespace PethouseAPI.Data;

public static class SeedData
{
    public static async Task Seed(DbContext context, CancellationToken cancellationToken,ConfigurationManager config)
    {
                var userToAdd = new User();
                var hashPassword = new PasswordHasher<User>().HashPassword(userToAdd,config["SuperUser:Password"]!);
                userToAdd.Username = config["SuperUser:User"]!.ToUpper();
                userToAdd.Name = config["SuperUser:Name"]!;
                userToAdd.Role = config["SuperUser:Role"]!.ToUpper();
                userToAdd.PasswordHash = hashPassword;
                //add SuperUser
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
                        DateOfBirth = new DateOnly(2022, 12, 10),
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
                        DateOfBirth = new DateOnly(2020, 8, 10),
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
                        DateOfBirth = new DateOnly(2018, 3, 10),
                        BreedName = "Labrador",
                        IsMedicated = false,
                        Notes = "None",
                        BreedSizeId = 3,
                        UserId = admin.Entity.Id
                    });

                context.Set<Appointment>().AddRange(new Appointment
                {
                    Id = 1,
                    AppointmentType = AppointmentType.Hospedaje,
                    StartDate = new DateOnly(2022, 10, 10),
                    EndDate = new DateOnly(2022, 12, 10),
                    IsTosAppointmentDocumentSigned = true,
                    MedicalChecked = true,
                    CarnetCheked = true
            
                },
                new Appointment
                {
                    Id = 2,
                    AppointmentType = AppointmentType.Guarderia,
                    StartDate = new DateOnly(2022, 8, 10),
                    EndDate = new DateOnly(2022, 5, 8),
                    IsTosAppointmentDocumentSigned = true,
                    MedicalChecked = true,
                    CarnetCheked = true
            
                });
                
                await context.SaveChangesAsync(cancellationToken);
    }
}