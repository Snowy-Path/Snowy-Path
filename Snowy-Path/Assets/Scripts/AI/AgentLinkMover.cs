using UnityEngine;
using UnityEngine.AI;
using System.Collections;

/// <summary>
/// OffMeshLink movement method
/// </summary>
public enum OffMeshLinkMoveMethod {
    Teleport,
    NormalSpeed
    //Parabola,
    //Curve
}

/// <summary>
/// Move an agent when traversing a OffMeshLink given specific animated methods
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class AgentLinkMover : MonoBehaviour {
    public OffMeshLinkMoveMethod m_Method = OffMeshLinkMoveMethod.NormalSpeed;
    public AnimationCurve m_Curve = new AnimationCurve();

    IEnumerator Start() {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.autoTraverseOffMeshLink = false;
        while (true) {
            if (agent.isOnOffMeshLink) {
                if (m_Method == OffMeshLinkMoveMethod.NormalSpeed)
                    yield return StartCoroutine(NormalSpeed(agent));
                //else if (m_Method == OffMeshLinkMoveMethod.Parabola)
                //    yield return StartCoroutine(Parabola(agent, 2.0f, 0.5f));
                //else if (m_Method == OffMeshLinkMoveMethod.Curve)
                //    yield return StartCoroutine(Curve(agent, 0.5f));
                agent.CompleteOffMeshLink();
            }
            yield return null;
        }
    }

    /// <summary>
    /// Move along the link using the agent's speed.
    /// </summary>
    /// <param name="agent">Agent to move.</param>
    /// <returns></returns>
    IEnumerator NormalSpeed(NavMeshAgent agent) {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
        agent.updateRotation = false;
        while (agent.transform.position != endPos) {
            RotateAgent(agent, endPos);
            agent.transform.position = Vector3.MoveTowards(agent.transform.position, endPos, agent.speed * Time.deltaTime);
            yield return null;
        }
        agent.updateRotation = true;
    }

    //IEnumerator Parabola(NavMeshAgent agent, float height, float duration) {
    //    OffMeshLinkData data = agent.currentOffMeshLinkData;
    //    Vector3 startPos = agent.transform.position;
    //    Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
    //    float normalizedTime = 0.0f;
    //    agent.updateRotation = false;
    //    while (normalizedTime < 1.0f) {
    //        RotateAgent(agent, endPos);
    //        float yOffset = height * 4.0f * (normalizedTime - normalizedTime * normalizedTime);
    //        agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
    //        normalizedTime += Time.deltaTime / duration;
    //        yield return null;
    //    }
    //    agent.updateRotation = true;
    //}

    //IEnumerator Curve(NavMeshAgent agent, float duration) {
    //    OffMeshLinkData data = agent.currentOffMeshLinkData;
    //    Vector3 startPos = agent.transform.position;
    //    Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
    //    float normalizedTime = 0.0f;
    //    agent.updateRotation = false;
    //    while (normalizedTime < 1.0f) {
    //        RotateAgent(agent, endPos);
    //        float yOffset = m_Curve.Evaluate(normalizedTime);
    //        agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
    //        normalizedTime += Time.deltaTime / duration;
    //        yield return null;
    //    }
    //    agent.updateRotation = true;
    //}

    /// <summary>
    /// Rotate the agent to make it look at the end position of the OffMeshLink.
    /// </summary>
    /// <param name="agent">Agent to rotate.</param>
    /// <param name="endPos">Position to look at.</param>
    private void RotateAgent(NavMeshAgent agent, Vector3 endPos) {
        Quaternion rotate = Quaternion.LookRotation(endPos - agent.transform.position);
        agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, rotate, Time.deltaTime * Mathf.Deg2Rad * agent.angularSpeed);
    }

}
