using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private bool tileRevelada = false; //indicador da carta virada ou não
    public Sprite originalCarta; //Face da carta
    public Sprite backCarta; //Parte de tras da carta
    private SpriteRenderer spriteRenderer; //Referencia para o sprite renderer para mudar os sprites
    public int cartaID; //Mesma coisa que rank
    public int linhaID; //Mesma coisa que tag
    internal string naipe; //Guarda o naipe em string
    private static ManageCartas manager; //referencia ao game manager

    // Start is called before the first frame update
    void Start()
    {
        /*
         * condicao inial das cartas e set de referencias
         */
        spriteRenderer = GetComponent<SpriteRenderer>();
        manager = GetComponentInParent<ManageCartas>();
        EscondeCarta();
    }

    public void OnMouseDown()
    {
        /*
         * Ao clicar na carta o metodo de carta selecionada no manager e chamado
         */
        print("Voce pressionou uma tile");

        manager.CartaSelecionada(gameObject);
    }

    public void EscondeCarta()
    {
        /*
         * Update do sprite para a carta virada para baixo
         */
        spriteRenderer.sprite = backCarta;
        tileRevelada = false;
    }

    public void RevelaCarta()
    {
        /*
         * Update do sprite para a carta virada para cima
         */
        spriteRenderer.sprite = originalCarta;
        tileRevelada = true;
    }

    public void setOriginalSprite(Sprite sprite)
    {
        /*
         * Update para um novo sprite da face
         */
        originalCarta = sprite;
    }

    internal void setBackSprite(Sprite sprite)
    {
        /*
         * Update para um novo sprite da parte de tras
         */
        backCarta = sprite;
    }
}
