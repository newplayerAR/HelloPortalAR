using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.XR.iOS
{
    public class CubeController : MonoBehaviour
    {
        public GameObject prefab;
        private bool useGravity = true;
        private bool isKinematic = false;

        public bool UseGravity
        {
            get
            {
                return useGravity;
            }

            set
            {
                useGravity = value;
            }
        }

        public bool IsKinematic
        {
            get
            {
                return isKinematic;
            }

            set
            {
                isKinematic = value;
            }
        }

        // Update is called once per frame
        //private void Update()
        //{
        //    //if (Input.touchCount > 0 && GetComponent<CursorController>().GetPointIsValid())
        //    if (Input.touchCount > 0 && (GetComponent<CursorController>().GetPointIsValid() || GetComponent<CursorController>().GameObjectHitted))
        //    {
        //        var touch = Input.GetTouch(0);
        //        if (touch.phase == TouchPhase.Began)
        //        {
        //            Transform m_HitTransform = GetComponent<CursorController>().m_HitTransform;

        //            // workaround attempt#0
        //            //m_HitTransform.position = new Vector3(m_HitTransform.position.x, m_HitTransform.position.y + prefab.transform.localScale.y / 2.0f, m_HitTransform.position.z);

        //            Instantiate(prefab, m_HitTransform.position, m_HitTransform.rotation);
        //        }
        //    }
        //}
        public void InstantiateCube()
        {
            if (GetComponent<CursorController>().GetPointIsValid() || GetComponent<CursorController>().GameObjectHitted)
            {
                Transform m_HitTransform = GetComponent<CursorController>().m_HitTransform;
                var obj = Instantiate(prefab, m_HitTransform.position, m_HitTransform.rotation);

                // Set rigidbody
                obj.GetComponent<Rigidbody>().useGravity = this.useGravity;
                obj.GetComponent<Rigidbody>().isKinematic = this.isKinematic;
            }
        }

        public void DestoryCube()
        {
            if (GetComponent<CursorController>().GetPointIsValid() || GetComponent<CursorController>().GameObjectHitted)
            {
                GetComponent<CursorController>().DeleteCurCube();
            }
        }
    }
}