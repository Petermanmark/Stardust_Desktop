using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Note
    {
        public string noteId;
        public string title;
        public string content;

        public Note(string noteId, string title, string content)
        {
            this.noteId = noteId;
            this.title = title;
            this.content = content;
        }
    }
}
