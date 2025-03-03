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

            RussianRailways russianRailways = new RussianRailways(new Disparcher());
            russianRailways.Menu();
        }
    }

    class Train
    {
        private List<Carriage> _carriages;
        private Direction _direction;

        public Train(Direction direction)
        {
            _direction = direction;
            _carriages = new List<Carriage>();
        }

        public void Show()
        {
            Console.WriteLine($"{_direction.DeparturePoint} - {_direction.ArrivalPoint}");
        }

        public void ShowCarriagesInformation()
        {
            foreach(Carriage carriage in _carriages)
            {
                carriage.Show();
            }
        }

        public void AddCarriage(Carriage carriage)
        {
            _carriages.Add(carriage);
        }
    }

    class Carriage
    {
        public Carriage(int maxSeats, int number)
        {
            MaxSeats = maxSeats;
            Number = number;
            ReservedSeats = 0;
        }

        public int MaxSeats { get; private set; }
        public int ReservedSeats { get; private set; }
        public int Number { get; private set; }

        public void ReserveSeats(int reservedSeats)
        {
            if (reservedSeats > 0)
            {
                ReservedSeats += reservedSeats;
            }
        }

        public void Show()
        {
            Console.WriteLine($"Номер варона:{Number}\n" +
                $"Количество мест: {MaxSeats}\n" +
                $"Забронировано мест: {ReservedSeats}\n");
        }
    }

    class Direction
    {
        public Direction(string departurePoint, string arrivalPoint)
        {
            DeparturePoint = departurePoint;
            ArrivalPoint = arrivalPoint;
        }

        public string DeparturePoint { get; private set; }
        public string ArrivalPoint { get; private set; }
    }

    class Disparcher
    {
        private const int maxCarriageСapacity = 200;
        private const int minCarriageСapacity = 60;
        private const int maxTrainСapacity = 1500;

        private List<Train> _trains;

        public Disparcher()
        {
            _trains = new List<Train>();
        }

        public void ShowTrainsInformation()
        {
            foreach (Train train in _trains)
            {
                train.Show();
            }
        }

        public void CreateTrain()
        {            
            Direction direction = CreateDirection();

            Train train = new Train(direction);

            Random random = new Random();

            int passengerCount = SellTikets(random);

            FormTrain(passengerCount, train, random);

            _trains.Add(train);

            train.ShowCarriagesInformation();
        }

        private void FormTrain(int passengerCount, Train train, Random random)
        {
            int carriageNumber = 1;

            while (passengerCount > 0)
            {
                int currentPassengerCount = random.Next(minCarriageСapacity, maxCarriageСapacity + 1);

                Carriage carriage = new Carriage(currentPassengerCount, carriageNumber++);

                if (currentPassengerCount <= passengerCount)
                {
                    passengerCount -= currentPassengerCount;
                    carriage.ReserveSeats(currentPassengerCount);
                }
                else
                {
                    carriage.ReserveSeats(passengerCount);
                    passengerCount = 0;
                }

                train.AddCarriage(carriage);
            }
        }

        private Direction CreateDirection()
        {
            Console.Write("Введите пукт отправление: ");
            string departurePoint = Console.ReadLine();
            Console.Write("Введите пункт прибытия: ");
            string arrivalPoint = Console.ReadLine();

            return new Direction(departurePoint, arrivalPoint);
        }

        private int SellTikets(Random random)
        {
            return random.Next(maxTrainСapacity);
        }
    }

    class RussianRailways
    {
        private const string CreateTrainCommand = "1";
        private const string ExitCommant = "2";

        private Disparcher _disparcher;

        public RussianRailways(Disparcher disparcher)
        {
            _disparcher = disparcher;
        }

        public void Menu()
        {
            bool isActive = true;

            while (isActive)
            {
                _disparcher.ShowTrainsInformation();
                Console.WriteLine($"{CreateTrainCommand} - создать поезд\n{ExitCommant} - выйти");
                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case CreateTrainCommand:
                        _disparcher.CreateTrain();
                        break;

                    case ExitCommant:
                        isActive = false;
                        Console.WriteLine("Вы вышли.");
                        break;

                    default:
                        Console.WriteLine("Неверный ввод.");
                        break;
                }

                Console.ReadLine();
                Console.Clear();
            }
        }
    }
}