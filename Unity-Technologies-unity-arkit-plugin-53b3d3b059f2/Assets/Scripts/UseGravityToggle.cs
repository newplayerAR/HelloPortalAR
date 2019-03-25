using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.XR.iOS
{ 

    public class UseGravityToggle : MonoBehaviour
    {
        public GameObject interactions;

        public void OnToggleChanged(bool b)
        {
            this.interactions.GetComponent<CubeController>().UseGravity = b;
        }
    }
}