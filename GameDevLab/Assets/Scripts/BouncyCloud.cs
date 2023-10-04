using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyCloud : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource cloudAudio;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Check if Mario is above the cloud and moving downward (bouncing).
            if (collision.relativeVelocity.y < 0)
            {
                // Play the bounce sound.
                cloudAudio.Play();
            }
        }
    }
}
