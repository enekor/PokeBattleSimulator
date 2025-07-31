using teamRandomizer.model;

public class Battle
{
    public static Unit? StartBattle(Unit unit1, Unit unit2)
    {
        ShowInfo(unit1, unit2);

        List<int> damages = new List<int>();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Battle started!");
        Console.ResetColor();
        List<Unit> order = unit1.Speed >= unit2.Speed ? new List<Unit> { unit1, unit2 } : new List<Unit> { unit2, unit1 };
        do
        {
            Console.WriteLine($"\n{order[0].Name} attacks {order[1].Name}");
            int damage = calculateDamage(order[0], order[1]);
            damages.Add(damage);
            order[1].Health -= damage;
            Console.WriteLine($"{order[1].Name} takes {damage} damage and has {order[1].Health} HP left\n");

            if (order[1].Health <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{order[1].Name} has fainted!");
                Console.WriteLine($"{order[0].Name} wins!");
                Console.ResetColor();
                return order[0];
            }

            Console.WriteLine($"\n{order[1].Name} attacks {order[0].Name}");
            damage = calculateDamage(order[1], order[0]);
            damage = damage < 0 ? 0 : damage;
            damages.Add(damage);
            order[0].Health -= damage;
            Console.WriteLine($"{order[0].Name} takes {damage} damage and has {order[0].Health} HP left\n");

            if (order[0].Health <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{order[0].Name} has fainted!");
                Console.WriteLine($"{order[1].Name} wins!");
                Console.ResetColor();
                return order[1];
            }


            if (damages.Contains(0))
            {
                if (damages[damages.Count-1] == 0 && damages[damages.Count-2] == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("Both units dealt no damage, it's a draw!");
                    Console.ResetColor();
                    return null;
                }
                //else if (damages[0] == 0)
                //{
                //    Console.ForegroundColor = ConsoleColor.Green;
                //    Console.WriteLine($"{order[0].Name} dealt no damage to {order[1].Name}!");
                //    Console.WriteLine($"{order[1].Name} is too strong for {order[0].Name}!");
                //    Console.WriteLine($"{order[1].Name} wins!");
                //    Console.ResetColor();
                //    return order[1];
                //}
                //else
                //{
                //    Console.ForegroundColor = ConsoleColor.Green;
                //    Console.WriteLine($"{order[1].Name} dealt no damage to {order[0].Name}!");
                //    Console.WriteLine($"{order[0].Name} is too strong for {order[1].Name}!");
                //    Console.WriteLine($"{order[0].Name} wins!");
                //    Console.ResetColor();
                //    return order[0];
                //}
            }
        } while (unit1.Health > 0 && unit2.Health > 0);

        return null;
    }

    // static void Main(string[] args)
    // {
    //     Unit unit1 = new Unit()
    //     {
    //         Name = "Ancestral Dragon ultra necrozma",
    //         Health = 15000,
    //         Attack = 160,
    //         Defense = 150,
    //         Speed = 95.0,
    //         Types = new List<EType> { EType.Dragon, EType.Psychic }
    //     };
    //     Unit unit2 = new Unit()
    //     {
    //         Name = "Lord bidoof",
    //         Health = 300000,
    //         Attack = 200,
    //         Defense = 100,
    //         Speed = 1500.0,
    //         Types = new List<EType> { EType.Normal }
    //     };

    //     StartBattle(unit1, unit2);
    // }

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

    private static int calculateDamage(Unit attacker, Unit defender)
    {
        

        int level = 100;
        double atk = attacker.Attack;
        double def = defender.Defense;
        List<double> power = GetRandAttk(attacker.Attacks, attacker.Name, attacker.Types, defender);

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

        int damage = Math.Max(1, (int)baseDamage); // daño mínimo de 1

        return damage;
    }

    private static List<double> GetRandAttk(List<atk> attacks, string name, List<EType> types, Unit defender)
    {
        if (attacks == null || attacks.Count == 0)
        {
            Console.WriteLine($"{name} has no attacks available and uses his body!");
            return [1,1]; // Default power if no attacks are available
        }

        Random random = new Random();
        int randomIndex = random.Next(attacks.Count);
        atk selectedAtk = attacks[randomIndex];

        double multiplier = GetStrengthMultiplier.GetMultiplier(selectedAtk, defender);
        if (multiplier == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{name}'s attack {selectedAtk.Name} has no effect on {defender.Name}!");
            Console.ResetColor();
            return [0,0]; // No damage if the multiplier is 0 (immunity)
        }

        Console.WriteLine($"{name} used {selectedAtk.Name}");
        double power = selectedAtk.Power;

        if(IsStab(selectedAtk.Type, types))
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{name} gets STAB bonus for using {selectedAtk.Name}!");
            Console.ResetColor();
            power *= 1.5;
        }

        return [power <= 0 && multiplier != 0 ? 1 : power , multiplier]; // Ensure power is at least 1
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