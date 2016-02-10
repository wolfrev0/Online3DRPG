using UnityEngine;

public class Meteor : MonoBehaviour
{
    public GameObject pfMagicCircle;
    public GameObject pfExplosion;

    Projectile _projectile;
    GameObject _magicCircle;
    MeshRenderer _magicCircleRenderer;
    Vector3 _startPos;
    Vector3 _endPos;

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
        _magicCircleRenderer = _magicCircle.GetComponentInChildren<MeshRenderer>();
        _startPos = transform.position;
        _endPos = hit.point;
    }

    void Update()
    {
        _magicCircleRenderer.material.color = Color.Lerp(Color.red, Color.yellow, (_endPos - transform.position).magnitude / (_endPos - _startPos).magnitude - 0.15f);
    }

    void OnDestroy()
    {
        Destroy(_magicCircle);
        var fxExplosion = Instantiate(pfExplosion);
        fxExplosion.transform.position = transform.position;
        Destroy(fxExplosion, 2);
    }
}