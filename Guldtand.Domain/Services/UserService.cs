using Guldtand.Data.Entities;
using Guldtand.Domain.Helpers;
using System;
using System.Text;
using Guldtand.Data;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Guldtand.Domain.Models;

namespace Guldtand.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public (UserDTO user, string role) Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                throw new ArgumentException("Username and password required.");

            var user = _context.Users.Include("Role").SingleOrDefault(x => x.Username == username);

            if (user == null)
                throw new ArgumentException("Username or password incorrect.");

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                throw new ArgumentException("Username or password incorrect.");

            var userDto = _mapper.Map<UserDTO>(user);
            return (userDto, user.Role.RoleName);
        }

        public UserDTO Create(UserDTO userDto, string password)
        {
            var user = _mapper.Map<User>(userDto);

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password is required.");

            if (_context.Users.Any(x => x.Username == user.Username))
                throw new AppException($"Username \"{user.Username}\" is already taken");

            if (_context.Users.Any(x => x.Email == user.Email))
                throw new AppException($"Email \"{user.Email}\" is already taken");

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.Users.Add(user);
            _context.SaveChanges();

            return _mapper.Map<UserDTO>(user);
        }

        public IEnumerable<UserDTO> GetAll()
        {
            var users = _context.Users;
            var userDtos = _mapper.Map<IEnumerable<UserDTO>>(users);
            return userDtos;
        }

        public UserDTO GetById(int id)
        {
            var user = _context.Users.Find(id);
            return _mapper.Map<UserDTO>(user);
        }

        public void Update(UserDTO userDto, string password = null)
        {
            
            var user = _context.Users.Find(userDto.Id);

            if (user == null)
                throw new AppException("User not found");

            if (userDto.Username != user.Username)
            {
                if (_context.Users.Any(x => x.Username == userDto.Username))
                    throw new AppException($"Username \"{userDto.Username}\" is already taken");
            }

            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.Username = userDto.Username;

            if (!string.IsNullOrWhiteSpace(password))
            {
                CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null)
                throw new ArgumentNullException($"Parameter {nameof(password)} cannot be null");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException($"Parameter {nameof(password)} cannot be empty or only contain whitespace.");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException($"Parameter {nameof(password)} cannot be null");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException($"Parameter {nameof(password)} cannot be empty or only contain whitespace.");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}
