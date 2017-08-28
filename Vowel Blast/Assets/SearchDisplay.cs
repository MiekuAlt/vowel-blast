using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchDisplay : MonoBehaviour
{

    public GameObject letterPrefab;
    public GameManager gm;
    private string[,] map;
    private float space = 0.8f;
    private GameObject[,] letters;
    private int numCols, numRows;

    public GameObject textBG;

    // Use this for initialization
    void Start()
    {
        map = gm.GetMap();
        numCols = gm.GetCols();
        numRows = gm.GetRows();
        letters = new GameObject[numCols, numRows];

        DebugMap();
        DisplayMap();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void DisplayMap()
    {
        // The start position
        Vector3 pos = DetermineStartPos();
        DetermineBGSize();

        for (int r = 0; r < numRows; r++)
        {
            for (int c = 0; c < numCols; c++)
            {
                if (map[r, c] != null)
                {
                    GameObject newLetter = Instantiate(letterPrefab, pos, Quaternion.identity);
                    newLetter.transform.parent = gameObject.transform;
                    letters[r, c] = newLetter;
                    newLetter.GetComponent<Letter>().SetID(c, r);
                    newLetter.GetComponent<Letter>().UpdateDisplay(map[r, c]);
                    newLetter.transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
                }

                pos.x = pos.x + space;
            }
            pos.x = DetermineStartPos().x;
            pos.y = pos.y - space;
        }
    }

    // Figures out the start position based on number of rows and columns
    Vector3 DetermineStartPos()
    {
        float x, y;
        x = ((numCols - 1) * space) / 2 * -1;
        y = ((numRows - 1) * -1 * space) / 2 * -1;
        Vector3 result = new Vector3(x, y, -1f);

        return result;
    }

    // Determines the text's bg's size
    void DetermineBGSize()
    {
        textBG.transform.localScale = new Vector3 (numCols, numRows, 1);
    }

    void DebugMap()
    {
        string line = "";
        for (int r = 0; r < numRows; r++)
        {
            for (int c = 0; c < numCols; c++)
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
}
