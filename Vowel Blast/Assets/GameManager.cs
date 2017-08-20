using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public int userPoints;
    public float userTimer;

    private GameDictionary dict;

    public string userWord;
    public List<GameObject> wordLetters;

    public TextMesh pointsDisplay;
    private int points;

    // Use this for initialization
    void Start () {
        dict = gameObject.GetComponent<GameDictionary>();
        points = 0;
        UpdatePoints();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Triggered when letters are selected and sends what they are to here
    public void AddLetter(string letter, GameObject letterObj)
    {
        wordLetters.Add(letterObj);
        userWord += letter;
    }

    public void RemoveLastLetter()
    {
        userWord = userWord.Remove(userWord.Length);
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

    void AddPoints(int pointsToAdd)
    {
        points += pointsToAdd;
        UpdatePoints();
    }

    // Updates what is displayed to the user
    void UpdatePoints()
    {
        pointsDisplay.text = "" + points;
    }
} // end of the GameManager class
