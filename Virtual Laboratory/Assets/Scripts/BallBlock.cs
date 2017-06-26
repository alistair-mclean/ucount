using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BallBlock : MonoBehaviour {
  //DESCRIPTION - The script for a simple ball block 
  // setup with friction adjustment on the block and 
  // an adjustable force applied onto the ball.

  //Public
  public Rigidbody Ball;
  public Rigidbody Block;
  public float BallForceConstant = 1.0f; 

  //Private
  private Vector3 _acceleration = new Vector3(0.0f, 0.0f, 0.0f);

  void Update() {
    if (Input.touchCount > 0)
    {
      Ray FingerRay = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
      RaycastHit hit;
      if (Physics.Raycast(FingerRay, out hit)) 
      {
        if (hit.rigidbody != null)
        {
          Debug.Log("Raycast hit rigidbody: " + hit.rigidbody.gameObject.name);
        }
      }
    }
  }

  private void LateUpdate() {
    Vector3 lastVelocity = Ball.velocity;
    _acceleration = (Ball.velocity - lastVelocity) / Time.deltaTime;
  }

  // Adjusts the force modifier on the ball
  public void VLBallSliderControl(float val) {
    Vector3 ballForce = new Vector3(1.0f, 0.0f, 0.0f);
    ballForce = ballForce * val * BallForceConstant;
    Ball.AddForce(ballForce);
    FreeBodyDiagram ballForceDiagram = Ball.GetComponent<FreeBodyDiagram>();
    ballForceDiagram.NewForce("Applied Force", Ball.transform.position, ballForce);
  }

  // Adjusts the static friction coefficient of the block
  public void VLBlockSliderControl(float val) {
    float currentFriction = Block.GetComponent<PhysicMaterial>().staticFriction;
    Block.GetComponent<PhysicMaterial>().staticFriction = currentFriction * val;
  }
}