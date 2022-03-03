using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARRaycastManager))]//this script comes with AR foundation package so is added as a component to the object that this script is on, see AR session Inspector window

public class TapToPlaceOnce : MonoBehaviour
{
    [SerializeField]// Makes our private variable visible in the inspector 
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]//Hover mouse over public field in inspectore to view this tip
    GameObject m_PlacedPrefab;// Private variable waiting for object in editor

    [SerializeField]
    GameObject placementMarker;

    public GameObject placedPrefab// Prefab instantiated on touch.
    {
        get { return m_PlacedPrefab; }
        set { m_PlacedPrefab = value; }//
    }
    
    public GameObject spawnedARObject { get; private set; }
    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    ARRaycastManager m_RaycastManager;

    UnityEvent placementUpdate;

    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();

        if (placementUpdate == null) 
            placementUpdate = new UnityEvent();

        //    placementUpdate.AddListener(DisableVisual);
    }

    /// <summary>
    /// Testing for any touches/raycasts/hits etc  
    /// </summary>
    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
    #if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            var mousePosition = Input.mousePosition;
            touchPosition = new Vector2(mousePosition.x, mousePosition.y);
            return true;
        }
    #else
            if (Input.touchCount > 0)
            {
                touchPosition = Input.GetTouch(0).position;
                return true;
            }
    #endif

        touchPosition = default;
        return false;
    }
    
    void Update()// Update is called once per frame
    {
        if (!TryGetTouchPosition(out Vector2 touchPosition))
            return;// do nothing if no touches 

        if (m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = s_Hits[0].pose;

            if (spawnedARObject == null)
            {
                spawnedARObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);
            }
            else
            {
                spawnedARObject.transform.position = hitPose.position;// Reposition
            }

            placementUpdate.Invoke();// Shouting!!
        }
    }
}
