using System;
using System.Collections.Generic;
using System.Text;
using OOP;

namespace OOP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string AddCommand = "1";
            const string RemoveCommand = "2";
            const string BanCommand = "3";
            const string DisbanCommand = "4";
            const string ShowCommand = "5";
            const string ExitCommand = "6";

            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;

            Database database = new Database();

            bool isActive = true;

            while (isActive)
            {
                Console.WriteLine($"{AddCommand} - добавить игрока\n" +
                    $"{RemoveCommand} - удалить игрока\n" +
                    $"{BanCommand} - забанить игрока\n" +
                    $"{DisbanCommand} - разбанить игрока\n" +
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
                        database.RemovePlayer();
                        break;

                    case BanCommand:
                        database.BanPlayer();
                        break;

                    case DisbanCommand:
                        database.DisbanPlayer();                        
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
        public Player(int id, string nickname, int level, bool isBanned)
        {
            Id = id;
            Nickname = nickname;
            Level = level;
            IsBanned = isBanned;
        }
                
        public string Nickname {private set; get; }
        public int Level {private set; get; }
        public bool IsBanned { private set; get; }
        public int Id { private set; get; }

        public void ShowInformation()
        {
            Console.WriteLine(" ID: " + Id);
            Console.WriteLine(" NICKNAME: " + Nickname);
            Console.WriteLine(" LEVEL: " + Level);
            Console.WriteLine(" BAN: " + IsBanned);
        }

        public void Ban()
        {
            IsBanned = true;
        }

        public void Disban()
        {
            IsBanned = true;
        }
    }

    class Database
    {
        private List<Player> _players;

        public Database()
        {
            _players = new List<Player>();
        }               

        public void BanPlayer()
        {
            Player player;

            if (TryGetPlayer(out player))
                player.Ban();
            else
                Console.WriteLine("Неверный ID.");
        }

        public void DisbanPlayer()
        {
            Player player;

            if (TryGetPlayer(out player))
                player.Disban();
            else
                Console.WriteLine("Неверный ID.");
        }

        public void AddPlayer()
        {
            int id = GetNewPlayerId();
            Player player = CreatePlayer(id);
            _players.Add(player);
        }

        public void RemovePlayer()
        {
            Player player;

            if(TryGetPlayer(out player))
                _players.Remove(player);
            else
                Console.WriteLine("Неверный ID.");
        }

        public void ShowInfo()
        {
            foreach (Player player in _players)
            {
                player.ShowInformation();
            }
        }

        private int ReadPlayerId()
        {
            int id = 0;
            bool isCorrectInput = false;

            while (isCorrectInput == false)
            {
                Console.Write("Введите ID игрока: ");
                
                if (int.TryParse(Console.ReadLine(), out id) && id >= 0)
                    isCorrectInput = true;
                else
                    Console.WriteLine("Некорректный ввод.");
            }

            return id;
        }

        private int GetNewPlayerId()
        {
            int id = 0;
            bool isIdCorrect = false;

            while (isIdCorrect == false)
            {
                id = ReadPlayerId();

                if (IsIdExists(id) == false)
                    isIdCorrect = true;                
                else
                    Console.WriteLine("Неверный ввод.");                
            }

            return id;
        }

        private Player CreatePlayer(int id)
        {
            Console.WriteLine("Введите Nickname игрока");
            string nickname = Console.ReadLine();
            int level = 0;
            bool isLevelCorrect = false;
            bool isBanned = false;

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

        private bool IsIdExists(int id)
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
        
        private bool TryGetPlayer(out Player player)
        {
            int id = ReadPlayerId();

            foreach (Player playerInList in _players)
            {
                if (playerInList.Id == id)
                {
                    player = playerInList;
                    return true;
                }
            }

            player = null;
            return false;
        }
    }
}
