using teamRandomizer.model;

public class Battle
{
    public static Unit? StartBattle(Unit unit1, Unit unit2)
    {
        ShowInfo(unit1, unit2);

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Battle started!");
        Console.ResetColor();
        List<Unit> order = unit1.Speed >= unit2.Speed ? new List<Unit> { unit1, unit2 } : new List<Unit> { unit2, unit1 };
        Unit first = order[0];
        Unit second = order[1];
        do
        {
            if(checkIfContinue(first, second, out Unit? winner))
            {
                return winner;
            }

            Console.WriteLine($"\n{first.Name} attacks {second.Name}");
            int damage = calculateDamage(ref first, ref second);

            second.Health -= damage;
            Console.WriteLine($"{second.Name} takes {damage} damage and has {second.Health} HP left\n");

            if (second.Health <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{second.Name} has fainted!");
                Console.WriteLine($"{first.Name} wins!");
                Console.ResetColor();
                return first;
            }

            Console.WriteLine($"\n{second.Name} attacks {first.Name}");
            damage = calculateDamage(ref second, ref first);
            damage = damage < 0 ? 0 : damage;

            first.Health -= damage;
            Console.WriteLine($"{first.Name} takes {damage} damage and has {first.Health} HP left\n");

            if (first.Health <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{first.Name} has fainted!");
                Console.WriteLine($"{second.Name} wins!");
                Console.ResetColor();
                return second;
            }
        } while (unit1.Health > 0 && unit2.Health > 0);

        return null;
    }

    private static bool checkIfContinue(Unit first, Unit second, out Unit? winner)
    {
        if (!first.Attacks[0].dealsDamage && !first.Attacks[1].dealsDamage)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{first.Name} cant deal damage to {second.Name}!");
            Console.WriteLine($"{second.Name} wins by default!");
            Console.ResetColor();
            winner = second;
            return true; // Continue the battle
        }
        else if (!second.Attacks[0].dealsDamage && !second.Attacks[1].dealsDamage)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{second.Name} cant deal damage to {first.Name}!");
            Console.WriteLine($"{first.Name} wins by default!");
            Console.ResetColor();
            winner = first;
            return true; // Continue the battle
        }
        else
        {
            winner = null;
            return false;
        }
    }

    private static void ShowInfo(Unit unit, Unit unit2)
    {
        int width = unit.Name.Length + unit2.Name.Length + 5 + 4 - 2; // 5 for " VS. ", 4 for padding and 2 for the borders
        string horizontal = new string('═', width);
        Console.WriteLine("╔" + horizontal + "╗");
        Console.WriteLine($"║ {unit.Name} VS. {unit2.Name} ║");
        Console.WriteLine("╚" + horizontal + "╝");

        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine($"{unit.Name} - Health: {unit.Health}, Attack: {unit.Attack}, Defense: {unit.Defense}, Speed: {unit.Speed}, Types: {string.Join(", ", unit.Types)}");
        Console.WriteLine($"{unit2.Name} - Health: {unit2.Health}, Attack: {unit2.Attack}, Defense: {unit2.Defense}, Speed: {unit2.Speed}, Types: {string.Join(", ", unit2.Types)}\n");
        Console.ResetColor();
    }

    private static int calculateDamage(ref Unit attacker, ref Unit defender)
    {
        

        int level = 100;
        double atk = attacker.Attack;
        double def = defender.Defense;
        List<double> power = GetRandAttk(ref attacker, defender);

        double baseDamage = (((2 * level / 5 + 2) * power[0] * atk / def) / 50 + 2) * power[1];


        Random rnd = new Random();
        double randomFactor = rnd.NextDouble() * 0.15 + 0.85; // entre 0.85 y 1.00
        baseDamage *= randomFactor;

        if (IsCriticalHit())
        {
            baseDamage *= 1.5; // aplicar el multiplicador crítico
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Critical hit!");
            Console.ResetColor();
        }

        int damage = (int)baseDamage;

        return damage;
    }

    private static List<double> GetRandAttk(ref Unit attacker, Unit defender)
    {
        if (attacker.Attacks == null || attacker.Attacks.Count == 0)
        {
            Console.WriteLine($"{attacker.Name} has no attacks available and uses his body!");
            return [1,1]; // Default power if no attacks are available
        }

        Random random = new Random();
        int randomIndex = random.Next(attacker.Attacks.Count);
        atk selectedAtk = attacker.Attacks[randomIndex];

        if(selectedAtk.dealsDamage == false)
        {
            selectedAtk = attacker.Attacks.Where(a => a.Name != selectedAtk.Name).FirstOrDefault() ?? selectedAtk; // Fallback to first attack that deals damage
        }

        double multiplier = GetStrengthMultiplier.GetMultiplier(selectedAtk, defender);
        if (multiplier == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{attacker.Name}'s attack {selectedAtk.Name} has no effect on {defender.Name}!");
            Console.ResetColor();

            attacker.Attacks.Where(a => a.Name == selectedAtk.Name).First().dealsDamage = false;
            return [0,0]; // No damage if the multiplier is 0 (immunity)
        }

        Console.WriteLine($"{attacker.Name} used {selectedAtk.Name}");
        double power = selectedAtk.Power;

        if(IsStab(selectedAtk.Type, attacker.Types))
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{attacker.Name} gets STAB bonus for using {selectedAtk.Name}!");
            Console.ResetColor();
            power *= 1.5;
        }

        return [power <= 0 ? 1 : power , multiplier]; // Ensure power is at least 1
    }

    private static bool IsCriticalHit()
    {
        Random rnd = new Random();
        return rnd.NextDouble() < 0.0625; // 6.25% chance
    }

    private static bool IsStab(EType atkType, List<EType> types)
    {
        return types.Contains(atkType); // Check if the attack type is one of the unit's types
    }
}