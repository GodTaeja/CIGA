using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveBoxScript : MonoBehaviour
{
    public float moveLength;
    public float moveSpeed;
    public float oneMoveLifeTime;
    


    private GameObject selfGo;
    private Vector3 startMovePos;

    public void InitBoxData(float moveLength,float moveSpeed,float oneMoveLifeTime)
    {

    }


    private void Awake()
    {
        selfGo = gameObject;

    }
    
    private void Update()
    {
        
    }
    
    private void FixedUpdate()
    {
        gameObject.transform.position += (Vector3.left * Time.fixedDeltaTime * moveSpeed);


    }









}
