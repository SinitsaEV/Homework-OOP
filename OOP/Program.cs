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

            SellerCreator creator = new SellerCreator();
            Store store = new Store(creator.Create(), new Buyer("Евгений", 100), "Свежие фрукты");
            store.Open();
        }
    }

    class SellerCreator
    {
        public Seller Create()
        {
            List<Product> products = new List<Product>();
            products.Add(new Product("Яблоко", 100, "Россия"));
            products.Add(new Product("Банан", 192, "Эквадор"));
            products.Add(new Product("Виноград", 95, "Испания"));
            products.Add(new Product("Мандарин", 52, "Турция"));
            products.Add(new Product("Апельсин", 79, "Турция"));
            products.Add(new Product("Груша", 30, "Беларусь"));

            return new Seller("Игорь", 0, products);
        }
    }

    class Product
    {
        public Product(string name, int price, string description)
        {
            Name = name;
            Price = price;
            Description = description;
        }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public int Price { get; private set; }

        public void Show()
        {
            Console.WriteLine(Name + " " + Price);
            Console.WriteLine(Description);
        }
    }
    abstract class Person
    {
        protected List<Product> _products;

        public Person(List<Product> products,int money, string name)
        {
            _products = products;
            Name = name;
            Money = money;
        }

        public string Name { get; protected set; }
        public int Money {  get; protected set; }

        public void AddProduct(Product product)
        {
            _products.Add(product);
        }

        public void ShowProducts()
        {
            foreach (Product product in _products)
            {
                product.Show();
                Console.WriteLine();
            }
        }
    }

    class Seller : Person
    {
        public Seller(string name,int money, List<Product> products) : base(products,money, name) { }
        
        public bool SellProduct(string productName, Buyer buyer)
        {
            if (TryGetProduct(productName, out Product product) == true)
            {
                if (buyer.TryBuyProduct(product))
                {
                    _products.Remove(product);
                    Money += product.Price;
                    return true;
                }
            }

            return false;
        }

        private bool TryGetProduct(string productName, out Product product)
        {
            foreach(Product currentProduct in _products)
            {
                if(currentProduct.Name == productName)
                {
                    product = currentProduct;
                    return true;
                }
            }

            product = null;
            return false;
        }
    }

    class Buyer : Person
    {
        public Buyer(string name, int money) : base(new List<Product>(), money, name) { }

        public bool TryBuyProduct(Product product)
        {
            if(Money >= product.Price)
            {
                Money -= product.Price;
                AddProduct(product);
                return true;
            }

            return false;
        }        
    }

    class Store
    {
        const string ShowProductsCommand = "1";
        const string BuyCommand = "2";
        const string ExitCommand = "3";


        private Seller _seller;
        private Buyer _buyer;

        public Store(Seller seller, Buyer buyer, string name)
        {
            _seller = seller;
            _buyer = buyer;
            Name = name;
        }

        public string Name { get; private set;}

        public void Open()
        {
            Console.WriteLine($"{_buyer.Name} - Добро пожаловать в магазин {Name}");

            bool isOpen = true;

            while (isOpen)
            {
                Console.Clear();
                Console.WriteLine($"{ShowProductsCommand} - показать товары\n" +
                    $"{BuyCommand} - купить товар\n" +
                    $"{ExitCommand} - выйти.");

                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case ShowProductsCommand:
                        _seller.ShowProducts();
                        break;

                    case BuyCommand:
                        Sell();
                        break;

                    case ExitCommand:
                        Console.WriteLine("Вы вышли.");
                        isOpen = false;
                        break;

                    default:
                        Console.WriteLine("Неверный ввод.");
                        break;
                }

                Console.ReadKey();
            }

            Console.WriteLine("Ваши покупки: ");
            _buyer.ShowProducts();
            Console.WriteLine($"у вас осталось: {_buyer.Money} денег.");
        }

        private void Sell()
        {
            Console.Write("Введите название товара: ");
            string productName = Console.ReadLine();

            if (_seller.SellProduct(productName, _buyer) == true)
            {
                Console.WriteLine($"Вы купили - {productName}");
            }
            else
            {
                Console.WriteLine("Вы не смогли купить товар.");
            }
        }
    }
}