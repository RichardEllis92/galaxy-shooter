using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
   [SerializeField]
    private bool _isGameOver;
    public bool isCoopMode = false;
    private SpawnManager _spawnManager;
    [SerializeField]
    private GameObject _pauseMenu;

    private Animator _PauseAnim;

    private void Start()
    {
        _PauseAnim = GameObject.Find("Pause_Menu_Panel").GetComponent<Animator>();
        _PauseAnim.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    private void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true && sceneName == "Single_Player")
        {
            SceneManager.LoadScene(1); //Single Player Game Scene
        }    
        else if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true && sceneName == "Co-Op_Mode")
        {
            SceneManager.LoadScene(2); //Coop Game Scene
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (sceneName != "Main_Menu")
            {
                SceneManager.LoadScene(0); //Main Menu Scene
            }
            else
            {
                Application.Quit();
            }
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (Time.timeScale == 1)
            {

                Time.timeScale = 0;
                _pauseMenu.SetActive(true);
                _PauseAnim.SetBool("isPaused", true);
            }
        }

    }
    public void GameOver()
    {
        _isGameOver = true;
    }    

    public void ResumeGame()
    {
        Time.timeScale = 1;
        _pauseMenu.SetActive(false);
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1;
        _pauseMenu.SetActive(false);
        SceneManager.LoadScene(0);
    }    
}
