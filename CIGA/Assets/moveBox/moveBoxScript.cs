using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveBoxScript : MonoBehaviour
{
    public List<OnePointToNextPoint> onePointToNextPoints = new List<OnePointToNextPoint>();

    private OnePointToNextPoint currentPoint;



    public float moveLength;
    public float moveSpeed;
    public float oneMoveLifeTime;



    private Vector3 startMovePos;
    private Vector3 goStartPos;
    private Vector3 currentPointPos;

    private float time;

    private Vector3 currentVelocity;


    public void InitBoxData(float _moveLength, float _moveSpeed, float _oneMoveLifeTime, Vector3 _startMovenormalize)
    {
        moveLength = _moveLength;
        moveSpeed = _moveSpeed;
        oneMoveLifeTime = _oneMoveLifeTime;
        startMovePos = _startMovenormalize * _moveLength;
    }

    private void Awake()
    {
        goStartPos = gameObject.transform.position;
        currentPointPos = gameObject.transform.position;
        for (int i = 0; i < onePointToNextPoints.Count; i++)
        {
            if (i == onePointToNextPoints.Count - 1)
            {
                onePointToNextPoints[i].nextPoint = onePointToNextPoints[0];
            }
            else
            {
                onePointToNextPoints[i].nextPoint = onePointToNextPoints[i + 1];
            }
        }
        currentPoint = onePointToNextPoints[0];
    }

    private void Update()
    {
        //TODO Action
    }

    private void FixedUpdate()
    {
        MoveLine();
    }

    void MoveLine()
    {
        //time += Time.fixedDeltaTime;

        //if (time <= oneMoveLifeTime / 2)
        //{
        //    gameObject.transform.position = Vector3.SmoothDamp(gameObject.transform.position,
        //   startMovePos, ref currentVelocity, oneMoveLifeTime / 2);
        //}

        //if (time > oneMoveLifeTime / 2 && time <= oneMoveLifeTime)
        //{
        //    gameObject.transform.position = Vector3.SmoothDamp(gameObject.transform.position,
        //  goStartPos, ref currentVelocity, oneMoveLifeTime / 2);
        //}

        //if (time > oneMoveLifeTime)
        //{
        //    time = 0;
        //}

        gameObject.transform.position = Vector3.SmoothDamp(gameObject.transform.position,
           currentPoint.moveTarget, ref currentVelocity, currentPoint.moveTime);
        if (Mathf.Abs(Vector3.Distance(gameObject.transform.position, currentPointPos + currentPoint.moveTarget)) <= 0.1f)
        {
            currentPoint = currentPoint.nextPoint;
        }



    }







}

[Serializable]
public class OnePointToNextPoint
{
    public OnePointToNextPoint nextPoint;
    public Vector3 moveTarget;
    public float moveTime;
    [HideInInspector]
    public bool isOnComplet;
}


