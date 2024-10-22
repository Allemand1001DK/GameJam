using System;
using System.Collections.Generic;
using Spectre.Console;

class Program
{
    static void Main(string[] args)
    {
        bool playAgain = true;

        while (playAgain)
        {
            Console.Clear();
            AnsiConsole.Write(new FigletText("Escape the Orhan!")
                .Centered()
                .Color(Color.Orange1));

            AnsiConsole.Markup("[bold red]Fang eller bliv fanget af Orhan, få 20 points for at vinde .[/]\n");
            AnsiConsole.Markup("[bold aqua]Klik for at begynde...[/]");
            Console.ReadKey();
            Console.Clear();

            int points = 0;
            bool hasWeapon = false;
            Random random = new Random();

            // Liste over scenarier, mens spilleren bliver jagtet af Orhan
            List<(string scenario, string correctAction, string wrongAction1, string wrongAction2)> actionsWhileBeingChased = new List<(string, string, string, string)>
            {
                ("Du ser en vej dele sig foran dig. Hvad vil du gøre?", "Drej til venstre", "Drej til højre", "Løb ligeud"),
                ("Der er en stor kantsten foran dig. Hvad vil du gøre?", "Hop over stenen", "Gå udenom", "Stop op"),
                ("Du hører Orhan tættere på. Hvad vil du gøre?", "Gem dig bag et træ", "Løb hurtigere", "Råb om hjælp"),
                ("Det har regnet meget og der en vandpyt foran dig. Hvad vil du gøre?", "Svøm over", "Byg en tømmerflåde", "Løb langs vandpytten"),
                ("Du ser Brainbox til højre. Hvad vil du gøre?", "Gå ind i hulen", "Løb forbi", "Sæt dig ned og hvile"),
                ("Det er weekend og går forbi skolen og Orhan kommer ud. Hvad gøre du?", "Jeg stivner at skræk", "Jeg råber jeg har fri og han skal blive indenfor", "Jeg løber alt hvad jeg kan")
            };

            // Liste over scenarier, når spilleren jager Orhan
            List<(string scenario, string correctAction, string wrongAction1, string wrongAction2)> actionsWhileChasingOrhan = new List<(string, string, string, string)>
            {
                ("Du ser Orhan gemme sig bag en busk. Hvad vil du gøre?", "Snige dig op og overraske ham", "Råbe for at skræmme ham ud", "Kaste en sten mod busken"),
                ("Orhan springer i en flod for at slippe væk. Hvad vil du gøre?", "Følge efter ham i floden", "Løbe langs floden", "Søge efter en bro længere fremme"),
                ("Du finder Orhan fanget i en kløft. Hvad vil du gøre?", "Efterlade ham i kløften", "Prøve at klatre ned til ham", "Råbe og sviner ham til"),
                ("Orhan taber noget på jorden, mens han løber. Hvad vil du gøre?", "Samle det op", "Ignorere det og fortsætte jagten", "Stoppe op og undersøge det"),
                ("Du ser Orhan forsøge at åbne en dør til et hus. Hvad vil du gøre?", "Løbe hen og fange ham ved døren", "Forsøge at slå ham ihjel", "Gå rundt om huset for at finde en anden indgang")
            };

            AnsiConsole.MarkupLine("Orhan jagter dig! Vælg de rigtige handlinger for at overleve.");

            while (true)
            {
                // Vælg den rigtige liste baseret på om spilleren jager eller bliver jagtet
                var currentActions = hasWeapon ? actionsWhileChasingOrhan : actionsWhileBeingChased;

                // Vælg et tilfældigt scenario
                var randomActionIndex = random.Next(currentActions.Count);
                var (scenario, correctAction, wrongAction1, wrongAction2) = currentActions[randomActionIndex];

                // Bland valgmulighederne tilfældigt
                var choices = new List<string> { correctAction, wrongAction1, wrongAction2 };
                choices.Sort((a, b) => random.Next(-1, 2)); // Tilfældig sortering af valgene

                // Brug SelectionPrompt fra Spectre.Console til at vise valgmulighederne
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title($"[greenyellow]{scenario}[/]")
                        .PageSize(3)
                        .MoreChoicesText("[grey](Brug op/ned piletasterne for at se flere valgmuligheder)[/]")
                        .AddChoices(choices));

                // Tjek om valget er korrekt
                if (choice == correctAction)
                {
                    points += random.Next(3, 7); // Tilføj tilfældige points mellem 3 og 6
                    AnsiConsole.MarkupLine("[green]Korrekt valg! Du får flere points.[/]");
                    Console.Clear();
                }
                else
                {
                    points -= random.Next(2, 5); // Træk tilfældige points mellem 2 og 4
                    points = Math.Max(points, 0); // Sørg for, at points ikke bliver mindre end 0
                    AnsiConsole.MarkupLine("[red]Forkert valg! Du mister nogle points.[/]");
                    Console.Clear();
                }

                // Opdater tilstanden baseret på points
                if (points >= 20)
                {
                    AnsiConsole.MarkupLine("\n[bold green]Tillykke! Du har 20 points og vinder spillet![/]");
                    break;
                }
                else if (points >= 10)
                {
                    if (!hasWeapon)
                    {
                        hasWeapon = true;
                        AnsiConsole.MarkupLine("\n[bold yellow]Du har fået et våben og jager nu Orhan![/]");
                    }
                }
                else
                {
                    if (hasWeapon)
                    {
                        hasWeapon = false;
                        AnsiConsole.MarkupLine("\n[bold yellow]Du har mistet dit våben, og Orhan jagter dig igen![/]");
                    }
                }

                if (points == 0)
                {
                    AnsiConsole.MarkupLine("\n[bold red]Du har 0 points! Du skal slå en terning mod Orhan for at overleve. Klik for at slå med terningen[/]");
                    Console.ReadKey();
                    int playerRoll = random.Next(1, 7);
                    int orhanRoll = random.Next(1, 7);
                    AnsiConsole.MarkupLine($"Du slog [bold yellow]{playerRoll}[/], og Orhan slog [bold yellow]{orhanRoll}[/].");

                    if (playerRoll > orhanRoll)
                    {
                        points += 2;
                        AnsiConsole.MarkupLine("[green]Du vandt terningekastet og fik 2 points! Klik for at forsætte spillet.[/]");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[red]Du tabte terningekastet. Orhan fanger dig, og du taber spillet.[/]");
                        break;
                    }
                }

                // Vis spillerens nuværende points
                AnsiConsole.MarkupLine($"[greenyellow]Dine nuværende points: {points}[/]");
            }

            // Spørg spilleren om de vil spille igen
            var response = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold]Vil du spille igen?[/]")
                    .AddChoices(new[] { "Ja", "Nej" }));

            playAgain = response == "Ja";
        }

        AnsiConsole.MarkupLine("[bold red]Spillet er slut. Tak for at spille![/]");
    }
}
