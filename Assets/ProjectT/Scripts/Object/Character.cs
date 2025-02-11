using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private static CharacterInfo characterData;
    public static CharacterInfo CharacterData
    {
        get
        {
            if (characterData == null)
            {
                int index = PlayerPrefs.GetInt("CharacterIndex");
                characterData = Resources.Load<CharacterInfo>($"Character{index}");
            }
            return characterData;
        }
    }

    public static float Speed
    {
        get 
        {
            return CharacterData.moveSpeed;
        }
    }
    public static float WeaponSpeed
    {
        get
        {
            return CharacterData.attackSpeed;
        }
    }
    public static float WeaponRate // �̰� �߰� �ؾ���
    {
        get
        {
            return CharacterData.attackSpeed;
        }
    }
    public static float Damage
    {
        get
        {
            return CharacterData.damage;
        }
    }
    public static int Count // �߻�ü ���� �߰� �ؾ���
    {
        get
        {
            return 1;
        }
    }
}
