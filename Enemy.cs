using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    private Player _player;
    private Animator _anim;
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _explosionSoundClip;
    [SerializeField]
    private GameObject _laserPrefab;
    private float _fireRate = 3.0f;
    private float _canFire = -1;
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();

        if (_player == null)
        {
            Debug.Log("_player is null");
        }
        _anim = GetComponent<Animator>();

        if (_anim == null)
        {
            Debug.Log("The Animator is null");
        }

        if (_audioSource == null)
        {
            Debug.LogError("Audio Source on the Enemy is NULL");
        }
        else
        {
            _audioSource.clip = _explosionSoundClip;
        }
    }
    void Update()
    {
        CalculateMovement();

        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5.7f)
        {
            float randomX = Random.Range(-9.24f, 9.26f);
            transform.position = new Vector3(randomX, 5.97f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }
            _anim.SetTrigger("OnEnemyDeath");
            _audioSource.PlayOneShot(_explosionSoundClip);
            _speed = 0;
            Destroy(this.gameObject, 2.8f);
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(10);
            }
            _anim.SetTrigger("OnEnemyDeath");
            _audioSource.PlayOneShot(_explosionSoundClip);
            Destroy(GetComponent<Collider2D>());
            _speed = 0;
            Destroy(this.gameObject, 2.8f);
        }


    }

}
