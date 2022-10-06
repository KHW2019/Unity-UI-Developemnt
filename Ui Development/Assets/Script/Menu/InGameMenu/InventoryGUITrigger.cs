using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryGUITrigger : MonoBehaviour
{
    [Header("Reference")]
    public GameObject InventoryGUI;
    public GameObject CrossHair;

    [Header("Input")]
    public KeyCode InventoryTrigger = KeyCode.B;

    private static bool isOpen = false;

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        InventoryStatus();
    }

    void InventoryStatus()
    {
        if (Input.GetKeyDown(InventoryTrigger))
        {
            if (isOpen)
            {
                CloseInventory();
            }
            else
            {
                OpenInventory();
            }
        }
    }

    public void CloseInventory()
    {
        InventoryGUI.SetActive(false);
        CrossHair.SetActive(true);
        Time.timeScale = 1f;
        isOpen = false;
        CusorLocked();
    }

    void OpenInventory()
    {
        InventoryGUI.SetActive(true);
        CrossHair.SetActive(false);
        Time.timeScale = 0f;
        isOpen = true;
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
}
