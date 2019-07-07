using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leftOrRightMove : MonoBehaviour
{
    private Vector3 m_startPos;
    [Tooltip("移动速度")]
    public float m_moveSpeed;
    [Tooltip("以初始点为中心的移动半径")]
    public float m_moveRange;

    private Vector3 m_moveVector;
    private bool isLeft=true;
        void Start()
      {
        m_moveVector = this.transform.localPosition;    
        }

    // Update is called once per frame
    void Update()
    {
        m_moveVector = this.transform.localPosition;
        if (isLeft)
        {

            m_moveVector.x -= Time.deltaTime * m_moveSpeed;
            this.transform.localPosition = m_moveVector;
            if (m_moveVector.x<m_startPos.x-m_moveRange)
            {
    
                isLeft = false;
            }
        }
        else
        {
            m_moveVector.x += Time.deltaTime * m_moveSpeed;
            this.transform.localPosition = m_moveVector;
            if (m_moveVector.x > m_startPos.x + m_moveRange)
            {
                this.transform.localPosition = m_moveVector;
                isLeft = true;
            }
        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.LogError("gg");
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (isLeft)
            {
                Vector3 temp = gameObject.transform.position;
                temp.x -= Time.deltaTime * m_moveSpeed;
                gameObject.transform.position = temp;
             }
            else
            {
                Vector3 temp = gameObject.transform.position;
                temp.x += Time.deltaTime * m_moveSpeed;
                gameObject.transform.position = temp;
            }
        }
    }
 
}
