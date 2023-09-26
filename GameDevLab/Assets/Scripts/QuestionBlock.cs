using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionBlock : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public GameObject coinPrefab;
    public AudioClip coinSound;
    private bool isActive = true;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Mario") && isActive)
        {
            // Check if hit from below
            if (collision.contacts[0].normal.y > 0.5f)
            {
                SpawnCoin();
                StartCoroutine(DisableBlock());
            }
        }
    }

    private void SpawnCoin()
    {
        if (coinPrefab)
        {
            Instantiate(coinPrefab, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(coinSound, transform.position);
        }
    }

    private IEnumerator DisableBlock()
    {
        isActive = false;
        // Change the sprite to indicate it's disabled here
        yield return new WaitForSeconds(0.2f); // Adjust the duration as needed
        // Stop bouncing or set isKinematic to true for the Rigidbody
    }
}
