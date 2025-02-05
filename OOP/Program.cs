using System;

namespace OOP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int playerPositionX = 25;
            int playerPositionY = 10;
            char playerSymbol = '@';

            Player player = new Player(playerPositionX, playerPositionY, playerSymbol);
            Renderer renderer = new Renderer();
            renderer.DrawPlayer(player);
        }
    }

    class Player
    {
        public int PositionX { get; }
        public int PositionY { get; }
        public char Symbol { get; }

        public Player(int positionX, int positionY, char symbol)
        {
            PositionX = positionX;
            PositionY = positionY;
            Symbol = symbol;
        }       
    }

    class Renderer
    {
        public void DrawPlayer(Player player)
        {
            Console.SetCursorPosition(player.PositionX, player.PositionY);
            Console.Write(player.Symbol);
        }
    }
}