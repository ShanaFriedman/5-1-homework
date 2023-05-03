using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace May1Homework.Data
{
    public class UserRepository
    {
        private string _connectionString;
        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void SignUp(User u, string password)
        {
            using var context = new QuestionAnswerDbContext(_connectionString);
            u.HashPassword = BCrypt.Net.BCrypt.HashPassword(password);
            context.Users.Add(u);
            context.SaveChanges();
        }
        public User Login(string email, string password)
        {
            User user = GetByEmail(email);
            if(user == null)
            {
                return null;
            }
            if(!BCrypt.Net.BCrypt.Verify(password, user.HashPassword))
            {
                return null;
            }
            return user;
        }
        public User GetByEmail(string email)
        {
            using var context = new QuestionAnswerDbContext(_connectionString);
            return context.Users.FirstOrDefault(u => u.Email == email);
        }
    }
}
