using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public int userPoints;
    public float userTimer;

    private GameDictionary dict;

    public string userWord;

	// Use this for initialization
	void Start () {
        dict = gameObject.GetComponent<GameDictionary>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Triggered when letters are selected and sends what they are to here
    public void AddLetter(string letter)
    {
        userWord += letter;
    }

    public void RemoveLastLetter()
    {
        userWord = userWord.Remove(userWord.Length);
    }

    // Once the user is done, this validate request is called
    public bool Validate()
    {
        bool result;
        result = dict.CheckWord(userWord);
        userWord = "";

        Debug.Log("Word is: " + result);

        return result;
    }
} // end of the GameManager class
