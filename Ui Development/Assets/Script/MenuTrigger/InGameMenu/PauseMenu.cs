using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    
    [Header("Reference")]
    public GameObject pauseMenuUI;
    public GameObject OptionMenuUI;
    public GameObject CrossHair;
    public GameObject InventoryUI;
    public InventoryObject inventory;
    public InventoryObject Equipment;

    [Header("Input")]
    public KeyCode pauseGameTirgger = KeyCode.Escape;

    public static bool isPause = false;

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
            else if (InventoryUI.activeSelf)
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
        CrossHair.SetActive(false);
    }

    void CusorLocked()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        CrossHair.SetActive(true);
    }

    public void backToMenu(int sceneIndex)
    {
        SceneManager.LoadSceneAsync(sceneIndex);
        Time.timeScale = 1f;
        isPause = false;
        inventory.Container.Clear();
        Equipment.Container.Clear();
    }
}
