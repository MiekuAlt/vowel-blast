using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameDictionary : MonoBehaviour {

    private string _alphabet = "abcdefghijklmnopqrstwuvxyz";
    private Dictionary<char, List<string>> _allWordsTree = new Dictionary<char, List<string>>();

    public TextAsset allWordsDoc;

    // Use this for initialization
    void Start()
    {
        CreateAllWordsTree();
    }

    // Checks if the word exists
    public bool CheckWord(string userWord)
    {
        userWord = userWord.ToLower();
        return _allWordsTree[userWord[0]].Contains(userWord);
    }



    // Creates a tree of all the words
    void CreateAllWordsTree()
    {
        foreach (char __letter in _alphabet)
        {
            _allWordsTree.Add(__letter, new List<string>());
        }

        foreach (string __word in allWordsDoc.text.Split("\n"[0]))
        {
            string __lowerCaseWord = __word.ToLower().TrimEnd();

            if (__lowerCaseWord.Length >= 1 && __lowerCaseWord.Length <= 6)
            {
                _allWordsTree[__lowerCaseWord[0]].Add(__lowerCaseWord);
            }
        }
    }

} // end of the GameDictionary class
