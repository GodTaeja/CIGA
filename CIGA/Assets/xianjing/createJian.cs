using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createJian : MonoBehaviour
{
    public GameObject m_jian;
    public float m_createTime;
    public float m_flySpeed;
    private float m_timer=0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_timer += Time.deltaTime;
        if(m_timer> m_createTime)
        {
            m_timer = 0;
               GameObject go = Instantiate(m_jian);
            go.transform.SetParent(this.transform);
            go.GetComponent<jian>().speed = m_flySpeed;
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;

        }
    }
}
