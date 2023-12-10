using AlgorizaProject.DAL.DbContext;
using AlgorizaProject.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AlgorizaProject.DAL.Data
{
    public class SeedData
    {

        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager , VezeetaDbContext vezeetaDbContext)
        {
            if (!roleManager.Roles.Any())
            {
                var brandData = File.ReadAllText("../AlgorizaProject.DAL/Data/roles.json");
                var roles = JsonSerializer.Deserialize<List<IdentityRole>>(brandData);
                foreach (var item in roles)
                {
                    await roleManager.CreateAsync(item);
                }
                await vezeetaDbContext.SaveChangesAsync();
            }
        }
    }
}
