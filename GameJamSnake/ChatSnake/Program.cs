using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Spectre.Console;

namespace SnakeGame
{
    class Program
    {
        static int width = 40;
        static int height = 20;
        static int score;
        static int speed;
        static List<(int X, int Y)> snake;
        static (int X, int Y) food;
        static (int X, int Y) direction;
        static bool gameRunning;
        static (int X, int Y) lastTailPosition;
        static string[][] levels =
        {
            new string[] { "Orhan" },          // Niveau 1
            new string[] { "Jan" },            // Niveau 2
            new string[] { "Alice", "Bob" },   // Niveau 3
            new string[] { "Charlie", "Diana" } // Niveau 4
        };

        static List<char> foodCharacters;
        static int currentFoodIndex = 0; // Indeks til det aktuelle mad
        static int currentLevelIndex = 0; // Indeks til det aktuelle niveau
        static int currentNameIndex = 0; // Indeks til det aktuelle navn

        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            bool playAgain = true;

            while (playAgain)
            {
                Console.Clear();
                InitializeGame();
                while (gameRunning)
                {
                    Update();
                    Render();
                    Thread.Sleep(speed);
                }
                Console.SetCursorPosition(0, height + 2);
                Console.WriteLine($"Game Over! Your score: {score}");
                Console.WriteLine("Press 'R' to restart or any other key to exit.");

                var key = Console.ReadKey(true).Key;
                playAgain = key == ConsoleKey.R;
            }
        }

        static void InitializeGame()
        {
            snake = new List<(int X, int Y)>();
            score = 0;
            speed = 100;
            direction = (1, 0);
            gameRunning = true;

            snake.Add((width / 2, height / 2));
            snake.Add((width / 2 - 1, height / 2));
            snake.Add((width / 2 - 2, height / 2));

            foodCharacters = levels[currentLevelIndex][currentNameIndex].ToCharArray().ToList(); // Opret en liste af bogstaver fra det aktuelle navn
            currentFoodIndex = 0; // Reset indeks for mad
            GenerateFood();
        }

        static void Update()
        {
            HandleInput();
            MoveSnake();
            CheckCollision();
        }

        static void Render()
        {
            Console.SetCursorPosition(food.X, food.Y);
            Console.Write(foodCharacters[currentFoodIndex]); // Vis det aktuelle bogstav fra foodCharacters

            Console.SetCursorPosition(snake[0].X, snake[0].Y);
            Console.Write("X");

            Console.SetCursorPosition(lastTailPosition.X, lastTailPosition.Y);
            Console.Write(" ");

            if (score == 0)
            {
                for (int x = 0; x < width; x++)
                {
                    Console.SetCursorPosition(x, 0);
                    Console.Write("-");
                    Console.SetCursorPosition(x, height - 1);
                    Console.Write("-");
                }
                for (int y = 0; y < height; y++)
                {
                    Console.SetCursorPosition(0, y);
                    Console.Write("|");
                    Console.SetCursorPosition(width - 1, y);
                    Console.Write("|");
                }
            }

            Console.SetCursorPosition(0, height);
            Console.Write($"Score: {score}");
        }

