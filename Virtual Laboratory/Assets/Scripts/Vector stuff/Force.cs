using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Force : VectorComponent  {
  // DESCRIPTION - A script to add a free body diagram to an object. 
  // Adds a new Vector for each force and applys a specific 
  // Material depending upon what direction the force is in. 

  // Private 
  private string _name;
  private VectorComponent _components = new VectorComponent();

  // Public  
  public enum ForceType {Gravitational, Physical, Electromagnetic}; // for future enumerations of forces
  

  public Force(string name, Vector3 components, Transform origin)
  {
    _components.SetVectorName(name);
    _components.SetVectorComponents(components);
    _components.SetVectorOrigin(origin);
    if (GetComponent<FreeBodyDiagram>())
    {
      GetComponent<FreeBodyDiagram>().NewForce(name, origin.position, components);
    }
  }

  public Force(GameObject Parent, GameObject Target, Vector3 Components)
  {
    _components.SetVectorName("Force from " + Parent.name + " on " + Target.name);
    _components.SetVectorOrigin(Target.transform);
    _components.SetVectorComponents(Components);
  }

  public string GetForceName() {
    return _name;
  }

  public void SetForceName(string newName) {
    _name = newName;
  }
  
  public void SetForceVectorComponents(VectorComponent newComponents) {
    _components.SetVectorName(newComponents.GetVectorName());
    _components.SetVectorComponents(newComponents.GetVectorComponents());
    _components.SetVectorOrigin(newComponents.GetVectorOrigin());
  }

  public VectorComponent GetForceVectorComponents()
  {
    return _components;
  }

  
};

