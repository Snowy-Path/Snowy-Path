using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

public class BreathVelocity : MonoBehaviour {

    [SerializeField]
    private VisualEffect m_coldBreath;
    private ExposedProperty m_MyProperty;

    [SerializeField]
    private PlayerController m_playerController;


    void Start() {
        m_MyProperty = "PlayerVelocity"; // Assign A string
    }

    private void Update() {
        m_coldBreath.SetVector3(m_MyProperty, (m_playerController.XZVelocity + m_playerController.ActualVelocity) * m_playerController.SpeedFactor);
    }


}
