using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AchiveManager : MonoBehaviour
{
    enum Achive { UnlockPotato , UnlockBean }

    [SerializeField]
    private GameObject[] _lockCharacter;
    public GameObject[] LockCharacter { get { return _lockCharacter; } }
    [SerializeField]
    private GameObject[] _unlockCharacter;
    public GameObject[] UnlockCharacter { get { return _unlockCharacter; } }
    [SerializeField]
    private GameObject _uiNotice;

    private Achive[] _achives;
    private WaitForSecondsRealtime _wait;

    private void Awake()
    {
        _achives = (Achive[])Enum.GetValues(typeof(Achive));
        _wait = new WaitForSecondsRealtime(5);
        if (!PlayerPrefs.HasKey("MyData"))
        { 
            Init();
        }
    }
    private void Init()
    {
        PlayerPrefs.SetInt("MyData", 1);

        foreach (var item in _achives)
        {
            PlayerPrefs.SetInt(item.ToString(), 0);

        }
    }
    private void Start()
    {
        UnlockCharacterLoad();
    }
    private void UnlockCharacterLoad()
    {
        for (int i = 0; i < _lockCharacter.Length; i++)
        {
            string achiveName = _achives[i].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;
            _lockCharacter[i].SetActive(!isUnlock);
            _unlockCharacter[i].SetActive(isUnlock);
        }
    }
    private void LateUpdate()
    {
        foreach (var item in _achives)
        {
            CheckAchive(item);
        }
    }
    void CheckAchive(Achive achive)
    {
        bool isAchive = false;

        switch (achive) 
        {
            case Achive.UnlockPotato:
                isAchive = GameManager.Instance.Kill >= 10;
                break;
            case Achive.UnlockBean:
                isAchive = GameManager.Instance.GameTime == GameManager.Instance.MaxGameTime;
                break;
        }

        if (isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0) 
        {
            PlayerPrefs.SetInt(achive.ToString(), 1);

            for (int i = 0; i < _uiNotice.transform.childCount; i++)
            {
                bool isActive = i == (int)achive;
                _uiNotice.transform.GetChild(i).gameObject.SetActive(isActive);
            }

            StartCoroutine(NoticeRoutine());
        }
    }
    private IEnumerator NoticeRoutine()
    {
        _uiNotice.SetActive(true);
        yield return _wait;
        _uiNotice.SetActive(false);
    }
}
