using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public Slider progressBar;

    void Start()
    {
        StartCoroutine(LoadAsyncOpearation());
    }

    IEnumerator LoadAsyncOpearation()
    {
        AsyncOperation gameLevel = SceneManager.LoadSceneAsync(MultiplayerSettings.multiplayerSettings.multiplayerScene);

        while (gameLevel.progress < 1.0f)
        {
            progressBar.value = gameLevel.progress;
            yield return new WaitForEndOfFrame();
        }
    }
}