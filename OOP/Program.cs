using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;

            LibraryManager libraryManager = new LibraryManager();

            libraryManager.InitLibrary();
        }        
    }

    class Library
    {
        private List<Book> _books;

        public Library(List<Book> books)
        {
            _books = books;
        }

        public void Show()
        {
            foreach (Book book in _books)
            {
                book.Show();
            }
        }

        public bool Add(Book book)
        {
            if (book != null)
            {
                _books.Add(book);
                return false;
            }

            return false;
        }

        public bool Remote(Book book)
        {
            foreach(Book currentBook in _books)
            {
                if (currentBook.Equals(book))
                {
                    _books.Remove(currentBook);
                    return true;
                }
            }

            return false;
        }

        public bool TryGetFilterBooks(Func<Book, bool> predicate, out List<Book> filteredBooks)
        {
            filteredBooks = _books.Where(predicate).ToList();

            return filteredBooks.Count > 0;
        }
    }

    class LibraryCreator
    {
        public Library Create()
        {
            List<Book> books = new List<Book>();
            books.Add(new Book("Война и мир", new Author("Толстой", "Лев", "Николаевич"), 1868));
            books.Add(new Book("Мастер и Маргарита", new Author("Булгаков", "Михаил", "Афанасьевич"), 1966));
            books.Add(new Book("Преступление и наказание", new Author("Достоевский", "Фёдор", "Михайлович"), 1866));
            books.Add(new Book("Мертвые души", new Author("Гоголь", "Николай", "Васильевич"), 1842));
            books.Add(new Book("Капитанская дочка", new Author("Пушкин", "Александр", "Сергеевич"), 1836));
            books.Add(new Book("Муму", new Author("Тургенев", "Иван", "Сергеевич"), 1854));
            books.Add(new Book("Евгений Онегин", new Author("Пушкин", "Александр", "Сергеевич"), 1831));
            books.Add(new Book("Ревизор ", new Author("Гоголь", "Николай", "Васильевич"), 1836));
            books.Add(new Book("А зори здесь тихие…", new Author("Васильев", "Борис", "Львович"), 1969));
            books.Add(new Book("Тихий дон", new Author("Шолохов", "Михаил", "Александрович"), 1928));
            Library library = new Library(books);

            return library;
        }
    }

    class Book
    {
        public Book(string title, Author author, int releaseYear)
        {
            Title = title;
            Author = author;
            ReleaseYear = releaseYear;
        }

        public string Title { get; private set; }
        public Author Author { get; private set; }
        public int ReleaseYear { get; private set; }

        public void Show()
        {
            Console.WriteLine($" Название: {Title}");
            Console.Write($" Автор: ");
            Author.Show();
            Console.WriteLine($" Год релиза: {ReleaseYear} \n");
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (obj is Book other)
            {
                return other.Title == Title && other.ReleaseYear == ReleaseYear && other.Author.Equals(Author);
            }

            return false;
        }
    }

    class Author
    {
        public Author(string lastName, string firstName, string patronymic)
        {
            LastName = lastName;
            FirstName = firstName;
            Patronymic = patronymic;
        }

        public string LastName { get; private set; }
        public string FirstName { get; private set; }
        public string Patronymic { get; private set; }

        public void Show()
        {
            Console.WriteLine($"{LastName} {FirstName} {Patronymic}");
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj is Author other)
            {
                return other.LastName == LastName && other.FirstName == FirstName && other.Patronymic == Patronymic;
            }

            return false;
        }
    }

    class LibraryManager
    {
        const string AddCommand = "1";
        const string RemoteCommand = "2";
        const string FilterByYearCommand = "3";
        const string FilterByTitleCommand = "4";
        const string FilterByAutorCommand = "5";
        const string ShowCommand = "6";
        const string ExitCommand = "7";

        private Library _library;

        public LibraryManager()
        {
            LibraryCreator libraryCreator = new LibraryCreator();
            _library = libraryCreator.Create();
        }

        public void InitLibrary()
        {
            bool isActive = true;

            while (isActive)
            {
                Console.WriteLine($"" +
                    $"{AddCommand} - добавить книгу\n" +
                    $"{RemoteCommand} - удалить книгу\n" +
                    $"{FilterByYearCommand} - фильтр по году\n" +
                    $"{FilterByTitleCommand} - фильтр по названию\n" +
                    $"{FilterByAutorCommand} - фильтр по автору\n" +
                    $"{ShowCommand} - показать все книги\n" +
                    $"{ExitCommand} - выход");
                string playerInput = Console.ReadLine();

                switch (playerInput)
                {
                    case AddCommand:
                        AddBook();
                        break;

                    case RemoteCommand:
                        RemoteBook();
                        break;

                    case FilterByYearCommand:
                        FilterByYear();
                        break;

                    case FilterByTitleCommand:
                        FilterByTitle();
                        break;
                    case FilterByAutorCommand:
                        FilterByAuthor();
                        break;

                    case ShowCommand:
                        _library.Show();
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

        private void AddBook()
        {
            if (_library.Add(ReadBook()))
            {
                Console.WriteLine("Книга добавлена.");
            }
        }

        private Book ReadBook()
        {
            Author author = ReadAuthor();
            string title = ReadString("Введите название книги: ");
            int releaseYear = ReadYear();

            return new Book(title, author, releaseYear);
        }

        private Author ReadAuthor()
        {
            string lastName = ReadString("Введите фамилию автора: ");
            string firstName = ReadString("Введите имя автора: ");
            string patronymic = ReadString("Введите отчество автора: ");

            return new Author(lastName, firstName, patronymic);
        }

        private string ReadString(string massage)
        {
            Console.Write(massage);
            return Console.ReadLine();
        }

        private int ReadYear()
        {
            int releaseYear = 0;
            Console.Write("Введите год релиза: ");

            while (int.TryParse(Console.ReadLine(), out releaseYear) == false || releaseYear < 0)
            {
                Console.WriteLine(" Неверный ввод. Попробуйте снова. ");
            }

            return releaseYear;
        }

        private void RemoteBook()
        {
            if (_library.Remote(ReadBook()))
            {
                Console.WriteLine("Книга удалена.");
            }
            else
            {
                Console.WriteLine("Такой книги нет в библиотеке.");
            }
        }

        private void FilterByYear()
        {
            int year = ReadYear();

            if (_library.TryGetFilterBooks(book => book.ReleaseYear == year, out List<Book> books))
            {
                foreach (Book book in books)
                {
                    book.Show();
                }
            }
            else
            {
                Console.WriteLine("Книги не найдены.");
            }
        }

        private void FilterByTitle()
        {
            string title = ReadString("Введите название книги: ");

            if (_library.TryGetFilterBooks(book => book.Title == title, out List<Book> books))
            {
                foreach (Book book in books)
                {
                    book.Show();
                }
            }
            else
            {
                Console.WriteLine("Книги не найдены.");
            }
        }

        private void FilterByAuthor()
        {
            string lastName = ReadString("Введите фамилию автора: ");

            if (_library.TryGetFilterBooks(book => book.Author.LastName == lastName, out List<Book> books))
            {
                foreach (Book book in books)
                {
                    book.Show();
                }
            }
            else
            {
                Console.WriteLine("Книги не найдены.");
            }
        }
    }
}
