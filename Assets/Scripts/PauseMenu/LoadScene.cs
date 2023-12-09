using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour 
{
    [SerializeField] private int sceneID;
    [SerializeField] private int seconds = 1;
    [SerializeField] private int finalTimeScale = 1;
    [SerializeField] private GameObject loadingScreen;

    public void LoadSceneWithLoadingScreen()
    {
        loadingScreen.SetActive(true);

        StartCoroutine(LoadAsync());
    }

    private IEnumerator LoadAsync()
    {
        yield return new WaitForSecondsRealtime(seconds);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneID);

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.allowSceneActivation)
            {
                Time.timeScale = finalTimeScale;
            }
            yield return null;
        }
    }
}


