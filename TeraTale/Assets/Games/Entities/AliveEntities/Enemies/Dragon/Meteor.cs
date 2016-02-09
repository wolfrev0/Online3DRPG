using UnityEngine;

public class Meteor : MonoBehaviour
{
    public GameObject pfMagicCircle;

    Projectile _projectile;
    GameObject _magicCircle;

    void Awake()
    {
        _projectile = GetComponent<Projectile>();
    }

    void Start()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, _projectile.direction, out hit, float.MaxValue, LayerMask.GetMask("Terrain"));
        _magicCircle = Instantiate(pfMagicCircle);
        _magicCircle.transform.position = hit.point;
    }

    void OnDestroy()
    {
        Destroy(_magicCircle);
    }
}