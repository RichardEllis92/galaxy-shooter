using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public bool isPlayerOne = false;
    public bool isPlayerTwo = false;
    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private float _speedMutliplier = 2;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    [SerializeField]
    private bool _isTripleShotActive = false;
    [SerializeField]
    private GameObject _tripleshotPrefab;
    [SerializeField]
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;
    [SerializeField]
    private GameObject _ShieldVisualiser;
    [SerializeField]
    private int _score;
    private int _scoreCoop;
    private UIManager _uiManager;
    [SerializeField]
    private GameObject _RightEngine;
    [SerializeField]
    private GameObject _LeftEngine;
    [SerializeField]
    private AudioClip _laserSoundClip;
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _explosionSoundClip;
    [SerializeField]
    private AudioClip _powerupSoundClip;
    private GameManager _gameManager;
    private int _bestScore;
    public Text _bestscoreText;
    private int _bestScoreCoop;
    public Text _bestscoreCoopText;
    void Start()
    {

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (_gameManager.isCoopMode == false)
        {
            transform.position = new Vector3(0, 0, 0);
        }

        if (sceneName == "Single_Player")
        {
            _bestScore = PlayerPrefs.GetInt("BestScore", 0);
            _bestscoreText.text = "Best: " + _bestScore;
        }
        if (sceneName == "Co-Op_Mode")
        {
            _bestScoreCoop = PlayerPrefs.GetInt("BestScoreCoop", 0);
            _bestscoreCoopText.text = "Best: " + _bestScoreCoop;
        }
    }

    void Update()
    {
        if (isPlayerOne == true)
        {
            CalculateMovementPlayer1();
        }
        else if (isPlayerTwo == true)
        {
            CalculateMovementPlayer2();
        }

        if (isPlayerOne == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
               
                if(Time.time > _canFire)
                {
                    FireLaser();
                }

        }
        
        else if (isPlayerTwo == true)
        {
            if (Input.GetKeyDown(KeyCode.Keypad0))
            
            if(Time.time > _canFire)
            {
                FireLaser();
            }
   
        }
    }

    void CalculateMovementPlayer1()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); // Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical"); //Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x >= 11.3)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11.3)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    void CalculateMovementPlayer2()
    {
        float horizontalInput = Input.GetAxis("Horizontal2"); // Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical2"); //Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x >= 11.3)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11.3)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        
        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleshotPrefab, transform.position + new Vector3(0.83f, 1.05f, -25.0916f), Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }

        _audioSource.PlayOneShot(_laserSoundClip);

    }

    public void Damage()
    {
        
        
        if (_isShieldActive == true)
        {
            _isShieldActive = false;
            _ShieldVisualiser.SetActive(false);
            return;
        }


        _lives --; //subtracts 1 from lives

        if (_lives == 2)
        {
            _RightEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _LeftEngine.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);

        if(_lives < 1)
        {
            _audioSource.PlayOneShot(_explosionSoundClip);
            _spawnManager.OnPlayerDeath();
            CheckBestScore();
            Destroy(this.gameObject);

        }
    }
    
    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        _audioSource.PlayOneShot(_powerupSoundClip);
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _audioSource.PlayOneShot(_powerupSoundClip);
        _speed *= _speedMutliplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        _speed /= _speedMutliplier;
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _audioSource.PlayOneShot(_powerupSoundClip);
        _ShieldVisualiser.SetActive(true);
    }

    public void AddScore(int points)
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (sceneName == "Single_Player")
        {
            _score += points;
            _uiManager.UpdateScore(_score);
        }
        if (sceneName == "Co-Op_Mode")
        {
            _scoreCoop += points;
            _uiManager.UpdateScore(_scoreCoop);
        }


    }

    public void CheckBestScore()
    {
        if (_score > _bestScore)
        {
            _bestScore = _score;
            PlayerPrefs.SetInt("BestScore", _bestScore);
            _bestscoreText.text = "Best: " + _bestScore;
        }
        if (_scoreCoop > _bestScoreCoop)
        {
            _bestScoreCoop = _scoreCoop;
            PlayerPrefs.SetInt("BestScoreCoop", _bestScoreCoop);
            _bestscoreCoopText.text = "Best: " + _bestScoreCoop;
        }
    }
  
}
