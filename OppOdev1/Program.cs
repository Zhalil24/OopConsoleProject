using OppOdev1;
using System;
namespace ConsoleApp
{
    class Program
    {
        static void AddUser(UserAction userAction)
        {

            Console.Write("İsim: ");
            string ad = ReadNotNullInput();

            Console.Write("Soyisim: ");
            string soyad = ReadNotNullInput();

            Console.Write("Telefon: ");
            string telefon = ReadNotNullInput();




            while (!IsValidTelefon(telefon,userAction))
            {
                Console.WriteLine("Hata: Başında sıfır ile tuşladınız veya daha önce kullanılmış bir telefon numarası girdiniz.");
                Console.Write("Telefon Numarasını Tekrar Giriniz: ");
                telefon = ReadNotNullInput();
            }

            Console.WriteLine("Telefon numarası geçerli.");

            Console.Write("Email: ");
            string email = ReadNotNullInput();

            Console.Write("Password: ");
            string password = ReadNotNullInput();

            Console.Write("Admin mi: ");
            string isAdmin = ReadNotNullInput();

            Users newUser = new Users
            {
                Name = ad,
                Surname = soyad,
                Phone = telefon,
                Email = email,
                Password = password,
                IsAdmin = bool.Parse(isAdmin)
            };

            userAction.AddUser(newUser);

            File.AppendAllText(userAction.filePath, $"Id:{newUser.Id},Ad:{ad},Soyad:{soyad},Telefon:{telefon},Email:{email},Admin mi :{isAdmin}, Sifre:{password}\n");
            
            Console.WriteLine("Yeni kullanıcı başarıyla eklendi.");
        }

        static bool IsValidTelefon(string telefon, UserAction userAction)
        {
            return !telefon.StartsWith("0") && !File.ReadAllText(userAction.filePath).Contains($"Telefon:{telefon}");
        }



        static string ReadNotNullInput()
        {
              string input = Console.ReadLine();
            while (string.IsNullOrEmpty(input))
                 {
                       Console.WriteLine("Hata: Boş bir giriş yapamazsınız. Tekrar deneyin.");
                       input = Console.ReadLine();
                  }
        return input;
        }

       
        static void UserByFilter(UserAction userAction)
        {
            Console.Write("Aranacak kelime: ");
            string aramaKelimesi = Console.ReadLine();
            userAction.GetUserByFilter(aramaKelimesi);
        }

        static void DeleteUser(UserAction userAction)
        {
            Console.Write("Silinecek kullanıcının telefon numarasını girin: ");
            string silinecekKullanici = Console.ReadLine();
            userAction.DeleteUser(silinecekKullanici);
        }

        static void AddNote(NoteAction noteAction, Users users)
        {
            Console.Write("Notunuzu giriniz: ");
            string userNote = Console.ReadLine();
            string userdate = DateTime.UtcNow.ToString("dd.MM.yyyyTHH:mm:ssZ");
            noteAction.AddNote(users,userNote, userdate);
        }


        static List<Users> DosyadanKullanicilariYukle(string dosyaYolu)
        {
            List<Users> kullaniciListesi = new List<Users>();

            try
            {
                string[] satirlar = File.ReadAllLines(dosyaYolu);

                foreach (string satir in satirlar)
                {
                    string[] parcalar = satir.Split(',');
                    Users kullanici = new Users
                    {
                        Id = int.Parse(parcalar[0].Split(':')[1]),
                        Name = parcalar[1].Split(':')[1],
                        Surname = parcalar[2].Split(':')[1],
                        Phone = parcalar[3].Split(':')[1],
                        Email = parcalar[4].Split(':')[1],
                        IsAdmin = bool.Parse(parcalar[5].Split(':')[1]),
                        Password = parcalar[6].Split(':')[1]
                    };
                    kullaniciListesi.Add(kullanici);

                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Dosya bulunamadı: {dosyaYolu}");
            }

            return kullaniciListesi;
        }




        static void Main(string[] args)
        {
            NoteAction noteAction = new NoteAction();
            string dosyaYolu = "C:/Users/ibrah/Desktop/users.txt";
            List<Users> kullaniciListesi = DosyadanKullanicilariYukle(dosyaYolu);
            UserAction userAction = new UserAction(kullaniciListesi);
           
           



            Console.Write("Mail giriniz: ");
            string userEmail = Console.ReadLine();

            Console.Write("Parola giriniz: ");
            string userPassword = Console.ReadLine();

            Users authenticatedUser = userAction.AuthenticateUser(userEmail, userPassword);

            if (authenticatedUser.IsAdmin)
            {
                bool exitProgram = false;

                while (!exitProgram)
                {
                    Console.WriteLine("Menü:");
                    Console.WriteLine("1. Kullanıcı Ekle");
                    Console.WriteLine("2. Kullanıcı Ara");
                    Console.WriteLine("3. Kullanıcı Sil");
                    Console.WriteLine("4. Programı Kapat");

                    Console.Write("Seçiminizi yapınız: ");
                    int choice = int.Parse(Console.ReadLine());

                    switch (choice)
                    {
                        case 1:
                            AddUser(userAction);
                            break;
                        case 2:
                            UserByFilter(userAction);
                            break;
                        case 3:
                            DeleteUser(userAction);
                            break;
                        case 4:
                            exitProgram = true;
                            break;
                        default:
                            Console.WriteLine("Geçersiz seçim");
                            break;
                    }
                }

                Console.WriteLine("Program Kapatıldı");
            }
            else
            {
                bool exitNoteProgram = false;

                while(!exitNoteProgram)
                {
                    Console.WriteLine("Kullanıcı Paneline Hoşgeldiniz");
                    Console.WriteLine("Menü:");
                    Console.WriteLine("1. Not Ekle");
                    Console.WriteLine("2. Notlarımı Listele");
                    Console.WriteLine("3. Note Programını Kapat");
                    
                    Console.Write("Seçiminizi yapınız: ");
                    int userChoice = int.Parse(Console.ReadLine());

                    switch (userChoice)
                    {
                        case 1:

                            AddNote(noteAction, authenticatedUser);

                            break;
                        case 2:
                            noteAction.GetNoteList(authenticatedUser);
                            break;
                        case 3:
                            exitNoteProgram = true;
                             break;
                        default:
                            Console.WriteLine("Geçersiz seçim.");
                            break;

                    }

                }
               
            }


        }
    }
}

