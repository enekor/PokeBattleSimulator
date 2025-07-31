public enum EType
{
    Normal = 0,
    Fire = 1,
    Water = 2,
    Grass = 3,
    Electric = 4,
    Psychic = 5,
    Ice = 6,
    Fighting = 7,
    Poison = 8,
    Ground = 9,
    Flying = 10,
    Bug = 11,
    Rock = 12,
    Ghost = 13,
    Dragon = 14,
    Dark = 15,
    Steel = 16,
    Fairy = 17
}

public static class TypeData
{
    /// <summary>
    /// Strengths dictionary where the key is the type and the value is an array of three lists:
    /// - The first list contains the types that this type is strong against.
    /// - The second list contains the types that this type is weak against.
    /// - Tthe third list contains the types that this type is inmune against.
    /// </summary>
    public static Dictionary<EType, List<List<EType>>> Strengths = new(){
        {EType.Normal,      [[], [EType.Fighting], [EType.Ghost]]}, // Normal no afecta a Fantasma
        {EType.Fire,        [[EType.Grass, EType.Ice, EType.Bug, EType.Steel], [EType.Water, EType.Ground, EType.Rock], []]},
        {EType.Water,       [[EType.Fire, EType.Ground, EType.Rock], [EType.Electric, EType.Grass], []]},
        {EType.Grass,       [[EType.Water, EType.Ground, EType.Rock], [EType.Fire, EType.Ice, EType.Poison, EType.Flying, EType.Bug], []]},
        {EType.Electric,    [[EType.Water, EType.Flying], [EType.Ground], [EType.Ground]]}, // No afecta a Tierra
        {EType.Psychic,     [[EType.Fighting, EType.Poison], [EType.Bug, EType.Ghost, EType.Dark], [EType.Dark]]}, // No afecta a Siniestro
        {EType.Ice,         [[EType.Grass, EType.Ground, EType.Flying, EType.Dragon], [EType.Fire, EType.Fighting, EType.Rock, EType.Steel], []]},
        {EType.Fighting,    [[EType.Normal, EType.Ice, EType.Rock, EType.Dark, EType.Steel], [EType.Flying, EType.Psychic, EType.Fairy], [EType.Ghost]]}, // No afecta a Fantasma
        {EType.Poison,      [[EType.Grass, EType.Fairy], [EType.Ground, EType.Psychic], [EType.Steel]]}, // No afecta a Acero
        {EType.Ground,      [[EType.Fire, EType.Electric, EType.Poison, EType.Rock, EType.Steel], [EType.Water, EType.Grass, EType.Ice], []]},
        {EType.Flying,      [[EType.Grass, EType.Fighting, EType.Bug], [EType.Electric, EType.Ice, EType.Rock], []]},
        {EType.Bug,         [[EType.Grass, EType.Psychic, EType.Dark], [EType.Fire, EType.Flying, EType.Rock], []]},
        {EType.Rock,        [[EType.Fire, EType.Ice, EType.Flying, EType.Bug], [EType.Water, EType.Grass, EType.Fighting, EType.Ground, EType.Steel], []]},
        {EType.Ghost,       [[EType.Psychic, EType.Ghost], [EType.Ghost, EType.Dark], [EType.Normal]]}, // No afecta a Normal
        {EType.Dragon,      [[EType.Dragon], [EType.Ice, EType.Fairy, EType.Dragon], [EType.Fairy]]}, // No afecta a Hada
        {EType.Dark,        [[EType.Psychic, EType.Ghost], [EType.Fighting, EType.Bug, EType.Fairy], []]},
        {EType.Steel,       [[EType.Ice, EType.Rock, EType.Fairy], [EType.Fire, EType.Fighting, EType.Ground], []]},
        {EType.Fairy,       [[EType.Fighting, EType.Dragon, EType.Dark], [EType.Poison, EType.Steel], []]}
    };

}