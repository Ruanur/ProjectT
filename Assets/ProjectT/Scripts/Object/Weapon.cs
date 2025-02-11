using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private int _id;
    public int ID { get { return _id; } }
    [SerializeField]
    private int _prefabId;
    public int PrefabId { get { return _prefabId; } }
    [SerializeField]
    private float _damage;
    public float Damage { get { return _damage; } }
    [SerializeField]
    private int _count;
    public int Count { get { return _count; } }
    [SerializeField]
    private float _speed;
    public float Speed { get { return _speed; } set { _speed = value; } }
    private float _timer;
    private Player _player;
    private void Awake()
    {
        _player = GameManager.Instance.Player;
    }

    private void Update()
    {
        if (!GameManager.Instance.IsLive) return;

        switch (_id)
        {
            case 0:
                transform.Rotate(Vector3.back * _speed * Time.deltaTime);
                break;
            default:
                _timer += Time.deltaTime;

                if (_timer > _speed)
                {
                    _timer = 0f;
                    Fire();
                }
                break;
        }

        if (Input.GetButtonDown("Jump"))
        {
            LevelUp(10, 1);
        }
    }

    public void LevelUp(float damage, int count)
    {
        _damage = damage * Character.Damage;
        _count += count;

        if (_id == 0)
        {
            Batch();
        }
        _player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void Init(ItemData data)
    {
        // Basic Set
        name = "Weapon "  + data.itemId;
        transform.parent = _player.transform;
        transform.localPosition = Vector3.zero;

        // Property Set
        _id = data.itemId;
        _damage = data.baseDamage * Character.Damage;
        _count = data.baseCount + Character.Count;

        for (int i = 0; i < GameManager.Instance.PoolManager.Prefabs.Length ; i++)
        {
            if (data.projectile == GameManager.Instance.PoolManager.Prefabs[i])
            {
                _prefabId = i;
                break;
            }
        }

        switch (_id)
        {
            case 0:
                _speed = 150 * Character.WeaponSpeed;
                Batch();
                break;
            default:
                _speed = 0.3f * Character.WeaponRate;
                break;
        }
        // Hand Set
        //Hand hand = _player.Hands[(int)data.itemType];
        //hand.SpriteRenderer.sprite = data.hand;
        //hand.gameObject.SetActive(true);

        _player.BroadcastMessage("ApplyGear",SendMessageOptions.DontRequireReceiver);
    }

    private void Batch()
    {
        for (int i = 0; i < _count; i++)
        {
            Transform bullet;

            if (i < transform.childCount)
            {
                bullet = transform.GetChild(i);
            }
            else
            {
                bullet = GameManager.Instance.PoolManager.Get(_prefabId).transform;
                bullet.parent = transform;
            }
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * i / Count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            bullet.GetComponent<Bullet>().Init(_damage, -100, Vector3.zero); // -1 is Infinity per
        }
    }
    private void Fire()
    {
        if (!_player.Scanner.NearestTarget) return;

        Vector3 targetPos = _player.Scanner.NearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Transform bullet = GameManager.Instance.PoolManager.Get(_prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(_damage, _count, dir);
    }
}
