using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    public AudioSource coinAudio;
    public AnimationEventIntTool animEventTool;
    void Start () {
        Debug.Log("Hellow");
        animEventTool = GetComponent<AnimationEventIntTool>();
        animEventTool.useInt.AddListener(FindObjectOfType<GameManager>().IncreaseScore);
    }

    public void playCoinSound()
    {
        coinAudio.PlayOneShot(coinAudio.clip);
    }
    public void removeCoin()
    {
        Destroy(transform.parent.gameObject);
    }
}
