using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Camera : MonoBehaviour
{

    public GameObject m_player = null;

    public GameObject m_cameraBase = null;

    int i = 0;

    bool m_sawFlag = false;

    bool m_isSeing = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_player.transform.position.x >= 105)
        {
            m_isSeing = true;
        }
        if (m_sawFlag || !m_isSeing) return;
        if (i >= 100)
        {
            m_sawFlag = true;
            i = 0;
            transform.SetParent(m_player.transform);
            transform.localPosition = new Vector3(2.3f, 2.7f, -8.2f);
            transform.rotation = Quaternion.Euler(5, 0, 0);
            return;
        }
        transform.SetParent(m_cameraBase.transform);
        transform.position = Vector3.Slerp(transform.position, m_cameraBase.transform.position, 5.0f * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(-12, 70, 0), 5.0f * Time.deltaTime);
        i++;
    }
}
