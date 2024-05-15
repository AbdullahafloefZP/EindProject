using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class CursorManager : MonoBehaviour
{
    private EventSystem eventSystem;
    public string excludeTag = "ExcludeFromCursorChange";

    void Awake() 
    {
        Cursor.visible = false;
        eventSystem = FindObjectOfType<EventSystem>();
    }

    void Update()
    {
        CheckCursorUIHover();
        UpdateCursor();
    }

    void UpdateCursor()
    {
        if (!IsPointerOverUI())
        {
            Vector2 mouseCursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mouseCursorPos;
        }
    }

    void CheckCursorUIHover()
    {
        Cursor.visible = IsPointerOverUI();
    }

    public bool IsPointerOverUI()
    {
        PointerEventData pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        eventSystem.RaycastAll(pointerEventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag(excludeTag))
            {
                continue;
            }
            return true;
        }
        return false;
    }
}