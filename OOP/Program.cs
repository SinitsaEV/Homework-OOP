using System;

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
        private int _id;
        private string _name;
        private string _description;

        public Player(int id, string name, string description)
        {
            _id = id;
            _name = name;
            _description = description;
        }

        public void ShowInformation()
        {
            Console.WriteLine( " ID: " + _id);
            Console.WriteLine(" NAME: " + _name);
            Console.WriteLine(" DESCRIPTION: " + _description);
        }
    }
}
