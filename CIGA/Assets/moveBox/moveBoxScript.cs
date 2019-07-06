using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveBoxScript : MonoBehaviour
{
    public BoxType boxType = BoxType.None;

    #region 移动
    public List<OnePointToNextPoint> onePointToNextPoints = new List<OnePointToNextPoint>();
    private OnePointToNextPoint currentPoint;
    private Vector3 goStartPos;
    private Vector3 currentPointPos;
    private Vector3 currentVelocity;
    #endregion

    #region 旋转
    public Transform rootPoint;
    public Transform objPoint;
    #endregion

    private void Awake()
    {
        goStartPos = gameObject.transform.position;
        SetBoxType(boxType);

    }

    public void SetBoxType(BoxType type)
    {
        boxType = type;

        switch (type)
        {
            case BoxType.None:
                break;
            case BoxType.Move:
                MoveType();
                break;
            case BoxType.Rotate:
                RotateType();
                break;
        }
    }

    private void MoveType()
    {
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

    private void RotateType()
    {

    }

    private void Update()
    {
        //TODO Action
    }

    private void FixedUpdate()
    {
        if (boxType == BoxType.Move)
            MoveLine();
        if (boxType == BoxType.Rotate)
            RotateLine();
    }

    void MoveLine()
    {
        gameObject.transform.position = Vector3.SmoothDamp(gameObject.transform.position,
           currentPoint.moveTarget, ref currentVelocity, currentPoint.moveTime);
        if (Mathf.Abs(Vector3.Distance(gameObject.transform.position, currentPointPos + currentPoint.moveTarget)) <= 0.1f)
        {
            currentPoint = currentPoint.nextPoint;
        }
    }

    void RotateLine()
    {
        gameObject.transform.position = objPoint.position;
        rootPoint.transform.Rotate(rootPoint.forward, 2);
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

public enum BoxType
{
    None,
    Move,
    Rotate,
}

