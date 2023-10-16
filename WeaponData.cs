using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Game Data/New Weapon Data", order = 1)]
public class WeaponData : ScriptableObject
{
    [Tooltip("�ּ� ������")]
    public int m_minDamage = 0;

    [Tooltip("�ִ� ������")]
    public int m_maxDamage = 0;
}
