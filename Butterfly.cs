using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Butterfly : MonoBehaviour
{
    /// <summary>
    /// ���� UI
    /// </summary>
    public ExplainUI m_explainUI = null;

    /// <summary>
    /// ������ǥ
    /// </summary>
    public Vector3 m_destinationPos = Vector3.zero;

    /// <summary>
    /// ��ã�� ������Ʈ
    /// </summary>
    public NavMeshAgent m_agent = null;

    /// <summary>
    /// Ž�� �÷���
    /// </summary>
    bool m_searchFlag = false;

    /// <summary>
    /// ���̵� �÷���
    /// </summary>
    bool m_isGuide = false;

    /// <summary>
    /// �̹� �����ߴ�
    /// </summary>
    bool m_startExplained = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_searchFlag)
        {
            m_searchFlag = GManager.Instance.CheckLength(transform.position, 10.0f);
            return;
        }

        Vector3 _targetPos = Vector3.zero;
        if (m_isGuide)
        {
            if (Vector3.Distance(transform.position, m_destinationPos) <= m_agent.stoppingDistance)
            {
                _targetPos = transform.position;
                Vector3 _lookAt = m_destinationPos;
                _lookAt.y = transform.position.y;
                transform.LookAt(_lookAt);
            }
            else
            {
                _targetPos = m_destinationPos;
                if (Vector3.Distance(transform.position, GManager.Instance.IsPlayer.transform.position) >= 7.0f)
                {
                    _targetPos = transform.position;
                }
            }
        }
        else
        {
            if (GManager.Instance.CheckLength(transform.position, m_agent.stoppingDistance))
            {
                _targetPos = transform.position;
                Vector3 _lookAt = GManager.Instance.IsPlayer.transform.position;
                _lookAt.y = transform.position.y;
                transform.LookAt(_lookAt);
                m_isGuide = true;
                if (m_startExplained) return;
                m_startExplained = true;
                StartCoroutine(m_explainUI.StartExplain());
            }
            else
            {
                _targetPos = GManager.Instance.IsPlayer.transform.position;
            }
        }
        m_agent.SetDestination(_targetPos);
    }

    void StartGuide()
    {
        m_isGuide = true;
    }
}
