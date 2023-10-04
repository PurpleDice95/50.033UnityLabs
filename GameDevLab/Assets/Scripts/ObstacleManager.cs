using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
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
            GameObject boxObj = child.GetChild(0).gameObject;
            CoinHandler boxCoinHandler = boxObj.GetComponent<CoinHandler>();
            if (boxCoinHandler.hasCoin) { boxCoinHandler.spawnedCoin = false; }
            if (boxObj.tag == "QuestionBlock") { boxObj.GetComponent<Animator>().SetBool("Collected", false); }
        }
    }
}