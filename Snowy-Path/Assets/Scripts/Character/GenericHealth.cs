using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericHealth : MonoBehaviour
{
    public int maxHealth = 1;
    private int m_currentHealth = 0;

    [System.Serializable]
    public class OnHitEvent : UnityEvent<string, int> {}
    [System.Serializable]
    public class OnHealEvent : UnityEvent<string, int> {}
    [System.Serializable]
    public class OnDeathEvent : UnityEvent<string> {}

    [Header("Events")]
    public OnHitEvent onHit;
    public OnHealEvent onHeal;
    public OnDeathEvent onDeath;

    void Start()
    {
        m_currentHealth = maxHealth;
    }

    public bool IsAlive()
    {
        return m_currentHealth > 0;
    }

    public void Heal(int value)
    {
        if (!IsAlive()) return;

        m_currentHealth += value;
        if (m_currentHealth > maxHealth)
            m_currentHealth = maxHealth;
        onHeal.Invoke(gameObject.tag, value);
    }

    public void Hit(int value)
    {
        if (!IsAlive()) return;

        m_currentHealth -= value;
        if (m_currentHealth <= 0) {
            m_currentHealth = 0;
            onDeath.Invoke(gameObject.tag);
        }
        else
            onHit.Invoke(gameObject.tag, value);
    }

    /// WARNING: For debug purpose only
    public int GetCurrentHealth()
    {
        return m_currentHealth;
    }
}
