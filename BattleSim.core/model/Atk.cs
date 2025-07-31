using System.ComponentModel.DataAnnotations;

public class atk
{
    public string Name { get; set; } = "Dummy Atk";
    public int Power { get; set; } = 1;
    public EType Type { get; set; } = EType.Normal;
    public bool dealsDamage { get; set; } = true;

    public string url { get; set; } = "https://pokeapi.co/api/v2/move/1/";

}