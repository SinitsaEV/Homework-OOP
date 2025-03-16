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

            ServiceStationFabric serviceStationFabric = new ServiceStationFabric();
            ServiceStationAdministrator administrator = new ServiceStationAdministrator(serviceStationFabric.CreateServiceStation(), serviceStationFabric);
            administrator.OpenServiceStation();
        }
    }

    class Detail
    {
        public Detail(string name, bool isBroken, int prise)
        {
            Name = name;
            IsBroken = isBroken;
            Prise = prise;
        }

        public string Name { get; private set; }
        public bool IsBroken {  get; private set; }
        public int Prise { get; private set; }

        public override bool Equals(object obj)
        {
            if(obj ==  null || GetType() != obj.GetType())
            {
                return false;
            } 

            Detail other = obj as Detail;

            return Name == other.Name;
        }

        public void Show()
        {
            Console.WriteLine(Name);
        }

        public Detail Clone()
        {
            return new Detail(Name, IsBroken, Prise);
        }
    }

    class Car
    {
        private List<Detail> _details;

        public Car(List<Detail> details, string brand)
        {
            _details = details;
            Brand = brand;
        }

        public string Brand {  get; private set; }    
        
        public List<Detail> GetDetails()
        {
            return _details;
        }

        public bool TryRemoveDetail(Detail brokenDetail)
        {
            if(brokenDetail == null)
            {
                return false;
            }

            for(int i = 0; i < _details.Count; i++)
            {
                if (_details[i].Equals(brokenDetail))
                {
                    _details.Remove(_details[i]);
                    return true;
                }
            }

            return false;
        }

        public bool TryAddDetail(Detail detail)
        {
            if(detail != null)
            {
                _details.Add(detail);
                return true;
            }

            return false;
        }
    }

    class ServiceStation
    {
        private Queue<Client> _clients;
        private Storage _storage;

        public ServiceStation(Queue<Client> clients, Storage storage)
        {
            _clients = clients;
            _storage = storage;
            RepairCost = 1000;
            FineAmount = 500;
            Balanse = 0;
        }

        public int Balanse {  get; private set; }

        public int RepairCost {  get; private set; }

        public int FineAmount {  get; private set; }

        public bool TryAddClient(Client client)
        {
            if(client != null)
            {
                _clients.Enqueue(client);
                return true;
            }

            return false;
        }

        public bool TryServeClient()
        {
            Client client = _clients.Peek();

            if (TryRepairCar(client.GetCar()))
            {
                _clients.Dequeue();
                return true;
            }

            return false;
        }

        private bool IsRepairAccepted(string message)
        {
            const string Agree = "1";
            const string Disagree = "2";

            Console.WriteLine(message);

            while (true)
            {
                Console.WriteLine($"{Agree} - согласен\n{Disagree} - не согласен");
                string clientInput = Console.ReadLine();

                switch (clientInput)
                {
                    case Agree:
                        return true;

                    case Disagree:
                        return false;

                    default:
                        Console.WriteLine("Неверный ввод.");
                        break;
                }
            }
        }

        private void PayPenalty(int penalty)
        {
            if(penalty > Balanse)
            {
                Balanse = 0;
                Console.WriteLine(" Баланс на 0 сервис банкрот. ");
            }
            else
            {
                Balanse -= penalty;
                Console.WriteLine($"Автосервис заплатил штраф {penalty}");
            }
        }

        private void ShowDetails(List<Detail> details)
        {
            foreach(Detail detail in details)
            {
                detail.Show();
            }
        }

        private bool TryRepairCar(Car car)
        {
            List<Detail> brokenDetails = GetBrokenDetails(car);
            Console.WriteLine("Эти детали сломались:");
            ShowDetails(brokenDetails);

            if (IsRepairAccepted("Желаете починить машину?") == false)
            {
                PayPenalty(FineAmount);
                return false;
            }

            for (int i = 0; i < brokenDetails.Count; i++)
            {
                if (IsRepairAccepted($"Желаете починить {brokenDetails[i].Name}?") == false)
                {
                    PayPenalty(FineAmount * GetBrokenDetails(car).Count);
                    return true;
                }

                if (TryReplaceDetail(brokenDetails[i], car, out int detailRepairCost))
                {
                    _clients.Peek().Pay(detailRepairCost);
                    Console.WriteLine($"Вы заплатили {detailRepairCost}");
                    Balanse += detailRepairCost;
                }
                else
                {
                    Console.WriteLine("Деталь не смогли поменять.");
                    PayPenalty(FineAmount * GetBrokenDetails(car).Count);
                    return true;
                }
            }

            return true;
        }               

        private List<Detail> GetBrokenDetails(Car car)
        {
            List<Detail> broken = new List<Detail>();

            foreach(Detail detail in car.GetDetails())
            {
                if (detail.IsBroken)
                {
                    broken.Add(detail);
                }
            }

            return broken;
        }

        private bool TryReplaceDetail(Detail brokenDetail, Car car, out int detailRepairCost)
        {
            detailRepairCost = 0;

            if (_storage.TryGetDetail(brokenDetail, out Detail serviceableDetail) == false)
            {
                Console.WriteLine("Такой детали нет на складе.");
                return false;
            }

            int totalCost = GetDetailRepairCost(serviceableDetail);

            if (_clients.Peek().HasEnoughMoney(totalCost) == false)
            {
                Console.WriteLine("У киента недостаточно денег для замены детали.");
                return false;
            }

            if(car.TryRemoveDetail(serviceableDetail) == false)
            {
                Console.WriteLine("Деталь не получилось снять.");
                return false;
            }

            if(car.TryAddDetail(serviceableDetail) == false)
            {
                Console.WriteLine("Деталь не получилось поставить.");
                car.TryAddDetail(brokenDetail);
                return false;
            }

            detailRepairCost = totalCost;
            return true ;
        }

        private int GetDetailRepairCost(Detail detail)
        {
            return RepairCost + detail.Prise;
        }
    }

    class Storage
    {
        private Dictionary<Detail, int> _details;

        public Storage(Dictionary<Detail, int> details)
        {
            _details = details;
        }

        public bool TryGetDetail(Detail brokenDetail,out Detail serviceableDetail)
        {
            foreach(Detail detail in _details.Keys)
            {
                if (detail.Equals(brokenDetail) && _details[detail] > 0)
                {
                    _details[detail]--;
                    serviceableDetail = detail.Clone();
                    return true;
                }
            }

            serviceableDetail = null;
            return false;
        }
    }

    class Client
    {
        private Car _car;

        public Client(Car car, int money, string name)
        {
            _car = car;
            Money = money;
            Name = name;
        }

        public int Money { get; private set; }
        public string Name { get; private set; }

        public Car GetCar()
        {
            return _car;
        }

        public bool HasEnoughMoney(int money)
        {
            return Money >= money;
        } 
        
        public void Pay(int money)
        {
            Money -= money;
        }
    }

    class ServiceStationAdministrator
    {
        private ServiceStation _serviceStation;
        private ServiceStationFabric _serviceStationFabric;

        public ServiceStationAdministrator(ServiceStation serviceStation, ServiceStationFabric serviceStationFabric)
        {
            _serviceStation = serviceStation;
            _serviceStationFabric = serviceStationFabric;
        }

        public void OpenServiceStation()
        {
            const string AddClientCommand = "1";
            const string ServeClientCommand = "2";
            const string AddDetailsToStorageCommand = "3";
            const string ExitCommand = "4";

            bool isActive = true;

            while (isActive)
            {
                Console.WriteLine($"{AddClientCommand} - добавить клиента\n" +
                    $"{ServeClientCommand} - обслужить клиента\n" +
                    $"{AddDetailsToStorageCommand} - добавить детали на склад\n" +
                    $"{ExitCommand} - выйти");
                string adminInput = Console.ReadLine();

                switch (adminInput)
                {
                    case AddClientCommand:
                        AddClient();
                        break;

                    case ServeClientCommand:
                        InitiateClientService();
                        break;

                    case AddDetailsToStorageCommand:
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

        private void InitiateClientService()
        {
            if (_serviceStation.TryServeClient())
            {
                Console.WriteLine("Клиент обслужен.");
                Console.WriteLine($"Баланс: {_serviceStation.Balanse}");
            }
        }

        private void AddClient()
        {
            Client client = _serviceStationFabric.CreateClient();

            if (_serviceStation.TryAddClient(client))
            {
                Console.WriteLine("Клиент добавлен.");
            }
            else
            {
                Console.WriteLine("Клиент не был добавлен.");
            }
        }
    }

    class ServiceStationFabric
    {
        private int _maxDetailsPrise = 5000;
        private int _minDetailsPrise = 1000;

        private List<string> _detailsName = new List<string>
        {
                "Ремень",
                "Колодка",
                "Стекло",
                "Колесо",
                "Поршень",
                "Коробка передачь"
        };

        public ServiceStation CreateServiceStation()
        {
            return new ServiceStation(CreateClients(), CreateStorage());
        }

        private Queue<Client> CreateClients()
        {
            Queue<Client> clients = new Queue<Client>(new List<Client>());

            int maxClientsCount = 20;
            int minClientsCount = 5;

            int clientsCount = UserUtils.GenerateRandomNumber(minClientsCount, maxClientsCount);

            for (int i = 0; i < clientsCount; i++)
            {
                clients.Enqueue(CreateClient());
            }

            return clients;
        }

        public Client CreateClient()
        {
            int maxClientMoney = 50000;
            int minClientMoney = 5000;
            int clientMoney = UserUtils.GenerateRandomNumber(minClientMoney, maxClientMoney);

            string[] clientsName = new string[]
            {
                "Евгений",
                "Иван",
                "Владислав",
                "Владимир",
                "Илья"
            };

            int clientNameRandomIndex = UserUtils.GenerateRandomNumber(0, clientsName.Length);

            return new Client(CreateCar(), clientMoney, clientsName[clientNameRandomIndex]);
        }

        private Car CreateCar()
        {
            string[] brandsName = new string[]
            {
                "Audi",
                "BMW",
                "Citroën",
                "Peugeot",
                "Renault"
            };

            int randomBrandNameIndex = UserUtils.GenerateRandomNumber(0, brandsName.Length);

            return new Car(CreateCarDetails(), brandsName[randomBrandNameIndex]);
        }

        private Storage CreateStorage()
        {
            Dictionary<Detail, int> details = new Dictionary<Detail, int>();
            int maxDetailsInStorageCount = 5;
            int minDetailsInStorageCount = 1;

            foreach (Detail detail in CreateStorageDetails())
            {
                int randomDetailsCount = UserUtils.GenerateRandomNumber(minDetailsInStorageCount, maxDetailsInStorageCount);
                details.Add(detail, randomDetailsCount);
            }

            return new Storage(details);
        }

        private List<Detail> CreateStorageDetails()
        {
            List<Detail> details = new List<Detail>();

            foreach (string detaileName in _detailsName)
            {
                int detailePrise = UserUtils.GenerateRandomNumber(_minDetailsPrise, _maxDetailsPrise);
                details.Add(new Detail(detaileName, false, detailePrise));
            }

            return details;
        }

        private List<Detail> CreateCarDetails()
        {
            List<Detail> details = new List<Detail>();

            bool[] isBrokenArray = new bool[] { true, false };
            bool isActive = true;

            while (isActive)
            {
                foreach (string detaileName in _detailsName)
                {
                    int detailePrise = UserUtils.GenerateRandomNumber(_minDetailsPrise, _maxDetailsPrise);
                    int randomIndex = UserUtils.GenerateRandomNumber(0, isBrokenArray.Length);
                    details.Add(new Detail(detaileName, isBrokenArray[randomIndex], detailePrise));
                }

                if (IsContainsBrokenDetails(details))
                {
                    isActive = false;
                }
                else
                {
                    details.Clear();
                }
            }            

            return details;
        }

        private bool IsContainsBrokenDetails(List<Detail> details)
        {
            foreach (Detail detail in details)
            {
                if (detail.IsBroken)
                {
                    return true;
                }
            }

            return false;
        }
    }

    class UserUtils
    {
        private static Random s_random = new Random();

        public static int GenerateRandomNumber(int min, int max)
        {
            return s_random.Next(min, max);
        }
    }
}