using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Child : MonoBehaviour
{
    bool m_startRun = false;

    /// <summary>
    /// æ∆¿Ã
    /// </summary>
    public GameObject m_child = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_startRun && Vector3.Distance(transform.position, GManager.Instance.IsPlayerSc.transform.position) <= 6.0f)
        {
            m_child.SetActive(true);
            m_startRun = true;
        }
        if (!m_startRun) return;
        transform.Translate(Vector3.right * 12.0f * Time.deltaTime);
        Destroy(gameObject, 2.5f);
    }
}
