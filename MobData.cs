using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MobData", menuName = "Game Data/New Mob Data", order = 1)]
public class MobData : ScriptableObject
{
    [Tooltip("�ִ� ü��")]
    public int m_maxHp = 0;

    [Tooltip("�ӵ�")]
    public float m_speed = 0.0f;

    [Tooltip("Ž�� ����")]
    public float m_searchLength = 0.0f;

    [Tooltip("���� ������")]
    public int m_damage = 0;
}
