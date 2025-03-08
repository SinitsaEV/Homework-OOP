using System;
using System.Collections.Generic;
using System.Text;

namespace OOP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;

            Aquarium aquarium = new Aquarium(5);
            Aquarist aquarist = new Aquarist(aquarium);
            aquarist.Work();
        }
    }

    class Fish
    {
        public Fish(string name, int healse)
        {
            Name = name;
            Health = healse;
        }

        public string Name { get; private set; }
        public int Health { get; private set; }
       
        public void Show()
        {
            Console.WriteLine($"Название: {Name} Возраст: {Health}");
        }

        public void ReduceHealth()
        {
            if (Health > 0)
                Health--;
            else
                Console.WriteLine($"Рыбка {Name} погибла.");
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (!(obj is Fish other))
                return false;

            return Name.ToLower() == other.Name.ToLower() && Health == other.Health;
        }
    }

    class Aquarium
    {
        private List<Fish> _fish;

        public Aquarium(int capacity)
        {
            _fish = new List<Fish>
            {
                new Fish("Карась",4),
                new Fish("Окунь",5),
                new Fish("Сом",5),
                new Fish("Линь",3)
            };
            Capacity = capacity;
        }

        public int Capacity { get; private set; }

        public bool TryAddFish(Fish fish)
        {
            if(fish == null)
            {
               return false;
            }

            if(_fish.Count < Capacity)
            {
                _fish.Add(fish);
                return true;
            }

            return false;
        }

        public bool TryRemoveFish(Fish fish)
        {
            foreach(Fish fishInList in _fish)
            {
                if (fishInList.Equals(fish))
                {
                    _fish.Remove(fishInList);
                    return true;
                }
            }

            return false;
        }

        public void RemoveDeadFish()
        {
            for (int i = _fish.Count - 1; i >= 0; i--)
            {
                if (_fish[i].Health <= 0)
                {
                    Console.WriteLine($"Рыбку {_fish[i].Name} смыли в туалет.");
                    _fish.Remove(_fish[i]);
                }
            }           
        }

        public void ShowFish()
        {
            foreach(Fish fish in _fish)
            {
                fish.Show();
            }
        }

        public void UpdateFishHealth()
        {
            foreach(Fish fish in _fish)
            {
                fish.ReduceHealth();
            }
        }
    }

    class Aquarist
    {
        private Aquarium _aquarium;
        private FishFactory _fishFactory;

        public Aquarist(Aquarium aquarium)
        {
            _aquarium = aquarium;
            _fishFactory = new FishFactory();
        }

        public void Work()
        {
            const string AddCommand = "1";
            const string RemoveCommand = "2";
            const string PassedYearCommand = "3";
            const string ExitCommand = "4";

            bool isActive = true;

            while (isActive)
            {
                _aquarium.RemoveDeadFish();
                _aquarium.ShowFish();

                Console.WriteLine($"{AddCommand} - добавить рыбку\n" +
                    $"{RemoveCommand} - удалить рыбку\n" +
                    $"{PassedYearCommand} - пропустить год\n" +
                    $"{ExitCommand} - выйти");

                Console.Write("Введите команду: ");
                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case AddCommand:
                        AddFish();
                        break;

                    case RemoveCommand:
                        RemoveFish();
                        break;

                    case PassedYearCommand:
                        _aquarium.UpdateFishHealth();
                        break;

                    case ExitCommand:
                        isActive = false;
                        Console.WriteLine("Вы вышли.");
                        break;

                    default:
                        Console.WriteLine("Неверный ввод.");
                        break;
                }
            }
        }

        private void RemoveFish()
        {
            if (_aquarium.TryRemoveFish(_fishFactory.GetFish()))
            {
                Console.WriteLine("Вы достали рыбку.");
            }
        }

        private void AddFish()
        {
            if (_aquarium.TryAddFish(_fishFactory.GetFish()))
            {
                Console.WriteLine("Рыба добавлена.");
            }
        }       
    }

    class FishFactory
    {
        public Fish GetFish()
        {
            Console.Write("Введите имя рыбки: ");
            string name = Console.ReadLine();
            Console.WriteLine("Введите здоровье рыбки: ");
            int healse = GetInt();

            return new Fish(name, healse);            
        }

        private int GetInt()
        {
            bool isCorrectInput = false;
            int result = 0;

            while (isCorrectInput == false)
            {
                if (int.TryParse(Console.ReadLine(), out result) && result > 0)
                {
                    isCorrectInput = true;
                }
                else
                {
                    Console.WriteLine("Неверный ввод.");
                }
            }

            return result;
        }
    }
}