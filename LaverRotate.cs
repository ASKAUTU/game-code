using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaverRotate : MonoBehaviour
{
    /// <summary>
    /// 레버
    /// </summary>
    public GameObject m_laver = null;

    /// <summary>
    /// 문
    /// </summary>
    public GameObject m_door = null;

    /// <summary>
    /// 스파클 이펙트
    /// </summary>
    public GameObject m_sparkleEffect = null;

    /// <summary>
    /// 카메라
    /// </summary>
    public GameObject m_Camera = null;

    /// <summary>
    /// 레버 오디오 소스
    /// </summary>
    public AudioSource m_laverAudioSource = null;

    /// <summary>
    /// 문 오디오 소스
    /// </summary>
    public AudioSource m_doorAudioSource = null;

    /// <summary>
    /// 레버 댕겨지는 소리
    /// </summary>
    public AudioClip m_laverAudio = null;

    /// <summary>
    /// 문 열리는 소리
    /// </summary>
    public AudioClip m_doorAudio = null;

    /// <summary>
    /// 회전유무
    /// </summary>
    bool m_isRotating = false;

    /// <summary>
    /// 회전했다
    /// </summary>
    bool m_isRotated = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_isRotating || !m_isRotated) return;
        m_laver.transform.localRotation = Quaternion.Slerp(m_laver.transform.localRotation, Quaternion.Euler(0, 0, 135), 4.0f * Time.deltaTime);
        m_door.transform.localPosition = Vector3.Slerp(m_door.transform.position, new Vector3(33.8614006f, 25, 7.91949987f), 0.2f * Time.deltaTime);
        if (m_door.transform.localPosition == new Vector3(33.8614006f, 25, 7.91949987f)) m_isRotating = false;
    }

    public void RotateLaver() 
    {
        Instantiate(m_sparkleEffect, transform);
        if (m_isRotating || m_isRotated) return;
        m_isRotating = true;
        m_isRotated = true;
        m_laverAudioSource.PlayOneShot(m_laverAudio);
        Invoke("DoorSound", 0.5f);
    }

    void DoorSound()
    {
        m_doorAudioSource.PlayOneShot(m_doorAudio);
        StartCoroutine(ShakeCamera());
    }

    IEnumerator ShakeCamera()
    {
        for (int i = 0; i < 12; i++)
        {
            m_Camera.transform.Translate((Vector2)Random.insideUnitCircle * 1.0f);
            yield return new WaitForSeconds(0.1f);
            m_Camera.transform.localPosition = new Vector3(2.31999969f, 1.58000004f, -8.21000004f);
            yield return new WaitForSeconds(0.1f);
        }
        m_Camera.transform.localPosition = new Vector3(2.31999969f, 1.58000004f, -8.21000004f);
    }
}
