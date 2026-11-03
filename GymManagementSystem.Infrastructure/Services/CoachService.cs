using GymManagementSystem.Core.Models.Responses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymManagementSystem.Infrastructure.Services
{
    public class CoachService
    {
        private readonly AppDbContext _db;
        public CoachService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<CoachResponse>> GetActiveAsync()
        {
            return await _db.Coaches
                .AsNoTracking()
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .Select(c => new CoachResponse(c.Id, c.Name, c.gender))
                .ToListAsync();
        }

        public async Task<List<CoachStatsResponse>> GetCoachStatsAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            return await _db.Coaches
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .Select(c => new CoachStatsResponse(
                    c.Id,
                    c.Name,
                    c.gender, 
                    _db.Subscriptions.Count(s => s.CoachId == c.Id && s.EndDate >= today)
                ))
                .ToListAsync();
        }
    }
}