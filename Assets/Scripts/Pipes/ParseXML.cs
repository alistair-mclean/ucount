using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using System.Text;
using System;
using System.Xml.Linq;




public class ParseXML : MonoBehaviour
{

  public TextMesh fileDataTextbox;
  private string path;
  private string fileInfo;
  private XmlDocument xmlDoc;
  private WWW www;
  private TextAsset textXml;
  private string fileName;
  private string _xmlFile;


  void Awake()
  {
    fileName = "Stormwater_Storm_Sewers.kml"; 
    fileDataTextbox.text = "";
  }

  void Start()
  {
    XNamespace ns = "http://earth.google.com/kml/2.2";
    loadXMLFromAssest();
    print(GetXMLAsString(xmlDoc));
  }
  
  // Following method load xml file from resouces folder under Assets
  private void loadXMLFromAssest()
  {
    xmlDoc = new XmlDocument();
    if (System.IO.File.Exists(getPath()))
    {
      xmlDoc.LoadXml(System.IO.File.ReadAllText(getPath()));
    }
    else
    {
      textXml = (TextAsset)Resources.Load(fileName, typeof(TextAsset));
      xmlDoc.LoadXml(textXml.text);
    }
  }

  public string GetXMLAsString(XmlDocument myxml)
  {
    return myxml.OuterXml;
  }


  // Following method reads the xml file and display its content
  private void readXml()
  {
    int count = 0;
    print("READING ");
    foreach (XmlElement node in xmlDoc)
    {
      print("Count = " + count);
      print(node.Name);
      print(node.Value);
      count++;
    }

  }



  // Following method is used to retrive the relative path as device platform
  private string getPath()
  {
#if UNITY_EDITOR
        return Application.dataPath +"/Resources/"+fileName;
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