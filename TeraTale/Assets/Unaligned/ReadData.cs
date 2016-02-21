using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class ReadData : MonoBehaviour
{
    public Texture[] ReadHead;
    public Texture[] ReadBody;
    public Texture[] ReadFoot;

    SkinnedMeshRenderer mesh;

	// Use this for initialization
	void Start ()
    {
        StreamReader sr = new StreamReader(new FileStream("TextureInfo.txt", FileMode.Open));

        mesh = GetComponent<SkinnedMeshRenderer>();
        mesh.materials[0].mainTexture = ReadHead[Convert.ToInt32(sr.ReadLine())];

        sr.Close();
    }
}
