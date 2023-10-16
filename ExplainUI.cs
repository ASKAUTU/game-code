using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplainUI : MonoBehaviour
{
    /// <summary>
    /// 설명 내용들
    /// </summary>
    public string[] m_contants = null;

    public float[] m_skipTimes = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator StartExplain()
    {
        for (int i = 0; i < m_contants.Length; i++)
        {
            StartCoroutine(ShowText(i));
            yield return new WaitForSeconds(5.0f);
        }
    }

    IEnumerator ShowText(int argIndex)
    {
        Text _text = gameObject.GetComponent<Text>();
        _text.text = m_contants[argIndex];
        Color _color = _text.color;
        _color.a = 0;
        gameObject.GetComponent<Text>().color = _color;
        for (float i = _color.a; i <= 1; i += 0.025f)
        {
            _color.a = i;
            gameObject.GetComponent<Text>().color = _color;
            yield return new WaitForSeconds(0.025f);
        }
        yield return new WaitForSeconds(m_skipTimes[argIndex]);
        _color.a = 1;
        gameObject.GetComponent<Text>().color = _color;
        for (float i = _color.a; i >= 0; i -= 0.025f)
        {
            _color.a = i;
            gameObject.GetComponent<Text>().color = _color;
            yield return new WaitForSeconds(0.025f);
        }
    }
}
