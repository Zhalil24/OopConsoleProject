
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OppOdev1
{
    public interface IUserAction
    {
        void AddUser(Users user);

        public void UpdateUsersFile();

        void GetUserList(List<Users> users);

        void DeleteUser(string phoneNumber);

        void GetUserByFilter(string filter);

        Users AuthenticateUser(string email, string password);

        bool AuthenticatePassword(Users user, string password);
    }
}
