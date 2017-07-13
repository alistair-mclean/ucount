///<summary>
/// LoadLevel.cs - Loads the enumerated level controlled by the Scenemanager.
/// 
/// Copyright - VARIAL Studios LLC
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour {  
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
