using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void SceneLoad(string sceneName)
    {
        GameState.Instance.SetPlayer();
        SceneManager.LoadScene(sceneName); 
    }

}
