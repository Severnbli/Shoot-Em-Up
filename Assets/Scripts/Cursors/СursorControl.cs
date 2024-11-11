using UnityEngine;

public class CursorControl : MonoBehaviour
{
    [SerializeField] private KeyCode _keyToManipulateCursor = KeyCode.Escape;
    [SerializeField] private bool _isLock = true;
    [SerializeField] private bool _isVisible = false;
    private bool _isUnlocked = false;

    void Start() {
        LockCursor();
    }

    void Update() {
        if (Input.GetKeyDown(_keyToManipulateCursor))
        {
            if (_isUnlocked) {
                LockCursor();
            } else {
                UnlockCursor();
            }

            _isUnlocked = !_isUnlocked;
        }
    }

    public void UnlockCursor() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LockCursor() {
        if (_isLock) {
            Cursor.lockState = CursorLockMode.Locked;
        } else {
            Cursor.lockState = CursorLockMode.None;
        }

        Cursor.visible = _isVisible;
    }
}