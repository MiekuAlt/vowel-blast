using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public int userPoints;
    public float userTimer;

    private GameDictionary dict;

    public string userWord;
    public List<GameObject> wordLetters;

    // Use this for initialization
    void Start () {
        dict = gameObject.GetComponent<GameDictionary>();
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
        userWord = "";

        if (result)
        {
            WordSuccess();
        } else
        {
            WordFail();
        }

        // Clearing out the stored gameobjects
        wordLetters = new List<GameObject>();
    }

    // Triggers when the word is successful
    void WordSuccess()
    {
        Debug.Log("Success!");
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
} // end of the GameManager class
