using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour {
  // DESCRIPTION - Loads a given level  
  public Scene FirstLevel;
  public Scene SceondLevel;
 

  private AssetBundle _loadedAssetBundle;
  private string[] _scenePaths;

  private void Start() {
    _loadedAssetBundle = AssetBundle.LoadFromFile("Assets/AssetBundles/scenes");
    _scenePaths = _loadedAssetBundle.GetAllScenePaths();
  }

  public void LoadIndexedLevel(int num)
  {
    switch(num)
    {
      case (0):
        Application.LoadLevel(0);
        break;
      case (1):
        Application.LoadLevel(1);
        break;
      case (2):
        Application.LoadLevel(2);
        break;
      default:
        Application.LoadLevel(0);
        break;
    };
  }
}
