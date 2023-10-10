using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    void Awake()
    {
        // Here they are, the
        // other instructions
        GameManager.instance.gameRestart.AddListener(GameRestart);
        GameManager.instance.marioDeath.AddListener(StopScoreCount);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameRestart()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<EnemyMovement>().GameRestart();
        }
    }

    public void StopScoreCount()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<EnemyMovement>().StopScoreCount();
        }
    }
}