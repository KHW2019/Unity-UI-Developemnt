using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    
    [Header("Reference")]
    public GameObject pauseMenuUI;

    [Header("Input")]
    public KeyCode pauseGameTirgger = KeyCode.Escape;

    private static bool isPause = false;

    // Update is called once per frame
    void Update()
    {
        PauseMenuStatus();
    }

    void PauseMenuStatus()
    {
        if (Input.GetKeyDown(pauseGameTirgger))
        {
            if (isPause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPause = false;
        CusorLocked();
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPause = true;
        CusorUnlocked();
    }

    void CusorUnlocked()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    void CusorLocked()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void backToMenu(int sceneIndex)
    {
        SceneManager.LoadSceneAsync(sceneIndex);
        Time.timeScale = 1f;
    }
}
