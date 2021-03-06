﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour {

    [SerializeField]
    public GameObject cardTras;
    [SerializeField]
    public GameManager gameManager;

    public void OnMouseDown()
    {
        if(cardTras.activeSelf && gameManager.podeRevelar)
        {
            revelaCard(cardTras);
            gameManager.CardRevelado(this);
        }
        gameObject.GetComponent<AudioSource>().Play();
    }

    private int _id;
    public int id
    {
        get { return _id; }
    }

    public void MudaSprite(int id, Sprite image)
    {
        _id = id;
        GetComponent<SpriteRenderer>().sprite = image;
    }
    public void voltaTras()
    {
        cardTras.SetActive(true);
    }
    public void revelaCard(GameObject card){
        card.SetActive(false);

    }

}
