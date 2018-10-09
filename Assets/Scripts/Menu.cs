using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {
	public void IniciaJogo(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
	public void Sair() {
		Application.Quit();
	}
	
}
