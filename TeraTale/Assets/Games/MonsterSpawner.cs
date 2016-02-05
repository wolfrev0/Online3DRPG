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

    public float spawnRange { get { return _spawnRange.radius; } }

    protected void Start()
    {
        _enemies = new Enemy[enemyCount];
        _spawnRange = GetComponent<SphereCollider>();

        if (isLocal)
            return;

        for (int i = 0; i < _enemies.Length; i++)
            NetworkInstantiate(pfEnemy, "OnEnemyInstantiate");
    }

    void OnEnemyInstantiate(Enemy enemy)
    {
        _enemies[_index++] = enemy;
        enemy.name = pfEnemy.name;
        enemy.spawner = this;
        enemy.Respawn();
    }
}