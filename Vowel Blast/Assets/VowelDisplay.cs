using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VowelDisplay : MonoBehaviour {

    private char vowel;
    public GameManager gm;
    public TextMesh letter, outline;

    // Use this for initialization
    void Start () {
        vowel = gm.GetVowel();
        UpdateDisplay();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Updates the display of the letter and the outline
    public void UpdateDisplay()
    {
        letter.text = "" + vowel;
        outline.text = "" + vowel;
    }
}
