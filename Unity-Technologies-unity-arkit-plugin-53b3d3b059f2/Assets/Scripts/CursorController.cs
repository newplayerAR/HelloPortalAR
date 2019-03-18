using System;
using System.Collections.Generic;

namespace UnityEngine.XR.iOS
{
    public class CursorController : MonoBehaviour
    {
        public GameObject cursor;
        public Transform m_HitTransform;
        private bool pointIsValid = false;
        //public float maxRayDistance = 200.0f;
        public LayerMask collisionLayer = 1 << 10;  //ARKitPlane layer

        bool HitTestWithResultType(ARPoint point, ARHitTestResultType resultTypes)
        {
            List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest(point, resultTypes);
            if (hitResults.Count > 0)
            {
                Debug.Log("Got hit!");
                foreach (var hitResult in hitResults)
                {
                    m_HitTransform.position = UnityARMatrixOps.GetPosition(hitResult.worldTransform);
                    m_HitTransform.rotation = UnityARMatrixOps.GetRotation(hitResult.worldTransform);
                    Debug.Log(string.Format("HitTest::: x:{0:0.######} y:{1:0.######} z:{2:0.######}", m_HitTransform.position.x, m_HitTransform.position.y, m_HitTransform.position.z));
                    // Make the portal to always be facing camera on spawn
                    Vector3 currAngle = transform.eulerAngles;
                    transform.LookAt(Camera.main.transform);
                    transform.eulerAngles = new Vector3(currAngle.x, transform.eulerAngles.y, currAngle.z);
                    //pointIsValid = true;
                    return true;
                }
            }
            //pointIsValid = false;
            return false;
        }

        void CheckCursorPosition()
        {
            var screenPosition = Camera.main.ScreenToViewportPoint(new Vector2(Screen.width / 2, Screen.height / 2));
            ARPoint point = new ARPoint
            {
                x = screenPosition.x,
                y = screenPosition.y
            };

            // check if it's hitting any GameObject

            // prioritize reults types
            ARHitTestResultType[] resultTypes = {
                        //ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingGeometry,
                        ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent, 
                        // if you want to use infinite planes use this:
                        //ARHitTestResultType.ARHitTestResultTypeExistingPlane,
                        //ARHitTestResultType.ARHitTestResultTypeEstimatedHorizontalPlane, 
                        //ARHitTestResultType.ARHitTestResultTypeEstimatedVerticalPlane, 
                        //ARHitTestResultType.ARHitTestResultTypeFeaturePoint
                    };

            foreach (ARHitTestResultType resultType in resultTypes)
            {
                pointIsValid = HitTestWithResultType(point, resultType);
                if (pointIsValid)
                {
                    return;
                }
            }
        }

        void SetCursor()
        {
            if(pointIsValid)
            {
                cursor.SetActive(true);
                Debug.Log(string.Format("SetCursor::: x:{0:0.######} y:{1:0.######} z:{2:0.######}", m_HitTransform.position.x, m_HitTransform.position.y, m_HitTransform.position.z));
                cursor.transform.SetPositionAndRotation(m_HitTransform.position, m_HitTransform.rotation);
            }
            else
            {
                cursor.SetActive(false);
            }
        }

        // Update is called once per frame
        void Update()
        {
            CheckCursorPosition();
            SetCursor();
        }

        private void Start()
        {
            m_HitTransform = cursor.transform;
        }

        public bool GetPointIsValid()
        {
            return pointIsValid;
        }
    }
}
