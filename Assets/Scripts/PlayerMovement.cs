using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : NetworkBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim;
    private SpriteRenderer sprite;
    private AudioSource source;
    //private GameMaster gm;

    [SerializeField] private LayerMask jumpableGround;

    private float dirX = 0f;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;

    [SerializeField] private Transform firePointR;
    [SerializeField] private Transform firePointL;
    [SerializeField] private GameObject projectile;
    private int delay = 0;

    [SerializeField] private int health = 5;

    public AudioClip hitSound;
    public AudioClip jumpSound;

    private enum MovementState { idle, walking, jumping, falling, punching, kicking, crouching, crouchKicking, flyKicking}

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();

        if (IsOwner)
        {
            GameObject.Find("Main Camera").gameObject.transform.parent = this.transform;
        }
        //gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();

        //transform.position = gm.GetLastCheckpointPos();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        //Get left and right input
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        //Jump logic
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, 0);
            source.PlayOneShot(jumpSound);
        }

        //Crouch Logic
        if (Input.GetButton("Crouch"))
        {
            coll.enabled = false;
        }
        else
        {
            coll.enabled = true;
        }

        //Punching Logic
        if (Input.GetButtonDown("Fire1") && delay>35)
        {
            ShootServerRPC();
        }

        //Fire delay
        delay++;

       UpdateAnimation();

    }
  
    private void UpdateAnimation()
    {
        MovementState state;

        //Left/Right walking
        //No movement is idle
        if (dirX > 0f)
        {
            state = MovementState.walking;
            sprite.flipX = false;
        }
        else if (dirX < 0)
        {
            state = MovementState.walking;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        //If the velocity moves in the y direction it is jumping or falling
        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }

        //if the collider is disabled, the player crouches
        if (!coll.enabled)
        {
            state = MovementState.crouching;
        }

        anim.SetInteger("state", (int)state);

    }

    [ServerRpc]
    private void ShootServerRPC()
    {
        delay = 0;

        //if the player faces left, bullets are instantiated on left side, right if player is right
        if (sprite.flipX)
        {
            GameObject temp = Instantiate(projectile, firePointL.position, firePointL.rotation);
            temp.GetComponent<NetworkObject>().Spawn();
            temp.GetComponent<Projectile>().ChangeDirection();

            anim.SetTrigger("punching");

        }
        else
        {
            GameObject temp = Instantiate(projectile, firePointR.position, firePointR.rotation);
            temp.GetComponent<NetworkObject>().Spawn();

            anim.SetTrigger("punching");

        }
    }

    //check is the player is on the ground
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    //Lower health
    public void Damage()
    {
        //health--;
        GetComponent<NetworkHealthDisplay>().HealthPoint.Value -= 5;
        anim.SetTrigger("hurt");
        source.PlayOneShot(hitSound);
        //if (health == 0)
        //{
        //Die();
        //}
    }

    
    //Death logic
    public void Die()
    {
        Destroy(gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //Get health to display to canvas
    public int GetHealth()
    {
        return health;
    }
   
}
