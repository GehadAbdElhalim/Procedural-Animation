using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralAnimation
{
    public class ArmMovement : MonoBehaviour
    {
        [SerializeField] Movement body;

        [SerializeField] Transform raycastStartTransform;
        [SerializeField] LayerMask raycastLayers;
        [SerializeField] float maxDistanceAllowedBetweenArmAndRayCast = 5f;

        public bool IsGrounded { get; private set; }
        public bool canMove = true;

        public Action OnArmStartedMoving;
        public Action OnArmStoppedMoving;

        private void FixedUpdate()
        {
            if (!canMove) return;

            if (Physics.Raycast(raycastStartTransform.position, raycastStartTransform.forward, out RaycastHit hitInfo, 1000f, raycastLayers))
            {
                Debug.DrawLine(raycastStartTransform.position, hitInfo.point, Color.red);
                if (Vector3.Distance(transform.position, hitInfo.point) > maxDistanceAllowedBetweenArmAndRayCast)
                {
                    StopAllCoroutines();
                    StartCoroutine(MoveArmToPosition(hitInfo.point, 0.5f));
                }
            }
        }

        IEnumerator MoveArmToPosition(Vector3 targetPosition, float duration)
        {
            OnArmStartedMoving?.Invoke();
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.fixedDeltaTime;
                transform.position = Vector3.Lerp(transform.position, targetPosition, elapsedTime / duration);
                //transform.position += body.transform.up * Mathf.Lerp(0, 2f, duration - elapsedTime / duration);

                if (Vector3.Distance(transform.position, targetPosition) <= 0.5f)
                    OnArmStoppedMoving?.Invoke();

                yield return new WaitForFixedUpdate();
            }
        }
    }
}