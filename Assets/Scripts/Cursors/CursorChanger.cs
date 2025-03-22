using UnityEngine;

public class CursorChanger : MonoBehaviour
{
    [SerializeField] private Texture2D _highLightCursorTexture;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

     public void SetHighlightCursor()
    {
        Cursor.SetCursor(_highLightCursorTexture, Vector2.zero, CursorMode.Auto);
    }

    public void SetDefaultCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
