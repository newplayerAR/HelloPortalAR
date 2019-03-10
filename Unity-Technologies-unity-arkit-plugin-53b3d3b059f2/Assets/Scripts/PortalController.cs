using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEngine.XR.iOS
{

    public class PortalController : MonoBehaviour
    {
        public Transform device;
        private bool wasInFront;
        private bool inOtherWorld;
        private bool isColliding;

        void Start()
        {
            //start outside other world
            SetMaterials(false);
        }

        void SetMaterials(bool fullRender)
        {
            var stencilTest = fullRender ? CompareFunction.NotEqual : CompareFunction.Equal;
            Shader.SetGlobalInt("_StencilTest", (int)stencilTest);
        }

        bool GetIsInFront()
        {
            Vector3 worldPos = device.position + device.forward * Camera.main.nearClipPlane;

            Vector3 pos = transform.InverseTransformPoint(worldPos);
            return pos.z >= 0 ? true : false;
        }


        //This technique registeres if the device has hit the portal, flipping the bool

        void OnTriggerEnter(Collider other)
        {
            if (other.transform != device)
                return;
            //Important to do this for if the user re-enters the portal from the same side
            wasInFront = GetIsInFront();
            isColliding = true;
        }

        void OnTriggerExit(Collider other)
        {
            if (other.transform != device)
                return;
            isColliding = false;
        }


        /*If there has been a change in the relative position of the device to the portal, flip the
         *Stencil Test
         */

        void WhileCameraColliding()
        {
            if (!isColliding)
                return;
            bool isInFront = GetIsInFront();
            if ((isInFront && !wasInFront) || (wasInFront && !isInFront))
            {
                inOtherWorld = !inOtherWorld;
                SetMaterials(inOtherWorld);
            }
            wasInFront = isInFront;
        }

        void OnDestroy()
        {
            //ensure geometry renders in the editor
            SetMaterials(true);
        }


        void Update()
        {
            WhileCameraColliding();
        }
    }
}