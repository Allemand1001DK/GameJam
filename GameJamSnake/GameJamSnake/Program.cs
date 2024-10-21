namespace GameJamSnake
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool finish = false;
            Canvas canvas = new Canvas();
            Snake snake = new Snake();

            while (!finish)
            {
                canvas.Draw();
                snake.DrawSnake();
                snake.moveSnake();
                //Console.ReadLine();
            }
        }
    }
}
