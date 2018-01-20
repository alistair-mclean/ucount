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

  public TextMesh _fileDataTextbox;
  private string _path;
  private string _fileInfo;
  private XmlDocument _xmlDoc;
  private WWW _www;
  private TextAsset _textXml;
  private string _fileName;
  private string _xmlFile;

  /// <summary>
  /// KML FILES CAN BE IMPORTED THROUGH XML. 
  /// Currently reading them as strings. I was unable to get them working with SharpKml. Hopefully this can suffice. 
  /// </summary>
  struct PlaceMark
  {
    public string Name  ;
    public string Description;
  };

  void Awake()
  {
    _fileName = "Stormwater__Storm_Sewers.kml"; 
    //fileName = "books.xml"; //TEST XML FILE 
  }

  void Start()
  {
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
          print("Reader node position: " + reader.ReadContentAsString());
          switch (reader.NodeType)
          {
            case XmlNodeType.Element:
              writer.WriteStartElement(reader.Name);
              break;
            case XmlNodeType.Text:
              writer.WriteString(reader.Value);
              break;
            case XmlNodeType.XmlDeclaration:
            case XmlNodeType.ProcessingInstruction:
              writer.WriteProcessingInstruction(reader.Name, reader.Value);
              break;
            case XmlNodeType.Comment:
              writer.WriteComment(reader.Value);
              break;
            case XmlNodeType.EndElement:
              writer.WriteFullEndElement();
              break;
          }
        }

      }
    }
    print(output.ToString());
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
      print("FILE DIDN't EXIST");
      _textXml = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
      _xmlDoc.LoadXml(_textXml.text);
      
    }
  }

  public string GetXMLAsString(XmlDocument myxml)
  {
    return myxml.OuterXml;
  }


  // Following method reads the xml file and display its content
  private void ReadXml()
  {
    

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