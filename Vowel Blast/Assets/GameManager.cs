using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
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

    private int rows, columns;
    private string[,] map;
    public TextAsset levelsXML;
    public List<string> correctWords;

    // Use this for initialization
    void Start () {
        dict = gameObject.GetComponent<GameDictionary>();
        points = 0f;
        timeLeft = roundTime;
        UpdatePoints();

        map = ImportLevel(0);
        DebugMap();
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

    void DebugMap()
    {
        string line = "";
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                if (map[r, c] == null)
                {
                    line += "[ ]";
                }
                else
                {
                    line += "[" + map[r, c] + "]";
                }
            }
            line += '\n';
        }

        Debug.Log(line);
    }

    string[,] ImportLevel(int levelNum)
    {
        var levelDoc = XDocument.Parse(levelsXML.text).Element("document").Elements("level").ElementAt(levelNum);
        correctWords = new List<string>();
        // Collecting the words
        var words = levelDoc.Elements("word");
        for (int i = 0; i < words.Count(); i++)
        {
            XElement element = words.ElementAt(i);
            string temp = element.ToString().Replace("<word>", "").Replace("</word>", "");
            correctWords.Add(temp);
        }

        // Collecting the level map
        var levelMap = levelDoc.Element("map");
        rows = int.Parse(levelMap.Attribute("rows").Value);
        columns = int.Parse(levelMap.Attribute("cols").Value);
        string[,] tempMap = new string[rows, columns];
        var mapRows = levelMap.Elements("row");
        for(int r = 0; r < mapRows.Count(); r++)
        {
            XElement element = mapRows.ElementAt(r);
            string tempRow = element.ToString().Replace("<row>", "").Replace("</row>", "");

            for(int c = 0; c < tempRow.Length; c++)
            {
                tempMap[r, c] = "" + tempRow.Substring(c, 1);
            }
        }

        return tempMap;
    }
} // end of the GameManager class
