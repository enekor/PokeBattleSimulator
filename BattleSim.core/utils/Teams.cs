using teamRandomizer.model;

public class BattleTeams
{
    private List<Unit> GenerateTeam(List<EType> types, bool isMonoType)
    {
        GetRandUnit unitFetcher = new();

        types = types.Count <= 3 ? types : types.Take(3).ToList();

        if (isMonoType)
        {
            types = new()
            {
                types[0],
                types[0],
                types[0],
            };
        }

        List<Unit> team = new();
        foreach (EType type in types)
        {
            team.Add(unitFetcher.GetUnit(type).Result);
        }

        if (team.Count < 3)
        {
            while (team.Count < 3)
            {
                team.Add(unitFetcher.GetUnit().Result);
            }
        }

        return team;

    }
    
    public List<List<Unit>> GenerateTeams(List<EType> types1, bool isMonoType1, List<EType> types2, bool isMonoType2)
    {

        List<Unit> team1 = GenerateTeam(types1, isMonoType1);
        List<Unit> team2 = GenerateTeam(types2, isMonoType2);

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Team 1:");
        foreach (var unit in team1)
        {
            Console.WriteLine($"- {unit.Name} (Health: {unit.Health}, Attack: {unit.Attack}, Defense: {unit.Defense}, Speed: {unit.Speed})");
        }
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Team 2:");
        foreach (var unit in team2)
        {
            Console.WriteLine($"- {unit.Name} (Health: {unit.Health}, Attack: {unit.Attack}, Defense: {unit.Defense}, Speed: {unit.Speed})");
        }
        Console.ResetColor();
        Console.WriteLine("\n");

        team1 = Shuffle.shuffle(team1) as List<Unit> ?? [];
        team2 = Shuffle.shuffle(team2) as List<Unit> ?? [];

        return new List<List<Unit>> { team1, team2 };
    }
}