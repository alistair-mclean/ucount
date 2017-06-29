using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class FreeBodyDiagram : VectorComponent {
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


  // The following method may have been depreciated by now. NEEDS UPDATING OR DELETING! 
  public void NewForce(string newForceName, Vector3 newForceOrigin, Vector3 newForceVector)
  {
    //Validity checks and errors
    if (newForceName == null) {
      Debug.LogError("Error: Bad name with new Force.");
      return;
    }
    if (newForceOrigin == null)
    {
      Debug.LogError("Error: Bad origin with new Force.");
      return;
    }
    if (newForceVector == null)
    {
      Debug.LogError("Error: Bad force vector with new Force.");
      return;
    }

    //Create a new force and add it to the Force List
    Force newForceToAdd;
    //newForceToAdd.Name = newForceName;
    //newForceToAdd.Components.SetVectorOrigin(newForceOrigin);
    //newForceToAdd.Components.SetVectorComponents(newForceVector);
    //newForceToAdd.VectorModel = PrefabVectorModel;
    //AddForceToList(newForceToAdd);

    // Instantiate a new Force vector model at the object's location, and scale 
    Instantiate(PrefabVectorModel, _object.transform);
  } 

  void LateUpdate () {
    foreach(Force listedForce in ForceList)
    {
      //GameObject vectorModel = listedForce.VectorModel;
      //Vector3 updatedVectorModelScale = listedForce.Components * 0.3f;
      //vectorModel.GetComponent<VectorControl>().SetVectorScale(updatedVectorModelScale);
    }
	}
}
