using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GManager : MonoBehaviour
{
    /// <summary>
    /// 자기 자신
    /// </summary>
    static GManager g_instance = null;

    /// <summary>
    /// 몬스터 데이터
    /// </summary>
    public List<MobData> m_mobData = null;

    /// <summary>
    /// 무기 데이터
    /// </summary>
    public List<WeaponData> m_weaponData = null;

    /// <summary>
    /// 플레이어 오브젝트
    /// </summary>
    GameObject m_playerObj = null;

    /// <summary>
    /// 플레이어 스크립트
    /// </summary>
    Player m_playerSc = null;

    /// <summary>
    /// 플레이어 죽음 플래그
    /// </summary>
    bool m_playDieFlag = false;

    /// <summary>
    /// 자기 자신 반환
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
