using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class MMPowerupHandler : MonoBehaviour
{
    private bool goRight = true;
    private Rigidbody2D rigidBody;
    void Awake()
    {
        GameManager.instance.gameRestart.AddListener(GameRestart);
    }
    // base methods
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            this.transform.parent.gameObject.GetComponent<QuestionBoxPowerupController>().ApplyPowerup(null);
            Destroy(this.gameObject);
        }
        else if (other.gameObject.layer == 6)
        {
            goRight = !goRight;
            rigidBody.AddForce(Vector2.right * (goRight ? 1 : -1) * 3, ForceMode2D.Impulse);
        }
    }

    public void GameRestart()
    {
        Destroy(this.gameObject);
    }


}
