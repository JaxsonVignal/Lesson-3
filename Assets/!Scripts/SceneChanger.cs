// Gives access to Unity's core types like MonoBehaviour
using UnityEngine;
// Gives access to SceneManager, which loads scenes
using UnityEngine.SceneManagement;

// Script that loads scenes; added to an object so buttons can call it
public class SceneChanger : MonoBehaviour
{

    public string sceneName;
    public void ChangeScene()
    {
        
        // Unloads the current scene and loads the scene that was passed in
        SceneManager.LoadScene(sceneName);
    }
}