using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;
    public TMP_Text LoadingPercentage;

    public void Loading(int sceneIndex)
    {
        StartCoroutine(LoadAsynchornously(sceneIndex));
    }

    IEnumerator LoadAsynchornously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            Debug.Log(progress);

            slider.value = progress;

            LoadingPercentage.text = progress * 100f + "%";

            yield return null;
        }
    }

    public void quitGame()
    {
        Application.Quit();
        Debug.Log("Quit game");
    }
}
