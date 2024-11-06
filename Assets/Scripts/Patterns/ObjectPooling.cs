using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    [SerializeField] private GameObject[] _poolingReplicas;
    
    private static Dictionary<string, List<GameObject>> _replicas = new Dictionary<string, List<GameObject>>();
    private static Dictionary<string, Queue<GameObject> > _poolingObjects = new Dictionary<string, Queue<GameObject>>();
    
    protected void Awake() {
        foreach (var replica in _poolingReplicas) {
            if (replica.tag == "Untagged") {
                Debug.LogError("Object Pooling: some of the replica objects doesn't have tags!");
            } else {
                if (_replicas.ContainsKey(replica.tag)) {
                    Debug.LogWarning($"Object Pooling: find same tagged objects! Tag: {replica.tag}.");
                }

                if (!_replicas.ContainsKey(replica.tag)) {
                    _replicas[replica.tag] = new List<GameObject>();
                }

                _replicas[replica.tag].Add(replica);
            }
        }
    }

    public static void PushObject(GameObject pushObject) {
        if (pushObject == null || pushObject.tag == "Untagged") {
            Debug.LogWarning("Object Pooling: pushing object is null or has no tag!");
            return;
        }

        pushObject.SetActive(false);

        if (!_poolingObjects.ContainsKey(pushObject.tag)) {
            _poolingObjects[pushObject.tag] = new Queue<GameObject>();
        }

        _poolingObjects[pushObject.tag].Enqueue(pushObject);
    }

    public static GameObject PopObject(string tag, Vector3 position) {
        if (!_replicas.ContainsKey(tag)) {
            if (_poolingObjects.ContainsKey(tag) && _poolingObjects[tag].Count > 0) {
                Debug.LogWarning("Object Pooling: dangerous system object pooling usage: Object Pooling script must be \"familar\" " +
                                 "with objects wich contains!");
            } else {
                Debug.LogError("Object Pooling: method PopObject: it is impossible to obtain the required object!");
                return null;
            } 
        } 

        GameObject popingObject;

        if (_poolingObjects.ContainsKey(tag) && _poolingObjects[tag].Count > 0) {
            popingObject = _poolingObjects[tag].Dequeue();

            if (_replicas.ContainsKey(tag)) {
                popingObject.transform.localScale = _replicas[tag][0].transform.localScale;   
            }

            popingObject.transform.position = position;
        } else {
            popingObject = Instantiate(_replicas[tag][Random.Range(0, _replicas[tag].Count)], position, Quaternion.identity);
        }

        popingObject.SetActive(true);
        return popingObject;
    }
}
