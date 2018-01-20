using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using System.Text;
using System;
using System.Xml.Linq;
using System.Linq;

public class ParseXML : MonoBehaviour
{
  struct PlaceMark
  {
    public int ID;
    public String Name;
    public String Description;
    public List<String> ExtendedData;
    public List<Vector2> Coordinates;
  };

  public TextMesh _fileDataTextbox;
  private string _path;
  private string _fileInfo;
  private XmlDocument _xmlDoc;
  private WWW _www;
  private TextAsset _textXml;
  private string _fileName;
  private string _xmlFile;
  private List<PlaceMark> _pipeList;
  private int _pipeCount = 0;
 

  /// <summary>
  /// KML FILES CAN BE IMPORTED THROUGH XML. 
  /// Currently reading them as strings. I was unable to get them working with SharpKml. Hopefully this can suffice. 
  /// </summary>
  

  void Awake()
  {
    _fileName = "Stormwater__Storm_Sewers.kml"; 
    //fileName = "books.xml"; //TEST XML FILE 
  }

  void Start()
  {
    _pipeList = new List<PlaceMark>();
    XNamespace ns = "http://earth.google.com/kml/2.2";
    LoadXMLFromAssets();
    PrintXmlFileFromAssets();
    //PrintRequestedElements("PlaceMark");
    //readXml();
  }
  
  private void PrintXmlFileFromAssets()
  {
    StringBuilder output = new StringBuilder();

    using (XmlReader reader = XmlReader.Create(new StringReader(GetXMLAsString(_xmlDoc))))
    {
      XmlWriterSettings ws = new XmlWriterSettings();
      ws.Indent = true;
      using (XmlWriter writer = XmlWriter.Create(output, ws))
      {

        // Parse the file and display each of the nodes.
        while (reader.Read())
        {
          String tempName = "";
          String tempDescription = "";
          String tempSimpleData = "";
          Vector2 tempCoordinate = new Vector2(0f, 0f);

          List<String> tempExtendedData = new List<String>();
          List<Vector2> tempCoordList = new List<Vector2>();
          
          switch (reader.NodeType)
          {
            case XmlNodeType.Element:
              writer.WriteStartElement(reader.Name);
              //This gets the name, like placemark, extended data, simpledata, etc. 

              if(reader.Name == "Placemark")
              {
                _pipeCount++;
                //print("FOUND!!!!!!!!!!!!!!!!! "+ reader.Name + " value = " + reader.Value); //DEBUG
              }
              print(reader.Name); // DEBUG
              break;
            case XmlNodeType.Text:
              // What does this retreive? 
              // It retreives the value in between the brackets. 
              writer.WriteString(reader.Value);
              print(reader.Value); //DEBUG
              break;
            case XmlNodeType.XmlDeclaration:
            case XmlNodeType.ProcessingInstruction:
              writer.WriteProcessingInstruction(reader.Name, reader.Value);
              break;
            case XmlNodeType.Comment:
              writer.WriteComment(reader.Value);
              print(reader.Value);//DEBUG
              break;
            case XmlNodeType.EndElement:
              if(reader.Name == "Placemark")
              {
                print("Ending placemark" + _pipeCount + "."); //DEBUG

              }
              writer.WriteFullEndElement();
              break;
          }
        }

      }
    }
    print(output.ToString()); // DEBUG
    print("Found " + _pipeCount + " pipes."); // DEBUG
  }

  private void PrintRequestedElements(string element)
  {
    StringBuilder output = new StringBuilder();
    using (XmlReader reader = XmlReader.Create(new StringReader(GetXMLAsString(_xmlDoc) )))
    {
      reader.ReadToFollowing(element);
      reader.ReadInnerXml();
      string value = reader.Value;
      output.AppendLine("The value: " + value);

      reader.ReadToFollowing("title");
      output.AppendLine("Content of the title element: " + reader.ReadElementContentAsString());
    }
  }

  // Following method loads an xml file from resouces folder under Assets
  private void LoadXMLFromAssets()
  {
    _xmlDoc = new XmlDocument();
    if (System.IO.File.Exists(GetPath()))
    {
      _xmlDoc.LoadXml(System.IO.File.ReadAllText(GetPath()));
    }
    else
    {
      Debug.Log("ERROR FILE DOESN'T EXIST");
      _textXml = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
      _xmlDoc.LoadXml(_textXml.text);
      
    }
  }

  public string GetXMLAsString(XmlDocument myxml)
  {
    return myxml.OuterXml;
  }


  // Following method is used to retrive the relative path as device platform
  private string GetPath()
  {
#if UNITY_EDITOR
        return Application.dataPath +"/Resources/"+_fileName;
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