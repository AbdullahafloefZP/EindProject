using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    
    [SerializeField] private Texture2D cursonTexture;

    private Vector2 cursorHotspot;

    void Start()
    {
        cursorHotspot = new Vector2(cursonTexture.width / 2, cursonTexture.height / 2);
        Cursor.SetCursor(cursonTexture, cursorHotspot, CursorMode.Auto);
    }
}
