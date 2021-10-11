using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private bool tileRevelada = false; //indicador da carta virada ou não
    public Sprite originalCarta;
    public Sprite backCarta;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        EscondeCarta();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMouseDown()
    {
        print("Voce pressionou uma tile");
        if (tileRevelada)
        {
            EscondeCarta();
        }
        else
        {
            RevelaCarta();
        }
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
}
