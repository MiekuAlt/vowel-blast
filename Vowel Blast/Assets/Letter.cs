﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letter : MonoBehaviour {

    private string letter;
    public TextMesh text, outline;
    public Sprite idle, selected;
    private Color idleShade = new Color(0, 175, 220);
    private Color selectedShade = new Color(113, 196, 9);
    private bool isSelected;

    public float timer, popTime;
    public bool beingHeld;

    // Use this for initialization
    void Start () {
        popTime = 5;
	}

    private void FixedUpdate()
    {
        if (beingHeld)
        {
            timer += Time.deltaTime;
            if (timer > popTime)
            {
                Debug.Log("Pop!");
                timer = 0f;
            }
        }
        else if (timer > 0)
        {
            timer = 0f;
        }
    }

    // Updates the display of the letter and the outline
    public void UpdateDisplay(string let)
    {
        letter = let;
        text.text = "" + let;
        outline.text = "" + let;
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
        isSelected = true;
        ChangeButLook("selected");
    }

    // Deselects the letter
    public void DeselectLetter()
    {
        isSelected = false;
        ChangeButLook("idle");
    }

    // Changes the appearance of the button
    void ChangeButLook(string type)
    {
        if (type.Equals("selected"))
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = selected;
            outline.GetComponent<TextMesh>().color = selectedShade;
        }
        else if (type.Equals("idle"))
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = idle;
            outline.GetComponent<TextMesh>().color = idleShade;
        }
    }
}
