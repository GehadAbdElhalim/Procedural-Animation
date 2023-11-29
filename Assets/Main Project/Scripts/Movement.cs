using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralAnimation
{
    public class Movement : MonoBehaviour
    {
        private void Update()
        {
            transform.Translate(Input.GetAxis("Vertical") * transform.forward * 5f * Time.deltaTime);
        }
    }
}