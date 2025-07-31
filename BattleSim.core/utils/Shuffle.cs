using teamRandomizer.model;

static class Shuffle
{

    public static IEnumerable<Unit> shuffle(this IEnumerable<Unit> cards)
    {
        return cards.shuffle1().shuffle3().shuffle2();
    }
    private static List<Unit> shuffle1(this IEnumerable<Unit> cards)
    {
        if(cards.Count()%2 != 0) return cards.ToList();

        var deck1 = cards.Take(cards.Count() / 2);
        var deck2 = cards.Skip(cards.Count() / 2).Take(cards.Count() / 2);

        HashSet<int> orders = [];
        while (orders.Count < deck1.Count())
        {
            orders.Add(new Random().Next(deck1.Count()));
        }

        List<Unit> shuffledDeck = [];

        foreach (int order in orders)
        {
            shuffledDeck.Add(deck1.ElementAt(order));
            shuffledDeck.Add(deck2.ElementAt(order));
        }

        return shuffledDeck;
    }

    private static List<Unit> shuffle2(this IEnumerable<Unit> cards)
    {
        HashSet<int> orders = [];
        while (orders.Count < cards.Count())
        {
            orders.Add(new Random().Next(cards.Count()));
        }

        IEnumerable<Unit> shuffledDeck = from o in orders
                                           select cards.ElementAt(o);

        return shuffledDeck.ToList();
    }

    private static IEnumerable<Unit> shuffle3(this IEnumerable<Unit> cards)
    {

        if(cards.Count()%2 != 0) return cards.ToList();
        
        var deck1 = cards.Take(cards.Count() / 2);
        var deck2 = cards.Skip(cards.Count() / 2).Take(cards.Count() / 2);

        return deck1._shuffle3(deck2);
    }

    static IEnumerable<Unit> _shuffle3(this IEnumerable<Unit> d1, IEnumerable<Unit> d2)
    {
        
        var firstIter = d1.GetEnumerator();
        var secondIter = d2.GetEnumerator();

        while (firstIter.MoveNext() && secondIter.MoveNext())
        {
            yield return firstIter.Current;
            yield return secondIter.Current;
        }
    }
}

