using JacobCorp.Data;
using JacobCorp.Models;
using Microsoft.EntityFrameworkCore;
using ZstdSharp.Unsafe;

namespace JacobCorp.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUserById(int id);
        Task<User> Authenticate(string email, string password);
        Task<User> Register(RegisterModel registerModel);
    }

    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _db;
        private readonly JwtTokenService _jwtTokenService;

        public UserService(ApplicationDbContext db, JwtTokenService jwtTokenService)
        {
            _db = db;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return _db.Users;
        }

        public async Task<User> GetUserById(int id)
        {
            return _db.Users.FirstOrDefault(x => x.Id == id);
        }

        public async Task<User> Register(RegisterModel model)
        {
            var user = new User
            {
                Email = model.Email,
                Password = model.Password
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return user;
        }

        public async Task<User> Authenticate(string email, string password)
        {
          
            var user = await _db.Users
                .Where(u => u.Email == email && u.Password == password)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return null;
            }

            return user;
        }
    }
}
