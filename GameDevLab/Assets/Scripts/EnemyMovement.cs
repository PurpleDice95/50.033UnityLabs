using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    private float originalX;
    private float maxOffset = 5.0f;
    private float enemyPatroltime = 2.0f;
    private int moveRight = -1;
    private Vector2 velocity;

    private Rigidbody2D enemyBody;
    private BoxCollider2D enemyCollider;
    private Animator goombaAnimator;

    public AudioSource goombaAudio;

    public Vector3 startPosition = new Vector3(3.0f, -2.9f, 0.0f);

    public Vector3 stompBoxSize;
    public float maxDistance;
    public LayerMask layerMask;
    private bool stomped = false;
    private bool countScoreState = true;
    GameManager gameManager;
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
        enemyBody = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<BoxCollider2D>();
        goombaAnimator = GetComponent<Animator>();
        // get the starting position
        originalX = transform.position.x;
        ComputeVelocity();
    }
    void ComputeVelocity()
    {
        velocity = new Vector2((moveRight) * maxOffset / enemyPatroltime, 0);
    }
    void Movegoomba()
    {
        if (!stomped) enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime);
    }

    void Update()
    {
        if (Mathf.Abs(enemyBody.position.x - originalX) < maxOffset)
        {// move goomba
            Movegoomba();
        }
        else
        {
            // change direction
            moveRight *= -1;
            ComputeVelocity();
            Movegoomba();
        }
    }

    void FixedUpdate()
    {
        if (countScoreState && !stomped && Physics2D.BoxCast(transform.position, stompBoxSize, 0, transform.up, maxDistance, layerMask)) {
            gameManager.IncreaseScore(1);
            goombaAudio.PlayOneShot(goombaAudio.clip);
            enemyCollider.enabled = false;
            stomped=true;
            goombaAnimator.SetBool("dead", true);
        }
    }

    public void StopScoreCount()
    {
        countScoreState = false;
    }

    public void HideGoomba()
    {
        gameObject.SetActive(false);
    }


    // helper
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(transform.position + transform.up * maxDistance, stompBoxSize);
    }

    public void GameRestart()
    {
        transform.localPosition = startPosition;
        originalX = transform.position.x;
        moveRight = -1;

        stomped=false;
        enemyCollider.enabled = true;
        countScoreState = true;
        gameObject.SetActive(true);
        goombaAnimator.SetBool("dead", false);
        ComputeVelocity();
    }

}