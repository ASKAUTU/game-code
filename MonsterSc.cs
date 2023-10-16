using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterSc : MonoBehaviour
{
    /// <summary>
    /// 길찾기 에이전트
    /// </summary>
    public NavMeshAgent m_agent = null;

    /// <summary>
    /// 로드 인덱스
    /// </summary>
    public int m_index = 0;

    /// <summary>
    /// 애니메이터
    /// </summary>
    public Animator m_animator = null;

    /// <summary>
    /// 맞는 이펙트
    /// </summary>
    public GameObject m_hitEffect = null;

    /// <summary>
    /// 도는 물체
    /// </summary>
    public GameObject m_rot = null;

    /// <summary>
    /// 오디오 소스
    /// </summary>
    public AudioSource m_audioSource = null;

    /// <summary>
    /// 베이는 소리
    /// </summary>
    public AudioClip m_bloodClip = null;

    /// <summary>
    /// 탐색 플래그
    /// </summary>
    bool m_searchFlag = false;

    /// <summary>
    /// 죽음 플래그
    /// </summary>
    bool m_dieFlag = false;

    /// <summary>
    /// 공격 플래그
    /// </summary>
    bool m_attackFlag = false;

    /// <summary>
    /// 현재 속도
    /// </summary>
    float m_currentSpeed = 0.0f;

    /// <summary>
    /// 현재 체력
    /// </summary>
    public int m_currentHealth = 0;

    /// <summary>
    /// 데미지
    /// </summary>
    int m_damage = 0;

    // Start is called before the first frame update
    void Start()
    {
        // 공격데미지&속도&체력 설정
        m_damage = GManager.Instance.GetMonsterData(m_index).m_damage;
        m_currentSpeed = GManager.Instance.GetMonsterData(m_index).m_speed;
        m_currentHealth = GManager.Instance.GetMonsterData(m_index).m_maxHp;
        m_agent.speed = m_currentSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // 애니메이션 설정
        m_animator.SetFloat("State", 0.0f);

        // 자신이 죽거나 플레이어가 죽었을경우 강제 리턴
        if (m_dieFlag || GManager.Instance.IsPlayerDie) return;

        // 탐색범위안에 플레이어가 들어왔는지 판별
        if (!m_searchFlag)
        {
            m_searchFlag = GManager.Instance.CheckLength(transform.position, GManager.Instance.GetMonsterData(m_index).m_searchLength);
            return;
        }

        // 공격중일경우 리턴
        if (m_attackFlag) return;

        // 도착 지점
        Vector3 _targetPos = Vector3.zero;

        // 플레이어 앞에 멈추기
        if (GManager.Instance.CheckLength(transform.position, m_agent.stoppingDistance))
        {
            _targetPos = transform.position;
            m_attackFlag = true;
            Vector3 _lookAt = GManager.Instance.IsPlayer.transform.position;
            _lookAt.y = transform.position.y;
            m_rot.transform.LookAt(_lookAt);  
            m_animator.SetTrigger("Attack");
        }
        // 플레이어 위치를 도착지점으로 지정
        else
        {
            _targetPos = GManager.Instance.IsPlayer.transform.position;
            m_animator.SetFloat("State", 1.0f);
        }

        // 도착지점으로 이동
        m_agent.SetDestination(_targetPos);
    }

    /// <summary>
    /// 데미지 받기
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
    /// 공격 중단
    /// </summary>
    /// <returns></returns>
    public IEnumerator ClearAttack()
    {
        yield return new WaitForSeconds(1.0f);
        m_attackFlag = false;
    }

    /// <summary>
    /// 공격
    /// </summary>
    public void Attack()
    {
        if (GManager.Instance.IsPlayerDie) return;

        if (GManager.Instance.CheckLength(transform.position, m_agent.stoppingDistance + 0.7f)) GManager.Instance.IsPlayerSc.Damage(m_damage);
    }
}
