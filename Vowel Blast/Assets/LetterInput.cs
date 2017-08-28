using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterInput : MonoBehaviour {

    public GameManager gm;

    // The trail
    public GameObject trail;

    // Use this for initialization
    void Start () {

	}

    // Triggers when the mouse or touch is over the input collider
    private void OnMouseOver()
    {
        trail.SetActive(true);
        // Creates the touch trail
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {

            Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 8f));
            trail.transform.position = pos;
        }
    }

    // Triggers when the mouse or touch leaves the input collider
    private void OnMouseUp()
    {
        trail.GetComponent<TrailRenderer>().Clear();
        trail.SetActive(false);
        trail.transform.position = new Vector3(-4f, 0f, 0f);
        gm.Validate();
        DeselectAllLetters();
    }

    // Deselects all the letters
    private void DeselectAllLetters()
    {
        foreach (GameObject letterBut in GameObject.FindGameObjectsWithTag("LetterButton"))
        {
            letterBut.GetComponent<Letter>().DeselectLetter();
        }
    }


} // end of the LetterInput class
