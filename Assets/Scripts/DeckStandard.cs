using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DeckStandard : CardDeck
{
	public override void Initialize()
	{
		string [] suits = new string[]{"Heart","Spades","Diamond","Club"};
		List<CardStruct> defs = new List<CardStruct>();
		for (int i=0; i<4; ++i)
		{
			string symbol = suits[i];
			defs.Add( new CardStruct("A",symbol,1) );
			defs.Add( new CardStruct("2",symbol,2) );
			defs.Add( new CardStruct("3",symbol,3) );
			defs.Add( new CardStruct("4",symbol,4) );
			defs.Add( new CardStruct("5",symbol,5) );
			defs.Add( new CardStruct("6",symbol,6) );
			defs.Add( new CardStruct("7",symbol,7) );
			defs.Add( new CardStruct("8",symbol,8) );
			defs.Add( new CardStruct("9",symbol,9) );
			defs.Add( new CardStruct("10",symbol,10) );
            defs.Add( new CardStruct("J", symbol,11) );
            defs.Add( new CardStruct("Q", symbol, 12));
            defs.Add( new CardStruct("K", symbol, 13));
		}
		
		deckItems = new DeckItem[52];
		for (int i=0; i<defs.Count; ++i)
		{
			DeckItem item = new DeckItem();
			item.Card = defs[i];
			deckItems[i] = item;
		}
	}
}
