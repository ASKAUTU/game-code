using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterSc : MonoBehaviour
{
    /// <summary>
    /// ��ã�� ������Ʈ
    /// </summary>
    public NavMeshAgent m_agent = null;

    /// <summary>
    /// �ε� �ε���
    /// </summary>
    public int m_index = 0;

    /// <summary>
    /// �ִϸ�����
    /// </summary>
    public Animator m_animator = null;

    /// <summary>
    /// �´� ����Ʈ
    /// </summary>
    public GameObject m_hitEffect = null;

    /// <summary>
    /// ���� ��ü
    /// </summary>
    public GameObject m_rot = null;

    /// <summary>
    /// ����� �ҽ�
    /// </summary>
    public AudioSource m_audioSource = null;

    /// <summary>
    /// ���̴� �Ҹ�
    /// </summary>
    public AudioClip m_bloodClip = null;

    /// <summary>
    /// Ž�� �÷���
    /// </summary>
    bool m_searchFlag = false;

    /// <summary>
    /// ���� �÷���
    /// </summary>
    bool m_dieFlag = false;

    /// <summary>
    /// ���� �÷���
    /// </summary>
    bool m_attackFlag = false;

    /// <summary>
    /// ���� �ӵ�
    /// </summary>
    float m_currentSpeed = 0.0f;

    /// <summary>
    /// ���� ü��
    /// </summary>
    public int m_currentHealth = 0;

    /// <summary>
    /// ������
    /// </summary>
    int m_damage = 0;

    // Start is called before the first frame update
    void Start()
    {
        // ���ݵ�����&�ӵ�&ü�� ����
        m_damage = GManager.Instance.GetMonsterData(m_index).m_damage;
        m_currentSpeed = GManager.Instance.GetMonsterData(m_index).m_speed;
        m_currentHealth = GManager.Instance.GetMonsterData(m_index).m_maxHp;
        m_agent.speed = m_currentSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // �ִϸ��̼� ����
        m_animator.SetFloat("State", 0.0f);

        // �ڽ��� �װų� �÷��̾ �׾������ ���� ����
        if (m_dieFlag || GManager.Instance.IsPlayerDie) return;

        // Ž�������ȿ� �÷��̾ ���Դ��� �Ǻ�
        if (!m_searchFlag)
        {
            m_searchFlag = GManager.Instance.CheckLength(transform.position, GManager.Instance.GetMonsterData(m_index).m_searchLength);
            return;
        }

        // �������ϰ�� ����
        if (m_attackFlag) return;

        // ���� ����
        Vector3 _targetPos = Vector3.zero;

        // �÷��̾� �տ� ���߱�
        if (GManager.Instance.CheckLength(transform.position, m_agent.stoppingDistance))
        {
            _targetPos = transform.position;
            m_attackFlag = true;
            Vector3 _lookAt = GManager.Instance.IsPlayer.transform.position;
            _lookAt.y = transform.position.y;
            m_rot.transform.LookAt(_lookAt);  
            m_animator.SetTrigger("Attack");
        }
        // �÷��̾� ��ġ�� ������������ ����
        else
        {
            _targetPos = GManager.Instance.IsPlayer.transform.position;
            m_animator.SetFloat("State", 1.0f);
        }

        // ������������ �̵�
        m_agent.SetDestination(_targetPos);
    }

    /// <summary>
    /// ������ �ޱ�
    /// </summary>
    public void Damage(int argWeaponDamage)
    {
        if (m_dieFlag) return;

        m_currentHealth -= argWeaponDamage;
        m_audioSource.PlayOneShot(m_bloodClip);
        GameObject _slashObj = Instantiate(m_hitEffect);
        _slashObj.transform.SetParent(transform);
        _slashObj.transform.position = transform.position;

        if (m_currentHealth <= 0)
        {
            m_dieFlag = true;
            m_agent.SetDestination(transform.position);
            Destroy(gameObject, 1.0f);
        }
    }

    /// <summary>
    /// ���� �ߴ�
    /// </summary>
    /// <returns></returns>
    public IEnumerator ClearAttack()
    {
        yield return new WaitForSeconds(1.0f);
        m_attackFlag = false;
    }

    /// <summary>
    /// ����
    /// </summary>
    public void Attack()
    {
        if (GManager.Instance.IsPlayerDie) return;

        if (GManager.Instance.CheckLength(transform.position, m_agent.stoppingDistance + 0.7f)) GManager.Instance.IsPlayerSc.Damage(m_damage);
    }
}