        static void HandleInput()
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(intercept: true).Key;
                direction = key switch
                {
                    ConsoleKey.UpArrow when direction != (0, 1) => (0, -1),
                    ConsoleKey.DownArrow when direction != (0, -1) => (0, 1),
                    ConsoleKey.LeftArrow when direction != (1, 0) => (-1, 0),
                    ConsoleKey.RightArrow when direction != (-1, 0) => (1, 0),
                    _ => direction
                };
            }
        }

        static void MoveSnake()
        {
            var head = (X: snake[0].X + direction.X, Y: snake[0].Y + direction.Y);
            snake.Insert(0, head);

            lastTailPosition = snake[^1];

            // Kontroller om slangen spiser maden
            if (head == food)
            {
                score += 10;
                speed = Math.Max(10, speed - 5);

                currentFoodIndex++; // Gå til næste bogstav

                if (currentFoodIndex >= foodCharacters.Count) // Hvis alle bogstaver er spist
                {
                    // Vis hvem der er spist
                    Console.Clear();
                    ShowMessage($"Du har spist hele navnet '{levels[currentLevelIndex][currentNameIndex]}'.");
                    Thread.Sleep(2000); // Vent lidt tid før spillet fortsætter

                    currentNameIndex++; // Gå til næste navn
                    if (currentNameIndex >= levels[currentLevelIndex].Length) // Hvis der ikke er flere navne på niveauet
                    {
                        currentLevelIndex++; // Gå til næste niveau
                        currentNameIndex = 0; // Reset indeks for næste navn

                        if (currentLevelIndex >= levels.Length) // Hvis der ikke er flere niveauer
                        {
                            AskQuestion();
                            gameRunning = false; // Stop spillet
                            return;
                        }

                        foodCharacters = levels[currentLevelIndex][currentNameIndex].ToCharArray().ToList(); // Opdater maden til næste navn
                        currentFoodIndex = 0; // Reset indeks for næste omgang
                    }
                    else
                    {
                        foodCharacters = levels[currentLevelIndex][currentNameIndex].ToCharArray().ToList(); // Opdater maden til næste navn
                    }
                }

                GenerateFood();
            }
            else
            {
                snake.RemoveAt(snake.Count - 1);
            }
        }

        static void CheckCollision()
        {
            var head = snake[0];

            if (head.X <= 0 || head.X >= width - 1 || head.Y <= 0 || head.Y >= height - 1)
            {
                gameRunning = false;
            }

            if (snake.Skip(1).Any(part => part == head))
            {
                gameRunning = false;
            }
        }

        static void GenerateFood()
        {
            var rand = new Random();
            food = (rand.Next(1, width - 1), rand.Next(1, height - 1));
        }

        static void AskQuestion()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Du har spist alle navne i de fire niveauer! Den lille boss");
            Thread.Sleep(2000); // Vent lidt tid før spillet afsluttes
        }

        static void ShowMessage(string message)
        {
            int messageWidth = message.Length + 4;
            int messageHeight = 5; // Height of the message box
            int startX = (width - messageWidth) / 2;
            int startY = (height - messageHeight) / 2;

            // Draw the border
            for (int x = 0; x < messageWidth; x++)
            {
                Console.SetCursorPosition(startX + x, startY);
                Console.Write("-");
                Console.SetCursorPosition(startX + x, startY + messageHeight - 1);
                Console.Write("-");
            }
            for (int y = 0; y < messageHeight; y++)
            {
                Console.SetCursorPosition(startX, startY + y);
                Console.Write("|");
                Console.SetCursorPosition(startX + messageWidth - 1, startY + y);
                Console.Write("|");
            }

            // Write the message
            Console.SetCursorPosition(startX + 2, startY + 2);
            Console.WriteLine(message);

            // Pause for a moment to let the player read the message
            Thread.Sleep(2000);

            // Clear the message box by redrawing the border
            for (int x = 0; x < messageWidth; x++)
            {
                Console.SetCursorPosition(startX + x, startY);
                Console.Write(" ");
                Console.SetCursorPosition(startX + x, startY + messageHeight - 1);
                Console.Write(" ");
            }
            for (int y = 0; y < messageHeight; y++)
            {
                Console.SetCursorPosition(startX, startY + y);
                Console.Write(" ");
                Console.SetCursorPosition(startX + messageWidth - 1, startY + y);
                Console.Write(" ");
            }
            
            // Restore the borders of the game area
            DrawBorders();
        }

        static void DrawBorders()
        {
            for (int x = 0; x < width; x++)
            {
                Console.SetCursorPosition(x, 0);
                Console.Write("-");
                Console.SetCursorPosition(x, height - 1);
                Console.Write("-");
            }
            for (int y = 0; y < height; y++)
            {
                Console.SetCursorPosition(0, y);
                Console.Write("|");
                Console.SetCursorPosition(width - 1, y);
                Console.Write("|");
            }
        }

    }
}
