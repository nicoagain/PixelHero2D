using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject panelPause;
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Debug.Log("Paused");
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        panelPause.SetActive(true);
        isPaused = true;
    }

    public void Resume()
    {
        Cursor.lockState= CursorLockMode.Locked;
        Time.timeScale = 1;
        panelPause.SetActive(false);
        isPaused = false;
    }
    public void Restart()
    {
        PlayerPrefs.DeleteAll();
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(("MainMenu"));
    }
}
