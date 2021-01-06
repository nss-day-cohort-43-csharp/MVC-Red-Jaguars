using TabloidMVC.Models;
using System.Collections.Generic;

namespace TabloidMVC.Repositories
{
    public interface IUserProfileRepository
    {
        UserProfile GetByEmail(string email);

        List<UserProfile> GetAllUsers();
        UserProfile GetUserById(int id);
        void DeactivateUser(int id);
        List<UserProfile> GetAllDeactiveUsers();
        void ActivateUser(int id);
        public void Register(UserProfile userProfile);
        void ChangeUserType(UserProfile user);
        int AdminCount();
    }
}