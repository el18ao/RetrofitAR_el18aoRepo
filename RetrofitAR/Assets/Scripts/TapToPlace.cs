using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;//additional 
using UnityEngine.XR.ARFoundation;//additional

[RequireComponent(typeof(ARRaycastManager))]//this script comes with AR foundation package so is added as a component to the object that this script is on, see AR session Inspector window

public class TapToPlace : MonoBehaviour
{
    [SerializeField]//makes our private variable visible in the inspector 
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]//Hover mouse over public field in inspectore to view this tip
    GameObject m_PlacedPrefab;//private variable 
    public float waitTime = 5.0f;
    private float secondsPassed; 
    /// <summary>
    /// The prefab to instantiate on touch.
    /// </summary>
    public GameObject placedPrefab
    {
        get { return m_PlacedPrefab; }
        set { m_PlacedPrefab = value; }
    }

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    ARRaycastManager m_RaycastManager;


    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
    #if UNITY_EDITOR//for testing in the unity editor
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

    void Update()
    {
        secondsPassed += Time.deltaTime;//need to add using ... time
        
        if (!TryGetTouchPosition(out Vector2 touchPosition))
            return;

        if (m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon) && secondsPassed > waitTime)
        {
            // Raycast hits are sorted by distance, so the first one
            // will be the closest hit.
            var hitPose = s_Hits[0].pose;
            Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);
            // Instantiate clones the object which is why this script produces the m_PlacedPrefab on every singl touch
            // need to create a new object and update it for new touches rather than clone for new touches
        }
    }
}
