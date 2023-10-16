using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mask : MonoBehaviour
{
    

    /// <summary>
    /// ����� �ҽ�
    /// </summary>
    public AudioSource m_audioSource = null;

    /// <summary>
    /// ���̴� �Ҹ�
    /// </summary>
    public AudioClip m_hitAudio = null;

    /// <summary>
    /// �´� ����Ʈ
    /// </summary>
    public GameObject m_hitEffect = null;

    /// <summary>
    /// ���� ü��
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
    /// ������ �ޱ�
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
