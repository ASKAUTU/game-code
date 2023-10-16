using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    /// <summary>
    /// ���丮 ��ũ��Ʈ
    /// </summary>
    public InformationScript m_informationScript = null;

    /// <summary>
    /// ü�¹� ��ũ��Ʈ
    /// </summary>
    public HpBar m_hpBar = null;

    /// <summary>
    /// ������ Ÿ��
    /// </summary>
    MoveType.TYPE m_moveType = MoveType.TYPE.Idle;

    /// <summary>
    /// ĳ���� �ӵ�
    /// </summary>
    public float m_speed = 0.0f;

    /// <summary>
    /// ȸ���ӵ�
    /// </summary>
    public float m_rotateSpeed = 0.0f;

    /// <summary>
    /// ĳ���� ������
    /// </summary>
    public float m_jumpPower = 0.0f;

    /// <summary>
    /// �ִ� ü��
    /// </summary>
    public float m_maxHealth = 0;

    /// <summary>
    /// ��ü
    /// </summary>
    public Rigidbody m_rigidbody = null;

    /// <summary>
    /// �ִϸ�����
    /// </summary>
    public Animator m_animator = null;

    /// <summary>
    /// ��ī�� �ִϸ�����
    /// </summary>
    public Animator m_scafAnimator = null;

    /// <summary>
    /// ���� �ִϸ�����
    /// </summary>
    public Animator m_capeAnimator = null;

    /// <summary>
    /// ĳ������ ����� �ҽ�
    /// </summary>
    public AudioSource m_audioSource = null;

    /// <summary>
    /// �������� �Ҹ�
    /// </summary>
    public AudioClip m_dropAudio = null;

    /// <summary>
    /// ���� �Ҹ�
    /// </summary>
    public AudioClip m_landAudio = null;

    /// <summary>
    /// �ȴ� �Ҹ�
    /// </summary>
    public AudioClip m_walkAudio = null;

    /// <summary>
    /// ������ �ٴ� �Ҹ�
    /// </summary>
    public AudioClip m_bloodClip = null;

    /// <summary>
    /// ȸ�� ������Ʈ
    /// </summary>
    public GameObject m_rot = null;

    /// <summary>
    /// ����Ʈ ���̽�
    /// </summary>
    public GameObject m_effectBase = null;

    /// <summary>
    /// ����Ʈ��
    /// </summary>
    public GameObject[] m_effects = null;

    /// <summary>
    /// ������ ���
    /// </summary>
    public Vector3 m_respawnPos = Vector3.zero;

    /// <summary>
    /// ���������ƴ���
    /// </summary>
    public bool m_isStart = false;

    /// <summary>
    /// ���� �ӵ�
    /// </summary>
    float m_currentSpeed = 0.0f;

    /// <summary>
    /// ���� ü��
    /// </summary>
    public float m_currentHealth = 0;

    /// <summary>
    /// ȸ��
    /// </summary>
    float m_healthRegen = 0;

    /// <summary>
    /// ������ ����
    /// </summary>
    bool m_MoveFlag = true;

    /// <summary>
    /// ���� ����
    /// </summary>
    bool m_jumpFlag = false;

    /// <summary>
    /// �뽬 ����
    /// </summary>
    bool m_dashFlag = false;

    /// <summary>
    /// ���������ִ��� �ƴ���
    /// </summary>
    bool m_dropFlag = false;

    /// <summary>
    /// ���������� �ƴ���
    /// </summary>
    bool m_droppedFlag = false;

    /// <summary>
    /// �޸��� ����
    /// </summary>
    bool m_runFlag = false;

    /// <summary>
    /// ���� �÷���
    /// </summary>
    bool m_dieFlag = false;

    /// <summary>
    /// �뽬 �ɷ�
    /// </summary>
    bool m_dashAbility = false;

    /// <summary>
    /// �������� �ɷ�
    /// </summary>
    bool m_doubleJumpAbility = false;

    /// <summary>
    /// ���� ����
    /// </summary>
    int m_jumpCount = 0;

    /// <summary>
    /// �������� ����Ʈ
    /// </summary>
    GameObject _fallEffect = null;

    // Start is called before the first frame update
    void Awake()
    {
        // ���� �÷��� ����
        GManager.Instance.IsPlayerDie = m_dieFlag;

        // ü�� ����
        m_currentHealth = m_maxHealth;

        // �ӵ� ����
        m_currentSpeed = m_speed;

        // ü�� �ڵ� �ڿ�ȸ�� ����
        m_healthRegen = 0.3f;
        StartCoroutine(HealthRegen());

        if (m_isStart)
        {
            StartSetting();
        }
    }

    /// <summary>
    /// ���� �����Ͻø� �۵�
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
        // UI�� HPǥ��
        m_hpBar.WriteHp(m_maxHealth, m_currentHealth);

        // ������, �����Ͻ�, �������Ͻ� ���� ��ȯ (�̴��� ��ɾ��� ������ ���´�)
        if (m_dieFlag || !m_MoveFlag || m_dropFlag) return;

        // �޸���
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

        // �̵��� ���� �Է� ���ϱ�
        Vector3 _input = Vector3.zero;
        _input.x = Input.GetAxisRaw("Horizontal");
        _input.z = Input.GetAxisRaw("Vertical");

        // �̵�Ÿ�� ����
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

        // ĳ���� ��ü�� �ִϸ��̼� ����
        m_animator.SetFloat("State", (int)m_moveType);
        m_animator.SetBool("Jump", m_jumpFlag);

        // ��ī���� �ִϸ��̼� ����
        m_scafAnimator.SetFloat("State", (int)m_moveType);
        m_scafAnimator.SetBool("Jump", m_jumpFlag);

        // ������ �ִϸ��̼� ����
        m_capeAnimator.SetFloat("State", (int)m_moveType);
        m_capeAnimator.SetBool("Jump", m_jumpFlag);
        
        // �̵�
        transform.Translate(_input.normalized * m_currentSpeed * Time.deltaTime);

        // ���¹��� ȸ��
        Turn(_input);

        // ���� �� �������� �ɷ�
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
        
        // �뽬 �ɷ�
        if (!m_dashFlag && m_dashAbility && Input.GetKeyDown(KeyCode.Q))
        {
            m_dashFlag = true;
            Instantiate(m_effects[4], m_rot.transform);
            transform.Translate(_input.normalized * 500f * Time.deltaTime);
            Invoke("CanDashAgain", 3.0f);
        }

        // ���׷� ��Ż�� ����
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
    /// ������� �ʴ� �浹�� �Ͼ��
    /// </summary>
    /// <param name="collision">�浹ü</param>
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
    /// ĳ������ ���� ����
    /// </summary>
    /// <param name="argInput">�Է¹��� �̵���</param>
    void Turn(Vector3 argInput)
    {
        if (argInput.x == 0 && argInput.z == 0) return;
        m_rot.transform.rotation = Quaternion.Slerp(m_rot.transform.rotation, Quaternion.LookRotation(argInput), m_rotateSpeed * Time.deltaTime);
    }

    /// <summary>
    /// �÷��̾� ���� ����
    /// </summary>
    void CanMoveAgain()
    {
        m_MoveFlag = true;
    }

    /// <summary>
    /// ���� ��Ÿ�� �ʱ�ȭ
    /// </summary>
    void CanDashAgain()
    {
        m_dashFlag = false;
    }

    /// <summary>
    /// ���� �׸��ϱ�
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
    /// ������ �Ա�
    /// </summary>
    /// <param name="argDamage">���������</param>
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
    /// ����ý���(�̿ϼ�)
    /// </summary>
    private void OnApplicationQuit()
    {
        SaveLoad.SaveData(this);
    }

    /// <summary>
    /// �ڿ� ü�� ȸ��
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
    /// Ư���ɷ� �ΰ�
    /// </summary>
    /// <param name="argIndex">���° �ɷ�����</param>
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
