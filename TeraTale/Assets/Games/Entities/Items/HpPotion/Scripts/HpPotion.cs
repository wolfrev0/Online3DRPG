using UnityEngine;

public class HpPotion : Item
{
    public float amplitude = 1;
    public float frequency = 1;
    public float rotationSpeed = 1;
    float elapsed = 0;
    float prevSin;

    new void Start ()
    {
        base.Start();
        prevSin = Mathf.Sin(elapsed * frequency);
    }
	
	void Update ()
    {
        elapsed += Time.deltaTime * frequency;
        var y = (Mathf.Sin(elapsed) - prevSin) * amplitude;
        transform.position += new Vector3(0, y, 0);
        prevSin = Mathf.Sin(elapsed);

        transform.Rotate(0, rotationSpeed, 0, Space.World);
    }

    void OnTriggerEnter(Collider coll)
    {
        if(coll.tag == "Player")
            Affect();
    }

    void Affect()
    {
        //플레이어 두명이 동시에 접촉하는경우를 생각하자. 서버에서 처리하도록 isServer써야되겠다.
        Debug.Log("HP 포션 Affect()");
        if (isServer)// Remove when the test ended.
            NetworkDestroy(this);
    }
}
