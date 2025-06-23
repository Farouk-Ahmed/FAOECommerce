using CleanArchitecture.Services.Interfaces;
using CleanArchitecture.DataAccess.Contexts;
using CleanArchitecture.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace CleanArchitecture.Services.Services
{
    public class OtpService : IOtpService
    {
        private readonly ApplicationDbContext _dbContext;

        public OtpService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SetOtpAsync(string key, string otp, string userName, string userId, TimeSpan expiration)
        {
            var expiresAt = DateTime.UtcNow.Add(expiration);
            var entity = await _dbContext.Otps.FirstOrDefaultAsync(x => x.Key == key);
            if (entity == null)
            {
                // إنشاء OTP جديد وتعيين IsUsed و UserId
                entity = new Otp { Key = key, Code = otp, ExpiresAt = expiresAt, UserName = userName, UserId = userId, IsUsed = false };
                _dbContext.Otps.Add(entity);
            }
            else
            {
                // تحديث OTP موجود وإعادة تعيين IsUsed إلى false (لأنك تُنشئ OTP جديدًا لنفس المفتاح)
                entity.Code = otp;
                entity.ExpiresAt = expiresAt;
                entity.UserName = userName;
                entity.UserId = userId; // تحديث UserId
                entity.IsUsed = false;
                _dbContext.Otps.Update(entity);
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task<string> GetOtpAsync(string key)
        {
            // استرجاع OTP فقط إذا كان صالحًا (لم تنته صلاحيته) ولم يتم استخدامه بعد
            var entity = await _dbContext.Otps.FirstOrDefaultAsync(x => x.Key == key && x.ExpiresAt > DateTime.UtcNow && !x.IsUsed);
            return entity?.Code;
        }


        public async Task RemoveOtpAsync(string key)
        {
            var entity = await _dbContext.Otps.FirstOrDefaultAsync(x => x.Key == key);
            if (entity != null)
            {
                _dbContext.Otps.Remove(entity);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task SetOtpAsUsedAsync(string key)
        {
            var entity = await _dbContext.Otps.FirstOrDefaultAsync(x => x.Key == key);
            if (entity != null)
            {
                entity.IsUsed = true;
                _dbContext.Otps.Update(entity);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
