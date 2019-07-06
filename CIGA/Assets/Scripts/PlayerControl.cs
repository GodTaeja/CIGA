using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    public DarkEffect m_darkEffect;
    private int m_minCir = 80;
    private int m_maxCir = 180;
    private float m_nowCir = 80;
    public float m_speed=5;
    public bool isClick = false;
    // Update is called once per frame
    public GameObject whiteParent;
    public GameObject blackParent;
    private bool m_isBlack;
    public Material m_blackMatri;
    public Material m_wahiteMatri;
    public GameObject m_anim;
    public Camera m_camera;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isClick = true;
        }
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            isClick = false;
        }
        if (Input.GetKey(KeyCode.Alpha1))
        {
            m_nowCir += Time.deltaTime* m_speed;
            m_nowCir = m_nowCir > m_maxCir ? m_maxCir : m_nowCir;
            m_darkEffect.setRoleDis(m_nowCir);
        }
        if(Input.GetKeyDown(KeyCode.W))
        {

            whiteParent.SetActive(!whiteParent.activeSelf);
            blackParent.SetActive(!blackParent.activeSelf);
            if(m_isBlack)
            {
                m_anim.gameObject.SetActive(false);
                this.GetComponent<SpriteRenderer>().enabled = true;
                m_camera.backgroundColor = Color.white;
                m_isBlack = false;

            }
            else
            {
                m_anim.gameObject.SetActive(true);
                this.GetComponent<SpriteRenderer>().enabled = false;
                m_camera.backgroundColor = Color.black;
                m_isBlack = true;

            }
        }
        if(!isClick)
        {
            m_nowCir -= Time.deltaTime* m_speed;
            m_nowCir= m_nowCir < m_minCir ? m_minCir : m_nowCir;
            m_darkEffect.setRoleDis(m_nowCir);
        }
    }
}
