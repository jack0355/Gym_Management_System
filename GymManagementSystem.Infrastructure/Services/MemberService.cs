using GymManagementSystem.Core.Entities;
using GymManagementSystem.Core.Enums;
using GymManagementSystem.Core.Helpers;
using GymManagementSystem.Core.Models;
using GymManagementSystem.Core.Models.Responses;
using GymManagementSystem.Core.Rules;
using GymManagementSystem.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.Api.Models;

public class MemberService
{
    private readonly AppDbContext _db;

    public MemberService(AppDbContext db)
    {
        _db = db;
    }


    public async Task<CreateMemberResponse?> CreateAsync(string fullName, string phone, Gender gender)
    {
        fullName = fullName.Trim();
        phone = phone.Trim();

        bool exists = await _db.Members.AnyAsync(m => m.FullName == fullName && m.Phone == phone);
        if (exists)
            return null;

        var member = new Member
        {
            FullName = fullName,
            Phone = phone,
            Gender = gender,
            QrToken = QrTokenGenerator.Create()
        };

        _db.Members.Add(member);
        await _db.SaveChangesAsync();


        return new CreateMemberResponse(member.Id, member.FullName, member.QrToken);
    }


    public async Task<string?> GetQrTokenAsync(int memberId)
    {
        return await _db.Members
            .AsNoTracking()
            .Where(m => m.Id == memberId)
            .Select(m => m.QrToken)
            .FirstOrDefaultAsync();
    }

    public async Task<List<MemberListItem>> SearchAsync(string? q, string? status)
    {
        var term = (q ?? "").Trim().ToLower();
        var wanted = (status ?? "").Trim();
        bool byStatus = wanted == "Green" || wanted == "Yellow" || wanted == "Red";
        if (term.Length < 2 && !byStatus) return new List<MemberListItem>();

        var today = GymClock.Today();

        var query = _db.Members.AsNoTracking().AsQueryable();
        if (term.Length >= 2)
            query = query.Where(m => m.FullName.ToLower().Contains(term) || m.Phone.Contains(term));

        var members = await query.OrderBy(m => m.FullName).Take(byStatus ? 1000 : 10).ToListAsync();
        var memberIds = members.Select(m => m.Id).ToList();

        var activeSubs = await _db.Subscriptions.AsNoTracking()
            .Include(s => s.Plan)
            .Include(s => s.Payments)
            .Where(s => memberIds.Contains(s.MemberId) && s.StartDate <= today && s.EndDate >= today)
            .OrderByDescending(s => s.EndDate)
            .ToListAsync();

        var result = new List<MemberListItem>();
        foreach (var m in members)
        {
            var sub = activeSubs.FirstOrDefault(s => s.MemberId == m.Id);
            var paid = sub?.Payments.Sum(p => p.Amount) ?? 0m;
            var colour = CheckInRules.Decide(m.IsActive, sub, paid, sub?.Plan.DurationInDays ?? 0, today).ToString();
            if (byStatus && colour != wanted) continue;
            result.Add(new MemberListItem(m.Id, m.FullName, m.Phone, m.IsActive, colour));
            if (result.Count == 50) break;
        }
        return result;
    }

    public async Task<MemberSummary?> GetSummaryAsync(int id)
    {

        var member = await _db.Members.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
        if (member is null) return null;

        var today = GymClock.Today();
        var sub = await _db.Subscriptions.AsNoTracking()
            .Include(s => s.Plan).Include(s => s.Payments).Include(s => s.Coach)
        .Where(s => s.MemberId == id && s.StartDate <= today && s.EndDate >= today)
        .OrderByDescending(s => s.EndDate)
        .FirstOrDefaultAsync();


        var paid = sub?.Payments.Sum(p => p.Amount) ?? 0m;
        var owed = sub is null ? 0m : Math.Max(0m, sub.PriceCharged - paid);
        var status = CheckInRules.Decide(member.IsActive, sub, paid, sub?.Plan.DurationInDays ?? 0, today);

        var waiting = await _db.Subscriptions.AnyAsync(s => s.MemberId == id && s.StartDate > today);


        return new MemberSummary(member.Id, member.FullName, member.Phone, member.IsActive, status,
        sub?.Id, sub?.Plan.Name, sub?.Plan.DurationInDays, sub?.Coach?.Name, sub?.WithProgram ?? false,
        sub?.StartDate, sub?.EndDate, sub?.PriceCharged, owed, waiting);
    }
}