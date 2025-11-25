
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader: MonoBehaviour
{
    [Tooltip("Name of the scene to load")]
    public string sceneToLoad;
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
