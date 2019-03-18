using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.XR.iOS
{
    public class CubeController : MonoBehaviour
    {
        public GameObject prefab;

        // Update is called once per frame
        private void Update()
        {
            if (Input.touchCount > 0 && GetComponent<CursorController>().GetPointIsValid())
            {
                var touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    Transform m_HitTransform = GetComponent<CursorController>().m_HitTransform;
                    Instantiate(prefab, m_HitTransform.position, m_HitTransform.rotation);
                }
            }
        }
    }
}