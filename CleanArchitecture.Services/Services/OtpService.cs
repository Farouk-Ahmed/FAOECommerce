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

        public async Task SetOtpAsync(string key, string otp, string userName, TimeSpan expiration)
        {
            var expiresAt = DateTime.UtcNow.Add(expiration);
            var entity = await _dbContext.Otps.FirstOrDefaultAsync(x => x.Key == key);
            if (entity == null)
            {
                entity = new Otp { Key = key, Code = otp, ExpiresAt = expiresAt, UserName = userName };
                _dbContext.Otps.Add(entity);
            }
            else
            {
                entity.Code = otp;
                entity.ExpiresAt = expiresAt;
                entity.UserName = userName;
                _dbContext.Otps.Update(entity);
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task<string> GetOtpAsync(string key)
        {
            var entity = await _dbContext.Otps.FirstOrDefaultAsync(x => x.Key == key && x.ExpiresAt > DateTime.UtcNow);
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
    }
}
