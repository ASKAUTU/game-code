using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Game Data/New Weapon Data", order = 1)]
public class WeaponData : ScriptableObject
{
    [Tooltip("최소 데미지")]
    public int m_minDamage = 0;

    [Tooltip("최대 데미지")]
    public int m_maxDamage = 0;
}
