using System.Collections.Generic;
using TeraTaleNet;
using UnityEngine;

public class Dragon : Enemy
{
    public Projectile pfMeteor;
    public FireBreath fireBreath;

    new void Awake()
    {
        base.Awake();
    }

    new void Start()
    {
        base.Start();
    }

    void MeteorStart()
    {
        for (int i = 0; i < 12; i++)
            Invoke("CastMeteor", Random.Range(0f, 3f));
    }

    void CastMeteor()
    {
        var meteor = Instantiate(pfMeteor);
        var xzSeed = Random.Range(0, Mathf.PI * 2);
        meteor.transform.position = transform.position + new Vector3(Mathf.Sin(xzSeed), 0, Mathf.Cos(xzSeed)) * Random.Range(0f, 10f) + Vector3.up * 40;
        meteor.direction = new Vector3(0, -1, 0);
    }

    void FireBreathBegin()
    {
        fireBreath.On();
    }

    void FireBreathEnd()
    {
        fireBreath.Off();
    }

    protected override List<Item> itemsForDrop
    {
        get
        {
            List<Item> ret = new List<Item>();
            if (Random.Range(0, 2) == 0)
                ret.Add(new HpPotion());
            ret.Add(new Apple());
            return ret;
        }
    }

    protected override float levelForDrop
    { get { return 10; } }
}