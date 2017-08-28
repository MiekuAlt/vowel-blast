using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letter : MonoBehaviour {

    private string letter;
    public Vector2 slotID;
    public TextMesh text;
    public Sprite idle, selected;
    private bool isSelected;
    public bool correctLetter;

    public GameManager gm;
    public LetterBoard letterBoard;

    public float timer, popTime;
    public bool beingHeld;

    // Use this for initialization
    void Awake () {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        letterBoard = GameObject.FindGameObjectWithTag("LetterBoard").GetComponent<LetterBoard>();
        popTime = 5;
	}

    public void SetID(int xNewID, int yNewID)
    {
        slotID = new Vector2(xNewID, yNewID);
    }

    private void FixedUpdate()
    {
        if (beingHeld)
        {
            timer += Time.deltaTime;
            if (timer > popTime)
            {
                RemoveFromMap();
                letterBoard.UpdateMap();
                timer = 0f;
            }
        }
        else if (timer > 0)
        {
            timer = 0f;
        }
    }

    // This removes the letter from the map and gameobject array in the letterboard
    public void RemoveFromMap()
    {
        letterBoard.RemoveLetterFromMap(slotID);
        //letterBoard.ShiftDown();
        Destroy(gameObject);
    }

    // Updates the display of the letter and the outline
    public void UpdateDisplay(string let)
    {
        letter = let;
        text.text = "" + let;
    }

    // Getters and Setters
    public string getLetter()
    {
        return letter;
    }
    public void setLetter(string letter)
    {
        this.letter = letter;
    }

    // Triggers when it collides with the finger trail
    private void OnTriggerEnter2D(Collider2D collision)
    {
        beingHeld = true;
        if (!isSelected)
        {
            SelectLetter();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        beingHeld = false;
    }

    // Triggers when the letter is selected
    void SelectLetter()
    {
        
        gm.AddLetter(letter, gameObject);
        
    }

    public void ConfirmLetter()
    {
        isSelected = true;
        ChangeButLook("selected");
    }

    // Deselects the letter
    public void DeselectLetter()
    {
        isSelected = false;
        if(!correctLetter)
        {
            ChangeButLook("idle");
        }
    }

    // Changes the appearance of the button
    void ChangeButLook(string type)
    {
        if (type.Equals("selected"))
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = selected;
        }
        else if (type.Equals("idle"))
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = idle;
        }
    }
}
