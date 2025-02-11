using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [SerializeField]
    private ItemData _data;
    public ItemData Data { get { return _data; } }
    [SerializeField]
    private int _level;
    public int Level { get { return _level; } }
    [SerializeField]
    private Weapon _weapon;
    [SerializeField]
    private Gear _gear;

    private Image _icon;
    private TextMeshProUGUI _textLevel;
    private TextMeshProUGUI _textName;
    private TextMeshProUGUI _textDesc;

    private void Awake()
    {
        _icon = GetComponentsInChildren<Image>()[1];
        _icon.sprite = _data.itemIcon;

        TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
        _textLevel = texts[0];
        _textName = texts[1];
        _textDesc = texts[2];
        _textName.text = _data.itemName;
    }
    private void OnEnable()
    {
        _textLevel.text = "Lv." + (_level + 1);

        switch (_data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                _textDesc.text = string.Format(_data.itemDesc, _data.damages[_level] * 100, _data.counts[_level]);
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                _textDesc.text = string.Format(_data.itemDesc, _data.damages[_level]);
                break;
            default:
                _textDesc.text = _data.itemDesc;
                break;
        }
    }

    public void OnClick()
    {
        switch (_data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                if (_level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    _weapon = newWeapon.AddComponent<Weapon>();
                    _weapon.Init(_data);
                }
                else
                {
                    float nextDamage = _data.baseDamage;
                    int nextCount = 0;
                    // damage and count;
                    nextDamage += _data.baseDamage * _data.damages[_level];
                    nextCount += _data.counts[_level];

                    _weapon.LevelUp(nextDamage, nextCount);
                }
                _level++;
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                if (_level == 0)
                {
                    GameObject newGear = new GameObject();
                    _gear = newGear.AddComponent<Gear>();
                    _gear.Init(_data);
                }
                else
                {
                    float nextRate = _data.damages[_level];
                    _gear.LevelUp(nextRate);
                }
                _level++;
                break;
            case ItemData.ItemType.Heal:
                GameManager.Instance.Health = GameManager.Instance.MaxHealth;
                break;
        }
        if (_level == _data.damages.Length)
        {
            GetComponent<Button>().interactable = false;
        }
    }
}
