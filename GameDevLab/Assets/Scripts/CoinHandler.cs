using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinHandler : MonoBehaviour
{
    public bool hasCoin = true;
    public bool spawnedCoin = false;
    public GameObject coinPrefab;
    void OnCollisionEnter2D(Collision2D col)
    {
        if (
            hasCoin && !spawnedCoin &&
            col.gameObject.CompareTag("Player") &&
            (col.transform.position.y + col.transform.localScale.y / 2 < transform.position.y - transform.localScale.y / 2 + 0.1)
        )
        {
            spawnedCoin = true;
            Instantiate(coinPrefab, transform.position, Quaternion.identity);
        }
    }

}
