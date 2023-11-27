using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

namespace ProceduralAnimation
{
    public class IKSolver : MonoBehaviour
    {
        [SerializeField] List<Transform> pointTransforms = new List<Transform>();
        [SerializeField] Transform targetTransform;
        [SerializeField] int iterationsCount = 10;
        [SerializeField] float toleranceDistance = 1f;

        private List<float> armLengths = new List<float>();

        private void Start()
        {
            for (int i = 1; i < pointTransforms.Count; i++)
            {
                float distanceBetweenPoints = Vector3.Distance(pointTransforms[i].position, pointTransforms[i - 1].position);
                armLengths.Add(distanceBetweenPoints);
            }
        }

        private void FixedUpdate()
        {
            //if (IsTargetBeyondReach())
            //    return;

            List<Vector3> newPoints = pointTransforms.Select(p => p.position).ToList();
            for (int i = 0; i < iterationsCount; i++)
            {
                newPoints = BackwardPass(newPoints);
                newPoints = ForwardPass(newPoints);

                if (IsArmNearTarget()) break;
            }
            DebugLines(newPoints);
            RotateArmPartsToMatchPoints(newPoints);
        }

        private bool IsTargetBeyondReach()
        {
            float totalArmLength = 0;
            foreach (var armLength in armLengths)
            {
                totalArmLength += armLength;
            }
            float distanceFromStartPointToTarget = Vector3.Distance(targetTransform.position, pointTransforms[0].position);
            return totalArmLength < distanceFromStartPointToTarget;
        }

        private List<Vector3> ForwardPass(List<Vector3> points)
        {
            points[0] = pointTransforms[0].position;

            for (int i = 1; i < armLengths.Count; i++)
            {
                Vector3 direction = (points[i] - points[i - 1]).normalized;
                points[i] = points[i - 1] + direction * armLengths[i];
            }

            return points;
        }

        private List<Vector3> BackwardPass(List<Vector3> points)
        {
            points[points.Count - 1] = targetTransform.position;

            for (int i = armLengths.Count - 1; i >= 0; i--)
            {
                Vector3 direction = (points[i] - points[i + 1]).normalized;
                points[i] = points[i + 1] + direction * armLengths[i];
            }

            return points;
        }

        private bool IsArmNearTarget()
        {
            return Vector3.Distance(pointTransforms[pointTransforms.Count - 1].position, targetTransform.position) <= toleranceDistance;
        }

        private void DebugLines(List<Vector3> newPoints)
        {
            for (int i = 0; i < armLengths.Count; i++)
            {
                Debug.DrawLine(newPoints[i], newPoints[i + 1], GetRandomColor(i));
            }

            Color GetRandomColor(int seed)
            {
                Color[] color = new Color[] { Color.red, Color.green, Color.blue };
                return color[seed % color.Length];
            }
        }

        private void RotateArmPartsToMatchPoints(List<Vector3> newPoints)
        {
            for (int i = 0; i < armLengths.Count; i++)
            {
                pointTransforms[i].LookAt(newPoints[i + 1]);
            }
        }
    }
}