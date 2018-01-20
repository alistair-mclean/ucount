using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Text;
using UnityEngine;
using System.Xml.Linq;
using System.Linq;
//using SharpKml;
//using SharpKml.Engine;
//using SharpKml.Dom;
using SharpKml;
using SharpKml.Engine;
using SharpKml.Dom;

public class PipeFromKml : MonoBehaviour {
  // This will read a Kml file into memory.

  private KmlFile _file;

  private void Start()
  {

    //_file = KmlFile.Load("Stormwater__Storm_Sewers.kml"); //UNITY DOESN'T LIKE THIS LINE....
    //KmlFile file = KmlFile.Load("YourKmlFile.kml");


    ////// It's good practice for the root element of the file to be a Kml element, though not compulsary
    //Kml kml = file.Root as Kml;
    //if (kml != null)
    //{
    //  foreach (var placemark in kml.Flatten().OfType<Placemark>())
    //  {
    //    print(placemark.Name);
    //  }
    //}

  }
  // Following method is used to retrive the relative path as device platform
  private string getPath()
  {
#if UNITY_EDITOR
    return Application.dataPath + "/Resources/";
#elif UNITY_ANDROID
        return Application.persistentDataPath+fileName;
#elif UNITY_IPHONE
        return GetiPhoneDocumentsPath()+"/"+fileName;
#else
    return Application.dataPath + "/" + fileName;
#endif
  }
  private string GetiPhoneDocumentsPath()
  {
    // Strip "/Data" from path
    string path = Application.dataPath.Substring(0, Application.dataPath.Length - 5);
    // Strip application name
    path = path.Substring(0, path.LastIndexOf('/'));
    return path + "/Documents";
  }
}

