using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public char vowel;
    private char[] possibleVowels = { 'A', 'E', 'I', 'O', 'U', 'Y' };
    private float[] probabilities = {21.36f, 28.06f, 18.97f, 18.01f, 9.13f, 4.47f};

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

    // The randomness is weighed based of the frequency of letter usage
    private char RandomVowel()
    {
        float percent = Random.Range(0.0f, 100.0f);

        int index = 0;
        float curValue = 0.0f;

        for(int i = 0; i < probabilities.Length; i++)
        {
            curValue += probabilities[i];
            if(percent < curValue)
            {
                index = i;
                break;
            }
        }

        return possibleVowels[index];
    }
}
