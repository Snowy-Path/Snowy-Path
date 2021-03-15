using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCursor : MonoBehaviour
{
    public Vector2 AnchoredPosition { get { return m_rectTransform.anchoredPosition; } }
    public Vector2 Velocity { set { m_velocity = value; } }

    private RectTransform m_rectTransform;
    private Vector2 m_velocity = Vector2.zero;

    public void Init()
    {
        m_rectTransform = GetComponent<RectTransform>();
    }

    public void UpdatePosition()
    {
        m_rectTransform.anchoredPosition = m_rectTransform.anchoredPosition + m_velocity * Time.deltaTime;
    }

    public void UpdateScale()
    {
        transform.localScale = new Vector3(
            1f / transform.parent.localScale.x,
            1f / transform.parent.localScale.y,
            1f / transform.parent.localScale.z
        );
    }
}
