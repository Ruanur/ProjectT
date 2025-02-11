using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "Characters", menuName = "Scriptable Object/Characters")]
public class Characters : ScriptableObject
{
    public AssetReference[] _assetReferences;
}
