using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageBotoes : MonoBehaviour
{
    /// <summary>
    /// Dependendo do botao apertado carregar a proxima cena, 1 comeca a forca com um delay, 2 vai para os creditos e 3 fecha o jogo.
    /// </summary>
    public int changeDelay = 60;
    // 1 = StartGame
    // 2 = MostrarCreditos
    // 3 = FecharJogo
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
        Dependendo do botao apertado carregar a proxima cena, 1 comeca a forca com um delay, 2 vai para os creditos e 3 fecha o jogo.
        */
        if (botao > 0)
        {
            switch (botao)
            {
                case 1://StartGame, delay definido por "changeDelay" frames de tempo
                    changeDelay--;
                    if (changeDelay <= 0)
                    {
                        SceneManager.LoadScene("Game");
                    }
                    break;
                case 2://Creditos
                    changeDelay--;
                    if (changeDelay <= 0)
                    {
                        SceneManager.LoadScene("Creditos");
                    }
                    break;
                case 3: //Fechar execultavel
                    Application.Quit();

                    //reseta o flag do botao para que nao repita o mesma coisa varios frames
                    botao = 0; 
                    Debug.Log("Game is exiting");
                    break;
            }
            
        }

    }

    public void StartGame()//Acionar acao do botao para comecar o jogo
    {
        botao = 1;
    }

    public void MostrarCreditos()//Acionar acao do botao para mostrar os creditos
    {
        botao = 2;
        changeDelay = 15;
    }
    
    public void FecharJogo()//Fechar o executavel do jogo, apos buildar
    {
        botao = 3;
        changeDelay = 15;
    }
}
