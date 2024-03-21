using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private int dir = 1;
    [SerializeField] private float projectileSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 2);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(projectileSpeed*dir, 0);
    }
    public void ChangeDirection()
    {
        dir *= -1;
        transform.Rotate(0f, 180f, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy_Behavior>().Damage();
            NetworkObject.Despawn();
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerMovement>().Damage();
            Destroy(gameObject);
        }

    }


}
