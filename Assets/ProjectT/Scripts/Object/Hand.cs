using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField]
    private bool _isLeft;
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    public SpriteRenderer SpriteRenderer { get { return _spriteRenderer; } }

    private SpriteRenderer _spriteRendererPlayer;
    private Vector3 _rightPos = new Vector3(0.35f, -0.15f, 0);
    private Vector3 _rightPosReverse = new Vector3(-0.15f, -0.15f, 0);
    private Quaternion _leftRot = Quaternion.Euler(0, 0, -35);
    private Quaternion _leftRotReverse = Quaternion.Euler(0, 0, -135);
    

    private void Awake()
    {
        _spriteRendererPlayer = GetComponentsInParent<SpriteRenderer>()[1];
    }

    private void LateUpdate()
    {
        bool isReverse = _spriteRendererPlayer.flipX;
        if (_isLeft)
        {
            transform.localRotation = isReverse ? _leftRotReverse : _leftRot;
            _spriteRenderer.flipY = isReverse;
            _spriteRenderer.sortingOrder = isReverse ? 4 : 6;
        }
        else
        {
            transform.localPosition = isReverse ? _rightPosReverse : _rightPos;
            _spriteRenderer.flipX = isReverse;
            _spriteRenderer.sortingOrder = isReverse ? 6 : 4;
        }
    }
}
