using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OppOdev1
{
    public class NoteAction : INoteAction
    {
        private List<Notes> notesList;
        private List<Users> userList;
        private string noteFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "notes.txt");

        public NoteAction()
        {
            notesList = new List<Notes>();
            userList = new List<Users>();
        }

        public void AddNote(Users users ,string userNote,string userdate)
        {
            string noteLine = $"Notunuz:{userNote},Tarih:{userdate},UserId:{users.Id}\n";
            File.AppendAllText(noteFilePath, $"Id:{users.Id},Notunuz:{userNote},Tarih:{userdate}\n");
            Console.WriteLine("Notunuz başarıyla eklendi.");
        }

        public void GetNoteList(Users user)
        {
            //string notesDosyaYolu = "C:/Users/ibrah/Desktop/notes.txt";
            if (File.Exists(noteFilePath))
            {
                string[] notlar = File.ReadAllLines(noteFilePath);

                if (notlar.Length > 0)
                {
                    Console.WriteLine($"Kullanıcının Notları (UserId = {user.Id}):");

                    foreach (var not in notlar)
                    {
                        // Not satırını parçalayarak UserId'sini al
                        var userIdIndex = not.IndexOf("Id:");
                        if (userIdIndex != -1)
                        {
                            var userIdStr = not.Substring(userIdIndex + 3).Split(',')[0].Trim();
                            if (int.TryParse(userIdStr, out int noteUserId) && noteUserId == user.Id)
                            {
                                Console.WriteLine(not);
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Kullanıcının henüz not girişi bulunmamaktadır.");
                }
            }
            else
            {
                Console.WriteLine("Not dosyası bulunamadı.");
            }
        }


    }
}
