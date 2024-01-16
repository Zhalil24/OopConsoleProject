
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OppOdev1
{
    public interface INoteAction
    {
        void AddNote(Users users, string userNote, string userdate);

        void GetNoteList(Users users);
    }
}

