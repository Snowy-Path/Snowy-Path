using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DynamicComponent))]
public class StelePuzzle : MonoBehaviour {

    [SerializeField]
    [Tooltip("Bowl used in this puzzle. MANDATORY: Put them in order.")]
    private BowlStelePuzzle[] m_bowls;

    [SerializeField]
    [Tooltip("Indexes that must be activated to finish the puzzle. Only (and only) them must be activated to validate the puzzle.")]
    private List<int> m_indexesToValidate;

    private DynamicComponent m_dynamicComponent;

    [SerializeField]
    [Tooltip("Sound played when the puzzle is finised.")]
    private FMODUnity.StudioEventEmitter m_finishPuzzleEmitter;

    [SerializeField]
    [Tooltip("Gradient used by particle when the puzzle is finished.")]
    private Gradient m_finishedColor;

    private void Start() {
        m_dynamicComponent = GetComponent<DynamicComponent>();
    }

    public void CheckValidity() {
        Debug.Log(IsPuzzleFinished());
        if (IsPuzzleFinished()) {
            m_dynamicComponent.SendDynamicEvent();
            ClosePuzzle();
        }
    }

    private bool IsPuzzleFinished() {
        for (int i = 0; i < m_bowls.Length; i++) {
            bool _contained = m_indexesToValidate.Contains(i);
            if ( (_contained && !m_bowls[i].IsActive) || (!_contained && m_bowls[i].IsActive)) {
                return false;
            }
        }
        return true;
    }

    private void ClosePuzzle() {
        foreach (var bowl in m_bowls) {
            bowl.LightBowl();
            bowl.ChangeParticleColor(m_finishedColor);
            bowl.GetComponent<Interactable>().SwitchActivation();
        }
        m_finishPuzzleEmitter.Play();
        this.enabled = false;
    }

}
