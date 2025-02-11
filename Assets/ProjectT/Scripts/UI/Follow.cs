using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>(); 
    }

    private void FixedUpdate()
    {
        _rectTransform.position = Camera.main.WorldToScreenPoint
            (GameManager.Instance.Player.transform.position);

    }
}
