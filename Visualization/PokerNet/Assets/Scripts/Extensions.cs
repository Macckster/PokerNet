using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions{

    public static Card DrawCard(this List<Card> cards)
    {
        if(cards.Count == 0)
        {
            return null;
        }

        Card a = cards[0];

        cards.RemoveAt(0);

        return a;
    }

    public static List<Card> Shuffle(this List<Card> cards)
    {
        System.Random rng = new System.Random();

        int n = cards.Count;
        while (n > 1)
        {
            int k = rng.Next(n--);
            Card temp = cards[n];
            cards[n] = cards[k];
            cards[k] = temp;
        }

        return cards;
    }

}
