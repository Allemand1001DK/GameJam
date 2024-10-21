using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJamSnake
{
    public class Canvas
    {
        public int Width { get; set; }
        public int Height { get; set; } 

        public Canvas()
        {
            Width = 100;
            Height = 20;

            Console.CursorVisible = false;
        }

        public void Draw()
        {
            Console.Clear();

            for (int i = 0; i < Width; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("-");
            }

            for (int i = 0; i < Width; i++)
            {
                Console.SetCursorPosition(i, Height);
                Console.Write("-");
            }

            for (int i = 0; i < Height; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("|");
            }

            for (int i = 0; i < Height; i++)
            {
                Console.SetCursorPosition(Width, i);
                Console.Write("|");
            }
        }
    }
}
