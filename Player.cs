using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    /// <summary>
    /// 스토리 스크립트
    /// </summary>
    public InformationScript m_informationScript = null;

    /// <summary>
    /// 체력바 스크립트
    /// </summary>
    public HpBar m_hpBar = null;

    /// <summary>
    /// 움직임 타입
    /// </summary>
    MoveType.TYPE m_moveType = MoveType.TYPE.Idle;

    /// <summary>
    /// 캐릭터 속도
    /// </summary>
    public float m_speed = 0.0f;

    /// <summary>
    /// 회전속도
    /// </summary>
    public float m_rotateSpeed = 0.0f;

    /// <summary>
    /// 캐릭터 점프력
    /// </summary>
    public float m_jumpPower = 0.0f;

    /// <summary>
    /// 최대 체력
    /// </summary>
    public float m_maxHealth = 0;

    /// <summary>
    /// 강체
    /// </summary>
    public Rigidbody m_rigidbody = null;

    /// <summary>
    /// 애니메이터
    /// </summary>
    public Animator m_animator = null;

    /// <summary>
    /// 스카프 애니메이터
    /// </summary>
    public Animator m_scafAnimator = null;

    /// <summary>
    /// 망토 애니메이터
    /// </summary>
    public Animator m_capeAnimator = null;

    /// <summary>
    /// 캐릭터의 오디오 소스
    /// </summary>
    public AudioSource m_audioSource = null;

    /// <summary>
    /// 떨어지는 소리
    /// </summary>
    public AudioClip m_dropAudio = null;

    /// <summary>
    /// 착지 소리
    /// </summary>
    public AudioClip m_landAudio = null;

    /// <summary>
    /// 걷는 소리
    /// </summary>
    public AudioClip m_walkAudio = null;

    /// <summary>
    /// 데미지 다는 소리
    /// </summary>
    public AudioClip m_bloodClip = null;

    /// <summary>
    /// 회전 오브젝트
    /// </summary>
    public GameObject m_rot = null;

    /// <summary>
    /// 이펙트 베이스
    /// </summary>
    public GameObject m_effectBase = null;

    /// <summary>
    /// 이펙트들
    /// </summary>
    public GameObject[] m_effects = null;

    /// <summary>
    /// 리스폰 장소
    /// </summary>
    public Vector3 m_respawnPos = Vector3.zero;

    /// <summary>
    /// 시작인지아닌지
    /// </summary>
    public bool m_isStart = false;

    /// <summary>
    /// 현재 속도
    /// </summary>
    float m_currentSpeed = 0.0f;

    /// <summary>
    /// 현재 체력
    /// </summary>
    public float m_currentHealth = 0;

    /// <summary>
    /// 회복
    /// </summary>
    float m_healthRegen = 0;

    /// <summary>
    /// 움직임 유무
    /// </summary>
    bool m_MoveFlag = true;

    /// <summary>
    /// 점프 유무
    /// </summary>
    bool m_jumpFlag = false;

    /// <summary>
    /// 대쉬 유무
    /// </summary>
    bool m_dashFlag = false;

    /// <summary>
    /// 떨어지고있는지 아닌지
    /// </summary>
    bool m_dropFlag = false;

    /// <summary>
    /// 떨어졌는지 아닌지
    /// </summary>
    bool m_droppedFlag = false;

    /// <summary>
    /// 달리기 유무
    /// </summary>
    bool m_runFlag = false;

    /// <summary>
    /// 죽음 플래그
    /// </summary>
    bool m_dieFlag = false;

    /// <summary>
    /// 대쉬 능력
    /// </summary>
    bool m_dashAbility = false;

    /// <summary>
    /// 더블점프 능력
    /// </summary>
    bool m_doubleJumpAbility = false;

    /// <summary>
    /// 점프 갯수
    /// </summary>
    int m_jumpCount = 0;

    /// <summary>
    /// 떨어지는 이펙트
    /// </summary>
    GameObject _fallEffect = null;

    // Start is called before the first frame update
    void Awake()
    {
        // 죽음 플래그 설정
        GManager.Instance.IsPlayerDie = m_dieFlag;

        // 체력 설정
        m_currentHealth = m_maxHealth;

        // 속도 설정
        m_currentSpeed = m_speed;

        // 체력 자동 자연회복 시작
        m_healthRegen = 0.3f;
        StartCoroutine(HealthRegen());

        if (m_isStart)
        {
            StartSetting();
        }
    }

    /// <summary>
    /// 게임 시작일시만 작동
    /// </summary>
    void StartSetting()
    {
        m_jumpFlag = true;
        m_dropFlag = true;
        m_animator.SetBool("Drop", m_dropFlag);
        m_scafAnimator.SetBool("Drop", m_dropFlag);
        m_capeAnimator.SetBool("Drop", m_dropFlag);
        m_audioSource.PlayOneShot(m_dropAudio);
        _fallEffect = Instantiate(m_effects[0], m_effectBase.transform);
        _fallEffect.transform.localPosition = new Vector3(0, 0, 0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        // UI에 HP표시
        m_hpBar.WriteHp(m_maxHealth, m_currentHealth);

        // 죽을시, 경직일시, 낙하중일시 강제 반환 (이다음 명령어의 실행을 막는다)
        if (m_dieFlag || !m_MoveFlag || m_dropFlag) return;

        // 달리기
        if (Input.GetKey(KeyCode.LeftShift) && !m_jumpFlag)
        {
            m_runFlag = true;
            m_currentSpeed = m_speed + 3.0f;
        }
        else
        {
            m_currentSpeed = m_speed;
            m_runFlag = false;
        }

        // 이동에 관한 입력 구하기
        Vector3 _input = Vector3.zero;
        _input.x = Input.GetAxisRaw("Horizontal");
        _input.z = Input.GetAxisRaw("Vertical");

        // 이동타입 설정
        if (!m_jumpFlag)
        {
            if (Math.Abs(_input.x) == 1 || Math.Abs(_input.y) == 1)
            {
                if (m_runFlag)
                {
                    m_moveType = MoveType.TYPE.Run;

                }
                else
                {
                    m_moveType = MoveType.TYPE.Walk;
                }
            }
            else
            {
                m_moveType = MoveType.TYPE.Idle;
            }
        }

        // 캐릭터 본체의 애니메이션 설정
        m_animator.SetFloat("State", (int)m_moveType);
        m_animator.SetBool("Jump", m_jumpFlag);

        // 스카프의 애니메이션 설정
        m_scafAnimator.SetFloat("State", (int)m_moveType);
        m_scafAnimator.SetBool("Jump", m_jumpFlag);

        // 망토의 애니메이션 설정
        m_capeAnimator.SetFloat("State", (int)m_moveType);
        m_capeAnimator.SetBool("Jump", m_jumpFlag);
        
        // 이동
        transform.Translate(_input.normalized * m_currentSpeed * Time.deltaTime);

        // 보는방향 회전
        Turn(_input);

        // 점프 및 더블점프 능력
        if (m_doubleJumpAbility)
        {
            if (m_jumpCount <= 1 && Input.GetKeyDown(KeyCode.Space))
            {
                m_jumpFlag = true;
                m_jumpCount++;
                if (m_jumpCount == 2) Instantiate(m_effects[3], transform);
                m_currentSpeed = m_speed - 3.0f;
                m_rigidbody.velocity += Vector3.up * m_jumpPower;
            }
        }
        else
        {
            if (!m_jumpFlag && Input.GetKeyDown(KeyCode.Space))
            {
                m_jumpFlag = true;
                m_currentSpeed = m_speed - 3.0f;
                m_rigidbody.velocity += Vector3.up * m_jumpPower;
            }
        }
        
        // 대쉬 능력
        if (!m_dashFlag && m_dashAbility && Input.GetKeyDown(KeyCode.Q))
        {
            m_dashFlag = true;
            Instantiate(m_effects[4], m_rot.transform);
            transform.Translate(_input.normalized * 500f * Time.deltaTime);
            Invoke("CanDashAgain", 3.0f);
        }

        // 버그로 맵탈출 방지
        if (transform.position.y <= -15)
        {
            m_MoveFlag = false;
            transform.position = m_respawnPos;
            StartCoroutine(m_informationScript.DontFall());
            m_animator.SetFloat("State", 0);
            m_animator.SetBool("Jump", false);
            m_animator.SetBool("Drop", false);

            m_scafAnimator.SetFloat("State", 0);
            m_scafAnimator.SetBool("Jump", false);
            m_scafAnimator.SetBool("Drop", false);

            m_capeAnimator.SetFloat("State", 0);
            m_capeAnimator.SetBool("Jump", false);
            m_capeAnimator.SetBool("Drop", false);
            Invoke("CanMoveAgain", 4.0f);
        }
    }

    /// <summary>
    /// 통과되지 않는 충돌이 일어날시
    /// </summary>
    /// <param name="collision">충돌체</param>
    private void OnCollisionEnter(Collision collision)
    {
        m_currentSpeed = m_speed;
        m_jumpFlag = false;
        m_jumpCount = 0;
        if (m_dropFlag)
        {
            m_droppedFlag = true;
            m_audioSource.Stop();
            Destroy(_fallEffect);
            m_audioSource.PlayOneShot(m_landAudio);
            m_animator.SetBool("Dropped", m_droppedFlag);
            m_scafAnimator.SetBool("Dropped", m_droppedFlag);
            m_capeAnimator.SetBool("Dropped", m_droppedFlag);
            GameObject _landEffect = Instantiate(m_effects[1], m_effectBase.transform);
            _landEffect.transform.localPosition = new Vector3(-0.02f, 2.8f, 0.02f);
            Invoke("StopLand", 3.0f);
        }
    }

    /// <summary>
    /// 캐릭터의 보는 방향
    /// </summary>
    /// <param name="argInput">입력받은 이동값</param>
    void Turn(Vector3 argInput)
    {
        if (argInput.x == 0 && argInput.z == 0) return;
        m_rot.transform.rotation = Quaternion.Slerp(m_rot.transform.rotation, Quaternion.LookRotation(argInput), m_rotateSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 플레이어 경직 해제
    /// </summary>
    void CanMoveAgain()
    {
        m_MoveFlag = true;
    }

    /// <summary>
    /// 데쉬 쿨타임 초기화
    /// </summary>
    void CanDashAgain()
    {
        m_dashFlag = false;
    }

    /// <summary>
    /// 낙하 그만하기
    /// </summary>
    void StopLand()
    {
        m_droppedFlag = false;
        m_dropFlag = false;
        m_animator.SetBool("Dropped", m_droppedFlag);
        m_scafAnimator.SetBool("Dropped", m_droppedFlag);
        m_capeAnimator.SetBool("Dropped", m_droppedFlag);

        m_animator.SetBool("Drop", m_dropFlag);
        m_scafAnimator.SetBool("Drop", m_dropFlag);
        m_capeAnimator.SetBool("Drop", m_dropFlag);
    }

    /// <summary>
    /// 데미지 입기
    /// </summary>
    /// <param name="argDamage">몇데미지인지</param>
    public void Damage(int argDamage)
    {
        if (m_dieFlag) return;

        m_audioSource.PlayOneShot(m_bloodClip);
        GameObject _slashObj = Instantiate(m_effects[2]);
        _slashObj.transform.SetParent(transform);
        _slashObj.transform.position = transform.position;
        m_currentHealth -= argDamage;

        if (m_currentHealth <= 0)
        {
            m_dieFlag = true;
            m_rot.transform.Rotate(Vector3.left * 90);
            GManager.Instance.IsPlayerDie = m_dieFlag;
        }
    }

    /// <summary>
    /// 저장시스템(미완성)
    /// </summary>
    private void OnApplicationQuit()
    {
        SaveLoad.SaveData(this);
    }

    /// <summary>
    /// 자연 체력 회복
    /// </summary>
    /// <returns></returns>
    IEnumerator HealthRegen()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (!(m_currentHealth >= m_maxHealth))
            {
                if (m_currentHealth + m_healthRegen > m_maxHealth)
                {
                    m_currentHealth = m_maxHealth;
                }
                else
                {
                    m_currentHealth += m_healthRegen;
                }
            }
        }
    }

    /// <summary>
    /// 특수능력 부가
    /// </summary>
    /// <param name="argIndex">몇번째 능력인지</param>
    public void GiveAbility(int argIndex)
    {
        switch (argIndex) 
        {
            case 1:
                {
                    m_dashAbility = true;
                    break;
                }
            case 2:
                {
                    m_doubleJumpAbility = true;
                    break;
                }
        }
    }
}
