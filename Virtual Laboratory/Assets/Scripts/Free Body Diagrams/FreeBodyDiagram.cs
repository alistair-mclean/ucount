using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeBodyDiagram : VectorControl {
  // DESCRIPTION - A script to add a free body diagram to an object. 
  // Adds a new Vector for each force and applys a specific 
  // Material depending upon what direction the force is in. 

  // Public 
  public struct Force
  {
    public string Name;
    public Vector3 Origin;
    public Vector3 Components;
    public GameObject VectorModel;
  };
  public List<Force> ForceList;
  public GameObject PrefabVectorModel;
  
  // Private
  private Rigidbody _object;
  private float _vectorSizingConstant = 3.5f;


  void Start()
  {
    _object = GetComponent<Rigidbody>();
  }

  private void AddForceToList(Force newForce)
  {
    ForceList.Add(newForce);
  }

  public void NewForce(string newForceName, Vector3 newForceOrigin, Vector3 newForceVector)
  {
    //Validity checks and errors
    if (newForceName == null) {
      Debug.LogError("Bad name with new Force.");
      return;
    }
    if (newForceOrigin == null) {
      Debug.LogError("Bad origin with new Force.");
      return;
    }

    //Create a new force and add it to the Force List
    Force newForceToAdd;
    newForceToAdd.Name = newForceName;
    newForceToAdd.Origin = newForceOrigin;
    newForceToAdd.Components = newForceVector;
    newForceToAdd.VectorModel = PrefabVectorModel;
    AddForceToList(newForceToAdd);

    // Create a new Force vector model at the object's location, and scale 
    Instantiate(PrefabVectorModel, _object.transform);

  } 

  void LateUpdate () {
    foreach(Force listedForce in ForceList)
    {
      GameObject vectorModel = listedForce.VectorModel;
      Vector3 updatedVectorModelScale = listedForce.Components * 0.3f;
      vectorModel.GetComponent<VectorControl>().SetVectorScale(updatedVectorModelScale);
    }
	}
}
