using teamRandomizer.model;
public class GetStrengthMultiplier
{
    public static double GetMultiplier(atk atk, Unit defender)
    {
        double multiplier = 1.0;

        foreach (var defType in defender.Types.ToHashSet())
        {
            if (TypeData.Strengths[atk.Type][2].Contains(defType))
            {
                return 0; // inmunidad
            }

            if (TypeData.Strengths[atk.Type][0].Contains(defType))
                multiplier *= 2;
            else if (TypeData.Strengths[atk.Type][1].Contains(defType))
                multiplier *= 0.5;
        }

        return multiplier;
    }

    // public static void Main(string[] args)
    // {
    //     // Example usage
    //     Unit unit1 = new Unit("Fire Dragon", 100, 50, 30, EType.Fire, EType.Dragon);
    //     Unit unit2 = new Unit("Water Serpent", 80, 40, 20, EType.Water);

    //     double multiplier = GetMultiplier(unit2, unit1);
    //     Console.WriteLine($"Strength multiplier: {multiplier}");
    // }
}
