using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ManageCartas : MonoBehaviour
{
    public GameObject cartaPrefab;
    public GameObject[,] cartasArray;
    public int numeroCartas;
    public int numeroLinhas;
    int gameMode;

    private bool primeiraCartaSelecionada, segundaCartaSelecionada;
    private GameObject carta1, carta2;
    private string linhaCarta1, linhaCarta2;

    bool timerPausado, timerAcionado;
    public float timer;
    public int numTentativas = 0;
    public int numAcertos = 0;

    // Start is called before the first frame update
    void Start()
    {
        gameMode = PlayerPrefs.GetInt("gamemode");
        InicializarCartas();
    }

    void InicializarCartas()
    {
        GameObject centro = GameObject.Find("centroDaTela");
        cartasArray = new GameObject[numeroLinhas, numeroCartas];
        
        for (int indiceVertical = 0; indiceVertical < numeroLinhas; indiceVertical++)
        {
            int[] arrayEmbaralhado = criaArrayEmbaralhado();
            for (int indiceHorizontal = 0; indiceHorizontal < numeroCartas; indiceHorizontal++)
            {

                float escalaCartaOriginal = cartaPrefab.transform.localScale.x;

                float fatorEscalaX = (650*escalaCartaOriginal)/ 110f;
                float fatorEscalaY = (945*escalaCartaOriginal)/ 110f;

                var novaPosicao = new Vector3(centro.transform.position.x + ((indiceHorizontal - ((numeroCartas-1) / 2f)) * fatorEscalaX),
                                              centro.transform.position.y + ((indiceVertical - ((numeroLinhas - 1) / 2f)) * fatorEscalaY),
                                              centro.transform.position.z);

                var rank = arrayEmbaralhado[indiceHorizontal];
                GameObject carta = CriarCarta(rank, indiceVertical, novaPosicao);

                cartasArray[indiceVertical, indiceHorizontal] = carta;
            }
        }
    }

    private GameObject CriarCarta(int rank, int indiceVertical, Vector3 novaPosicao)
    {
        
        var carta = Instantiate(cartaPrefab, novaPosicao, Quaternion.identity, transform);

        carta.GetComponent<Tile>().cartaID = rank;
        carta.GetComponent<Tile>().linhaID = indiceVertical;
        carta.tag = "" + rank;
        carta.name = indiceVertical + "_" + rank;
        string nomeDaCarta = "";
        string numeroCarta = "";
        if (rank == 0)
            numeroCarta = "ace";
        else if (rank == 10)
            numeroCarta = "jack";
        else if (rank == 11)
            numeroCarta = "queen";
        else if (rank == 12)
            numeroCarta = "king";
        else
            numeroCarta = "" + (rank + 1);

        switch (gameMode)
        {
            case 0:
                nomeDaCarta = numeroCarta + (indiceVertical%2 == 0 ? "_of_clubs" : "_of_spades");
                break;
            case 1:
                nomeDaCarta = numeroCarta + (indiceVertical%2 == 0 ? "_of_diamonds" : "_of_hearts");
                break;
            case 2:
                nomeDaCarta = numeroCarta + (indiceVertical%2 == 0 ? "_of_clubs" : "_of_spades");
                break;

        }
            

        Sprite s1 = Resources.Load<Sprite>("Sprites/cartas/" + nomeDaCarta);


        carta.GetComponent<Tile>().setOriginalSprite(s1);
        return carta;
    }

    public int[] criaArrayEmbaralhado()
    {
        int[] novaArray = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        int temp;
        for (int i = 0; i < 13; i++)
        {
            temp = novaArray[i];
            int r = Random.Range(temp, 13);
            novaArray[i] = novaArray[r];
            novaArray[r] = temp;
        }
        return novaArray;
    }

    public void CartaSelecionada(GameObject carta)
    {
        if (!primeiraCartaSelecionada)
        {
            string linha = carta.name.Substring(0, 1);
            linhaCarta1 = linha;
            primeiraCartaSelecionada = true;
            carta1 = carta;
            carta1.GetComponent<Tile>().RevelaCarta();
        }else if(primeiraCartaSelecionada && !segundaCartaSelecionada)
        {
            string linha = carta.name.Substring(0, 1);
            linhaCarta2 = linha;
            segundaCartaSelecionada = true;
            carta2 = carta;
            carta2.GetComponent<Tile>().RevelaCarta();
            VerificaCartas();
        }
    }

    void VerificaCartas()
    {
        DisparaTimer();
        
    }

    private void UpdateTentativas()
    {
        numTentativas++;
        GameObject.Find("numTentativas").GetComponent<Text>().text = "Tentativas = " + numTentativas;
    }

    private void DisparaTimer()
    {
        timerPausado = false;
        timerAcionado = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(timerAcionado)
        {
            timer += Time.deltaTime;
            print(timer);
            if(timer > 1)
            {
                timerPausado = true;
                timerAcionado = false;
                if(carta1.tag == carta2.tag)
                {
                    Destroy(carta1);
                    Destroy(carta2);
                    numAcertos++;
                    if(numAcertos == 13)
                    {
                        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    }
                }
                else
                {
                    carta1.GetComponent<Tile>().EscondeCarta();
                    carta2.GetComponent<Tile>().EscondeCarta();
                    UpdateTentativas();
                }
                primeiraCartaSelecionada = false;
                segundaCartaSelecionada = false;
                carta1 = null;
                carta2 = null;
                linhaCarta1 = "";
                linhaCarta2 = "";
                timer = 0;
            }
        }
    }

    
}
