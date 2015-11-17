using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class ParsingData : MonoBehaviour
{ 
    public Texture[] HeadTexture;
    public Texture[] BodyTexture;

    MeshRenderer mesh;
    int _curindex = 0;
    int __curindex = 0;

	void Start ()
    {
        mesh = GetComponent<MeshRenderer>();    
	}
	
	void Update ()
    {
        if (_curindex <= 0)
            _curindex = 0;
        else if (_curindex >= HeadTexture.Length)
            _curindex = HeadTexture.Length - 1;

        if (__curindex <= 0)
            __curindex = 0;
        else if (__curindex >= BodyTexture.Length)
            __curindex = BodyTexture.Length - 1;

        mesh.materials[0].mainTexture = HeadTexture[_curindex];
        mesh.materials[1].mainTexture = BodyTexture[__curindex];
    }

    public void PreHead()
    {
        _curindex--;
        Debug.Log(_curindex);
    }

    public void NextHead()
    {
        _curindex++;
        Debug.Log(_curindex);
    }

    public void PreBody()
    {
        __curindex--;
    }

    public void NextBody()
    {
        __curindex++;
    }

    public void SaveData()
    {
        StreamWriter sw = new StreamWriter(new FileStream("D:/Desktop/Projects/TeraTale/TextureInfo.txt", FileMode.Create));

        sw.WriteLine(_curindex);

        sw.WriteLine(__curindex);

        sw.Close();
    }
    public void LoadData()
    {
        Application.LoadLevel(4);
    }
}
