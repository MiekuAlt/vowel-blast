using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private GameDictionary dict;

    public string userWord;
    public List<GameObject> wordLetters;

    //public TextMesh pointsDisplay;
    private float points;
    private float timeLeft;
    public float roundPoints;
    public float roundTime;

    // GUI Bars
    public GameObject healthBar;
    public GameObject powerBar;

    // The displayed letters for the user
    public TextMesh displayedLetters;

    // Use this for initialization
    void Start () {
        dict = gameObject.GetComponent<GameDictionary>();
        points = 0f;
        timeLeft = roundTime;
        UpdatePoints();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if(timeLeft < 0)
        {
            TimesUp();
        } else
        {
            timeLeft -= Time.deltaTime;
            UpdateHealthBar(timeLeft / roundTime);
        }
	}

    // Triggered when letters are selected and sends what they are to here
    public void AddLetter(string letter, GameObject letterObj)
    {
        wordLetters.Add(letterObj);
        userWord += letter;
        displayedLetters.text = userWord;
    }

    public void RemoveLastLetter()
    {
        userWord = userWord.Remove(userWord.Length);
        displayedLetters.text = userWord;
    }

    // Once the user is done, this validate request is called
    public void Validate()
    {
        bool result;
        result = dict.CheckWord(userWord);

        if (result)
        {
            WordSuccess(userWord.Length);
        } else
        {
            WordFail();
        }

        userWord = "";
        displayedLetters.text = userWord;

        // Clearing out the stored gameobjects
        wordLetters = new List<GameObject>();
    }

    // Triggers when the word is successful
    void WordSuccess(int numLetters)
    {
        AddPoints(numLetters);
        RemoveWord();
    }

    // Triggers when the word doesn't exist
    void WordFail()
    {
        Debug.Log("Fail!");
    }

    // Removes all the selected letters
    void RemoveWord()
    {
        for(int i = 0; i < wordLetters.Count; i++)
        {
            if (wordLetters[i] != null)
            {
                wordLetters[i].GetComponent<Letter>().RemoveFromMap();
            }
        }
        LetterBoard lb = GameObject.FindGameObjectWithTag("LetterBoard").GetComponent<LetterBoard>();
        lb.UpdateMap();
    }

    void AddPoints(float pointsToAdd)
    {
        points += pointsToAdd;
        UpdatePoints();

        if(points >= roundPoints)
        {
            RoundWin();
        }
    }

    // Updates what is displayed to the user
    void UpdatePoints()
    {
        UpdatePowerBar(points/roundPoints);
    }

    // Changes the display of the health bar to show the percentage left
    private void UpdateHealthBar(float percent)
    {
        healthBar.transform.localScale = new Vector3(percent, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
    }

    // Changes the display of the power bar to show the percentage left
    private void UpdatePowerBar(float percent)
    {
        if(percent > 1f)
        {
            percent = 1f;
        }

        powerBar.transform.localScale = new Vector3(percent, powerBar.transform.localScale.y, powerBar.transform.localScale.z);
    }

    // This is triggered when time is up
    private void TimesUp()
    {
        Debug.Log("Time's up!");
    }

    // This is triggered when the user gets enough points to win the round
    private void RoundWin()
    {
        Debug.Log("You Win!");
    }
} // end of the GameManager class
