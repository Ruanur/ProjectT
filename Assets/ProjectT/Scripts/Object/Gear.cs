using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    [SerializeField]
    private ItemData.ItemType _itemType;
    [SerializeField]
    private float _rate;

    public void Init(ItemData data)
    {
        // Basic Set
        name = "Gear " + data.itemId;
        transform.parent = GameManager.Instance.Player.transform;
        transform.localPosition = Vector3.zero;

        // Property Set
        _itemType = data.itemType;
        _rate = data.damages[0];
        ApplyGear();
    }

    public void LevelUp(float rate)
    {
        _rate = rate;
        ApplyGear();
    }

    void ApplyGear()
    {
        switch (_itemType) 
        {
            case ItemData.ItemType.Glove:
                RateUp();
                break;
            case ItemData.ItemType.Shoe:
                SpeedUp();
                break;
        }
    }

    private void RateUp()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach (Weapon weapon in weapons) 
        {
            switch (weapon.ID)
            {
                case 0:
                    float speed = 150 * Character.WeaponSpeed;
                    weapon.Speed = speed + (speed * _rate);
                    break;
                default:
                    speed = 0.5f * Character.WeaponRate;
                    weapon.Speed = speed * (1f - _rate);
                    break;
            }
        }
    }
    private void SpeedUp()
    {
        float speed = 3 * Character.Speed;
        GameManager.Instance.Player.Speed = speed + speed * _rate;
    }
}
