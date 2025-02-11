using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    // ���� �ε��ϴ� �Լ�
    public void LoadScene(string nextSceneName)
    {
        UnloadScene();
        SceneManager.LoadScene(nextSceneName);
    }

    // ���� ���� ��ε��ϴ� �Լ�
    public void UnloadScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.UnloadSceneAsync(currentScene);
    }
}
