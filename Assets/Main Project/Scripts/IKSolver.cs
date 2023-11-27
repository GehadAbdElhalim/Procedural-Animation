using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralAnimation
{
    public class IKSolver : MonoBehaviour
    {
        [SerializeField] List<Transform> pointTransforms = new List<Transform>();
        [SerializeField] Transform targetTransform;
        [SerializeField] int iterationsCount = 10;

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
            if (IsTargetBeyondReach())
                return;

            List<Vector3> newPoints = pointTransforms.Select(p => p.position).ToList();
            for (int i = 0; i < iterationsCount; i++)
            {
                newPoints = BackwardCheck(newPoints);
                newPoints = ForwardCheck(newPoints);
            }

            //Lerp all original points to their new points
        }

        private List<Vector3> ForwardCheck(List<Vector3> points)
        {
            return new List<Vector3>();
        }

        private List<Vector3> BackwardCheck(List<Vector3> points)
        {
            return new List<Vector3>();
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
    }
}