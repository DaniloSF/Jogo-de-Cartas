using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ChecaRecorde : MonoBehaviour
{
    /// <summary>
    /// Apresenta a tela de fim de jogo, parabenizando o jogador caso tenha tingido um novo recorde
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        //checa se o numero de tentativas e menor que o recorde atual
        int tentativas, recorde;

        tentativas = PlayerPrefs.GetInt("Jogadas",0);
        recorde = PlayerPrefs.GetInt("Recorde",0);
        AudioClip vitoria = Resources.Load<AudioClip>("Sounds/victory");
        AudioClip ending = Resources.Load<AudioClip>("Sounds/end");

        if (tentativas >= recorde)
        {
            GameObject.Find("Text").GetComponent<Text>().text = "Fim de Jogo!";
            GameObject.Find("music").GetComponent<AudioSource>().clip = ending;
            GameObject.Find("music").GetComponent<AudioSource>().Play();
            GameObject.Find("music").GetComponent<AudioSource>().loop = true;
        }
        else
        {
            GameObject.Find("Text").GetComponent<Text>().text = "Parabéns! Você é o novo recordista!!";
            
            GameObject.Find("music").GetComponent<AudioSource>().clip = vitoria;
            GameObject.Find("music").GetComponent<AudioSource>().Play();
        }
         GameObject.Find("Tentativas").GetComponent<Text>().text += tentativas;
    }
}
