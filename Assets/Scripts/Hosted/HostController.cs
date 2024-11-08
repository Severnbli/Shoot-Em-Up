using System.Collections.Generic;
using UnityEngine;

public class HostController : MonoBehaviour
{
    [SerializeField] protected List<string> _objectsThatSubordinateTags;
    protected List<GameObject> _subordinateObjects = new List<GameObject>();

    protected virtual void Start() {}

    protected virtual void ConnectToSubordinate() {
        foreach (var tag in _objectsThatSubordinateTags) {
            _subordinateObjects.AddRange(GameObject.FindGameObjectsWithTag(tag));
        }
    }
}
