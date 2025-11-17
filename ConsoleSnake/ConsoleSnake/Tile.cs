using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSnake
{
    public enum TileState
    {
        Free,
        Snake,
        Apple,
        Border
    }

    public class Tile
    {
        public TileState state = TileState.Free;


        public Tile(bool border = false)
        {
            if (border)
            {
                state = TileState.Border;
            }
        }

        public void Print()
        {
            switch (state)
            {
                case TileState.Free:
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;

                case TileState.Snake:
                    Console.BackgroundColor = ConsoleColor.White;
                    break;

                case TileState.Apple:
                    Console.BackgroundColor = ConsoleColor.Red;
                    break;

                case TileState.Border:
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    break;

            }

            Console.Write("  ");
        }
    }
}
