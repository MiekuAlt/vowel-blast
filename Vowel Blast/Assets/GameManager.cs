using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour {

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

    public int level;

    public PossibleWords hintDisplay;

    // Use this for initialization
    void Awake () {
        points = 0f;
        timeLeft = roundTime;
        UpdatePoints();

        map = ImportLevel(level);

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
        bool result = false;
        
        for(int i = 0; i < correctWords.Count; i++)
        {
            if(userWord.Equals(correctWords[i]))
            {
                result = true;
                break;
            }
        }

        if (result)
        {
            for (int i = 0; i < wordLetters.Count(); i++)
            {
                wordLetters[i].GetComponent<Letter>().correctLetter = true;
            }

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
        hintDisplay.CheckOff(userWord);
    }

    // Triggers when the word doesn't exist
    void WordFail()
    {
        Debug.Log("Fail!");
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

    // Reads the XML and generates a map based on the XML's data
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

    public string[,] GetMap()
    {
        return map;
    }
    public int GetRows()
    {
        return rows;
    }
    public int GetCols()
    {
        return columns;
    }
} // end of the GameManager class
