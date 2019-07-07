using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dici : MonoBehaviour
{
    public List<Sprite> m_sprite;
    private int m_InitIndex;
    public PlayerControl m_gameControl;
    private bool isAdd = true;
    void Start()
    {
        m_gameControl.m_tiaoyue.AddListener(change);
        this.GetComponent<SpriteRenderer>().sprite = m_sprite[m_InitIndex];
    }

    // Update is called once per frame
    void change()
    {
        if (isAdd)
        {
            m_InitIndex++;
            if (m_InitIndex >= (m_sprite.Count))
            {
                m_InitIndex=1;
                isAdd = false;
            }
        }
        else
        {
            m_InitIndex--;
            if (m_InitIndex < 0)
            {
                m_InitIndex=1;
                isAdd = true;
            }
        }
        
        this.GetComponent<SpriteRenderer>().sprite = m_sprite[m_InitIndex];
    }
}
