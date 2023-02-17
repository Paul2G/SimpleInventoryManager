using System.Collections.Generic;

namespace ProyectoFinal
{
    public class Item
    {
        public string Name {get; set;}
        public int Stock {get; set;}
        public List<InOutPut> History {get; set;}

        public Item()
        {
            //Constructor vacio
        }

        public Item (string name, int stock)
        {
            this.Name = name;
            this.Stock = stock;
            History = new List<InOutPut>();
        }

        public void addStock (int num)
        {
            this.Stock += num;
        }
    }
}