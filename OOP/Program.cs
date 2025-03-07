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

            SupermarketCreator creator = new SupermarketCreator();
            Administrator administrator = new Administrator(creator.Create());

            administrator.InitializeAdministration();
        }
    }

    class Supermarket
    {
        private Queue<Client> _clients;
        private Storage _storage;

        public Supermarket(string name, Queue<Client> clients, Storage storage)
        {
            Name = name;
            _clients = clients;
            _storage = storage;
            Balanse = 0;
        }

        public string Name { get; private set; }
        public int Balanse { get; private set; }

        public bool TryAddClient(Client client)
        {
            if (client != null)
            {
                _clients.Enqueue(client);
                return true;
            }

            return false;
        }

        public void RemoveProduct(Product product)
        {
            if (_storage.TryRemoveProduct(product))
                Console.WriteLine("Продукт удален.");
            else
                Console.WriteLine("Продукт не был удален.");
        }

        public void AddProduct(Product product)
        {
            if (_storage.TryAddProduct(product))
                Console.WriteLine("Продукт добавлен.");
            else
                Console.WriteLine("Продукт не добавлен.");
        }

        public void ShowProducts()
        {
            Console.WriteLine($"Ассортимент супермаркета {Name}:");
            _storage.ShowProducts();
        }

        public void ShowClients()
        {
            foreach (Client client in _clients)
            {
                client.ShowInfo();
            }

            Console.WriteLine("Клиентов в очереди: " + _clients.Count);
        }

        public bool TryServiceClient()
        {
            if(_clients.Count == 0)
            {
                Console.WriteLine("Клиентов нет.");
                return false;

            }

            ChoiseProducts(_clients.Peek());

            if(_clients.Peek().TryBuyProducts(out int price))
            {
                Balanse += price;
                _clients.Dequeue();
                return true;
            }

            return false;
        }

        private void ChoiseProducts(Client client)
        {
            int maxProductsCount = 10;
            int minProductsCount = 5;

            int randomIndex = UserUtils.GenerateRandomNumber(minProductsCount, maxProductsCount);

            while (randomIndex > 0)
            {
                _clients.Peek().AddProductToBasket(_storage.GetRandomProduct());
                randomIndex--;
            }
        } 
    }

    class Administrator
    {
        private Supermarket _supermarket;

        public Administrator(Supermarket supermarket)
        {
            _supermarket = supermarket;
        }

        public void InitializeAdministration()
        {
            const string AddProductCommand = "1";
            const string RemoveProductCommand = "2";
            const string AddClientCommand = "3";
            const string ServiceClientCommand = "4";
            const string ShowProductsCommand = "5";
            const string ShowClientsCommand = "6";
            const string ExitCommand = "7";
                        
            bool isActive = true;

            while (isActive)
            {
                Console.WriteLine("Меню администратора:");
                Console.WriteLine($"{AddProductCommand} - добавить продукт\n" +
                    $"{RemoveProductCommand} - удалить продукт\n" +
                    $"{AddClientCommand} - добавить клиента\n" +
                    $"{ServiceClientCommand} - обслужить клиента\n" +
                    $"{ShowProductsCommand} - показать продукты\n" +
                    $"{ShowClientsCommand} - показать клиентов\n" +
                    $"{ExitCommand} - выйти");
                Console.Write("Введите команду: ");
                string adminInput = Console.ReadLine();

                switch (adminInput)
                {
                    case AddProductCommand:
                        _supermarket.AddProduct(ReadProduct());
                        break;

                    case RemoveProductCommand:
                        _supermarket.RemoveProduct(ReadProduct());
                        break;

                    case AddClientCommand:
                        AddClient();
                        break;

                    case ServiceClientCommand:
                        InitiateClientService();
                        break;

                    case ShowProductsCommand:
                        _supermarket.ShowProducts();
                        break;

                    case ShowClientsCommand:
                        _supermarket.ShowClients();
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

        private void InitiateClientService()
        {
            if (_supermarket.TryServiceClient())
            {
                Console.WriteLine("Клиент обслужен");
                Console.WriteLine($"Баланс магазина: {_supermarket.Balanse}");
            }
            
        }

        private void AddClient()
        {
            if (_supermarket.TryAddClient(ReadClient()))
                Console.WriteLine("Клиент добавлен.");
            else
                Console.WriteLine("Клиент не был добавлен.");
        }

        private Client ReadClient()
        {
            Console.Write("Введите имя клиента: ");
            string clientName = Console.ReadLine();
            Console.Write("Введите сумму клиента: ");
            int clientMoney = ReadInt();

            return new Client(clientName, clientMoney);
        }

        private Product ReadProduct()
        {
            Console.Write("Введите название продукта: ");
            string productName = Console.ReadLine();
            Console.Write("Введите цену продукта: ");
            int productPrise = ReadInt();

            return new Product(productName, productPrise);
        }

        private int ReadInt()
        {
            int input = 0;

            while (int.TryParse(Console.ReadLine(), out input) == false || input < 0)
            {
                Console.WriteLine("Ошибка ввода.");
            }

            return input;
        }
    }

    class Product
    {
        public Product(string name, int price)
        {
            Name = name;
            Price = price;
        }

        public string Name { get; private set; }
        public int Price { get; private set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Product other = obj as Product;

            return Name.ToLower() == other.Name.ToLower() && Price == other.Price;
        }

        public Product Clone()
        {
            return new Product(Name, Price);
        }

        public void Show()
        {
            Console.WriteLine($"Название: {Name} Цена: {Price}");
        }
    }

    class Client
    {
        private Bag _bag;
        private Basket _basket;

        public Client(string name, int money)
        {
            Name = name;
            Money = money;
            _bag = new Bag();
            _basket = new Basket();
        }

        public string Name { get; private set; }
        public int Money { get; private set; }

        public void ShowInfo()
        {
            Console.WriteLine($"Имя - {Name}");
        }

        public void AddProductToBasket(Product product)
        {
            if (_basket.TryAddProduct(product))
            {
                Console.WriteLine($"{Name} добавил в корзину {product.Name}");
            }
        }

        public bool TryBuyProducts(out int money)
        {
            RemoveExcessProducts();

            if(_basket.GetCount() > 0)
            {
                Money -= _basket.TotalPrice;
                _bag.AddProducts(_basket.GetProducts());
                money = _basket.TotalPrice;
                _basket.Clear();

                Console.WriteLine($"{Name} купил:");
                _bag.ShowProducts();
            }

            money = 0;
            return true;
        }

        private void RemoveExcessProducts()
        {
            while (_basket.TotalPrice > Money && _basket.GetCount() > 0)
            {
                int randomIndex = UserUtils.GenerateRandomNumber(0, _basket.GetCount());

                _basket.RemoveProduct(randomIndex);
            }
        }
    }

    abstract class ProductContainer
    {
        protected List<Product> _products;

        protected ProductContainer()
        {
            _products = new List<Product>();
        }

        public virtual bool TryAddProduct(Product product)
        {
            if (product != null)
            {
                _products.Add(product);
                return true;
            }

            return false;
        }

        public bool TryRemoveProduct(Product product)
        {
            if (product != null)
            {
                foreach (Product productInList in _products)
                {
                    if (productInList.Equals(product))
                    {
                        _products.Remove(productInList);
                        return true;
                    }
                }
            }

            return false;
        }

        public void ShowProducts()
        {
            foreach (Product product in _products)
            {
                product.Show();
            }
        }

        public int GetCount()
        {
            return _products.Count;
        }

    }

    class Basket : ProductContainer
    {
        public Basket() : base()
        {
            TotalPrice = 0;
        }

        public int TotalPrice { get; private set; }

        public void Clear()
        {
            _products.Clear();
            TotalPrice = 0;
        }

        public override bool TryAddProduct(Product product)
        {
            if (base.TryAddProduct(product))
            {
                TotalPrice += product.Price;
                return true;
            }

            return false;
        }

        public List<Product> GetProducts()
        {
            return new List<Product>(_products);
        }

        public void RemoveProduct(int index)
        {
            TotalPrice -= _products[index].Price;
            Console.WriteLine($"{_products[index].Name} - удален из корзины");
            _products.Remove(_products[index]);
        }
    }

    class Bag : ProductContainer
    {
        public Bag() : base() { }       

        public void AddProducts(List<Product> products)
        {
            if (_products != null)
            {
                _products.AddRange(products);                
            }
        }
    }

    class Storage : ProductContainer
    {
        public Storage(List<Product> products)
        {
            _products = products;
        }

        public Product GetRandomProduct()
        {
            int randomProductIndex = UserUtils.GenerateRandomNumber(0, _products.Count);

            return _products[randomProductIndex].Clone();
        }
    }

    class SupermarketCreator
    {
        public Supermarket Create()
        {
            Queue<Client> clients = new Queue<Client>();
            List<Product> products = new List<Product>();

            clients.Enqueue(new Client("Василий", 1000));
            clients.Enqueue(new Client("Антон", 2000));
            clients.Enqueue(new Client("Игорь", 6000));
            clients.Enqueue(new Client("Илья", 1000));
            clients.Enqueue(new Client("Настя", 4000));
            clients.Enqueue(new Client("Екатерина", 1000));

            products.Add(new Product("Яблоко", 100));
            products.Add(new Product("Банан", 150));
            products.Add(new Product("Груша", 200));
            products.Add(new Product("Собачий корм", 300));
            products.Add(new Product("Сосиски", 500));
            products.Add(new Product("Хлеб", 100));
            products.Add(new Product("Молоко", 130));

            return new Supermarket("Ашан", clients, new Storage(products));
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