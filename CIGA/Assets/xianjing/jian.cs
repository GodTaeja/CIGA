using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;

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


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            if (CharacterController2D.Instance.gameObject.transform.position.y < -10.0f)
                this.transform.position = SavePointManager.Instance.ReLoadPoint();
            
        }
    }

}
