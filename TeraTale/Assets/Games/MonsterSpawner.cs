using UnityEngine;
using TeraTaleNet;

[RequireComponent(typeof(SphereCollider))]
public class MonsterSpawner : NetworkScript
{
    public Enemy pfEnemy;
    public int enemyCount = 0;
    SphereCollider _spawnRange;
    Enemy[] _enemies;
    int _index = 0;

    new void Start()
    {
        base.Start();

        _enemies = new Enemy[enemyCount];

        if (isLocal)
            return;

        _spawnRange = GetComponent<SphereCollider>();
        for (int i = 0; i < _enemies.Length; i++)
            NetworkInstantiate(pfEnemy, "OnEnemyInstantiate");
    }

    void OnEnemyInstantiate(Enemy enemy)
    {
        _enemies[_index++] = enemy;
        enemy.name = pfEnemy.name;
        enemy.spawner = this;
        Spawn(enemy);
    }

    public void Spawn(Enemy enemy)
    {
        if (isLocal)
            return;
        var positionSeed = Random.Range(0f, Mathf.PI * 2);
        enemy.transform.position = new Vector3(Mathf.Sin(positionSeed), 0, Mathf.Cos(positionSeed)) * Random.Range(0f, _spawnRange.radius) + transform.position;
        enemy.transform.eulerAngles = new Vector3(0, Random.Range(0f, 360f), 0);
        enemy.Send(new SetActive(true));
    }
}