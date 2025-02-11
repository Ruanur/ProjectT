using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _prefabs;
    public GameObject[] Prefabs { get { return _prefabs; } }

    private List<GameObject>[] _pools;
    
    private void Awake()
    {
        int length = _prefabs.Length;
        _pools = new List<GameObject>[length];
        for (int i = 0; i < length; i++)
        {
            _pools[i] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;
        
        foreach (GameObject item in _pools[index]) 
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        if (!select)
        {
            select = Instantiate(_prefabs[index], transform);
            _pools[index].Add(select);
        }

        return select;
    }
}
