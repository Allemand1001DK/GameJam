using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJamSnake
{
    internal class Position
    {
        public int _x { get; set; }
        public int _y { get; set; }

        public Position(int x, int y)
        {
            _x = x;
            _y = y;
        }
    }
}
