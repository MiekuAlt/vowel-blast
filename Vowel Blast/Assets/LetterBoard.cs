using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterBoard : MonoBehaviour {

    public GameObject letterPrefab;
    private string[,] map;
    private GameObject[,] letters;
    private string availLetters = "BCDFGHJKLMNPQRSTVWXYZ";
    private float[] probabilities = { 3.34f, 7.32f, 5.46f, 2.92f, 3.98f, 4.84f, 0.32f, 1.77f, 8.85f, 4.86f, 10.73f, 5.11f, 0.32f, 12.23f, 9.25f, 11.21f, 1.62f, 2.08f, 0.47f, 2.07f, 0.44f };
    private Vector3 startPos = new Vector3(-2.4f, 3.5f, -2f);
    private float space = 0.8f;

	// Use this for initialization
	void Start () {
        map = new string[7,6];
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

        for (int r = 0; r < 6; r++)
        {
            for (int c = 0; c < 7; c++)
            {
                if (map[c, r] != null)
                {
                    GameObject newLetter = Instantiate(letterPrefab, pos, Quaternion.identity);
                    newLetter.transform.parent = gameObject.transform;
                    newLetter.GetComponent<Letter>().UpdateDisplay(map[c, r]);
                    newLetter.transform.position = new Vector3(pos.x, pos.y, pos.z);
                }

                pos.x = pos.x + space;
            }
            pos.x = startPos.x;
            pos.y = pos.y - space;
        }
    }


    // Fills the map with random letters
    void RandomizeMap()
    {
        for (int r = 0; r < 6; r++)
        {
            for (int c = 0; c < 7; c++)
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
        for (int r = 0; r < 6; r++)
        {
            for (int c = 0; c < 7; c++)
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
