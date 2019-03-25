using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.XR.iOS
{
    public class InstantiateCubeButton : MonoBehaviour
    {
        public GameObject interactions;

        // Update is called once per frame
        public void Pressed()
        {
            this.interactions.GetComponent<CubeController>().InstantiateCube();
        }
    }
}