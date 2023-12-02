using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralAnimation
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] List<ArmMovement> armMovements1;
        [SerializeField] List<ArmMovement> armMovements2;

        private void Awake()
        {
            foreach (var armMovement in armMovements1)
            {
                armMovement.OnArmStartedMoving += () => ToggleMovementCapabiltiy(armMovements2, false);
                armMovement.OnArmStoppedMoving += () => ToggleMovementCapabiltiy(armMovements2, true);
            }

            foreach (var armMovement in armMovements2)
            {
                armMovement.OnArmStartedMoving += () => ToggleMovementCapabiltiy(armMovements1, false);
                armMovement.OnArmStoppedMoving += () => ToggleMovementCapabiltiy(armMovements1, true);
            }
        }

        private void Update()
        {
            float xInput = Input.GetAxis("Horizontal");
            float zInput = Input.GetAxis("Vertical");
            float yInput = Input.GetKey(KeyCode.Space) ? 1f : 0f;
            yInput += Input.GetKey(KeyCode.C) ? -1f : 0;
            transform.Translate(zInput * transform.forward * 5f * Time.deltaTime, Space.World);
            transform.Translate(yInput * transform.up * 5f * Time.deltaTime, Space.World);
            transform.Rotate(0, xInput * 50 * Time.deltaTime, 0);
        }

        private void LateUpdate()
        {
            Vector3 v1 = (armMovements1[0].transform.position - armMovements1[1].transform.position).normalized;
            Vector3 v2 = (armMovements2[0].transform.position - armMovements2[1].transform.position).normalized;

            transform.rotation = Quaternion.FromToRotation(transform.up, Vector3.Cross(v1, v2));
        }

        private void ToggleMovementCapabiltiy(List<ArmMovement> armMovements, bool IsCapableOfMoving)
        {
            foreach(var armMovement in armMovements) 
            {
                armMovement.canMove = IsCapableOfMoving;
            }
        }
    }
}