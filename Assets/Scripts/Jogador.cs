using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class Jogador : MonoBehaviour {
	public InputField campoNome;
    public string nomeJogador;

	public void IniciaJogador() {

        nomeJogador = campoNome.text;
        if (campoNome.text == "" || campoNome.text == null)
        {
            return;
        }
        else
        {
            PlayerPrefs.SetString("Nome", nomeJogador);
            PlayerPrefs.Save();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
