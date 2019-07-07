using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    public bool isSave;
    public int pointIndex;


    private void Start()
    {
        SavePointManager.Instance.AddSavePoint(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        isSave = true;
        GetComponent<Animator>().SetBool("isSave", true);
    }
}
