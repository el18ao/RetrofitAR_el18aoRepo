using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTouches : MonoBehaviour
{
    public RectTransform debugTouchTemplate;

    RectTransform m_mouseDebugTouch;
    readonly List<RectTransform> m_debugTouchList = new List<RectTransform>();

    // Start is called before the first frame update
    void Start()
    {
        //add touch input no.
        for (int i = 0; i < 5; i++)
        {
            var newDebugTouch = CloneDebugTouchTemplate();
            m_debugTouchList.Add(newDebugTouch);
        }
        m_mouseDebugTouch = CloneDebugTouchTemplate();

        debugTouchTemplate.gameObject.SetActive(false);//start off

        Input.simulateMouseWithTouches = false;//start hidden
    }

    RectTransform CloneDebugTouchTemplate()// functon to instantiate registered touches
    {
        var clonedTouch = Instantiate(debugTouchTemplate);
        clonedTouch.transform.SetParent(debugTouchTemplate.transform.parent);
        clonedTouch.transform.localPosition = Vector3.zero;
        clonedTouch.transform.localRotation = Quaternion.identity;
        clonedTouch.transform.localScale = Vector3.one;
        return clonedTouch.GetComponent<RectTransform>();
    }

    void Update()
    {

        if (Input.GetMouseButton(0))//self explanatory - for mouse input
        {
            SetDebugTouch(m_mouseDebugTouch, Input.mousePosition);
        }
        else
        {
            m_mouseDebugTouch.gameObject.SetActive(false);//reset back to disabled once touch is removed
        }

        for (int i = 0; i < Input.touchCount; i++)//for touch input
        {
            var touch = Input.GetTouch(i);//check methods to see how this works
            var debugTouch = m_debugTouchList[i];
            if (i < m_debugTouchList.Count)
            {
                SetDebugTouch(debugTouch, touch.position);//assigning/repositioning touch locations for each touch in list
            }
        }

        for (int i = Input.touchCount; i < m_debugTouchList.Count; i++)
        {
            m_debugTouchList[i].gameObject.SetActive(false);//reset back to disabled once touch is removed
        }

    }

    void SetDebugTouch(RectTransform rect, Vector2 position) //function to set image location if touch is in canvas
    {
        var touchPositionInCanvas = Vector2.zero;
        var inCanvas = RectTransformUtility.ScreenPointToLocalPointInRectangle(rect.parent as RectTransform, position, null, out touchPositionInCanvas);
        if (inCanvas)
        {
            rect.anchoredPosition = touchPositionInCanvas;
            rect.gameObject.SetActive(true);
        }
    }
}
