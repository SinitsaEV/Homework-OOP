using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int id = 1;
            string name = "Sinitsa";
            string description = "Student";

            Player player = new Player(id, name, description);
            player.ShowInformation();
        }
    }

    class Player
    {
        private int id;
        private string name;
        private string description;

        public Player(int id, string name, string description)
        {
            this.id = id;
            this.name = name;
            this.description = description;
        }

        public void ShowInformation()
        {
            Console.WriteLine( " ID: " + id);
            Console.WriteLine(" NAME: " + name);
            Console.WriteLine(" DESCRIPTION: " + description);
        }
    }
}