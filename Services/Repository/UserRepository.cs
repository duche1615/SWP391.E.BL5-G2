﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quizpractice.Models;
using Quizpractice.Services.IRepository;
using Quizpractice.ViewModels;

namespace Quizpractice.Services.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly SWP391_DBContext _dbContext;
        public UserRepository(SWP391_DBContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<User> FindById(int id)
        {
            return await _dbContext.Users.Include(u => u.Role).FirstAsync(u => u.UserId == id);
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _dbContext.Users.Include(u => u.Role).ToListAsync();
        }

        public bool UpdateUser(User user)
        {
            if (user != null)
            {
                _dbContext.Update(user);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }
        public async Task<bool> CheckEmailExistAsync(string email)
        {
            return await _dbContext.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<User> RegisterUserAsync(RegisterViewModel registerModel)
        {
            var newUser = new User
            {
                Email = registerModel.Email,
                Password = registerModel.Password,
                Fullname = registerModel.Fullname,
                Phone = registerModel.Phone,
                Gender = registerModel.Gender,
                Address = registerModel.Address,
                CreatedDate = DateTime.Now,
                ModifyDate = DateTime.Now,
                Status = true, 
                RoleId = 2 
            };

            _dbContext.Users.Add(newUser);
            await _dbContext.SaveChangesAsync();

            return newUser;
        }


        public async Task<User> LoginAsync(LoginViewModel loginModel)
        {
            
            var user = await _dbContext.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == loginModel.Email);

            if (user != null && user.Password == loginModel.Password)
            {
                return user; 
            }

            return null; 
        }

        public async Task<bool> ValidatePassword(User user, string password)
        {
            return user.Password == password;
        }

        
        public async Task<bool> UpdatePasswordAsync(User user, string newPassword)
        {
            user.Password = newPassword;
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
