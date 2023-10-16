using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    /// <summary>
    /// 공지 스크립트
    /// </summary>
    public InformationScript m_informationScript = null;

    /// <summary>
    /// 인덱스 번호
    /// </summary>
    public int m_index = 0;

    /// <summary>
    /// 애니메이터
    /// </summary>
    public Animator m_animator = null;

    /// <summary>
    /// 오디오 소스
    /// </summary>
    public AudioSource m_audioSource = null;

    /// <summary>
    /// 칼휘두르는 소리
    /// </summary>
    public AudioClip[] m_slashSounds = null;

    public AudioClip m_slashLaver = null;

    /// <summary>
    /// 휘두르는 이펙트
    /// </summary>
    public GameObject[] m_effect = null;

    public GameObject m_effectBase = null;

    /// <summary>
    /// 휘두루는 플래그
    /// </summary>
    bool m_slashFlag = false;

    /// <summary>
    /// 휘두르는 종류
    /// </summary>
    float m_slashNum = 0.0f;

    /// <summary>
    /// 데미지
    /// </summary>
    int m_damage = 0;

    /// <summary>
    /// 이미 때렸는지 아닌지
    /// </summary>
    bool m_isHitted = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // 마우스 좌클릭시
        if (Input.GetMouseButtonDown(0))
        {
            if (m_slashFlag) return;
            m_slashFlag = true;
            m_animator.SetBool("Slash", true);
            m_animator.SetFloat("SlashNum", m_slashNum);
            CreateEffect();
            m_audioSource.PlayOneShot(m_slashSounds[(int)m_slashNum]);
            Invoke("CanSlashAgain", 0.4f);
        }
    }

    /// <summary>
    /// 검 베기 쿨타임 초기화
    /// </summary>
    void CanSlashAgain()
    {
        m_animator.SetBool("Slash", false);
        m_slashFlag = false;
        if (m_slashNum >= 3.0f)
        {
            ResetSlashNum();
            return;
        }
        m_slashNum++;
    }

    /// <summary>
    /// 검 베기 형태(애니메이션) 초기화
    /// </summary>
    void ResetSlashNum()
    {
        m_slashNum = 0.0f;
    }

    /// <summary>
    /// 이펙트 생성
    /// </summary>
    void CreateEffect()
    {
        if (m_slashNum == 3)
        {
            GameObject _stingObj = Instantiate(m_effect[1], m_effectBase.transform);
            _stingObj.transform.position = m_effectBase.transform.position;
            _stingObj.transform.localPosition = new Vector3(4.5f, 0.28f, 0);
            _stingObj.transform.localRotation = Quaternion.Euler(0, -90, 0);
            return;
        }
        GameObject _slashObj = Instantiate(m_effect[0]);

        _slashObj.transform.SetParent(m_effectBase.transform);
        _slashObj.transform.position = m_effectBase.transform.position; 
        switch (m_slashNum)
        {
            case 0:
                _slashObj.transform.localRotation = Quaternion.Euler(-90, 0, 0);
                break;
            case 1:
                _slashObj.transform.localRotation = Quaternion.Euler(-50, 0, 0);
                break;
            case 2:
                _slashObj.transform.localRotation = Quaternion.Euler(-120, 0, 0);
                break;
        }
    }

    /// <summary>
    /// 통과하는 충돌 발생시
    /// </summary>
    /// <param name="_hit">통과 충돌체</param>
    private void OnTriggerEnter(Collider _hit)
    {
        if (!m_slashFlag) return;
        switch (_hit.gameObject.layer)
        {
            case 7:
                m_damage = Random.Range(GManager.Instance.GetWeaponData(m_index).m_minDamage, GManager.Instance.GetWeaponData(m_index).m_maxDamage);
                _hit.gameObject.GetComponent<MonsterSc>().Damage(m_damage);
                break;
            case 9:
                if (m_isHitted) return;
                m_isHitted = true;
                _hit.GetComponent<LaverRotate>().RotateLaver();
                m_audioSource.PlayOneShot(m_slashLaver);
                Invoke("CanHitAgain", 0.4f);
                break;
            case 10:
                m_audioSource.PlayOneShot(m_slashLaver);
                if (m_isHitted) return;
                m_isHitted = true;
                m_informationScript.CrackVoice();
                Invoke("CanHitAgain", 2f);
                break;
            case 11:
                m_audioSource.PlayOneShot(m_slashLaver);
                if (m_isHitted) return;
                m_isHitted = true;
                m_damage = Random.Range(GManager.Instance.GetWeaponData(m_index).m_minDamage, GManager.Instance.GetWeaponData(m_index).m_maxDamage);
                _hit.GetComponent<Mask>().Damage(m_damage);
                Invoke("CanHitAgain", 0.4f);
                break;
        }
    }

    /// <summary>
    /// 충돌 쿨타임 초기화
    /// </summary>
    void CanHitAgain()
    {
        m_isHitted = false;
    }
}
