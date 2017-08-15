using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letter : MonoBehaviour {

    private string letter;
    public TextMesh text, outline;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Updates the display of the letter and the outline
    public void UpdateDisplay(string let)
    {
        letter = let;
        text.text = "" + let;
        outline.text = "" + let;
    }
}
