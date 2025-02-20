using System;
using System.Collections.Generic;
using System.Text;

namespace OOP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Player player = new Player("Евгений");
            PokerDeckCreator creator = new PokerDeckCreator();
            Deck pokerDeck = creator.Create();
            Dealer dealer = new Dealer(pokerDeck, player);
            Console.OutputEncoding = Encoding.Unicode;
            dealer.StartGame();
        }

        abstract class DeckCreator
        {
            public abstract Deck Create();
        }

        class PokerDeckCreator : DeckCreator 
        {
            public override Deck Create()
            {   
                List<Card> cards = new List<Card>();

                foreach (CardSuit suit in Enum.GetValues(typeof(CardSuit)))
                {
                    foreach(CardValue value in Enum.GetValues(typeof(CardValue)))
                    {
                        Card card = new Card(suit, value);
                        cards.Add(card);                        
                    }
                }

                return new Deck(cards);
            }                        
        }

        private enum CardSuit
        {
            Hearts,    // Черви
            Diamonds,  // Бубны
            Clubs,     // Трефы
            Spades     // Пики
        }

        private enum CardValue
        {
            Ace = 1,    // Туз
            Two = 2,    // Два
            Three = 3,  // Три
            Four = 4,   // Четыре
            Five = 5,   // Пять
            Six = 6,    // Шесть
            Seven = 7,  // Семь
            Eight = 8,  // Восемь
            Nine = 9,   // Девять
            Ten = 10,   // Десятка
            Jack = 11,  // Валет
            Queen = 12, // Дама
            King = 13   // Король
        }

        class Card
        {
            private CardSuit _suit;
            private CardValue _value;

            public Card(CardSuit suit, CardValue value)
            {
                _suit = suit;
                _value = value;
            }

            public void Show()
            {
                Console.WriteLine($"{_value} - {_suit}");
            }
        }

        class Deck
        {
            private List<Card> _cards;

            public Deck(List<Card> cards)
            {
                _cards = cards;
            }

            public void Show()
            {
                foreach ( Card card in _cards )
                    card.Show();
            }

            public void Shuffle()
            {
                Random random = new Random();

                for (int i = 0; i < _cards.Count; i++)
                {
                    int randomIndex = random.Next(_cards.Count);
                    Card buffer = _cards[i];
                    _cards[i] = _cards[randomIndex];
                    _cards[randomIndex] = buffer;
                }
            }

            public List<Card> GetCards(int count)
            {
                List<Card> cards = _cards.GetRange(0, count);
                _cards.RemoveRange(0, count);
                return cards;
            }

            public int GetCount => _cards.Count;
        }              

        class Player
        {
            private List<Card> _cards;

            public Player(string name)
            {
                Name = name;
                _cards = new List<Card>();
            }

            public string Name { get; set; }

            public void ShowCards()
            {
                foreach (Card card in _cards)
                    card.Show();
            }

            public void TakeCards(List<Card> cards)
            {
                _cards.AddRange(cards);
            }
        }

        class Dealer
        {
            private Deck _desk;
            private Player _player;

            public Dealer(Deck desk, Player player)
            {
                _desk = desk;
                _player = player;
            }

            private int ReadCardsNumber()
            {
                bool isCorrectInput = false;
                int cardsNumber = 0;

                while (isCorrectInput == false)
                {
                    Console.Write("Ведите число карт для раздачи: ");
                    string input = Console.ReadLine();

                    if (IsCorrectCardsNumber(input,out cardsNumber))
                    {
                        isCorrectInput = true;
                    }                    
                }

                return cardsNumber;
            }

            private bool IsCorrectCardsNumber(string input, out int cardsNumber)
            {
                if (int.TryParse(input, out cardsNumber) == true)
                {
                    if (cardsNumber > 0 && cardsNumber <= _desk.GetCount)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            public void StartGame()
            {
                _desk.Shuffle();
                int desiredCards = ReadCardsNumber();
                _player.TakeCards(_desk.GetCards(desiredCards));
                Console.WriteLine(_player.Name);
                _player.ShowCards();
            }
        }
    }
}