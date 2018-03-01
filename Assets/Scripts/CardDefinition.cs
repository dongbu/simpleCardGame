using UnityEngine;
using System.Collections;

public class CardDefinition : MonoBehaviour
{
	public CardStruct Data;
}

[System.Serializable]
public class CardStruct
{
	public string Symbol;
    public string Suit;
    public int Weight;

	public CardStruct(string symbol, string suit, int weight)
	{
        Symbol = symbol;
        Suit = suit;
		Weight = weight;
    }
}