using System;

namespace ProyectoFinal
{
    public class InOutPut
    {
        public string User {get; set;}
        public DateTime Date {get; set;}
        public int Change {get; set;}

        public InOutPut()
        {
            //Constructor vacio
        }

        public InOutPut (string user, DateTime date, int change)
        {
            this.User = user;
            this.Date = date;
            this.Change = change;
        }
    }
}