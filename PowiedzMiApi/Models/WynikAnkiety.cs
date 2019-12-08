namespace PowiedzMiApi.Models
{
    using System;
    using System.Collections.Generic;

    public partial class WynikAnkiety
    {
        public int id_uzytkownika { get; set; }
        public int id_ankiety { get; set; }
        public List<int> odpowiedzi { get; set; }
        public String komentarz { get; set; }
        public int ocena { get; set; }
        public WynikAnkiety()
        {
            this.odpowiedzi = new List<int>();
           }
    }

}