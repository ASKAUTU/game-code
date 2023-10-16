using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    /// <summary>
    /// 이동 속도
    /// </summary>
    public float m_moveSpeed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MoveRandom());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator MoveRandom()
    {
        yield return new WaitForSeconds(Random.Range(0.0f, 3.0f));
        while (true)
        {
            for (int i = 0; i < 100; i++)
            {
                transform.Translate(Vector3.up * m_moveSpeed * Time.deltaTime);
                yield return new WaitForSeconds(0.025f);
            }
            yield return new WaitForSeconds(Random.Range(0.0f, 1.0f));
            for (int i = 0; i < 100; i++)
            {
                transform.Translate(Vector3.down * m_moveSpeed * Time.deltaTime);
                yield return new WaitForSeconds(0.025f);
            }
            yield return new WaitForSeconds(Random.Range(0.0f, 1.0f));
        }
    }
}
