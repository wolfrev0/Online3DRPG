using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class ReadData : MonoBehaviour
{
    public Texture[] ReadHead;
    public Texture[] ReadBody;
   
    MeshRenderer mesh;

	// Use this for initialization
	void Start ()
    {
        StreamReader sr = new StreamReader(new FileStream("D:/Desktop/Projects/TeraTale/TextureInfo.txt", FileMode.Open));

        mesh = GetComponent<MeshRenderer>();
        mesh.materials[0].mainTexture = ReadHead[System.Convert.ToInt32(sr.ReadLine())];
        mesh.materials[1].mainTexture = ReadBody[System.Convert.ToInt32(sr.ReadLine())];

        Debug.Log(mesh.materials[0].mainTexture.name);
        Debug.Log(mesh.materials[1].mainTexture.name);

        sr.Close();
    }
}
