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
            russianRailways.ShowMenu();
        }
    }

    class Train
    {
        private List<Carriage> _carriages;
        private Direction _direction;

        public Train(Direction direction, List<Carriage> carriages)
        {
            _direction = direction;
            _carriages = carriages;
        }

        public void Show()
        {
            Console.WriteLine($"{_direction.DeparturePoint} - {_direction.ArrivalPoint}");
        }

        public void ShowCarriagesInformation()
        {
            foreach (Carriage carriage in _carriages)
            {
                carriage.Show();
            }
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

            Random random = new Random();

            int passengerCount = SellTikets(random);

            Train train = new Train(direction, FormTrain(passengerCount, random));

            _trains.Add(train);

            train.ShowCarriagesInformation();
        }

        private List<Carriage> FormTrain(int passengerCount, Random random)
        {
            const int MaxCarriageСapacity = 200;
            const int MinCarriageСapacity = 60;

            List<Carriage> carriages = new List<Carriage>();
            
            int carriageNumber = 1;

            while (passengerCount > 0)
            {
                int currentPassengerCount = random.Next(MinCarriageСapacity, MaxCarriageСapacity + 1);

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

                carriages.Add(carriage);
            }

            return carriages;
        }

        private Direction CreateDirection()
        {
            bool isActive = true;
           
            while(isActive)
            {
                Console.Write("Введите пукт отправление: ");
                string departurePoint = Console.ReadLine();
                Console.Write("Введите пункт прибытия: ");
                string arrivalPoint = Console.ReadLine();

                if(departurePoint.ToLower() == arrivalPoint.ToLower())
                {
                    Console.WriteLine("Неверный ввод.");
                    isActive = false;
                }
            }

            return new Direction(departurePoint, arrivalPoint);
        }

        private int SellTikets(Random random)
        {
            const int MaxTrainСapacity = 1500;

            return random.Next(MaxTrainСapacity);
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

        public void ShowMenu()
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
