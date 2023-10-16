using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MobData", menuName = "Game Data/New Mob Data", order = 1)]
public class MobData : ScriptableObject
{
    [Tooltip("최대 체력")]
    public int m_maxHp = 0;

    [Tooltip("속도")]
    public float m_speed = 0.0f;

    [Tooltip("탐색 범위")]
    public float m_searchLength = 0.0f;

    [Tooltip("공격 데미지")]
    public int m_damage = 0;
}
