using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityBot.Models
{
    public class Session
    {
        public string Task { get; set; }
        public bool NumbersSelected { get; set; }
        // Другие свойства, если необходимо

        public Session()
        {
            // Инициализация свойств по умолчанию
            Task = "";
            NumbersSelected = false;
        }
    }
}

