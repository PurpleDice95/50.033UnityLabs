using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuestionBlockController : MonoBehaviour
{
    public CoinHandler coinHandler;

    private Rigidbody2D qBlockBody;
    private float startPos;

    private Animator questionBoxAnimator;

    // Start is called before the first frame update
    void Start()
    {
        qBlockBody = GetComponent<Rigidbody2D>();
        startPos = qBlockBody.transform.position.y;
        questionBoxAnimator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (coinHandler.spawned && qBlockBody.transform.position.y <= startPos + 0.01)
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
