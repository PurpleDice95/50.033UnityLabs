using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinHandler : BasePowerup
{
    public bool hasCoin = true;
    public GameObject coinPrefab;

    void Awake () {
        GameManager.instance.gameRestart.AddListener(GameRestart);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (
            hasCoin && !spawned &&
            col.gameObject.CompareTag("Player") &&
            (col.transform.position.y + col.transform.localScale.y / 2 < transform.position.y - transform.localScale.y / 2 + 0.1)
        )
        {
            SpawnPowerup();
        }
    }

    public void GameRestart()
    {
        if (hasCoin) {spawned = false;}
    }

    public override void SpawnPowerup()
    {
        spawned = true;
        GameObject inst = Instantiate(coinPrefab, transform.position, Quaternion.identity);
        inst.transform.parent = transform;
    }

    public override void ApplyPowerup(MonoBehaviour i)
    {
        Debug.Log("Coin");
    }

}
