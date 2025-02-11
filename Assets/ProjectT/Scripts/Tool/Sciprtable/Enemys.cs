using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "Enemys", menuName = "Scriptable Object/Enemys")]
public class Enemys : ScriptableObject
{
    public AssetReference[] _assetReferences;
}
