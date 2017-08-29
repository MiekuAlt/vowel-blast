using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public string userWord;
    public List<GameObject> wordLetters;
    public string swipeDir;

    // The displayed letters for the user
    public TextMesh displayedLetters;

    private int rows, columns;
    private string[,] map;
    public TextAsset levelsXML;
    public List<string> correctWords;

    public int level;
    public float maxTime, timer;

    public PossibleWords hintDisplay;

    public Camera mainCam;
    private bool paused;
    public int numStars, totalStars;
    public TextMesh starCounter;

    public GameObject starBar, star1, star2, star3;
    public GameObject finStar1, finStar2, finStar3;

    // Use this for initialization
    void Awake () {
        Persistence.Load();
        totalStars = GameData.numStars;
        numStars = 3;
        map = ImportLevel(level);
        paused = false;
        timer = maxTime;

	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if(!paused)
        {
            timer -= Time.deltaTime;
            UpdateStarBar(timer / maxTime);
            if(timer <= maxTime/3f * 2f && star1 != null)
            {
                Destroy(star1);
                Destroy(finStar1);
                numStars = 2;
            } else if (timer <= maxTime / 3f && star2 != null)
            {
                Destroy(star2);
                Destroy(finStar2);
                numStars = 1;
            } else if(timer <= 0f && star3 != null)
            {
                Destroy(star3);
                Destroy(finStar3);
                numStars = 0;
            }
        }
	}

    void UpdateStarBar(float percent)
    {
        if (percent < 0 == false)
        {
            starBar.transform.localScale = new Vector3(percent, starBar.transform.localScale.y, 1f);
        }
    }

    // Triggered when letters are selected and sends what they are to here
    public void AddLetter(string letter, GameObject letterObj)
    {

        if(CheckInput(letterObj))
        {
            wordLetters.Add(letterObj);
            userWord += letter;
            displayedLetters.text = userWord;
            letterObj.GetComponent<Letter>().ConfirmLetter();
        }
        
    }

    // Checks if the user is allowed to select this letter
    private bool CheckInput(GameObject letterCheck)
    {
        bool result = false;

        // First 2 letters are always fine
        if(wordLetters.Count() < 2)
        {
            result = true;
            // Determining direction
            if(wordLetters.Count() == 1)
            {
                DetermineDir(wordLetters[0], letterCheck);
            }
        } else
        {
            float x1, x2, y1, y2;
            x1 = wordLetters[wordLetters.Count() - 1].GetComponent<Letter>().slotID.x;
            y1 = wordLetters[wordLetters.Count() - 1].GetComponent<Letter>().slotID.y;
            x2 = letterCheck.GetComponent<Letter>().slotID.x;
            y2 = letterCheck.GetComponent<Letter>().slotID.y;

            if(x2 == x1 + 1 && y2 == y1 && swipeDir.Equals("horRight"))
            {
                result = true;
            }
            else if (x2 + 1 == x1 && y2 == y1 && swipeDir.Equals("horLeft"))
            {
                result = true;
            }
            else if (x2 == x1 && y2 + 1 == y1 && swipeDir.Equals("up"))
            {
                result = true;
            }
            else if (x2 == x1 && y2 == y1 + 1 && swipeDir.Equals("down"))
            {
                result = true;
            }
            else if (x2 == x1 + 1 && y2 + 1 == y1 && swipeDir.Equals("upRight"))
            {
                result = true;
            }
            else if (x2 == x1 + 1 && y2 == y1 + 1 && swipeDir.Equals("downRight"))
            {
                result = true;
            }
            else if (x2 + 1 == x1 && y2 + 1 == y1 && swipeDir.Equals("upLeft"))
            {
                result = true;
            }
            else if (x2 + 1 == x1 && y2 == y1 + 1 && swipeDir.Equals("downLeft"))
            {
                result = true;
            }
        }


        return result;
    }

    private void DetermineDir(GameObject letter1, GameObject letter2)
    {
        Vector2 firstID = letter1.GetComponent<Letter>().slotID;
        Vector2 secondID = letter2.GetComponent<Letter>().slotID;
        float x1, x2, y1, y2;
        x1 = firstID.x;
        y1 = firstID.y;
        x2 = secondID.x;
        y2 = secondID.y;

        if(x2 > x1 && y2 == y1)
        {
            swipeDir = "horRight";
        } else if (x2 < x1 && y2 == y1)
        {
            swipeDir = "horLeft";
        } else if (x2 == x1 && y2 < y1)
        {
            swipeDir = "up";
        } else if (x2 == x1 && y2 > y1)
        {
            swipeDir = "down";
        } else if (x2 > x1 && y2 < y1)
        {
            swipeDir = "upRight";
        } else if (x2 > x1 && y2 > y1)
        {
            swipeDir = "downRight";
        } else if (x2 < x1 && y2 < y1)
        {
            swipeDir = "upLeft";
        } else if (x2 < x1 && y2 > y1)
        {
            swipeDir = "downLeft";
        } else
        {
            swipeDir = "ERROR!";
        }
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
                correctWords.RemoveAt(i);
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
        swipeDir = "";
        displayedLetters.text = userWord;

        

        // Clearing out the stored gameobjects
        wordLetters = new List<GameObject>();
    }

    // Triggers when the word is successful
    void WordSuccess(int numLetters)
    {
        hintDisplay.CheckOff(userWord);
        if(correctWords.Count() == 0)
        {
            LevelWin();
        }
    }

    void LevelWin()
    {
        paused = true;
        totalStars += numStars;
        GameData.numStars = totalStars;
        starCounter.text = "" + totalStars;
        Persistence.Save();
        mainCam.transform.position = new Vector3(20f, 0f, -10f);
    }

    // Triggers when the word doesn't exist
    void WordFail()
    {
        
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
            temp = temp.ToUpper();
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
            tempRow = tempRow.ToUpper();

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

    public void PauseGame()
    {
        paused = true;
        mainCam.transform.position = new Vector3(10f, 0f, -10f);
    }
    public void ResumeGame()
    {
        mainCam.transform.position = new Vector3(0f, 0f, -10f);
        paused = false;
    }

} // end of the GameManager class
