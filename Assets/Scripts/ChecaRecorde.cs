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

        if (tentativas >= recorde)
        {
            GameObject.Find("Text").GetComponent<Text>().text = "Fim de Jogo!";
        }
        else
        {
            GameObject.Find("Text").GetComponent<Text>().text = "Parabéns! Você é o novo recordista!!";
        }
         GameObject.Find("Tentativas").GetComponent<Text>().text += tentativas;
    }
}
