using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour
{
    public string Name;
    public int UpperLimit;

    private string displayText;
    private TextMesh testMesh;
    private int score = 0;

    // Use this for initialization
    void Start()
    {
        reset();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void reset()
    {
        score = 0;
        display();
    }

    public void increment()
    {
        score++;
        display();
    }

    void display()
    {
        displayText = Name + ": " + score.ToString();
        testMesh = GetComponent<TextMesh>();//
        testMesh.text = displayText;
    }

    public bool isMaxed()
    {
        return score >= UpperLimit;
    }

    public int getValue()
    {
        return score;
    }

}
