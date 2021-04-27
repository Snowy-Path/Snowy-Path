using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spline : MonoBehaviour {

    private Vector3[] splinePoint;
    private int splineCount;

    private void Start() {
        splineCount = transform.childCount;
        splinePoint = new Vector3[splineCount];

        for (int i = 0; i < splineCount; i++) {
            splinePoint[i] = transform.GetChild(i).position;
        }
    }

    internal Vector3 WhereOnSpline(Vector3 position) {
        int closestSpinePoint = GetClosestSplinePoint(position);

        if (closestSpinePoint == 0) {
            return SplineSegment(splinePoint[0], splinePoint[1], position);
        } else if(closestSpinePoint == splineCount - 1) {
            return SplineSegment(splinePoint[splineCount - 1], splinePoint[splineCount - 2], position);
        } else {
            Vector3 leftSeg = SplineSegment(splinePoint[closestSpinePoint - 1], splinePoint[closestSpinePoint], position);
            Vector3 rightSeg = SplineSegment(splinePoint[closestSpinePoint + 1], splinePoint[closestSpinePoint], position);

            if ((position - leftSeg).sqrMagnitude <= (position - rightSeg).sqrMagnitude) {
                return leftSeg;
            } else {
                return rightSeg;
            }
        }
    }

    private int GetClosestSplinePoint(Vector3 position) {
        int closestPoint = -1;
        float shortestDistance = float.PositiveInfinity;

        for (int i = 0; i < splineCount; i++) {
            float sqrDistance = (splinePoint[i] - position).sqrMagnitude;
            if (sqrDistance < shortestDistance) {
                shortestDistance = sqrDistance;
                closestPoint = i;
            }
        }

        return closestPoint;
    }

    private Vector3 SplineSegment(Vector3 v1, Vector3 v2, Vector3 position) {
        Vector3 v1ToPos = position - v1;
        Vector3 seqDirection = (v2 - v1).normalized;

        float distanceFromV1 = Vector3.Dot(seqDirection, v1ToPos);

        if (distanceFromV1 < 0.0f) {
            return v1;
        } else if (distanceFromV1 * distanceFromV1 > (v2 - v1).sqrMagnitude) {
            return v2;
        } else {
            Vector3 fromV1 = seqDirection * distanceFromV1;
            return v1 + fromV1;
        }
    }

    private void Update() {
#if DEBUG
        if (splineCount > 1) {
            for (int i = 0; i < splineCount-1; i++) {
                Debug.DrawLine(splinePoint[i], splinePoint[i+1], Color.yellow);
            }
        }
#endif
    }

}
