using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManageOptions : MonoBehaviour
{
    /// <summary>
    /// Dependendo do botao o modo do jogo e mudado, e em seguida o jogo e iniciado
    /// </summary>
    public int changeDelay = 60;
    // 1 = cartas vermelhas
    public bool botao = false;

    public string naipeEscolhido; //Opcao de naipe para gamemode 1
    public string backCor; //Cor da parte de tras da carta
    public int CorCarta; //Cor da carta relacionada ao naipe

    /*  Qual tipo de jogo sera jogado, 
     * gamemode 0 = 2 Linhas e 2 naipes
     * gamemode 1 = 2 linhas e 1 naipe, dois decks diferentes
     * gamemode 2 = 4 linhas e 4 naipes, 4 cartas iguais
     */
    public int gameMode;

    Dropdown optionsDropdown;
    Dropdown subOptionsDropdown;
    Dropdown backCorOptionsDropdown;

    // Start is called before the first frame update
    void Start()
    {
        optionsDropdown = GameObject.Find("Options").GetComponent<Dropdown>();
        subOptionsDropdown = GameObject.Find("SubOptions").GetComponent<Dropdown>();
        backCorOptionsDropdown = GameObject.Find("CorOptions").GetComponent<Dropdown>();

        gameMode = PlayerPrefs.GetInt("gamemode", 0);
        optionsDropdown.value = gameMode;
        if (gameMode == 0)
        {
            CorCarta = PlayerPrefs.GetInt("corCarta", 0);
            subOptionsDropdown.value = CorCarta;
        }
        else if (gameMode == 1)
        {
            naipeEscolhido = PlayerPrefs.GetString("naipeEscolhido", "clubs");
            switch (naipeEscolhido)// De string para int
            {
                case "clubs":
                    subOptionsDropdown.value = 0;
                    break;
                case "spades":
                    subOptionsDropdown.value = 1;
                    break;
                case "diamonds":
                    subOptionsDropdown.value = 2;
                    break;
                case "hearts":
                    subOptionsDropdown.value = 3;
                    break;

            }
        }

        backCor = PlayerPrefs.GetString("backCor", "Red");
        backCorOptionsDropdown.value = backCor == "Red" ? 0 : 1;
    }

    // Update is called once per frame
    void FixedUpdate()//Chamado uma vez por frame para que o timer seja constante nao importa a maquina do jogador
    {
        /*
         *carrega a cena do jogo.
            */
        if (botao)
        {
            
            changeDelay--;
            if (changeDelay <= 0)
            {
                SceneManager.LoadScene("Game");
            }
        }

    }

    public void StartGame()//inicia o jogo com as opcoes selecionadas
    {
        /*
         * Diz ao manager para comecar o timer e comecar o game
         */
    
        botao = true;
    }

    public void SubOptionsChange()
    {
        /*
         * Ao mudar o gamemode as sub opcoes atualizao para mostrar as novas opcoes decorrentes no gamemode
         */
        switch (optionsDropdown.value) 
        {
            case 0: //Um Baralho
                subOptionsDropdown.options.Clear();
                AddNewOption("Cartas Vermelhas");
                subOptionsDropdown.captionText.text = "Cartas Vermelhas";
                AddNewOption("Cartas Pretas");

                PlayerPrefs.SetInt("gamemode", optionsDropdown.value);
                PlayerPrefs.SetInt("corCarta", subOptionsDropdown.value);
                break;

            case 1: //Dois Baralhos
                subOptionsDropdown.options.Clear();
                AddNewOption("Naipe de Paus");
                AddNewOption("Naipe de Espadas");
                AddNewOption("Naipe de Ouros");
                AddNewOption("Naipe de Copas");

                subOptionsDropdown.captionText.text = subOptionsDropdown.options[subOptionsDropdown.value].text;

                PlayerPrefs.SetInt("gamemode", optionsDropdown.value);
                var valueStr = "";
                switch (subOptionsDropdown.value)// De int para string
                {
                    case 0:
                        valueStr = "clubs";
                        break;
                    case 1:
                        valueStr = "clubs";
                        break;
                    case 2:
                        valueStr = "diamonds";
                        break;
                    case 3:
                        valueStr = "hearts";
                        break;

                }
                PlayerPrefs.SetString("naipeEscolhido", valueStr);
                break;
            
            case 2: // Baralho completo
                subOptionsDropdown.options.Clear();
                AddNewOption("Sem Opcoes");
                subOptionsDropdown.captionText.text = "Sem Opcoes";

                PlayerPrefs.SetInt("gamemode", optionsDropdown.value);
                break;
        }
    }

    public void backCorOptionsChange()
    {
        /*
         * Atualiza as cores de tras das cartas 
         */
        PlayerPrefs.SetString("backCor", backCorOptionsDropdown.value == 0 ? "Red" : "Blue");
    }

    private void AddNewOption(string newText)
    {
        /*
         * Adiciona um novo item na lista de opcoes com novo texto
         */
        var data = new Dropdown.OptionData
        {
            text = newText
        };
        subOptionsDropdown.options.Add(data);
    }
}
