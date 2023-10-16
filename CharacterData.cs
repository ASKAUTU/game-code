using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

[System.Serializable]
public class CharacterData
{
    public float m_playerSpeed = 0;

    public CharacterData(Player argPlayerSc)
    {
        m_playerSpeed = argPlayerSc.m_speed;
    }
}
