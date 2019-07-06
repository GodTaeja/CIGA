using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public DarkEffect m_dark;
    public GameObject m_target;
    public GameObject m_targetTwo;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            m_dark.addLightItem(m_target.transform, 100);
            m_dark.addLightItem(m_targetTwo.transform, 100);
        }
        
    }
}
