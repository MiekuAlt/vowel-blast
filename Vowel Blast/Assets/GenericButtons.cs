using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericButtons : MonoBehaviour {

    public string buttonType;

    

	// Use this for initialization
	void Start () {
		
	}

    // Triggers when the mouse or touch leaves the input collider
    private void OnMouseUp()
    {
        DetermineAction();
    }

    void DetermineAction()
    {
        if(buttonType.Equals("pause"))
        {
            PauseGame();
        } else if(buttonType.Equals("help"))
        {
            HelpPlayer();
        } else
        {
            Debug.Log("Unknown Button Type: " + buttonType);
        }
    }

    void PauseGame()
    {
        Debug.Log("Pausing");
    }

    void HelpPlayer()
    {
        Debug.Log("Helping Player");
    }
}
