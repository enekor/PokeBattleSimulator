using System.Data;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using teamRandomizer.model;

public class GetRandUnit
{
    // static void Main(string[] args)
    // {
    //     var unitFetcher = new GetRandUnit();
    //     var unit = unitFetcher.GetUnit(EType.Fire).Result;
    //     Console.WriteLine($"Fetched Unit: {unit.Name}, Types: {string.Join(", ", unit.Types)}");
    // }
    public async Task<Unit> GetUnit(EType? unitType = null)
    {
        string type = "";
        Unit unit = new();

        Thread.Sleep(1000);

        if (unitType.HasValue)
        {
            type = unitType.Value.ToString();
        }
        else
        {
            var types = Enum.GetNames(typeof(EType));
            type = types.GetValue(new Random().Next(types.Length)).ToString();
        }

        var result = await new HttpClient().GetAsync($"https://pokeapi.co/api/v2/type/{type}/");
        if (!result.IsSuccessStatusCode)
        {
            throw new Exception("Failed to fetch data from PokeAPI");
        }

        var json = await result.Content.ReadAsStringAsync();
        var jsonObject = JsonNode.Parse(json);

        var unitList = jsonObject?["pokemon"]?.AsArray().ToList() ?? [];
        if (unitList.Any())
        {
            int randomIndex = new Random().Next(unitList.Count);
            var unitUrl = unitList[randomIndex][0]?["url"]?.GetValue<string>() ?? "";
            if (unitUrl == "")
            {
                throw new Exception("Failed to fetch unit URL from PokeAPI");
            }
            var unitResult = await new HttpClient().GetAsync(unitUrl);

            if (!unitResult.IsSuccessStatusCode)
            {
                throw new Exception("Failed to fetch unit data from PokeAPI");
            }

            var unitJson = await unitResult.Content.ReadAsStringAsync();
            var unitJsonObject = JsonNode.Parse(unitJson);

            int formCount = unitJsonObject?["forms"]?.AsArray()?.Count ?? 0;
            int selectedForm = new Random().Next(formCount);

            unit.Name = unitJsonObject?["forms"]?[selectedForm]?["name"]?.GetValue<string>() ?? "Unknown Unit";
            unit.Health = unitJsonObject?["stats"]?[0]?["base_stat"]?.GetValue<int>() ?? 1;
            unit.Attack = unitJsonObject?["stats"]?[1]?["base_stat"]?.GetValue<int>() ?? 1;
            unit.Defense = unitJsonObject?["stats"]?[2]?["base_stat"]?.GetValue<int>() ?? 1;
            unit.Speed = unitJsonObject?["stats"]?[5]?["base_stat"]?.GetValue<double>() ?? 1.0;
            unit.sprite = unitJsonObject?["sprites"]?["front_default"]?.GetValue<string>() ?? "default";
            var attacksArray = unitJsonObject?["moves"]?.AsArray();
            if (attacksArray != null)
            {
                try
                {
                    int random = new Random().Next(attacksArray.Count);
                    unit.Attacks.Add(new()
                    {
                        Name = attacksArray[random]?["move"]?["name"]?.GetValue<string>() ?? "Unknown Attack",

                        url = attacksArray[random]?["move"]?["url"]?.GetValue<string>() ?? "https://pokeapi.co/api/v2/move/1/"
                    });

                    random = new Random().Next(attacksArray.Count);
                    unit.Attacks.Add(new()
                    {
                        Name = attacksArray[random]?["move"]?["name"]?.GetValue<string>() ?? "Unknown Attack",
                        url = attacksArray[random]?["move"]?["url"]?.GetValue<string>() ?? "https://pokeapi.co/api/v2/move/1/"
                    });
                }
                catch (Exception ex)
                {

                    unit.Attacks.Add(new()
                    {
                        Name = "Max strike",
                        url = ""
                    });

                    unit.Attacks.Add(new()
                    {
                        Name = "Max slap",
                        url = ""
                    });
                }
            }
            var typesArray = unitJsonObject?["types"]?.AsArray();
            if (typesArray != null)
            {
                foreach (var typeNode in typesArray)
                {
                    var typeName = typeNode?["type"]?["name"]?.GetValue<string>();
                    if (Enum.TryParse<EType>(typeName, true, out var parsedType))
                    {
                        unit.Types.Add(parsedType);
                    }
                }
            }
        }

        GetAtkStats(ref unit);

        return unit;
    }

    private void GetAtkStats(ref Unit unit)
    {
        foreach (var atk in unit.Attacks)
        {
            if(atk.url == "")
            {
                atk.Power = 100;
                atk.Type = unit.Types[0];
            }
            else
            {
                var result = new HttpClient().GetAsync(atk.url).Result;
                if (!result.IsSuccessStatusCode)
                {
                    throw new Exception("Failed to fetch attack data from PokeAPI");
                }

                var json = result.Content.ReadAsStringAsync().Result;
                var jsonObject = JsonNode.Parse(json);

                atk.Power = jsonObject?["power"]?.GetValue<int>() ?? 1;
                var typeName = jsonObject?["type"]?["name"]?.GetValue<string>();
                if (Enum.TryParse<EType>(typeName, true, out var parsedType))
                {
                    atk.Type = parsedType;
                }
            }
        }
    }
}