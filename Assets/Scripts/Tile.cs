using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private bool tileRevelada = false; //indicador da carta virada ou não
    public Sprite originalCarta;
    public Sprite backCarta;
    private SpriteRenderer spriteRenderer;
    public int cartaID;
    public int linhaID;
    internal string naipe;
    private static ManageCartas manager;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        manager = GetComponentInParent<ManageCartas>();
        EscondeCarta();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMouseDown()
    {
        print("Voce pressionou uma tile");
        /*if (tileRevelada)
        {
            EscondeCarta();
        }
        else
        {
            RevelaCarta();
        }*/

        manager.CartaSelecionada(gameObject);
    }

    public void EscondeCarta()
    {
        spriteRenderer.sprite = backCarta;
        tileRevelada = false;
    }

    public void RevelaCarta()
    {
        spriteRenderer.sprite = originalCarta;
        tileRevelada = true;
    }

    public void setOriginalSprite(Sprite sprite)
    {
        originalCarta = sprite;
    }

    internal void setBackSprite(Sprite sprite)
    {
        backCarta = sprite;
    }
}
