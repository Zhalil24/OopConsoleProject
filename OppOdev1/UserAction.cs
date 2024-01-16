using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OppOdev1
{
    public class UserAction : IUserAction
    {
        private List<Users> users;
        public string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "users.txt");

        public UserAction(List<Users> userList)
        {
            users = userList;
        }
        private int GenerateNextId()
        {
            int maxId = users.Count > 0 ? users.Max(u => u.Id) : 0;
            return maxId + 1;
        }
        public void AddUser(Users user)
        {
            user.Id = GenerateNextId();
            user.Id = users.Count + 1;
            users.Add(user);
        }

        public void DeleteUser(string phoneNumber)
        {
            Users userToDelete = users.FirstOrDefault(u => u.Phone == phoneNumber);

            if (userToDelete != null)
            {
                users.Remove(userToDelete);
                Console.WriteLine($"Kullanıcı silindi Ad: {userToDelete.Name}, Soyad: {userToDelete.Surname}, Telefon: {userToDelete.Phone}");
                UpdateUsersFile();
            }
            else
            {
                Console.WriteLine($"Telefon numarası {phoneNumber} ile kayıtlı kullanıcı bulunamadı....");
            }
        }

        public void UpdateUsersFile()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    foreach (Users user in users)
                    {
                        sw.WriteLine( $"Id:{user.Id},Ad:{user.Name},Soyad:{user.Surname},Telefon:{user.Phone},Email:{user.Email},Admin mi :{user.IsAdmin}, Sifre:{user.Password}");
                    }
                }
                Console.WriteLine("Kullanıcılar dosyaya güncellendi.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Dosyaya yazma hatası: {ex.Message}");
            }
        }

        public void GetUserByFilter(string filter)
        {
            if (filter.Length >= 3)
            {

                if (File.Exists(filePath))
                {
                    string[] satirlar = File.ReadAllLines(filePath);
                    bool kullaniciBulundu = false;

                    foreach (var satir in satirlar)
                    {
                        if (satir.Contains($"Ad:{filter}") || satir.Contains($"Soyad:{filter}")
                            || satir.Contains($"Email:{filter}") || satir.Contains($"Telefon:{filter}"))
                        {
                            Console.WriteLine(satir);
                            kullaniciBulundu = true;
                        }
                    }

                    if (!kullaniciBulundu)
                    {
                        Console.WriteLine("Belirtilen kriterde kullanıcı bulunamadı");
                    }
                }
                else
                {
                    Console.WriteLine("Kullanıcı kayıtları bulunamadı");
                }
            }
            else
            {
                Console.WriteLine("Girdi uzunluğu en az 3 karakter olmalıdır");
            }
        }


        public void GetUserList(List<Users> users)
        {
            throw new NotImplementedException();
        }

        public Users AuthenticateUser(string email, string password)
        {
           
            Users authenticatedUser = users.FirstOrDefault(u => u.Email == email);

            if (authenticatedUser != null && AuthenticatePassword(authenticatedUser, password))
            {
                Console.WriteLine("Kimlik doğrulama başarılı!");
                return authenticatedUser;
            }
            else
            {
                Console.WriteLine("Kimlik doğrulama başarısız. Geçersiz e-posta veya şifre.");
                return null;
            }
        }

        public bool AuthenticatePassword(Users user, string password)
        {
            return user.Password == password;
        }

    }
}
