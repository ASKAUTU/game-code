using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GManager : MonoBehaviour
{
    /// <summary>
    /// �ڱ� �ڽ�
    /// </summary>
    static GManager g_instance = null;

    /// <summary>
    /// ���� ������
    /// </summary>
    public List<MobData> m_mobData = null;

    /// <summary>
    /// ���� ������
    /// </summary>
    public List<WeaponData> m_weaponData = null;

    /// <summary>
    /// �÷��̾� ������Ʈ
    /// </summary>
    GameObject m_playerObj = null;

    /// <summary>
    /// �÷��̾� ��ũ��Ʈ
    /// </summary>
    Player m_playerSc = null;

    /// <summary>
    /// �÷��̾� ���� �÷���
    /// </summary>
    bool m_playDieFlag = false;

    /// <summary>
    /// �ڱ� �ڽ� ��ȯ
    /// </summary>
    public static GManager Instance
    {
        get 
        { 
            return g_instance; 
        }
    }

    private void Awake()
    {
        if (GManager.Instance == null)
        {
            g_instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject IsPlayer
    {
        get
        {
            if (m_playerObj == null) m_playerObj = GameObject.Find("Player");

            return m_playerObj;
        }
    }

    public Player IsPlayerSc
    {
        get
        {
            if(m_playerSc == null) m_playerSc = IsPlayer.GetComponent<Player>();
            return m_playerSc;
        }
    }

    public bool IsPlayerDie
    {
        get
        {
            return m_playDieFlag;
        }
        set
        {
            m_playDieFlag = value;
        }
    }

    public bool CheckLength(Vector3 argPos, float argLength)
    {
        return Vector3.Distance(IsPlayer.transform.position, argPos) <= argLength ? true : false;
    }

    public MobData GetMonsterData(int argIndex)
    {
        MobData _data = null;

        _data = m_mobData[argIndex];
        return _data;
    }

    public WeaponData GetWeaponData(int argIndex)
    {
        WeaponData _data = null;

        _data = m_weaponData[argIndex];
        return _data;
    }
}
