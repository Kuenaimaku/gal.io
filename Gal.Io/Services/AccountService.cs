using Gal.Io.Interfaces;
using Gal.Io.Interfaces.DTOs;
using Gal.Io.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Gal.Io.Services
{
    public class AccountService : IAccountService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<AccountService> _logger;

        public AccountService(IConfiguration config, ILogger<AccountService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public UserDTO Authenticate(LoginDTO login)
        {
            UserDTO response = null;
            using(var db = new DataContext())
            {
                var user = db.Users.Where(x => x.Username == login.Username).FirstOrDefault();
                if (user != null)
                    if (!Hash.Validate(login.Password, user.Salt, user.Hash))
                        response = null;
                    else
                        response = new UserDTO() { Username = user.Username, Email = user.Email, UserId = user.UserId };
            }
            return response;
        }

        public UserDTO Register(RegisterDTO register)
        {
            UserDTO response = null;
            using (var db = new DataContext())
            {
                var user = db.Users.Where(x => x.Username == register.Username).FirstOrDefault();
                if (user == null)
                {
                    User u = new User();
                    string salt = Salt.Create();
                    string hash = Hash.Create(register.Password, salt);
                    if (Hash.Validate(register.Password, salt, hash))
                    {
                        u.Username = register.Username;
                        u.Email = register.Email;
                        u.Salt = salt;
                        u.Hash = hash;
                        db.Users.Add(u);
                        int count = db.SaveChanges();
                        _logger.LogInformation($"{count} records saved to database");
                        if (count == 1)
                            response = new UserDTO() { Username = u.Username, Email = u.Email, UserId = u.UserId };
                    }
                }
            }
            return response;
        }


        public class Salt
        {
            public static string Create()
            {
                byte[] randomBytes = new byte[128 / 8];
                using (var generator = RandomNumberGenerator.Create())
                {
                    generator.GetBytes(randomBytes);
                    return Convert.ToBase64String(randomBytes);
                }
            }
        }
        public class Hash
        {
            public static string Create(string value, string salt)
            {
                var valueBytes = KeyDerivation.Pbkdf2(
                                    password: value,
                                    salt: Encoding.UTF8.GetBytes(salt),
                                    prf: KeyDerivationPrf.HMACSHA512,
                                    iterationCount: 10000,
                                    numBytesRequested: 256 / 8);

                return Convert.ToBase64String(valueBytes);
            }

            public static bool Validate(string value, string salt, string hash)
                => Create(value, salt) == hash;
        }
    }
}
