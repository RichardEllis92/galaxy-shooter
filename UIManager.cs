using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //handle to Text
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _scoreCoopText;
    [SerializeField]
    private Image _LivesImg;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Text _GameOverText;
    [SerializeField]
    private Text _RestartText;
    private GameManager _gameManager;
    public int bestScore;

    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _scoreCoopText.text = "Score: " + 0;
        _GameOverText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.LogError("GameManager is NULL");
        }    

    }

    // Update is called once per frame
    void Update()
    {
       // SpawnManager _score = gameObject.GetComponent<SpawnManager>();
       // _scoreText.text = "Score: " + _score;
    }
    public void UpdateScore(int playerScore)
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (sceneName == "Single_Player")
            _scoreText.text = "Score: " + playerScore.ToString();
        if (sceneName == "Co-Op_Mode")
            _scoreCoopText.text = "Score: " + playerScore.ToString();
    }
    
    public void UpdateLives (int currentLives)
    {
        _LivesImg.sprite = _liveSprites[currentLives];

        if(currentLives == 0)
        {

            GameOverSequence();
           
        }
    }

    void GameOverSequence()
    {
        _gameManager.GameOver();
        _GameOverText.gameObject.SetActive(true);
        _RestartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while(true)
        {
            _GameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _GameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }

}


