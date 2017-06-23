using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectInformationValve : MonoBehaviour {

  public ArrayList DisplayInformation;
  public GameObject ObjectIDTextBox;
  public GameObject VelocityDisplayChunk;

  private Canvas _displayCanvas;
  private int _numberOfParameters;

  private void Start()
  {
    _displayCanvas = GetComponent<Canvas>();  
  }

  public void SetInformationToDisplay(int numberOfAttributes, ref ArrayList displayList)
  {
    DisplayInformation = displayList;
    // loop through the display list

    // load a display chunk for each item in the list 
  }

  private void LoadDisplayChunk(string whatToDisplay)
  {
    switch (whatToDisplay)
    {
      case ("Velocity"):
        // Instantiate the new velocity chunk
        GameObject velocityChunk = Instantiate(VelocityDisplayChunk);
        velocityChunk.transform.parent = gameObject.transform;
        // Start the data transfer to the newly created chunk
        break;

      default:
        // Instantiate the name of the object to the ObjectIDTextBox of the canvas. 
        break;
    }
  }

  void LateUpdate()
  {
    
  }
}
