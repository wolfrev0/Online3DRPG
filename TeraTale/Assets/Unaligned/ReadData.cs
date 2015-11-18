using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class ReadData : MonoBehaviour
{
    public Texture[] ReadHead;
    public Texture[] ReadBody;
    public Texture[] ReadFoot;
   
    MeshRenderer mesh;

	// Use this for initialization
	void Start ()
    {
        StreamReader sr = new StreamReader(new FileStream("TextureInfo.txt", FileMode.Open));

        mesh = GetComponent<MeshRenderer>();
        mesh.materials[0].mainTexture = ReadHead[System.Convert.ToInt32(sr.ReadLine())];
        mesh.materials[1].mainTexture = ReadBody[System.Convert.ToInt32(sr.ReadLine())];
        mesh.materials[2].mainTexture = ReadFoot[System.Convert.ToInt32(sr.ReadLine())];

        sr.Close();
    }
}
