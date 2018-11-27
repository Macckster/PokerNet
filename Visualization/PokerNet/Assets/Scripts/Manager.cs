using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        Card.LoadCards();
        UpdatePlayerHand(Card.Cards["2c"], Card.Cards["Ks"]);
	}

    public static void UpdatePlayerHand(Card a, Card b)
    {
        GameObject.Find("Player Card 1").GetComponent<SpriteRenderer>().sprite = a.cardImage;
        GameObject.Find("Player Card 2").GetComponent<SpriteRenderer>().sprite = b.cardImage;
    }
}