using System;
using System.Threading.Tasks;
using GymManagementSystem.Api.Models;
using GymManagementSystem.Core.Entities;
using GymManagementSystem.Core.Enums;
using GymManagementSystem.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GymManagementSystem.Tests.Services
{
    public class MemberServiceTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task SearchAsync_WhenSearchingByName_ReturnsFilteredMembers()
        {
            var db = GetInMemoryDbContext();
            db.Members.Add(new Member { Id = 1, FullName = "Zayn Ramadan", Phone = "0982574825", Gender = Gender.Male, IsActive = true });
            db.Members.Add(new Member { Id = 2, FullName = "Ahmad Ibrahim", Phone = "0999999999", Gender = Gender.Male, IsActive = true });
            await db.SaveChangesAsync();

            var service = new MemberService(db);

            
            var result = await service.SearchAsync("Zayn", null);

            Assert.Single(result);
            Assert.Equal("Zayn Ramadan", result[0].FullName);
        }

        [Fact]
        public async Task SearchAsync_WhenSearchTermIsTooShort_ReturnsEmptyList()
        {
            var db = GetInMemoryDbContext();
            var service = new MemberService(db);

            var result = await service.SearchAsync("Z", null);

            Assert.Empty(result);
        }
    }
}