using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterInfo", menuName = "Scriptable Object/CharacterData")]
public class CharacterInfo : ScriptableObject
{
    public int characterIndex;

    public float moveSpeed;
    public float damage;
    public float attackSpeed;
    public float hp;
}
