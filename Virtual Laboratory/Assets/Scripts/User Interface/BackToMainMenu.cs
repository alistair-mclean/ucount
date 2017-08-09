using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// This method seems super fuckin lazy
public class BackToMainMenu : MonoBehaviour {
	
	void Update () {
    if (Input.GetKeyDown(KeyCode.Escape))
      SceneManager.LoadScene(0);
	}
}
