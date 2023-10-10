using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D marioBody;
    private Animator marioAnimator;
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;
    public GameObject enemies;
    public GameObject obstacles;
    private Transform gameCamera;
    public AudioSource marioDeath;

    public GameConstants gameConstants;
    float deathImpulse;
    float upSpeed;
    float maxSpeed;
    float speed;
    float maxFallSpeed;
    float defaultGravity;
    float fallGravity;


    // state
    [System.NonSerialized]
    public bool alive = true;

    void  Awake(){
        // subscribe to Game Restart event
        GameManager.instance.gameRestart.AddListener(GameRestart);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Set constants
        speed = gameConstants.speed;
        maxSpeed = gameConstants.maxSpeed;
        deathImpulse = gameConstants.deathImpulse;
        upSpeed = gameConstants.upSpeed;
        maxFallSpeed = gameConstants.maxFallSpeed;
        defaultGravity = gameConstants.defaultGravity;
        fallGravity = gameConstants.fallGravity;

        gameCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        marioSprite = GetComponent<SpriteRenderer>();
        // Set to be 30 FPS
        Application.targetFrameRate = 30;
        marioBody = GetComponent<Rigidbody2D>();

        marioAnimator = GetComponent<Animator>();
        marioAnimator.SetBool("onGround", onGroundState);

        // // subscribe to scene manager scene change
    //     // SceneManager.activeSceneChanged += SetStartingPosition;
    }

    // // public void SetStartingPosition(Scene current, Scene next)
    // // {
    // //     if (next.name == "World 1-2")
    // //     {
    // //         // change the position accordingly in your World-1-2 case
    // //         this.transform.position = new Vector3(-4.39f, -3.38f, 0.0f);
    // //     }
    // // }

    // Update is called once per frame
    void Update()
    {
        marioAnimator.SetFloat("marioSpeed", Mathf.Abs(marioBody.velocity.x));
    }

    void FlipMarioSprite(int value)
    {
        if (value == -1 && faceRightState)
        {
            faceRightState = false;
            marioSprite.flipX = true;
            if (marioBody.velocity.x > 0.05f)
                marioAnimator.SetTrigger("onSkid");

        }

        else if (value == 1 && !faceRightState)
        {
            faceRightState = true;
            marioSprite.flipX = false;
            if (marioBody.velocity.x < -0.05f)
                marioAnimator.SetTrigger("onSkid");
        }
    }

    private bool onGroundState = true;

    // Pick out layers to check collison
    int collisionLayerMask = (1 << 3) | (1 << 6) | (1 << 7);

    // this method of ground detection is removed in favor of a BoxCast so wall jump can be implemented
    // Start of collison 2D
    void OnCollisionEnter2D(Collision2D col)
    {
        if (((collisionLayerMask & (1 << col.transform.gameObject.layer)) > 0) & !onGroundState)
        {
            // onGroundState = Physics2D.BoxCast(transform.position, jumpOverGoomba.boxSize, 0, -transform.up, jumpOverGoomba.maxDistance, collisionLayerMask);
            onGroundState = true;
            marioAnimator.SetBool("onGround", onGroundState);
        }
    }

    private bool moving = false;
    void FixedUpdate()
    {
        if (alive && moving)
        {
            Move(faceRightState == true ? 1 : -1);
        }
        if (marioBody.velocity.y < 0)
        {

            marioBody.gravityScale = fallGravity;
            // Fall speed cap
            marioBody.velocity = new Vector2(marioBody.velocity.x, Mathf.Max(marioBody.velocity.y, -maxFallSpeed));
        }
        else
        {
            marioBody.gravityScale = defaultGravity;
        }
    }

    void Move(int value)
    {

        Vector2 movement = new Vector2(value, 0);
        // check if it doesn't go beyond maxSpeed
        if (marioBody.velocity.magnitude < maxSpeed)
            marioBody.AddForce(movement * speed);
    }

    public void MoveCheck(int value)
    {
        if (value == 0)
        {
            moving = false;
        }
        else
        {
            FlipMarioSprite(value);
            moving = true;
            Move(value);
        }
    }


    // alternative grounded check
    //         onGroundState = Physics2D.BoxCast(transform.position, jumpOverGoomba.boxSize, 0, -transform.up, jumpOverGoomba.maxDistance, collisionLayerMask);
    //         if (onGroundState) { marioAnimator.SetBool("onGround", onGroundState); }

    // wall jump

    //         if (!onGroundState && Input.GetKeyDown("space"))
    //         {
    //             if (Physics2D.BoxCast(transform.position, wallBoxSize, 0, transform.right, 0.5f, wallJumpLayerMask))
    //             {
    //                 marioBody.velocity = Vector2.zero;
    //                 marioBody.AddForce(new Vector2(-10, upSpeed), ForceMode2D.Impulse);
    //             }
    //             else if (Physics2D.BoxCast(transform.position, wallBoxSize, 0, -transform.right, 0.5f, wallJumpLayerMask))
    //             {
    //                 marioBody.velocity = Vector2.zero;
    //                 marioBody.AddForce(new Vector2(10, upSpeed), ForceMode2D.Impulse);
    //             }
    //         }


    private bool jumpedState = false;

    public void Jump()
    {
        if (alive && onGroundState)
        {
            // jump
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            jumpedState = true;
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);

        }
    }

    public void JumpHold()
    {
        if (alive && jumpedState)
        {
            // jump higher
            marioBody.AddForce(Vector2.up * upSpeed * 30, ForceMode2D.Force);
            jumpedState = false;

        }
    }

    public AudioSource marioAudio;

    void PlayJumpSound()
    {
        // play jump sound
        marioAudio.PlayOneShot(marioAudio.clip);
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") && alive)
        {
            // play death animation
            GameManager.instance.MarioDeath();
            marioAnimator.Play("mario-die");
            marioDeath.PlayOneShot(marioDeath.clip);
            alive = false;
        }
    }

    public void GameOverScene()
    {
        GameManager.instance.GameOver();
    }
    void PlayDeathImpulse()
    {
        marioBody.AddForce(Vector2.up * deathImpulse, ForceMode2D.Impulse);
    }
    public void GameRestart()
    {
        // reset position
        marioBody.transform.position = new Vector3(-5.2f, -3.4f, 0.0f);
        // reset sprite direction
        faceRightState = true;
        marioSprite.flipX = false;

        // reset animation
        marioAnimator.SetTrigger("gameRestart");
        alive = true;

        // reset camera position
        gameCamera.position = new Vector3(0, 0, -10);
    }
}