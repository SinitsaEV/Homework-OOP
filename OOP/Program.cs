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
            dealer.RunGame();
        }

        class PokerDeckCreator 
        {
            public  Deck Create()
            {
                List<Card> cards = new List<Card>();

                foreach (CardSuit suit in Enum.GetValues(typeof(CardSuit)))
                {
                    foreach (CardValue value in Enum.GetValues(typeof(CardValue)))
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
            Hearts,    
            Diamonds,  
            Clubs,   
            Spades 
        }

        private enum CardValue
        {
            Ace = 1,    
            Two = 2,  
            Three = 3,
            Four = 4,  
            Five = 5,
            Six = 6,   
            Seven = 7,  
            Eight = 8,  
            Nine = 9, 
            Ten = 10,   
            Jack = 11,
            Queen = 12, 
            King = 13   
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

            public int DeckCount => _cards.Count;

            public void Show()
            {
                foreach (Card card in _cards)
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

            public List<Card> DrawCards(int count)
            {
                List<Card> cards = _cards.GetRange(0, count);
                _cards.RemoveRange(0, count);
                return cards;
            }            
        }

        class Player
        {
            private List<Card> _cards;

            public Player(string name)
            {
                Name = name;
                _cards = new List<Card>();
            }

            public string Name { get; private set; }

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

            public void RunGame()
            {
                _desk.Shuffle();
                int desiredCards = ReadCardsNumber();
                _player.TakeCards(_desk.DrawCards(desiredCards));
                Console.WriteLine(_player.Name);
                _player.ShowCards();
            }

            private int ReadCardsNumber()
            {
                bool isCorrectInput = false;
                int cardsNumber = 0;

                while (isCorrectInput == false)
                {
                    Console.Write("Ведите число карт для раздачи: ");
                    string input = Console.ReadLine();

                    if (IsCorrectCardsNumber(input, out cardsNumber))
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
                    return cardsNumber > 0 && cardsNumber <= _desk.DeckCount;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
