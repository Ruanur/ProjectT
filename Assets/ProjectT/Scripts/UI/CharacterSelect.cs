using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

public class CharacterSelect : MonoBehaviour
{
    [SerializeField]
    private Characters _characters;
    private int _characterIndex;
    private GameObject _character;

    private void Start()
    {
        _characterIndex = PlayerPrefs.GetInt("CharacterIndex", 0);
        StartCoroutine(ShowCharacter());
    }

    public void Next()
    {
        _characterIndex++;
        if (_characterIndex >= _characters._assetReferences.Length) _characterIndex = 0;
        StartCoroutine(ShowCharacter());
    }
    public void Prev()
    {
        _characterIndex--;
        if (_characterIndex < 0) _characterIndex = _characters._assetReferences.Length - 1;
        StartCoroutine(ShowCharacter());
    }
    private IEnumerator ShowCharacter()
    {
        if (_character != null)
        {
            Destroy(_character);
        }
        AsyncOperationHandle<GameObject> handle;
        yield return handle = _characters._assetReferences[_characterIndex].LoadAssetAsync<GameObject>();
        if (handle.IsValid())
            handle.Completed += HandleCompleted;
        Addressables.Release(handle);
    }
    private void HandleCompleted(AsyncOperationHandle<GameObject> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            _character = (GameObject)Instantiate(obj.Result,transform);
        }
        else
        {
            Debug.LogError($"AssetReference {obj.Result} failed to load.");
        }
    }
}
