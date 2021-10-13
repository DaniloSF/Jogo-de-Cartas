using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageOptions : MonoBehaviour
{
    /// <summary>
    /// Dependendo do botao o modo do jogo e mudado, e em seguida o jogo e iniciado
    /// </summary>
    public int changeDelay = 60;
    // 1 = cartas vermelhas
    // 2 = cartas pretas
    // 3 = naipe de paus
    // 4 = naipe de espadas
    // 5 = naipe de ouros
    // 6 = naipe de copas
    // 7 = baralho completo
    public int botao = 0;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("score", 0);
    }

    // Update is called once per frame
    void FixedUpdate()//Chamado uma vez por frame para que o timer seja constante nao importa a maquina do jogador
    {
        /*
        Dependendo do botao apertado muda as configuracoes necessarias e depois carrega a cena do jogo.
        */
        if (botao > 0)
        {
            switch (botao)
            {
                case 1:
                    PlayerPrefs.SetInt("gamemode", 0);
                    PlayerPrefs.SetInt("corCarta", 1);
                    break;
                case 2:
                    PlayerPrefs.SetInt("gamemode", 0);
                    PlayerPrefs.SetInt("corCarta", 0);
                    break;
                case 3:
                    PlayerPrefs.SetInt("gamemode", 1);
                    PlayerPrefs.SetString("naipeEscolhido", "clubs");
                    break;
                case 4:
                    PlayerPrefs.SetInt("gamemode", 1);
                    PlayerPrefs.SetString("naipeEscolhido", "spades");
                    break;
                case 5:
                    PlayerPrefs.SetInt("gamemode", 1);
                    PlayerPrefs.SetString("naipeEscolhido", "diamonds");
                    break;
                case 6:
                    PlayerPrefs.SetInt("gamemode", 1);
                    PlayerPrefs.SetString("naipeEscolhido", "hearts");
                    break;
                case 7:
                    PlayerPrefs.SetInt("gamemode", 2);
                    break;
            }
            changeDelay--;
            if (changeDelay <= 0)
            {
                SceneManager.LoadScene("Game");
            }
        }

    }

    public void Vermelhas()//inicia o jogo com dois naipes vermelhos de um baralho
    {
        botao = 1;
    }
    public void Pretas()//inicia o jogo com dois naipes pretos de um baralho
    {
        botao = 2;
    }
    public void Paus()//inicia o jogo com o naipe de paus de dois baralhos
    {
        botao = 3;
    }
    public void Espadas()//inicia o jogo com o naipe de espadas de dois baralhos
    {
        botao = 4;
    }
    public void Ouros()//inicia o jogo com o naipe de ouros de dois baralhos
    {
        botao = 5;
    }
    public void Copas()//inicia o jogo com o naipe de copas de dois baralhos
    {
        botao = 6;
    }
    public void BaralhoCompleo()//inicia o jogo com um baralho completo
    {
        botao = 7;
    }
}
