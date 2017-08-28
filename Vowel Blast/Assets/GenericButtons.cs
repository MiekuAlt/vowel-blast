using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericButtons : MonoBehaviour {

    public string buttonType;
    private GameManager gm;
    

	// Use this for initialization
	void Start () {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
	}

    // Triggers when the mouse or touch leaves the input collider
    private void OnMouseUp()
    {
        DetermineAction();
    }

    void DetermineAction()
    {
        if (buttonType.Equals("pause"))
        {
            PauseGame();
        } else if (buttonType.Equals("help"))
        {
            HelpPlayer();
        } else if (buttonType.Equals("resume"))
        {
            ResumeGame();
        } else
        {
            Debug.Log("Unknown Button Type: " + buttonType);
        }
    }

    void PauseGame()
    {
        gm.PauseGame();
    }
    void ResumeGame()
    {
        gm.ResumeGame();
    }

    void HelpPlayer()
    {
        Debug.Log("Helping Player");
    }
}
