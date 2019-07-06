using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{
    public GameObject m_tarGm;
    public GameObject m_tarGMtwo;
    public DarkEffect m_darkEffect;
    private int m_roleMixMaskDis = 50;
    private int m_roleMaxMaskDis = 180;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            m_darkEffect.setRoleDis(m_roleMixMaskDis);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            m_darkEffect.setRoleDis(m_roleMaxMaskDis);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            m_darkEffect.addLightItem(m_tarGm.transform, 50);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            m_darkEffect.removeLightItem(m_tarGm.transform);
        }
    }
}
