using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardDeck : MonoBehaviour
{
	[System.Serializable]
	public class DeckItem
	{
		public CardStruct Card;
	}
	
	protected DeckItem [] deckItems;
	
	// List of cards in live deck
	List<CardStruct> liveCards = new List<CardStruct>();
	
	public virtual void Initialize()
	{
	}
	
	public void Reset()
	{
		liveCards.Clear();
		
		foreach (DeckItem item in deckItems)
		{
			liveCards.Add(item.Card);
		}
	}
	
	public void Shuffle()
	{
		for (int i=0; i<liveCards.Count; ++i)
		{
			int other = Random.Range(0,liveCards.Count);
			if (other != i)
			{
                CardStruct swap = liveCards[i];
				liveCards[i] = liveCards[other];
				liveCards[other] = swap;
			}
		}
	}
	
	public CardStruct Pop()
	{
		int last = liveCards.Count-1;
		if (last >= 0)
		{
            CardStruct result = liveCards[last];
			liveCards.RemoveAt(last);
			return result;
		}
		return null;
	}

    public bool IsEmpty()
    {
        return liveCards.Count == 0;
    }

}
