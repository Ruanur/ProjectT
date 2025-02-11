using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Player : MonoBehaviour
{
    private Vector2 _inputVector;
    public Vector2 InputVector
    {
        get { return _inputVector; }
    }
    [SerializeField]
    private float _speed;
    public float Speed { get { return _speed; } set { _speed = value;  } }

    private Scanner _scanner;
    public Scanner Scanner { get { return _scanner; } }
    private Hand[] _hands;
    public Hand[] Hands { get { return _hands; } }
    [SerializeField]
    private RuntimeAnimatorController _runtimeAnimatorController;
    public RuntimeAnimatorController RuntimeAnimatorController { get { return _runtimeAnimatorController; } }

    [SerializeField]
    private Characters _characters;

    private Rigidbody2D _rigidbody2D;
    [SerializeField]
    private Animator _animator;

    private Vector3 _left = new Vector3(1, 1, 1);
    private Vector3 _right = new Vector3(-1, 1, 1);
    private Vector3 _spawnPos = new Vector3(0, -0.4f, 0);
    private GameObject _character;
    private AsyncOperationHandle<GameObject> _handle;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _scanner = GetComponent<Scanner>();
        //_hands = GetComponentsInChildren<Hand>(true);
    }
    IEnumerator Start()
    {
        yield return _handle = _characters._assetReferences[1].LoadAssetAsync<GameObject>();
        _handle.Completed += HandleCompleted;
        

    }
    private void HandleCompleted(AsyncOperationHandle<GameObject> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            _character = (GameObject)Instantiate(obj.Result, _spawnPos, Quaternion.identity,transform);
            _animator = GetComponentInChildren<Animator>();
            _animator.runtimeAnimatorController = _runtimeAnimatorController;
        }
        else
        {
            Debug.LogError($"AssetReference {_characters._assetReferences[1].RuntimeKey} failed to load.");
        }
    }

    private void OnEnable()
    {
        _speed *= Character.Speed;
    }
    private void FixedUpdate()
    {
        if (!GameManager.Instance.IsLive) return;

        Vector2 nextVec = _inputVector * _speed * Time.fixedDeltaTime;
        _rigidbody2D.MovePosition(_rigidbody2D.position + nextVec);
    }
    private void LateUpdate()
    {
        if (!GameManager.Instance.IsLive) return;

        if (_animator != null) _animator.SetFloat("Speed", _inputVector.magnitude);
        if (_inputVector.x != 0)
        {
            if (_inputVector.x < 0)
            {
                _character.transform.localScale = _left;
            }
            else
            {
                _character.transform.localScale = _right;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.Instance.IsLive) return;

        GameManager.Instance.Health -= Time.deltaTime * 10;

        if (GameManager.Instance.Health < 0)
        {
            for (int i = 2; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            _animator.SetTrigger("Dead");
            GameManager.Instance.GameOver();
        }
    }

    private void OnMove(InputValue inputValue)
    {
        _inputVector = inputValue.Get<Vector2>();
    }
}
