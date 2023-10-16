using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mask : MonoBehaviour
{
    

    /// <summary>
    /// 오디오 소스
    /// </summary>
    public AudioSource m_audioSource = null;

    /// <summary>
    /// 베이는 소리
    /// </summary>
    public AudioClip m_hitAudio = null;

    /// <summary>
    /// 맞는 이펙트
    /// </summary>
    public GameObject m_hitEffect = null;

    /// <summary>
    /// 현재 체력
    /// </summary>
    int m_currentHealth = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_currentHealth = 30;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 데미지 받기
    /// </summary>
    public void Damage(int argWeaponDamage)
    {
        m_currentHealth -= argWeaponDamage;
        m_audioSource.PlayOneShot(m_hitAudio);
        GameObject _slashObj = Instantiate(m_hitEffect);
        _slashObj.transform.SetParent(transform);
        _slashObj.transform.position = transform.position;

        if (m_currentHealth <= 0)
        {
            GManager.Instance.IsPlayerSc.GiveAbility(1);
            GManager.Instance.IsPlayerSc.GiveAbility(2);
            Destroy(gameObject);
        }
    }
}
