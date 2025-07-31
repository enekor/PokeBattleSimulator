namespace teamRandomizer.model;

public class Unit
{
    public string Name { get; set; } = "Dummy Unit";
    public int Health { get; set; } = 1;
    public int Attack { get; set; } = 1;
    public int Defense { get; set; } = 1;
    public double Speed { get; set; } = 1;
    public List<EType> Types { get; set; } = new List<EType>();
    public List<atk> Attacks { get; set; } = new List<atk>();
    public string sprite { get; set; } = "default";
}