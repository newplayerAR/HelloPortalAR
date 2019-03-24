using System;
using System.Collections.Generic;

namespace UnityEngine.XR.iOS
{
    public class CursorController : MonoBehaviour
    {
        public GameObject cursor;
        public Transform m_HitTransform;
        //public float maxRayDistance = 200.0f;
        public LayerMask collisionLayer = 1 << 10;  //ARKitPlane layer
        public int frameLimit = 15;
        private int curFrame;
        private bool pointIsValid;
        private bool gameObjectHitted;

        //private bool test = false;
        //private Vector3 original = new Vector3(0.025f, 0.025f, 0.025f);
        //private Vector3 changed = new Vector3(0.05f, 0.05f, 0.05f);

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
                    //Vector3 currAngle = transform.eulerAngles;
                    //transform.LookAt(Camera.main.transform);
                    //transform.eulerAngles = new Vector3(currAngle.x, transform.eulerAngles.y, currAngle.z);
                    return true;
                }
            }
            return false;
        }

        bool HitTestWithGameObject()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));

            if (Physics.Raycast(ray, out hit, 35))
            {
                if (hit.transform.tag == "Cube")
                {
                    Debug.Log("Touched Cube!");
                    m_HitTransform.position = hit.point;
                    m_HitTransform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                    Debug.Log(string.Format("HitTest::: x:{0:0.######} y:{1:0.######} z:{2:0.######}", m_HitTransform.position.x, m_HitTransform.position.y, m_HitTransform.position.z));

                    return true;
                }
            }
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

            // check if gameObject is hit
            pointIsValid = HitTestWithGameObject();

            if (pointIsValid) {
                curFrame = 0;
                gameObjectHitted = true;
                return;
            }
            // Temporary workaround for the issue "Physics.Raycast sometimes misses"
            else if (gameObjectHitted)
            {
                if (curFrame > frameLimit)
                {
                    curFrame = 0;
                    gameObjectHitted = false;
                }
                else
                {
                    curFrame += 1;
                    return;
                }
            }

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
            curFrame = 0;
            pointIsValid = false;
            gameObjectHitted = false;
        }

        public bool GetPointIsValid()
        {
            return pointIsValid;
        }
    }
}
