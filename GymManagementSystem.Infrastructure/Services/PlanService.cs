using GymManagementSystem.Core.Entities;
using GymManagementSystem.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.Infrastructure.Services
{
    public class PlanService
    {

        private readonly AppDbContext _db;

        public PlanService(AppDbContext db)
        {
            _db = db;

        }

        public async Task<PlanResponse?> CreateAsync(string name, int durationInDays, decimal price, bool WithCoach)
        {
            name = name.Trim();
            bool PlanExists = await _db.Plans.AnyAsync(p => p.Name.ToLower() == name.ToLower());
            if (PlanExists)
                return null;

            var plan = new Plan
            {
                Name = name,
                DurationInDays = durationInDays,
                Price = price,
                WithCoach = WithCoach
            };
            _db.Plans.Add(plan);
            await _db.SaveChangesAsync();

            return new PlanResponse(plan.Id , plan.Name, plan.DurationInDays, plan.Price, plan.WithCoach);


        }

        public async Task<List<PlanResponse>> GetAllAsync()
        {


            return await _db.Plans
                .AsNoTracking()
                .OrderBy(p => p.Price)
                .Select(p => new PlanResponse(p.Id, p.Name, p.DurationInDays, p.Price, p.WithCoach))
                .ToListAsync();
        }


    }

}
