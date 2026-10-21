using Gym_Managemnt_System_Core.Entities;
using Gym_Managemnt_System_Core.Enums;
using Gym_Managemnt_System_Core.Helpers;
using GymManagementSystem.Core.Models;
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


}