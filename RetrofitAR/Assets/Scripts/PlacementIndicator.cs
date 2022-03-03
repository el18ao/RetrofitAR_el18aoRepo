using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;

public class PlacementIndicator : MonoBehaviour
{
    ARRaycastManager m_RaycastManager;
    [SerializeField]
    GameObject m_visual;

    void Start()
    {
        // Ensure this visual is clickable - like a button
        m_RaycastManager = FindObjectOfType<ARRaycastManager>();
        // Get the assets (can't use GetComponent as the visual is just the child objects)
        m_visual = transform.GetChild(0).gameObject;
        // Hide visual
        m_visual.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // shoot raycasts from the centre of the screen and if AR surface plane is hit update visual
        List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
        m_RaycastManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), s_Hits, TrackableType.Planes);

        if (s_Hits.Count > 0)
        {
            m_visual.transform.position = s_Hits[0].pose.position;
            m_visual.transform.rotation = s_Hits[0].pose.rotation;

            if (m_visual)
                m_visual.gameObject.SetActive(true);
        }
    }
}
