using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PossibleWords : MonoBehaviour {

    public List<string> words;
    public GameObject textSlot;
    public GameObject[] wordHints;
    public GameManager gm;

    public float columnSpace;
    public float rowSpace;

    // Use this for initialization
    void Start()
    {
        words = gm.correctWords;
        wordHints = new GameObject[words.Count];
        PlaceAllWords(words);
    }

    List<string> SortByLength(List<string> stringList)
    {
        List<string> sortedStringList = stringList
            .OrderBy(n => n.Length)
            .ToList();

        sortedStringList.Reverse();

        return sortedStringList;
    }

    // Adds all the words in the words list
    void PlaceAllWords(List<string> wordsToPlace)
    {
        int numPlaced = 0;
        for (int i = 0; i < wordsToPlace.Count(); i++)
        {
            GameObject newWord = Instantiate(textSlot, gameObject.transform.position, Quaternion.identity);
            newWord.transform.parent = gameObject.transform;
            newWord.GetComponent<TextMesh>().text = wordsToPlace[i];
            wordHints[i] = newWord;

            float x = 0;
            float y = 0;

            if(i%2 == 0)
            {
                x = columnSpace * -1;
            } else
            {
                x = columnSpace;
            }


            y = numPlaced / 2 * rowSpace * -1;

            newWord.transform.localPosition = new Vector3(x, y, -1f);
            numPlaced++;
        }
    }

    public void CheckOff(string userWord)
    {
        for(int i = 0; i < wordHints.Count(); i++)
        {
            if(wordHints[i].GetComponent<TextMesh>().text.Equals(userWord))
            {
                wordHints[i].GetComponent<TextMesh>().color = Color.red;
            }
        }
    }

}
