using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMPowerupController : MonoBehaviour
{
    public AudioSource powerupAudio;
    private Rigidbody2D QuePowerupBody;
    private BoxCollider2D QuePowerupCol;


    public void playPowerupSpawnSound()
    {
        powerupAudio.PlayOneShot(powerupAudio.clip);
    }

    public void beginMove()
    {
        QuePowerupBody = transform.parent.gameObject.GetComponent<Rigidbody2D>();
        QuePowerupCol = transform.parent.gameObject.GetComponent<BoxCollider2D>();
        
        QuePowerupBody.gravityScale = 1;
        QuePowerupCol.enabled = true;
        
        Debug.Log("I moved");
        QuePowerupBody.AddForce(Vector2.right * 3, ForceMode2D.Impulse);
    }
}

