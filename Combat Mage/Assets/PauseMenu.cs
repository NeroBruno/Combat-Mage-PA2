using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    /*
     * This variable keeps track wether or not our Game is Paused
     * public - accessible from other scripts & static - we don't want to reference this specific PauseMenu script, we just want to be able to very easily check,
     * from other scripts, wether or not the game is currently Paused
     */
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume ()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // 1f is the normal rate game speed passes by
        GameIsPaused = false;
    }

    void Pause ()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // we can add slowmotion effects with timeScale, or completely freeze the game if set to 0f
        GameIsPaused = true;
    }

    public void LoadMenu ()
    {
        Debug.Log("Loading menu...");
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // create a variable for this and not hard code it
    }

    public void QuitGame ()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
