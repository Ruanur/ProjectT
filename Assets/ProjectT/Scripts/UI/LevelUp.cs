using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    private RectTransform _rectTransform;
    private Item[] _items;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _items = GetComponentsInChildren<Item>(true);
    }

    public void Show()
    {
        Next();
        _rectTransform.localScale = Vector3.one;
        GameManager.Instance.Stop();
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.LevelUp);
        AudioManager.Instance.EffectBgm(true);
    }
    public void Hide()
    {
        _rectTransform.localScale = Vector3.zero;
        GameManager.Instance.Resume();
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Select);
        AudioManager.Instance.EffectBgm(false);
    }
    public void Select(int index)
    {
        _items[index].OnClick();
    }
    // ���� �κ� Ȯ�� �� �������� �κ� ���� �ʿ� 
    private void Next()
    {
        // ��� ������ �� Ȱ��ȭ 
        foreach (Item item in _items) 
        {
            item.gameObject.SetActive(false);
        }
        // ���� 3�� 
        int[] rand = new int[3];
        while (true)
        {
            rand[0] = Random.Range(0, _items.Length);
            rand[1] = Random.Range(0, _items.Length);
            rand[2] = Random.Range(0, _items.Length);

            if (rand[0] != rand[1] && rand[1] != rand[2] && rand[0] != rand[2])
            {
                break;
            }
        }
        for (int i = 0; i < rand.Length ; i++)
        {
            Item randItem = _items[rand[i]];

            // ������
            if (randItem.Level == randItem.Data.damages.Length)
            {
                _items[4].gameObject.SetActive(true);
            }
            else
            {
                randItem.gameObject.SetActive(true);
            }
        }
    }
}
