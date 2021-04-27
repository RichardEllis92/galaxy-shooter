using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;

    void Update()
    {
        Vector3 direction = new Vector3(0, -1, 0);
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
}
