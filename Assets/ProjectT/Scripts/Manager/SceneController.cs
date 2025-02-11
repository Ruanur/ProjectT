using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    // 씬을 로드하는 함수
    public void LoadScene(string nextSceneName)
    {
        UnloadScene();
        SceneManager.LoadScene(nextSceneName);
    }

    // 현재 씬을 언로드하는 함수
    public void UnloadScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.UnloadSceneAsync(currentScene);
    }
}
