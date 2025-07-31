using teamRandomizer.model;

public class Program
{
    public static async Task Main(string[] args)
    {
        bool stop = false;
        while (!stop)
        {
            Console.WriteLine("Generating teams...");
            int wins1 = 0;
            int wins2 = 0;

            BattleTeams battleTeams = new();
            var teams = battleTeams.GenerateTeams(
                [],
                false,
                [],
                false
            );
            List<Unit> team1 = teams[0];
            List<Unit> team2 = teams[1];

            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine($"Battle {i + 1}:");
                Unit? winner = Battle.StartBattle(team1[i], team2[i]);
                if (winner != null)
                {
                    if (winner == team1[i])
                    {
                        wins1++;
                    }
                    else
                    {
                        wins2++;
                    }
                }
                Console.WriteLine();
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            if (wins1 > wins2)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Team 1 wins with {wins1} victories!");
            }
            else if (wins2 > wins1)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"Team 2 wins with {wins2} victories!");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("It's a draw!");
            }
            Console.ResetColor();

            Console.WriteLine("Press any key to re-match or 'q' to quit.");
            string input = Console.ReadLine();  
            if (input?.ToLower() == "q")
            {
                stop = true;
            }
            else
            {
                Console.WriteLine("\n------------------------------------------------------------------------------------------------\n");
            }
        }
    }
}