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
            // Tegn den øverste kant
            for (int i = 0; i < Width; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("-");
            }

            // Tegn den nederste kant
            for (int i = 0; i < Width; i++)
            {
                Console.SetCursorPosition(i, Height - 1); // Height - 1 for at undgå out of bounds
                Console.Write("-");
            }

            // Tegn den venstre kant
            for (int i = 0; i < Height; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("|");
            }

            // Tegn den højre kant
            for (int i = 0; i < Height; i++)
            {
                Console.SetCursorPosition(Width - 1, i); // Width - 1 for at undgå out of bounds
                Console.Write("|");
            }
        }
    }
}
