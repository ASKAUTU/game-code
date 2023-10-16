using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaverRotate : MonoBehaviour
{
    /// <summary>
    /// ����
    /// </summary>
    public GameObject m_laver = null;

    /// <summary>
    /// ��
    /// </summary>
    public GameObject m_door = null;

    /// <summary>
    /// ����Ŭ ����Ʈ
    /// </summary>
    public GameObject m_sparkleEffect = null;

    /// <summary>
    /// ī�޶�
    /// </summary>
    public GameObject m_Camera = null;

    /// <summary>
    /// ���� ����� �ҽ�
    /// </summary>
    public AudioSource m_laverAudioSource = null;

    /// <summary>
    /// �� ����� �ҽ�
    /// </summary>
    public AudioSource m_doorAudioSource = null;

    /// <summary>
    /// ���� ������� �Ҹ�
    /// </summary>
    public AudioClip m_laverAudio = null;

    /// <summary>
    /// �� ������ �Ҹ�
    /// </summary>
    public AudioClip m_doorAudio = null;

    /// <summary>
    /// ȸ������
    /// </summary>
    bool m_isRotating = false;

    /// <summary>
    /// ȸ���ߴ�
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
