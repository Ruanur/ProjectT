using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private Transform[] _spawnPoint;
    [SerializeField]
    private SpawnData[] _spawnData;
    [SerializeField]
    private float _levelTime;

    private int _level;
    private float _timer;

    private void Start()
    {
        _spawnPoint = GetComponentsInChildren<Transform>();
        _levelTime = GameManager.Instance.MaxGameTime / _spawnData.Length;
    }

    private void Update()
    {
        if (!GameManager.Instance.IsLive) return;

        _timer += Time.deltaTime;
        _level = Mathf.Min(Mathf.FloorToInt(GameManager.Instance.GameTime / 10f), _spawnData.Length - 1);

        if (_timer > _spawnData[_level].spawnTime)
        {
            _timer = 0;
            Spawn();
        }

    }

    private void Spawn()
    {
        GameObject enemy = GameManager.Instance.PoolManager.Get(0);
        enemy.transform.position = _spawnPoint[Random.Range(1, _spawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().Init(_spawnData[_level]);
    }
}
[System.Serializable]
public class SpawnData
{
    public int spriteType;
    public float spawnTime;
    public int health;
    public float speed;
}

