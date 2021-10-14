using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ManageCartas : MonoBehaviour
{ 
    /// <summary>
    /// Mantem todas as condicoes do jogo e governa as regras gerais, usado na cena de game exclusivamente
    /// </summary>
    public GameObject cartaPrefab; //Carta para se instanciar
    public GameObject[,] cartasArray; //Array/matrix para todas as cartas, separada por linha
    const int maxRankCartas = 13; //Constante do numero de colunas e numero de ranks de cartas
    public int numeroLinhas = 2; //Determina o numero de linhas de cartas
    
    public string naipeEscolhido; //Opcao de naipe para gamemode 1
    public string backCor; //Cor da parte de tras da carta
    public int CorCarta; //Cor da carta relacionada ao naipe

    /*  Qual tipo de jogo sera jogado, 
     * gamemode 0 = 2 Linhas e 2 naipes
     * gamemode 1 = 2 linhas e 1 naipe, dois decks diferentes
     * gamemode 2 = 4 linhas e 4 naipes, 4 cartas iguais
     */
    public int gameMode; 

    public int cartasSelecionadas = 0b0000; //Binario bool que mantem conta das cartas selecionadas, primeira carta e o primeiro algarismo, segunda o segundo algarismo, etc
    public GameObject[] cartasSelecionadasArray; //Array para referenciar cartas selecionadas

    bool timerPausado, timerAcionado; //Delay para resolucao de acerto ou tentativa
    float timer; //contador do timer
    int numTentativas = 0; //Mantem conta do numero de tentativas do player
    int numAcertos = 0; //Mantem conta do numero de acertos do player

    int recorde = 0; //Recorde de outros jogos

    AudioSource somOK; // Som para tocar ao acertar
    AudioSource somErro; //Som para tocar ao errar

    // Start is called before the first frame update
    void Start()
    {
        /*
         * Ao dar start as configuracoes sao lidas e resources sao carregados
         */
        gameMode = PlayerPrefs.GetInt("gamemode", 1);
        if(gameMode == 0)
        { 
            CorCarta = PlayerPrefs.GetInt("corCarta", 0);
        }
        else if(gameMode == 1)
        {
            naipeEscolhido = PlayerPrefs.GetString("naipeEscolhido", "clubs");
        }
        
        backCor = PlayerPrefs.GetString("backCor", "Red");
        recorde = PlayerPrefs.GetInt("Recorde", 0);
        numeroLinhas = gameMode == 2 ? 4 : 2;
        cartasSelecionadasArray = new GameObject[numeroLinhas];

        somOK = gameObject.AddComponent<AudioSource>();
        somOK.clip = Resources.Load<AudioClip>("beep");
        somErro = gameObject.AddComponent<AudioSource>();
        somErro.clip = Resources.Load<AudioClip>("somErro");

        GameObject.Find("ultimaJogada").GetComponent<Text>().text = "Recorde = " + recorde;
        InicializarCartas();
    }

    void InicializarCartas()
    {
        /*
         * Da setup na mesa do jogo colocando as cartas nos seus devidos lugares
         */
        GameObject centro = GameObject.Find("centroDaTela");
        cartasArray = new GameObject[numeroLinhas, maxRankCartas];
        
        for (int indiceVertical = 0; indiceVertical < numeroLinhas; indiceVertical++)
        {
            int[] arrayEmbaralhado = criaArrayEmbaralhado();
            for (int indiceHorizontal = 0; indiceHorizontal < maxRankCartas; indiceHorizontal++)
            {

                float escalaCartaOriginal = cartaPrefab.transform.localScale.x;

                float fatorEscalaX = (650*escalaCartaOriginal)/ 110f;
                float fatorEscalaY = (945*escalaCartaOriginal)/ 110f;

                var novaPosicao = new Vector3(centro.transform.position.x + ((indiceHorizontal - ((maxRankCartas - 1) / 2f)) * fatorEscalaX),
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
        /*
         * Cria a carta de acordo com o gamemode e com as configuracoes escolhidas inicialmente
         */
        var carta = Instantiate(cartaPrefab, novaPosicao, Quaternion.identity, transform);

        carta.GetComponent<Tile>().cartaID = rank;
        carta.GetComponent<Tile>().linhaID = indiceVertical;
        
        
        carta.name = indiceVertical + "_" + rank;
        string nomeDaCarta = "";
        string numeroCarta = "";
        string nomeNaipe = "spades";
        string nomeBack = "playCardBack";

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
        numeroCarta += "_of_";

        switch (gameMode)
        {
            case 0:
                
                if (CorCarta == 0)
                {
                    nomeNaipe = (indiceVertical % 2 == 0 ? "clubs" : "spades");
                    nomeDaCarta = numeroCarta + nomeNaipe;
                }
                else
                {
                    nomeNaipe = (indiceVertical % 2 == 0 ? "diamonds" : "hearts");
                    nomeDaCarta = numeroCarta + nomeNaipe;
                    
                }
                carta.tag = "" + rank;
                nomeBack += backCor;
                break;
            case 1:
                nomeNaipe = naipeEscolhido;
                nomeDaCarta = numeroCarta + nomeNaipe;
                nomeBack += indiceVertical % 2 == 0 ? "Red" : "Blue";
                carta.tag = "" + rank;
                break;
            case 2:
                if(indiceVertical % 2 == 0)
                {
                    nomeNaipe = (indiceVertical % 4 == 0 ? "clubs" : "spades");
                    nomeDaCarta = numeroCarta + nomeNaipe;
                }
                else
                {
                    nomeNaipe = (indiceVertical % 4 == 1 ? "diamonds" : "hearts");
                    nomeDaCarta = numeroCarta + nomeNaipe;
                }
                carta.tag = "" + rank;
                nomeBack += backCor;
                break;

        }
            

        Sprite s1 = Resources.Load<Sprite>("Sprites/cartas/" + nomeDaCarta);
        Sprite b1 = Resources.Load<Sprite>("Sprites/cartas/" + nomeBack);


        carta.GetComponent<Tile>().naipe = nomeNaipe;
        carta.GetComponent<Tile>().setOriginalSprite(s1);
        carta.GetComponent<Tile>().setBackSprite(b1);

        return carta;
    }

    public int[] criaArrayEmbaralhado()
    {
        /*
         * Retorna um array embaralhado de numeros entre 0 e 12
         */
        int[] novaArray = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        int temp;
        for (int i = 0; i < maxRankCartas; i++)
        {
            temp = novaArray[i];
            int r = Random.Range(temp, maxRankCartas);
            novaArray[i] = novaArray[r];
            novaArray[r] = temp;
        }
        return novaArray;
    }

    public void CartaSelecionada(GameObject carta)
    {
        /*
         * Chamada ao clicar na carta, verifica condicoes necessarias para se tornar selecionada, 
         * caso nao seja da mesma linha adiciona a carta ao array das cartas selecionadas e faz update do bool
         */
        if ((cartasSelecionadas & 1) == 0) //Equivalente a primeira carta nao selecionada
        {
            int linha = int.Parse(carta.name.Substring(0, 1));//Achar linha da carta
            if(LinhasIguais(linha)) return;

            cartasSelecionadas |= 0b1; //primeira carta
            cartasSelecionadasArray[0] = carta;
            cartasSelecionadasArray[0].GetComponent<Tile>().RevelaCarta();
        }
        else if((cartasSelecionadas & 0b11) == 0b01) //Equivalente a segunda carta nao selecionada e os anteriores sim
        {
            int linha = int.Parse(carta.name.Substring(0, 1));//Achar linha da carta
            if (LinhasIguais(linha)) return;

            cartasSelecionadas |= 0b11; //primera e segunda carta
            cartasSelecionadasArray[1] = carta;
            cartasSelecionadasArray[1].GetComponent<Tile>().RevelaCarta();
            if (gameMode != 2)
            {
                VerificaCartas();
                return;
            }
        }else if(gameMode == 2 && (cartasSelecionadas & 0b111) == 0b011) //Equivalente a terceira carta nao selecionada e os anteriores sim
        {
            int linha = int.Parse(carta.name.Substring(0, 1));//Achar linha da carta
            if (LinhasIguais(linha)) return;

            cartasSelecionadas |= 0b111; //primeira segunda e terceira carta
            cartasSelecionadasArray[2] = carta;
            cartasSelecionadasArray[2].GetComponent<Tile>().RevelaCarta();
        }else if(gameMode == 2 && (cartasSelecionadas & 0b1111) == 0b0111) //Equivalente a quarta carta nao selecionada e os anteriores sim
        {
            int linha = int.Parse(carta.name.Substring(0, 1));
            if (LinhasIguais(linha)) return;

            cartasSelecionadas |= 0b1111; //todas as cartas
            cartasSelecionadasArray[3] = carta;
            cartasSelecionadasArray[3].GetComponent<Tile>().RevelaCarta();
            VerificaCartas();
        }

        bool LinhasIguais(int linha)
        {
            /*
             * Verifica se a carta selecionada mais recente faz parte da linha de outra carta selecionada
             */

            for (int i = 0; i < cartasSelecionadasArray.Length; i++)
            {
                if (cartasSelecionadasArray[i] && cartasSelecionadasArray[i].GetComponent<Tile>().linhaID == linha)
                {
                    print(cartasSelecionadasArray[i].GetComponent<Tile>().linhaID == linha);
                    return true;
                }
            }
            return false;
        }
    }

    void VerificaCartas()
    {
        /*
         * Comeca timer
         */
        DisparaTimer();
    }

    private void UpdateTentativas()
    {
        /*
         * Aumenta o numero de tentativas e atualiza o texto na tela
         */
        numTentativas++;
        GameObject.Find("numTentativas").GetComponent<Text>().text = "Tentativas = " + numTentativas;
    }

    private void DisparaTimer()
    {
        /*
         * Aciona o timer
         */
        timerPausado = false;
        timerAcionado = true;
    }

    // Update is called once per frame
    void Update()
    {
        /*
         * Depois de um segundo do timer, Match é chamado para verificar as cartas selecionadas
         */
        if(timerAcionado)
        {
            timer += Time.deltaTime;
            print(timer);
            if(timer > 1)
            {
                timerPausado = true;
                timerAcionado = false;
                Match();
                
            }
        }
    }

    private void Match()
    {
        /*
         * Verifica as condições de vitoria, reseta as cartas selecionadas, toca os sons de acerto e erro
         */
        if (TagsIguais())
        {
            for (int i = 0; i < cartasSelecionadasArray.Length; i++)//Destroy selecionadas
            {
                if (cartasSelecionadasArray[i]) Destroy(cartasSelecionadasArray[i]);
            }
            somOK.Play();
            numAcertos++;
            if (numAcertos == maxRankCartas)//Condicao de vitoria
            {
                PlayerPrefs.SetInt("Jogadas", numTentativas);
                if(numTentativas < recorde || recorde == 0)
                {
                    PlayerPrefs.SetInt("Recorde", numTentativas);
                }
                SceneManager.LoadScene("EndScreen");
            }
        }
        else
        {
            for (int i = 0; i < cartasSelecionadasArray.Length; i++)//Esconde selecionadas
            {
                if (cartasSelecionadasArray[i]) cartasSelecionadasArray[i].GetComponent<Tile>().EscondeCarta();
            }
            somErro.Play();
            UpdateTentativas();
        }

        cartasSelecionadas = 0;//Reseta bool
        for (int i = 0; i < cartasSelecionadasArray.Length; i++)//Reset selecionadas
        {
            cartasSelecionadasArray[i] = null;
        }
        timer = 0;

        bool TagsIguais()//Compara todas as tags das cartas selecionadas
        {
            for (int i = 0; i < cartasSelecionadasArray.Length; i++)
            {
                if (cartasSelecionadasArray[i]
                    && cartasSelecionadasArray[0].GetComponent<Tile>().tag != cartasSelecionadasArray[i].GetComponent<Tile>().tag)
                {
                    return false;
                }
            }
            return true;
        }
    }



}
