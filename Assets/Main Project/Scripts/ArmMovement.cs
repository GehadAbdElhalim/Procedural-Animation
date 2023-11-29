using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralAnimation
{
    public class ArmMovement : MonoBehaviour
    {
        [SerializeField] Transform raycastStartTransform;
        [SerializeField] LayerMask raycastLayers;
        [SerializeField] float maxDistanceAllowedBetweenArmAndRayCast = 5f;

        private void FixedUpdate()
        {
            if (Physics.Raycast(raycastStartTransform.position, raycastStartTransform.forward, out RaycastHit hitInfo, 1000f, raycastLayers))
            {
                Debug.DrawLine(raycastStartTransform.position, hitInfo.point, Color.red);
                if (Vector3.Distance(transform.position, hitInfo.point) > maxDistanceAllowedBetweenArmAndRayCast)
                {
                    StopAllCoroutines();
                    StartCoroutine(LerpToPosition(hitInfo.point, 1f));
                }
            }
        }

        IEnumerator LerpToPosition(Vector3 targetPosition, float duration)
        {
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.fixedDeltaTime;
                transform.position = Vector3.Lerp(transform.position, targetPosition, elapsedTime / duration);
                yield return new WaitForFixedUpdate();
            }
        }
    }
}