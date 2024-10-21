using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJamSnake
{
    public class Snake
    {
        ConsoleKeyInfo keyInfo = Console.ReadKey();
        char key = 'w';
        char dir = 'u';

        //Store snake position
        List<Position> snakeBody;

        public int x { get; set; }
        public int y { get; set; }
        public Snake()
        {
            y = 10;
            x = 10;

            snakeBody = new List<Position>();
            snakeBody.Add(new Position(x, y));
        }

        public void DrawSnake()
        {
            foreach (Position pos in snakeBody)
            {
                Console.SetCursorPosition(pos._x, pos._y);
                Console.WriteLine("▐");
            }
        }

        public void Input()
        {
            if (Console.KeyAvailable)
            {
                keyInfo = Console.ReadKey(true);
                key = keyInfo.KeyChar;
            }
        }

        private void direction()
        {
            if (key == 'w' && dir != 'd')
            {
                dir = 'u';
            }
            else if (key == 's' && dir != 'u')
            {
                dir = 'd';
            }
            else if (key == 'd' && dir != 'l')
            {
                dir = 'r';
            }
            else if (key == 'a' && dir != 'r')
            {
                dir = 'l';
            }
        }

        public void moveSnake()
        {
            direction();
            if (dir == 'u')
            {
                y--;
            }
            else if (dir == 'd')
            {
                y++;
            }
            else if (dir == 'r')
            {
                x++;
            }
            else if (dir == 'l')
            {
                x--;
            }

            snakeBody.Add(new Position(x, y));
            snakeBody.RemoveAt(0);
            Thread.Sleep(100);
        }
    }
}
