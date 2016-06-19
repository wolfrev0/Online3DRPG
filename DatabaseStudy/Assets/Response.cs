using UnityEngine;
using System.Collections;
using JsonFx.Json;
using System.Xml;

public class Respown : MonoBehaviour
{

    public static void ReceiveLogin(string str)
    {
        string jsonData = str; //(string)JsonFx.Json.JsonReader.Deserialize(str);

        XmlDocument xmldoc = new XmlDocument();

        xmldoc.LoadXml(jsonData.Trim());

        if (xmldoc != null)
        {
            XmlNodeList nodes = xmldoc.SelectNodes("Server/Book");

            foreach (XmlNode node in nodes)
            {
                Debug.Log(node["EMPNO"].InnerText);
                Debug.Log(node["ENAME"].InnerText);
                Debug.Log(node["JOB"].InnerText);
                Debug.Log(node["MGR"].InnerText);
            }
        }

    }
}