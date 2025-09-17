using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneManager : MonoBehaviour
{
    public string sceneName = "Level 2";

    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
