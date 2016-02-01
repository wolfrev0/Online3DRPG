using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class ParsingData : MonoBehaviour
{
    public Texture[] HeadTexture;
    public Texture[] BodyTexture;
    public Texture[] FootTexture;

    SkinnedMeshRenderer mesh;
    int[] _curindex = new int[3];

	void Start ()
    {
        mesh = GetComponent<SkinnedMeshRenderer>();    
	}
	
	void Update ()
    {
        for (int i = 0; i < 3; i++)
        {
            if (_curindex[i] <= 0)
                _curindex[i] = 0;
            else if (_curindex[i] >= HeadTexture.Length)
                _curindex[i] = HeadTexture.Length - 1;

            if (i == 0 && _curindex[i] >= HeadTexture.Length)
                _curindex[i] = HeadTexture.Length - 1;
            else if (i == 1 && _curindex[i] >= BodyTexture.Length)
                _curindex[i] = BodyTexture.Length - 1;
            else if (i == 2 && _curindex[i] >= FootTexture.Length)
                _curindex[i] = FootTexture.Length - 1;
        }

        mesh.materials[0].SetTexture("_DiffuseMapSpecA", HeadTexture[_curindex[0]]);
        //mesh.materials[1].mainTexture = BodyTexture[_curindex[1]];
        //mesh.materials[2].mainTexture = FootTexture[_curindex[2]];
    }

    public void PreHead()
    {
        _curindex[0]--;
    }

    public void NextHead()
    {
        _curindex[0]++;
    }

    public void PreBody()
    {
        _curindex[1]--;
    }

    public void NextBody()
    {
        _curindex[1]++;
    }

    public void PreFoot()
    {
        _curindex[2]--;
    }
    public void NextFoot()
    {
        _curindex[2]++;
    }

    public void SaveData()
    {
        StreamWriter sw = new StreamWriter(new FileStream("TextureInfo.txt", FileMode.Create));

        for(int i=0;i<3;i++)
            sw.WriteLine(_curindex[i]);

        sw.Close();
    }
    public void LoadData()
    {
       SceneManager.LoadScene(5);
    }
}
