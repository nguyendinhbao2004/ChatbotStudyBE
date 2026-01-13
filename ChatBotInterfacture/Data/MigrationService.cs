using ChatBotApplication.Common.Interfaces;
using ChatBotInterfacture.Config;
using Domain.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ChatBotInterfacture.Data
{
    public class MigrationService : IMigrationService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManage;
        private readonly RoleManager<Role> _roleManage;

        public MigrationService(
            ApplicationDbContext context,
            UserManager<User> userManage,
            RoleManager<Role> roleManage)
        {
            _context = context;
            _userManage = userManage;
            _roleManage = roleManage;
        }
        public async Task MigrateAsync()
        {
            // 1. Tự động chạy Migration (tương đương lệnh Update-Database)
            // Nếu DB chưa có -> Tự tạo. Nếu DB cũ -> Tự update cột mới.
            try
            {
                if (_context.Database.IsRelational())
                {
                    await _context.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                // Log lỗi nếu kết nối DB thất bại
                Console.WriteLine($"Lỗi Migration: {ex.Message}");
                throw;
            }
            //Seed Data (Gieo dữ liệu mẫu)
            await SeedRoleAsync();
            await SeedSubjectAsync();
        }

        private async Task SeedRoleAsync()
        {
            var roles = new[] {"Admin", "Teacher", "Student"};

            foreach(var roleName in roles)
            {
                if(!await _roleManage.RoleExistsAsync(roleName))
                {
                    await _roleManage.CreateAsync(new Role(roleName));
                }
            }
        }

        private async Task SeedSubjectAsync()
        {
            // Nếu bảng Subject chưa có gì thì mới thêm
            if (!await _context.Subjects.AnyAsync())
            {
                var subjects = new List<Subject>
                {
                    new Subject("Lập trình C cơ bản", "PRF192", "Ngành Cntt"),
                    new Subject("Giới thiệu vềMaketing", "MKT101", "Ngành KT và NN"),
                    new Subject("Nhật LV1", "JPD101", "Ngành KT và NN")
                };

                await _context.Subjects.AddRangeAsync(subjects);
                await _context.SaveChangesAsync();
            }
        }
    }
}