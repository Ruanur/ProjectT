using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float _speed;
    public float _health;
    public float _maxHealth;
    public RuntimeAnimatorController[] _controller;

    private bool _isLive = true;

    private Rigidbody2D _target;
    private Rigidbody2D _rigidbody2D;
    private Collider2D _collider;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private WaitForFixedUpdate _waitForFixedUpdate;

    private void Awake()
    {
        _rigidbody2D  = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _waitForFixedUpdate = new WaitForFixedUpdate();
    }

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.IsLive) return;

        if (!_isLive || _animator.GetCurrentAnimatorStateInfo(0).IsName("Hit")) return;

        Vector2 dirVec = _target.position - _rigidbody2D.position;
        Vector2 nextVec = dirVec.normalized * _speed * Time.fixedDeltaTime;

        _rigidbody2D.MovePosition(_rigidbody2D.position + nextVec);
        _rigidbody2D.velocity = Vector2.zero;
    }

    private void LateUpdate()
    {
        if (!GameManager.Instance.IsLive) return;

        if (!_isLive) return;

        _spriteRenderer.flipX = _target.position.x < _rigidbody2D.position.x;
    }

    private void OnEnable()
    {
        _target = GameManager.Instance.Player.GetComponent<Rigidbody2D>();
        _isLive = true;
        _collider.enabled = true;
        _rigidbody2D.simulated = true;
        _spriteRenderer.sortingOrder = 2;
        _animator.SetBool("Dead", false);
        _health = _maxHealth;
        
    }
    public void Init(SpawnData data)
    {
        _animator.runtimeAnimatorController = _controller[data.spriteType];
        _speed = data.speed;
        _maxHealth = data.health;
        _health = data.health;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !_isLive)
        {
            return;
        }
        _health -= collision.GetComponent<Bullet>().Damage;
        StartCoroutine(KnockBack());
        if (_health > 0)
        {
            _animator.SetTrigger("Hit");
            AudioManager.Instance.PlaySfx(AudioManager.Sfx.Hit);
        }
        else
        {
            _isLive = false;
            _collider.enabled = false;
            _rigidbody2D.simulated = false;
            _spriteRenderer.sortingOrder = 1; 
            _animator.SetBool("Dead", true);
            GameManager.Instance.Kill++;
            GameManager.Instance.GetExp();

            if (GameManager.Instance.IsLive)
                AudioManager.Instance.PlaySfx(AudioManager.Sfx.Dead);
        }
    }

    private IEnumerator KnockBack()
    {

        yield return _waitForFixedUpdate;
        Vector3 playerPos = GameManager.Instance.Player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        _rigidbody2D.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }

    private void Daad()
    {
        gameObject.SetActive(false);
    }
}
