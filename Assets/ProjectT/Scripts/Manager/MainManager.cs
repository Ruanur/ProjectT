using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    private static MainManager _instance;
    public static MainManager Instance { get { return _instance; } }

    private void Awake()
    {
        _instance = this;
    }

}
