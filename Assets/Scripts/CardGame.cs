using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardGame : MonoBehaviour
{
    public CardDeck Deck;
    public float CardFlipDuration = 0.5f;

    Card OpponentHand;
    Card PlayerHand;
    GameObject DummyCard;

    GameObject Banner_YouWin;
    GameObject Banner_YouLose;
    GameObject Banner_Tie;

    Transform Transform_OpponentHand;
    Transform Transform_PlayerHand;
    Transform Transform_Deck;

    Score OpponentScore;
    Score PlayerScore;
    Score RoundCounter;

    GameObject Button_Deal;
    GameObject Button_Reset;

    enum GameState
    {
        Invalid,
        Started,
        Resolving,
        RoundEnd_YouLose,
        RoundEnd_YouWin,
        RoundEnd_Tie,
        GameOver
    };

    GameState gameState;


    // Use this for initialization
    void Start()
    {
        gameState = GameState.Invalid;
        Deck.Initialize();

        DummyCard = GameObject.Find("PlayingCard_Revers").gameObject;

        Banner_YouWin = this.transform.Find("Text_YouWin").gameObject;
        Banner_YouLose = this.transform.Find("Text_ComputerWins").gameObject;
        Banner_Tie = this.transform.Find("Text_Tie").gameObject;

        Button_Deal = this.transform.Find("Button_Deal").gameObject;
        Button_Reset = this.transform.Find("Button_Reset").gameObject;

        Transform_OpponentHand = this.transform.Find("DealPosition_Computer").transform;
        Transform_PlayerHand = this.transform.Find("DealPosition_Player").transform;
        Transform_Deck = DummyCard.transform;

        OpponentScore = this.transform.Find("Score_Computer").gameObject.GetComponent<Score>();
        PlayerScore = this.transform.Find("Score_Player").gameObject.GetComponent<Score>();
        RoundCounter = this.transform.Find("Round_Counter").gameObject.GetComponent<Score>();

        ResetTable();
    }

    void ResetTable()
    {
        gameState = GameState.Invalid;
        ClearMessages();
        Deck.Reset();
        Deck.Shuffle();

        UpdateButtons();

        OpponentScore.reset();
        PlayerScore.reset();
        RoundCounter.reset();

        DummyCard.GetComponent<MeshRenderer>().enabled = true;
        DummyCard.transform.position = Transform_Deck.position;

        OpponentHand = null;
        PlayerHand = null;
    }

    bool canDeal()
    {
        return (gameState == GameState.Invalid) ||
                (gameState == GameState.RoundEnd_YouWin) ||
                (gameState == GameState.RoundEnd_YouLose) ||
                (gameState == GameState.RoundEnd_Tie);
    }

    bool canReset()
    {
        return (gameState == GameState.GameOver);
    }

    void UpdateButtons()
    {
        
        Button_Deal.GetComponent<Renderer>().material.color = canDeal() ? Color.green : Color.red;
        Button_Reset.GetComponent<Renderer>().material.color = canReset() ? Color.green : Color.red;
    }

    void ClearMessages()
    {
        Banner_YouWin.SetActive(false);
        Banner_YouLose.SetActive(false);
        Banner_Tie.SetActive(false);
    }

    void UpdateGameCount()
    {
        ClearMessages();
        //if (PlayerScore.isMaxed())
        //{
        //    gameState = GameState.GameOver;
        //    Banner_YouWin.SetActive(true);
        //}

        //else if (OpponentScore.isMaxed())
        //{
        //    gameState = GameState.GameOver;
        //    Banner_YouLose.SetActive(true);
        //}
        if (RoundCounter.isMaxed())
        {
            gameState = GameState.GameOver;
            int playerFinal = PlayerScore.getValue();
            int computerFinal = OpponentScore.getValue();

            if (playerFinal > computerFinal)
            {
                Banner_YouWin.SetActive(true);
            }
            else if (computerFinal > playerFinal)
            {
                Banner_YouLose.SetActive(true);
            }
            else
            {
                Banner_Tie.SetActive(true);
            }
        }
        UpdateButtons();
    }


    void UpdateScore()
    {
        ClearMessages();

        switch (gameState)
        {
            case GameState.RoundEnd_YouLose:
                //Banner_YouLose.SetActive(true);
                OpponentScore.increment();
                break;
            case GameState.RoundEnd_YouWin:
                //Banner_YouWin.SetActive(true);
                PlayerScore.increment();
                break;
            case GameState.RoundEnd_Tie:
                //Banner_Tie.SetActive(true);
                break;
        }
    }
	
	// Update is called once per frame
	void Update ()
	{
        ResolveRound();
		UpdateButtons();
	}
	
    void Clear()
    {
        if (OpponentHand != null)
        {
            OpponentHand.gameObject.transform.position = Deck.transform.position;
            OpponentHand.gameObject.transform.rotation = Deck.transform.rotation;

            OpponentHand.gameObject.GetComponent<MeshRenderer>().enabled = false;
            Debug.Log(string.Format("Deactivated: {0}", 
                OpponentHand.gameObject.name));

        }

        if (PlayerHand != null)
        {
            PlayerHand.gameObject.transform.position = Deck.transform.position;
            PlayerHand.gameObject.transform.rotation = Deck.transform.rotation;

            PlayerHand.gameObject.GetComponent<MeshRenderer>().enabled = false;
            Debug.Log(string.Format("Deactivated: {0}",
                PlayerHand.gameObject.name));
        }
    }

    void OnDeckConsumed()
    {
        DummyCard.GetComponent<MeshRenderer>().enabled = false;
    }

    // DealComputer
    void DealComputer()
    {
        CardStruct pick = Deck.Pop();
        if (Deck.IsEmpty())
            OnDeckConsumed();

        if (pick != null)
        {
            string name = "PlayingCards_" + pick.Symbol + pick.Suit;
            GameObject newObj = GameObject.Find(name);
            if (newObj == null)
            {
                Debug.Log(string.Format("DealComputer() - Object Not Found: {0}", name));
            }

            Card newCard = newObj.AddComponent(typeof(Card)) as Card;
            newCard.Definition = pick;
            newObj.transform.parent = Deck.transform;
            OpponentHand = newCard;
            OpponentHand.gameObject.GetComponent<MeshRenderer>().enabled = true;

            newCard.SetFlyTarget(Transform_Deck.position,
                Transform_OpponentHand.position,
                CardFlipDuration);
        }
        else
        {
            OnDeckConsumed();
        }

    }

    // DealPlayer
    void DealPlayer()
	{
        CardStruct pick = Deck.Pop();
        if (Deck.IsEmpty())
            OnDeckConsumed();

        if (pick != null)
		{
            string name = "PlayingCards_" + pick.Symbol + pick.Suit;
            GameObject newObj = GameObject.Find(name);
            if (newObj == null)
            {
                Debug.Log(string.Format("DealPlayer() - Object Not Found: {0}", name));
            }

            Card newCard = newObj.AddComponent(typeof(Card)) as Card;
            newCard.Definition = pick;
            newObj.transform.parent = Deck.transform;
            PlayerHand = newCard;
            PlayerHand.gameObject.GetComponent<MeshRenderer>().enabled= true;

            newCard.SetFlyTarget(Transform_Deck.position,
                                Transform_PlayerHand.position,
                                CardFlipDuration);
		}
        else
        {
            OnDeckConsumed();
        }

    }

    static int Value(Card c)
	{
		return c.Definition.Weight;
	}
	
	static int GetScore(Card card)
	{
		return Value(card);
	}
	
	int GetDealerScore()
	{
		return GetScore(OpponentHand);
	}
	
	int GetPlayerScore()
	{
		return GetScore(PlayerHand);
	}
	
	public float DealDelay = 0.35f;

    IEnumerator OnDeal()
    {
        if (gameState != GameState.Resolving)
        {
            gameState = GameState.Resolving;
            ClearMessages();
            Clear();
            RoundCounter.increment();
            DealComputer();
            yield return new WaitForSeconds(DealDelay);
            DealPlayer();
            yield return new WaitForSeconds(DealDelay);
            gameState = GameState.Started;
        }
    }

    IEnumerator OnReset()
    {
        gameState = GameState.Invalid;
        UpdateButtons();
        Clear();
        ResetTable();
        yield return new WaitForSeconds(DealDelay);
    }

	void ResolveRound()
	{
		if (gameState == GameState.Started)
		{
			gameState = GameState.Resolving;
			UpdateButtons();
			int playerScore = GetPlayerScore();
			int dealerScore = GetDealerScore();
			Debug.Log(string.Format("Player={0}  Dealer={1}",playerScore, dealerScore));

            if (playerScore > dealerScore)
            {
                gameState = GameState.RoundEnd_YouWin;
            }
            else if (dealerScore > playerScore)
            {
                gameState = GameState.RoundEnd_YouLose;
            }
            else
            {
                gameState = GameState.RoundEnd_Tie;
            }
            UpdateScore();
            UpdateGameCount();
        }
	}

    public void OnButton(string msg)
	{
		Debug.Log(msg);
		switch (msg)
		{
		case "ClickedButtonDeal":
            if(canDeal())
            {
                StartCoroutine(OnDeal());
            }
            break;
		case "ClickedButtonReset":
            if (canReset())
            {
                StartCoroutine(OnReset());
            }
			break;
        }
    }
}
