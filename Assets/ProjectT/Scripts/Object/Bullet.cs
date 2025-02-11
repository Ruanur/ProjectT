using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float _damage;
    public float Damage { get { return _damage; } }
    [SerializeField]
    private int _per;
    public int Per { get { return _per; } }

    private Rigidbody2D _rigidbody2;

    private void Awake()
    {
        _rigidbody2 = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir)
    {
        _damage = damage;
        _per = per;

        if (per >= 0)
        {
            _rigidbody2.velocity = dir * 15f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || _per == -100) return;

        _per--;
        if (_per < 0)
        {
            _rigidbody2.velocity = Vector3.zero;
            gameObject.SetActive(false);
        } 
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area") || _per == -100) return;

        gameObject.SetActive(false);
    }
}
