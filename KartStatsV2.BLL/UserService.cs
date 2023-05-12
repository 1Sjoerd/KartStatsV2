using KartStatsV2.DAL;
using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KartStatsV2.Models;
using System.Web;

namespace KartStatsV2.BLL
{
    public class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService(string connectionString)
        {
            _userRepository = new UserRepository(connectionString);
        }

        public bool Authenticate(string username, string password)
        {
            var user = _userRepository.GetUserByUsername(username);

            if (user == null)
            {
                return false;
            }

            // Vergelijk het gehashte wachtwoord met het opgeslagen wachtwoord-hash
            return user.PasswordHash == HashPassword(password);
        }

        public int GetIdByUsername(string username)
        {
            var id = _userRepository.GetId(username);
            return id;
        }

        public string GetUsername()
        {
            var username = _userRepository.GetUsername();
            return username;
        }

        public void RegisterUser(User user, string password)
        {
            _userRepository.CreateUser(user, password);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLowerInvariant();
            }
        }

        public bool IsUserAdmin(Group group)
        {
            // Verkrijg het UserId van de huidige gebruiker
            int currentUserId = GetIdByUsername(GetUsername());

            // Controleer of de huidige gebruiker de beheerder van de groep is
            if (group.AdminUserId == currentUserId)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
