using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionBoxPowerupController : BasePowerup, IPowerupController
{
    private Rigidbody2D qBlockBody;
    private float startPos;

    private Animator questionBoxAnimator;

    void Awake () {
        GameManager.instance.gameRestart.AddListener(GameRestart);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        qBlockBody = GetComponent<Rigidbody2D>();
        startPos = qBlockBody.transform.position.y;
        questionBoxAnimator = GetComponent<Animator>();
    }
    // public BasePowerup powerup; // reference to this question box's powerup
    public GameObject powerupPrefab;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!spawned &&
            col.gameObject.CompareTag("Player") &&
            (col.transform.position.y + col.transform.localScale.y / 2 < transform.position.y - transform.localScale.y / 2 + 0.1)
        )
        {
            SpawnPowerup();
            
        }
    }

    // used by animator
    public void Disable() {}

    public void GameRestart()
    {
        spawned = false;
    }
    public override void SpawnPowerup()
    {
        spawned = true;
        GameObject inst = Instantiate(powerupPrefab, transform.position + new Vector3(0,0.76f,0), Quaternion.identity);
        inst.transform.parent = transform;
    }

    public override void ApplyPowerup(MonoBehaviour i)
    {
        Debug.Log("Powerup");
    }

    void FixedUpdate()
    {
        if (spawned && qBlockBody.transform.position.y <= startPos + 0.01)
        {
            qBlockBody.bodyType = RigidbodyType2D.Static;
            questionBoxAnimator.SetBool("Collected", true);
        }
        else
        {
            qBlockBody.bodyType = RigidbodyType2D.Dynamic;
            
            questionBoxAnimator.SetBool("Collected", false);
        }
    }


}