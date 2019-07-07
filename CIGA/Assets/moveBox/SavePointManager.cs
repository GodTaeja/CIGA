using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePointManager : MonoBehaviour {

    public static SavePointManager Instance;

    public List<SavePoint> savePoints = new List<SavePoint>();

    private void Awake()
    {
        Instance = this;
    }

    public void AddSavePoint(SavePoint savePoint)
    {
        savePoints.Add(savePoint);
    }


    public Vector3 ReLoadPoint()
    {
        if(savePoints.Count ==0)
        {
            return Vector3.zero;
        }

        SavePoint tempPoint = savePoints[0];
        for (int i = 0; i < savePoints.Count; i++)
        {
            if(savePoints[i].pointIndex>tempPoint.pointIndex && savePoints[i].isSave)
            {
                tempPoint = savePoints[i];
            }
        }

        return tempPoint.gameObject.transform.position;

    }
   
}
