using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    
    [Header("Reference")]
    public GameObject pauseMenuUI;
    public GameObject OptionMenuUI;
    public GameObject Inventory;

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
           
            if(OptionMenuUI.activeSelf)
            {
                backToMenu();
            }
            else if (isPause && pauseMenuUI.activeSelf)
            {
                Resume();
            }
            else if (Inventory.activeSelf)
            {

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

    public void backToMenu()
    {
        pauseMenuUI.SetActive(true);
        OptionMenuUI.SetActive(false);
        isPause = true;
        Time.timeScale = 0f;
        CusorUnlocked();
    }

    public void Pause()
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
        isPause = false;
    }
}
