using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class ParsingData : MonoBehaviour
{
    public Texture[] HeadTexture;
   
    SkinnedMeshRenderer mesh;
    int _curindex = 0;

	void Start ()
    {
        mesh = GetComponent<SkinnedMeshRenderer>();    
	}
	
	void Update ()
    {
            if (_curindex <= 0)
                _curindex = 0;
            else if (_curindex >= HeadTexture.Length)
                _curindex = HeadTexture.Length - 1;
       
        mesh.materials[0].mainTexture = HeadTexture[_curindex];
    }

    public void PreHead()
    {
        _curindex--;
    }

    public void NextHead()
    {
        _curindex++;
    }

    public void SaveData()
    {
        StreamWriter sw = new StreamWriter(new FileStream("TextureInfo.txt", FileMode.Create));

            sw.WriteLine(_curindex);

        sw.Close();
    }
    public void LoadData()
    {
       SceneManager.LoadScene("Login");
    }
}
