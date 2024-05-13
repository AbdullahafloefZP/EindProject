using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class CursorManager : MonoBehaviour
{
    private EventSystem eventSystem;

    void Awake() 
    {
        Cursor.visible = false;
        eventSystem = FindObjectOfType<EventSystem>();
    }

    void Update()
    {
        CheckCursorUIHover();
        Vector2 mouseCursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mouseCursorPos;
    }

    void CheckCursorUIHover()
    {
        if (IsPointerOverUI())
        {
            Cursor.visible = true;
        }
        else
        {
            Cursor.visible = false;
        }
    }

    bool IsPointerOverUI()
    {
        PointerEventData pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        eventSystem.RaycastAll(pointerEventData, results);

        return results.Count > 0;
    }
}
