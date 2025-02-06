using System;
using System.Collections.Generic;
using System.Text;

namespace OOP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string AddCommand = "1";
            const string RemoveCommand = "2";
            const string СhangeBanCommand = "3";
            const string ShowCommand = "4";
            const string ExitCommand = "5";

            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;

            Database database = new Database();

            bool isActive = true;

            while (isActive)
            {
                Console.WriteLine($"{AddCommand} - добавить игрока\n" +
                    $"{RemoveCommand} - удалить игрока\n" +
                    $"{СhangeBanCommand} - разбанить|забанить игрока\n" +
                    $"{ShowCommand} - показать игроков\n" +
                    $"{ExitCommand} - выйти");
                Console.WriteLine("Введите команду:");

                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case AddCommand:
                        database.AddPlayer();
                        break;

                    case RemoveCommand:
                        Console.Write("Введите ID игрока для удаления: ");

                        if (int.TryParse(Console.ReadLine(), out int removeId))
                            database.RemovePlayer(removeId);
                        else
                            Console.WriteLine("Неверный ввод.");

                        break;
                        
                    case СhangeBanCommand:
                        Console.Write("Введите ID игрока: ");

                        if (int.TryParse(Console.ReadLine(), out int changeBanId))
                            database.RemovePlayer(changeBanId);
                        else
                            Console.WriteLine("Неверный ввод.");

                        break;

                    case ShowCommand:
                        database.ShowInfo();
                        break;

                    case ExitCommand:
                        Console.WriteLine("Вы вышли.");
                        isActive = false;
                        break;

                    default:
                        Console.WriteLine("Неверный ввод.");
                        break;
                }
            }
        }
    }

    class Player
    {
        private int _id;
        private string _nickname;
        private int _level;
        private bool _isBanned;

        public Player(int id, string nickname, int level, bool isBanned)
        {
            _id = id;
            _nickname = nickname;
            _level = level;
            _isBanned = isBanned;
        }

        public int Id { set { _id = value; }  get { return _id; } }
        public string Nickname { set { _nickname = value; } get { return _nickname; } }
        public int Level { set { _level = value; } get { return _level; } }
        public bool IsBanned { set { _isBanned = value; } get { return _isBanned; } }

        public static Player CreatePlayer(int id)
        {
            Console.WriteLine("Введите Nickname игрока");
            string nickname = Console.ReadLine();
            int level = 0;
            bool isLevelCorrect = false;
            bool isBanned = true;

            while (isLevelCorrect == false)
            {
                Console.Write("Введите уровень угрока:");
                string userInput = Console.ReadLine();

                if (int.TryParse(userInput, out level) && level >= 0)
                {
                    isLevelCorrect = true;
                }
                else
                {
                    Console.WriteLine("Неверный ввод.");
                }
            }

            return new Player(id, nickname, level, isBanned);
        }

        public void ShowInformation()
        {
            Console.WriteLine(" ID: " + Id);
            Console.WriteLine(" NICKNAME: " + Nickname);
            Console.WriteLine(" LEVEL: " + Level);
            Console.WriteLine(" BAN: " + IsBanned);
        }
    }

    class Database
    {
        private List<Player> _players;

        public Database ()
        {
            _players = new List<Player>();
        }
        private int GetNewPlayerId()
        {
            int id = 0;
            bool isIdCorrect = false;

            while (isIdCorrect == false)
            {
                Console.Write("Введите ID игрока:");
                string userInput = Console.ReadLine();

                if (int.TryParse(userInput, out id) && id > 0)
                {                    
                    if(CheckIdExists(id) == false)
                    {
                        isIdCorrect = true;
                    }
                    else
                    {
                        Console.WriteLine("Неверный ввод.");
                        continue;
                    }                    
                }
                else
                {
                    Console.WriteLine("Неверный ввод.");
                }
            }

            return id;
        }

        private bool CheckIdExists(int id)
        {
            bool isIdExists = false;

            foreach (Player player in _players)
            {
                if (player.Id == id)
                {
                    isIdExists = true;
                    break;
                }
            }

            return isIdExists;
        }

        public Player GetPlayerById(int id)
        {
            foreach (Player player in _players)
            {
                if (player.Id == id)
                {
                    return player;
                }
            }

            return null;
        }        

        public void СhangePlayerBan(int id)
        {
            Player player = GetPlayerById(id);

            if (player != null)
                player.IsBanned = !player.IsBanned;
            else
                Console.WriteLine("Неверный ID.");
        }

        public void AddPlayer()
        {
            int id = GetNewPlayerId();
            Player player = Player.CreatePlayer(id);
            _players.Add(player);
        }

        public void RemovePlayer(int id)
        {
            Player player = GetPlayerById(id);

            if (player != null)
                _players.Remove(player);
            else
                Console.WriteLine("Неверный ID.");
        }

        public void ShowInfo()
        {
            foreach(Player player in _players)
            {
                player.ShowInformation();
            }
        }
    }
}