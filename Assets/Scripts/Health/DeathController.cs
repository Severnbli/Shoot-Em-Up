using System.Collections.Generic;
using UnityEngine;

public class DeathController : MonoBehaviour
{
    public static List<GameObject> _objectsOnDestroy = new List<GameObject>();

    public static void Death(GameObject deadObject) {

        if (_objectsOnDestroy.Contains(deadObject)) {
            return;
        } else {
            _objectsOnDestroy.Add(deadObject);
        }
        
        Utils.ObjectTags objectTag = Utils.GetTag(deadObject.tag);

        switch (objectTag) {
            case Utils.ObjectTags.ENEMY: {
                deadObject.GetComponent<ShipAnimatorController>()?.SetAnimatorIsDestroyedValue(true);

                break;
            }

            case Utils.ObjectTags.PLAYER: {
                deadObject.GetComponent<ShipAnimatorController>()?.SetAnimatorIsDestroyedValue(true);
                
                break;
            }

            default: {
                break;
            }
        }
    }
}
