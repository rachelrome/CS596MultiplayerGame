using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Enemy_Behavior : NetworkBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;
    private AudioSource source;

    public GameObject enemyPrefab;

    [SerializeField] private int health = 5;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float fireRate;
    [SerializeField] private bool canShoot = false;
    [SerializeField] private bool canWalk = false;
    [SerializeField] private int dir = 1;

    public Transform firePointR;
    public Transform firePointL;
    public GameObject projectile;

    public float changeDirChance = 1f;
    public float hitNoiseChance = 1f;

    public AudioClip hitNoise1;
    public AudioClip hitNoise2;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();

        //Destroy(gameObject, 30);

        if (Random.Range(0f, 1f) < changeDirChance)
        {
            ChangeDirection();
        }


        if (!canShoot)
        {
            return;
        }
        else
        {
            fireRate = fireRate + (Random.Range(fireRate / -2, fireRate / 2));
            InvokeRepeating("Shoot", 0.2f, fireRate);

        }

    }

    // Update is called once per frame
    void Update()
    {
        if (canWalk)
        {
            if (!sprite.flipX)
            {
                rb.velocity = new Vector2(walkSpeed * -dir, 0);
            }
            else
            {
                rb.velocity = new Vector2(walkSpeed * dir, 0);
            }
        }
        else
        {
            anim.SetBool("attacking", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerMovement>().Damage();
        }
    }

    public IEnumerator Flash()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }

    [ServerRpc]
    public void DieServerRPC()
    {
        anim.SetTrigger("death");
        NetworkObjectPool.Singleton.ReturnNetworkObject(NetworkObject, enemyPrefab);
        NetworkObject.Despawn();

        //Destroy(gameObject, 0.30f);
    }

    public void Damage()
    {
        health--;
        float randomNum = Random.Range(0f, 1f);
        if (randomNum < hitNoiseChance)
        {
            source.PlayOneShot(hitNoise1);
        }
        else if (randomNum > (1 - hitNoiseChance)){
            source.PlayOneShot(hitNoise2);
        }
        
        StartCoroutine(Flash());

        if (health == 0)
        {
            DieServerRPC();
        }
    }

    void Shoot()
    {
        if (!sprite.flipX)
        {
            GameObject temp = (GameObject)Instantiate(projectile, firePointL.position, firePointL.rotation);
            temp.GetComponent<Projectile>().ChangeDirection();
            anim.SetBool("attacking", true);

        }
        else
        {
            GameObject temp = (GameObject)Instantiate(projectile, firePointR.position, firePointR.rotation);
            anim.SetBool("attacking", true);
        }

    }

    public void ChangeDirection()
    {
        if (sprite.flipX)
        {
            sprite.flipX = false;
        }
        else
        {
            sprite.flipX = true;
        }
    }

    public void ResetEnemy()
    {
        health = 4;
    }

}
