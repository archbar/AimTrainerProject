using System.Collections;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    [SerializeField] private GameObject HUD;
    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private GameObject player; // Reference to the player GameObject

    // Reference to the GunManager script
    private FPSGunController FPSgunController;
    // Reference to the CameraController script
    private FPSCameraController FPScameraController;

    private bool isPaused;

    private void Start()
    {
        SetPauseMenuState(false);
        SetHUDState(true);
        CursorControl();

        // Get references to the GunManager and CameraController scripts on the player object
        if (player != null)
        {
            FPSgunController = player.GetComponentInChildren<FPSGunController>();
            FPScameraController = player.GetComponentInChildren<FPSCameraController>();
        }
        else
        {
            Debug.LogError("Player reference not set in the inspector!");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void PauseGame()
    {
        SetPauseMenuState(true);
        SetHUDState(false);
        Time.timeScale = 0f;
        isPaused = true;
        CursorOff();
        
        if (FPSgunController != null)
        {
            FPSgunController.enabled = false;
        }

        if (FPScameraController != null)
        {
            FPScameraController.enabled = false;
        }
    }

    private void ResumeGame()
    {
        SetPauseMenuState(false);
        SetHUDState(true);
        Time.timeScale = 1f;
        isPaused = false;
        CursorControl();

        
        if (FPSgunController != null)
        {
            FPSgunController.enabled = true;
        }

        if (FPScameraController != null)
        {
            FPScameraController.enabled = true;
        }
    }

    private void CursorControl()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void CursorOff()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void SetPauseMenuState(bool state)
    {
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(state);
        }
    }

    private void SetHUDState(bool state)
    {
        if (HUD != null)
        {
            HUD.SetActive(state);
        }
    }
}
