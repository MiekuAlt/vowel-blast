using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterBoard : MonoBehaviour {

    public GameObject letterPrefab;
    private string[,] map;
    private GameObject[,] letters;
    private string availLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private float[] probabilities = { 8.50f, 2.07f, 4.54f, 3.38f, 11.16f, 1.81f, 2.47f, 3.00f, 7.54f, 0.20f, 1.10f, 5.49f, 3.01f, 6.65f, 7.16f, 3.17f, 0.20f, 7.58f, 5.74f, 6.95f, 3.63f, 1.01f, 1.29f, 0.29f, 1.78f, 0.27f };
    private Vector3 startPos = new Vector3(-2.4f, 2.2f, -1f);
    private float space = 0.8f;
    private int numRows = 8;
    private int numCols = 7;

	// Use this for initialization
	void Start () {
        map = new string[numCols, numRows];
        letters = new GameObject[numCols, numRows];
        RandomizeMap();
        DisplayMap();

        DebugMap();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void DisplayMap()
    {
        // The start position
        Vector3 pos = new Vector3(startPos.x, startPos.y, startPos.z);

        for (int r = 0; r < numRows; r++)
        {
            for (int c = 0; c < numCols; c++)
            {
                if (map[c, r] != null)
                {
                    GameObject newLetter = Instantiate(letterPrefab, pos, Quaternion.identity);
                    newLetter.transform.parent = gameObject.transform;
                    letters[c, r] = newLetter;
                    newLetter.GetComponent<Letter>().SetID(c, r);
                    newLetter.GetComponent<Letter>().UpdateDisplay(map[c, r]);
                    newLetter.transform.position = new Vector3(pos.x, pos.y, pos.z);
                }

                pos.x = pos.x + space;
            }
            pos.x = startPos.x;
            pos.y = pos.y - space;
        }
    }

    void UpdateMap()
    {
        // Clear the letters
        for (int r = 0; r < numRows; r++)
        {
            for (int c = 0; c < numCols; c++)
            {
                Destroy(letters[c, r]);
            }
        }

        // Replace the lost letters
        ReplaceLetters();

        // Re-display the letters
        DisplayMap();
    }

    void ReplaceLetters()
    {
        for (int r = 0; r < numRows; r++)
        {
            for (int c = 0; c < numCols; c++)
            {
                if (map[c, r] == null)
                {
                    map[c, r] = RandomLetter();
                }
            }
        }
    }

    // This shifts all the letters down to fill any open spaces
    private void ShiftDown()
    {
        for (int r = numRows - 1; r >= 0; r--)
        {
            for (int c = numCols - 1; c >= 0; c--)
            {
                if(r == 0) // Ignore the top row for now...
                {
                    
                } else // All of the rest of the rows
                {
                    if(map[c, r] == null) // If the letter is missing...
                    {
                        Debug.Log("C: " + c + " R: " + r);
                        string letterStore = "";
                        for (int i = r; i >= 0; i--) // Tracking and clearing all the letters above it
                        {
                            if(map[c, r - i] != null)
                            {
                                letterStore += map[c, r - i];
                                map[c, r - i] = null;
                            }
                        }

                        // Reversing the letters
                        char[] charArray = letterStore.ToCharArray();
                        System.Array.Reverse(charArray);
                        letterStore = new string (charArray);

                        for (int i = letterStore.Length - 1; i >= 0; i--) // Inserting the stored letters
                        {
                            map[c, r - i] = letterStore.Substring(i, 1);
                        }
                    }
                }
            }
        }
        UpdateMap();
    }

    public void RemoveLetterFromMap(Vector2 location)
    {
        map[(int)location.x, (int)location.y] = null;
        letters[(int)location.x, (int)location.y] = null;

        DebugMap();
        ShiftDown();
        DebugMap();
    }


    // Fills the map with random letters
    void RandomizeMap()
    {
        for (int r = 0; r < numRows; r++)
        {
            for (int c = 0; c < numCols; c++)
            {
                map[c, r] = RandomLetter();
            }
        }
    }

    private string RandomLetter()
    {
        float percent = Random.Range(0.0f, 100.0f);

        int index = 0;
        float curValue = 0.0f;

        for (int i = 0; i < probabilities.Length; i++)
        {
            curValue += probabilities[i];
            if (percent < curValue)
            {
                index = i;
                break;
            }
        }

        return availLetters.Substring(index, 1);
    }

    // Used for debugging
    void DebugMap()
    {
        string line = "";
        for (int r = 0; r < numRows; r++)
        {
            for (int c = 0; c < numCols; c++)
            {
                if (map[c, r] == null)
                {
                    line += "[  ]";
                }
                else
                {
                    line += "[" + map[c, r] + "]";
                }

            }
            line += "\n";
        }
        Debug.Log(line);
    }
}
