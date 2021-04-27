using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;
    private bool _isEnemyLaser = false;
    void Update()
    {
        if (_isEnemyLaser == false)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }
    }

    void MoveUp()
    {
        Vector3 direction = new Vector3(0, 1, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

        if (transform.position.y > 8f)
        {
            //check if this object has a parent
            //destroy the parent too
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    void MoveDown()
    {
        Vector3 direction = new Vector3(0, -1, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

        if (transform.position.y < -8f)
        {
            //check if this object has a parent
            //destroy the parent too
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _isEnemyLaser == true)
        {
            Player player = other.GetComponent<Player>();
            
            if (player != null)
            {
                player.Damage();
            }
        }
    }
}
