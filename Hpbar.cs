using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    RectTransform m_rectTransform = null;

    void Start()
    {
        m_rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void WriteHp(float argMaxHealth, float argCurrentHealth)
    {
        m_rectTransform.offsetMax = new Vector2(-(400f - (argCurrentHealth / argMaxHealth * 400f)), 0);
    }
}
