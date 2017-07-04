using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour {
  // DESCRIPTION - This class controls loading scenes. 
  
  // Public
  public Scene FirstLevel;
  public Scene SceondLevel;
 
  // Private
  private AssetBundle _loadedAssetBundle;
  private string[] _scenePaths;

  private void Start() {
    _loadedAssetBundle = AssetBundle.LoadFromFile("Assets/AssetBundles/scenes");
    _scenePaths = _loadedAssetBundle.GetAllScenePaths();
  }

  public void LoadIndexedLevel(int num)
  {
    SceneManager.LoadScene(num);
  }
}
