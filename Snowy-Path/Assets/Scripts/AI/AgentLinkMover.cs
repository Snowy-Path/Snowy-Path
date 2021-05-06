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
    //public AnimationCurve m_Curve = new AnimationCurve();

    private NavMeshAgent agent;

    public bool teleported = false;

    /// <summary>
    /// Retrieves the NavMeshAgent and disable auto traverse link.
    /// </summary>
    void Start() {
        agent = GetComponent<NavMeshAgent>();
        agent.autoTraverseOffMeshLink = false;
    }

    /// <summary>
    /// Start the OffMeshLink navigation coroutine.
    /// </summary>
    void OnEnable() {
        StartCoroutine(TraverseLink());
    }


    /// <summary>
    /// The OffMeshLink navigation coroutine. It determines the correct way to move the agent on a link.
    /// </summary>
    /// <returns></returns>
    IEnumerator TraverseLink() {
        while (true) {
            if (agent != null && agent.isOnOffMeshLink) {
                if (m_Method == OffMeshLinkMoveMethod.NormalSpeed)
                    yield return StartCoroutine(NormalSpeed(agent));
                //else if (m_Method == OffMeshLinkMoveMethod.Parabola)
                //    yield return StartCoroutine(Parabola(agent, 2.0f, 0.5f));
                //else if (m_Method == OffMeshLinkMoveMethod.Curve)
                //    yield return StartCoroutine(Curve(agent, 0.5f));
                agent.CompleteOffMeshLink();
            }
            teleported = false;
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
        while (agent.transform.position != endPos && !teleported) {
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

        //Debug.Log(Quaternion.Dot(rotate, agent.transform.rotation));
        if (Quaternion.Dot(rotate, agent.transform.rotation) >= 0.95f) {
            return;
        }

        agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, rotate, Time.deltaTime * Mathf.Deg2Rad * agent.angularSpeed);

    }

}
