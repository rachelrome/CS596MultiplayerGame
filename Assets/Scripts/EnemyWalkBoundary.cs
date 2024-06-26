using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalkBoundary : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy_Behavior>().ChangeDirection();
        }

    }
}
