using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public char vowel;
    private char[] possibleVowels = { 'A', 'E', 'I', 'O', 'U', 'Y' };

	// Use this for initialization
	void Start () {
        vowel = RandomVowel();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public char GetVowel()
    {
        return vowel;
    }

    private char RandomVowel()
    {
        int index = Random.Range(0, possibleVowels.Length);
        return possibleVowels[index];
    }
}
