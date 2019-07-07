using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jian : MonoBehaviour
{
    public float speed=1;
    private Vector3 m_moveVec;
    private void Start()
    {
        m_moveVec = this.transform.localPosition;
    }
    private void Update()
    {
        m_moveVec.x -= Time.deltaTime * speed;
        this.transform.localPosition = m_moveVec;
    }

}
