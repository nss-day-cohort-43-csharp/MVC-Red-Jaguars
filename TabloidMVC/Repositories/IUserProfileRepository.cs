using TabloidMVC.Models;
using System.Collections.Generic;

namespace TabloidMVC.Repositories
{
    public interface IUserProfileRepository
    {
        UserProfile GetByEmail(string email);

        public void Register(UserProfile userProfile);
        public List<UserProfile> GetAllUsers();
        public UserProfile GetUserById(int id);
    }
}