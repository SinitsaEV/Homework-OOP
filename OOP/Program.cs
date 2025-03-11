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

            ZooFabric zooFabric = new ZooFabric();
            Zoo zoo = zooFabric.CreateZoo();
            ZooAdministration zooAdministration = new ZooAdministration(zoo);
            zooAdministration.OpenZoo();
        }
    }

    class Zoo
    {
        private List<Aviary> _aviaries;

        public Zoo(string name, List<Aviary> aviaries)
        {
            Name = name;
            _aviaries = aviaries;
        }

        public string Name { get; private set; }

        public int AviarysCount => _aviaries.Count;

        public void ShowAviarys()
        {
            for (int i = 0; i < _aviaries.Count; i++)
            {
                Console.WriteLine(i + " " + _aviaries[i].Name);
            }
        }

        public void ShowAviaryByIndex(int index)
        {
            _aviaries[index].Show();
        }
    }

    class Aviary
    {
        private List<Animal> _animals;

        private string _animalsSound;
        private string _animalsType;

        public Aviary(string name, List<Animal> animals)
        {
            Name = name;
            _animals = animals;
            _animalsSound = animals[0].Sound;
            _animalsType = animals[0].Type;
        }

        public string Name { get; private set; }

        public int AnimalsCount => _animals.Count;

        public void Show()
        {
            int animalsIsMaleCount = GetIsMaleCount();
            int animalsIsFemaleCount = _animals.Count - animalsIsMaleCount;

            Console.WriteLine($"В вольере {Name} находится: {AnimalsCount} {_animalsType}, самцов: {animalsIsMaleCount}, самок: {animalsIsFemaleCount}, издают звук: {_animalsSound}");
        }

        private int GetIsMaleCount()
        {
            int isMaleCount = 0;

            foreach (Animal animal in _animals)
            {
                if (animal.IsMale)
                {
                    isMaleCount++;
                }
            }

            return isMaleCount;
        }
    }

    class Animal
    {
        public Animal(string type, bool isMale, string sound)
        {
            Type = type;
            IsMale = isMale;
            Sound = sound;
        }

        public string Type { get; private set; }
        public bool IsMale { get; private set; }
        public string Sound { get; private set; }
    }

    class ZooAdministration
    {
        private Zoo _zoo;

        public ZooAdministration(Zoo zoo)
        {
            _zoo = zoo;
        }

        public void OpenZoo()
        {
            const string SeeAviaryCommand = "1";
            const string ExitCommand = "2";

            Console.WriteLine($"Добро пожаловать в зоопарк {_zoo.Name}.");
            bool isActive = true;

            while (isActive)
            {
                Console.WriteLine($"{SeeAviaryCommand} - подойти к вольеру\n{ExitCommand} - выйти");
                string visitorInput = Console.ReadLine();

                switch (visitorInput)
                {
                    case SeeAviaryCommand:
                        SeeAviary();
                        break;

                    case ExitCommand:
                        isActive = false;
                        Console.WriteLine("Вы вышли.");
                        break;

                    default:
                        Console.WriteLine("Неправильный ввод.");
                        break;
                }
            }
        }

        private void SeeAviary()
        {
            _zoo.ShowAviarys();
            _zoo.ShowAviaryByIndex(ReadAviaryIndex());
        }

        private int ReadAviaryIndex()
        {
            int aviaryIndex = 0;

            while (int.TryParse(Console.ReadLine(), out aviaryIndex) == false || aviaryIndex < 0 || aviaryIndex >= _zoo.AviarysCount)
            {
                Console.WriteLine("Неверный ввод.");
            }

            return aviaryIndex;
        }
    }

    class ZooFabric
    {
        public Zoo CreateZoo()
        {
            return new Zoo("Беловежская пуща", CreateZooAviaries());
        }

        private List<Aviary> CreateZooAviaries()
        {
            List<Aviary> aviaries = new List<Aviary>();

            string nameBison = "Зубр";
            string soundOfBison = "Рев: Муууууууу";

            string nameWildBoar = "Дикий кабан";
            string soundOfWildBoar = "Грру-грру";

            string nameRoeDeer = "Косуля";
            string soundOfRoeDeer = "Тяф-тяф!";

            string nameBadger = "Барсук";
            string soundOfBadger = "Шшшш";

            aviaries.Add(new Aviary("Подземное царство", CreateAnimals(nameBadger, soundOfBadger)));
            aviaries.Add(new Aviary("Кабаньи угодья", CreateAnimals(nameWildBoar, soundOfWildBoar)));
            aviaries.Add(new Aviary("Царство зубров", CreateAnimals(nameBison, soundOfBison)));
            aviaries.Add(new Aviary("Лесная грация", CreateAnimals(nameRoeDeer, soundOfRoeDeer)));

            return aviaries;
        }

        private List<Animal> CreateAnimals(string type, string sound)
        {
            List<Animal> animals = new List<Animal>();

            int maxAnimalsCount = 100;
            int minAnimalsCount = 10;

            int animalsCount = UserUtils.GenerateRandomNumber(minAnimalsCount, maxAnimalsCount);

            for (int i = 0; i < animalsCount; i++)
            {
                animals.Add(new Animal(type, GetRandomIsMale(), sound));
            }

            return animals;
        }

        private bool GetRandomIsMale()
        {
            int max = 1;
            int min = 0;

            return UserUtils.GenerateRandomNumber(min, max) == max;
        }
    }

    class UserUtils
    {
        private static Random s_random = new Random();

        public static int GenerateRandomNumber(int min, int max)
        {
            return s_random.Next(min, max + 1);
        }
    }
}
