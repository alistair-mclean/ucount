using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeMapManager : MonoBehaviour {
  // Public

  // Private
  private static Dictionary<string, PipeLine> _pipeMap = new Dictionary<string, PipeLine>();

  private void Start()
  {
    Vector3 startPos = new Vector3(0f, 0f, -1f);
    Vector3 endPos = new Vector3(1f, 1f, -1f);

    PipeSection pipesection1 = new PipeSection(10f, 20f, startPos, endPos);
    PipeSection pipesection2 = new PipeSection(10f, 20f, endPos, endPos * 2f); // we need to validate a couple things. 1 - that the diameters of subsequent segments match up.
    PipeSection pipesection3 = new PipeSection(10f, 20f, endPos, endPos * 3f); // we need to validate a couple things. 1 - that the diameters of subsequent segments match up.
    List<PipeSection> pipeSegments = new List<PipeSection>();
    pipeSegments.Add(pipesection1);
    pipeSegments.Add(pipesection2);
    pipeSegments.Add(pipesection3);
    PipeLine testPipe = new PipeLine("Alistair McLean", "Wood", pipeSegments);

    //Initialize coroutine
    //StartCoroutine(ShowPipe(testPipe));
    _pipeMap.Add("Test Pipe", testPipe);
    _pipeMap["Test Pipe"].DisplayPipe();
  }

  //Display pipe as a coroutine
  private void ShowPipe(PipeLine testPipe)
  {
    testPipe.DisplayPipe();
    //yield return null;
  }


}
