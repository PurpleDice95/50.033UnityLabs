using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10;
    private Rigidbody2D marioBody;
    private Animator marioAnimator;
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;
    public TextMeshProUGUI scoreText;
    public GameObject enemies;
    public GameObject obstacles;
    public JumpOverGoomba jumpOverGoomba;
    public Transform gameCamera;
    public GameObject gameOverScreen;
    public GameObject resetButton;
    public LayerMask wallJumpLayerMask;
    public Vector3 wallBoxSize;
    public AudioClip marioDeath;

    // state
    [System.NonSerialized]
    public bool alive = true;

    // Start is called before the first frame update
    void Start()
    {
        marioSprite = GetComponent<SpriteRenderer>();
        // Set to be 30 FPS
        Application.targetFrameRate = 30;
        marioBody = GetComponent<Rigidbody2D>();

        marioAnimator = GetComponent<Animator>();
        marioAnimator.SetBool("onGround", onGroundState);
    }

    // Update is called once per frame
    void Update()
    {
        // toggle state
        if (Input.GetKeyDown("a") && faceRightState)
        {
            faceRightState = false;
            marioSprite.flipX = true;
            if (marioBody.velocity.x > 0.05f)
                marioAnimator.SetTrigger("onSkid");
        }

        if (Input.GetKeyDown("d") && !faceRightState)
        {
            faceRightState = true;
            marioSprite.flipX = false;
            if (marioBody.velocity.x < -0.05f)
                marioAnimator.SetTrigger("onSkid");
        }
        marioAnimator.SetFloat("marioSpeed", Mathf.Abs(marioBody.velocity.x));
    }
    public float upSpeed = 20;
    public float deathImpulse = 10;
    public float maxFallSpeed = 10;
    public float defaultGravity = 1;
    public float fallGravity = 3;
    private bool onGroundState = true;

    // Pick out layers to check collison
    int collisionLayerMask = (1 << 3) | (1 << 6) | (1 << 7);

    // this method of ground detection is removed in favor of a BoxCast so wall jump can be implemented
    // Start of collison 2D
    // void OnCollisionEnter2D(Collision2D col)
    // {
    //     if (((collisionLayerMask & (1 << col.transform.gameObject.layer)) > 0) & !onGroundState)
    //     {
    //         onGroundState = true;
    //     }
    // }
    public float maxSpeed = 20;

    // FixedUpdate may be called once per frame. See documentation for details.
    void FixedUpdate()
    {
        if (alive)
        {
            float moveHorizontal = Input.GetAxisRaw("Horizontal");

            if (Mathf.Abs(moveHorizontal) > 0)
            {
                Vector2 movement = new Vector2(moveHorizontal, 0);
                // check if it doesn't go beyond maxSpeed
                if (marioBody.velocity.x < maxSpeed)
                    marioBody.AddForce(movement * speed);
            }

            // stop
            if (Input.GetKeyUp("a") || Input.GetKeyUp("d"))
            {
                // stop
                marioBody.velocity = new Vector2(0, marioBody.velocity.y);
            }






            // alternative grounded check
            onGroundState = Physics2D.BoxCast(transform.position, jumpOverGoomba.boxSize, 0, -transform.up, jumpOverGoomba.maxDistance, collisionLayerMask);
            if (onGroundState) { marioAnimator.SetBool("onGround", onGroundState); }

            // wall jump

            if (!onGroundState && Input.GetKeyDown("space"))
            {
                if (Physics2D.BoxCast(transform.position, wallBoxSize, 0, transform.right, 0.5f, wallJumpLayerMask))
                {
                    marioBody.velocity = Vector2.zero;
                    marioBody.AddForce(new Vector2(-10, upSpeed), ForceMode2D.Impulse);
                }
                else if (Physics2D.BoxCast(transform.position, wallBoxSize, 0, -transform.right, 0.5f, wallJumpLayerMask))
                {
                    marioBody.velocity = Vector2.zero;
                    marioBody.AddForce(new Vector2(10, upSpeed), ForceMode2D.Impulse);
                }
            }

            if (Input.GetKeyDown("space") && onGroundState)
            {
                marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
                onGroundState = false;
                marioAnimator.SetBool("onGround", onGroundState);
            }

            // Variable Jump height
            if ((!Input.GetKey("space") && !onGroundState) || marioBody.velocity.y < 0)
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
    }

    // for audio
    public AudioSource marioAudio;

    void PlayJumpSound()
    {
        // play jump sound
        marioAudio.PlayOneShot(marioAudio.clip);
    }

    // helper
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position + transform.right * 0.5f, wallBoxSize);
        Gizmos.DrawCube(transform.position - transform.right * 0.5f, wallBoxSize);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") && alive)
        {
            // play death animation
            marioAnimator.Play("mario-die");
            marioAudio.PlayOneShot(marioDeath);
            alive = false;
        }
    }

    public void GameOverScene()
    {
        gameOverScreen.SetActive(true);
        scoreText.transform.localPosition = new Vector3(100f, 90f, 0f);
        resetButton.transform.localPosition = new Vector3(0f, -35f, 0f);
        Time.timeScale = 0.0f;
    }
    void PlayDeathImpulse()
    {
        marioBody.AddForce(Vector2.up * deathImpulse, ForceMode2D.Impulse);
    }
    public void RestartButtonCallback(int input)
    {
        // reset everything
        ResetGame();
        // resume time
        Time.timeScale = 1.0f;
    }

    private void ResetGame()
    {
        // reset position
        marioBody.transform.position = new Vector3(-5.2f, -3.4f, 0.0f);
        // reset sprite direction
        faceRightState = true;
        marioSprite.flipX = false;
        // reset score
        scoreText.text = "Score: 0";
        jumpOverGoomba.score = 0;
        // reset Goomba
        foreach (Transform eachChild in enemies.transform)
        {
            eachChild.transform.localPosition = eachChild.GetComponent<EnemyMovement>().startPosition;
        }
        // reset blocks
        foreach (Transform eachChild in obstacles.transform)
        {
            CoinHandler boxCoinHandler = eachChild.GetChild(0).gameObject.GetComponent<CoinHandler>();
            if (boxCoinHandler.hasCoin) { boxCoinHandler.spawnedCoin = false; }
        }

        // reset animation
        marioAnimator.SetTrigger("gameRestart");
        alive = true;
        // reset camera position
        gameCamera.position = new Vector3(0, 0, -10);

        gameOverScreen.SetActive(false);
        scoreText.transform.localPosition = new Vector3(-660f, 490f, 0f);
        resetButton.transform.localPosition = new Vector3(880f, 490f, 0f);
    }
}