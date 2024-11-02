
using System;
using UnityEngine;

public class Utils {
    public static void horizontalSmoothlyMove(float speed, float target, Transform tr) {
        float distance = target - tr.position.x;

        if (distance != 0) {
            float step = Mathf.Clamp(speed, 0f, Math.Abs(distance)); // 0f - нижний предел,  Math.Abs(distance) - верхний предел

            if (distance < 0) {
                step = -step;
            }

            tr.position = new Vector3(tr.position.x + step * Time.deltaTime, tr.position.y, tr.position.z);
        }
    }

    public static GameObject InstantiateEmptyObject(string name, Vector3 position, Quaternion rotation)
    {
        GameObject emptyObject = new GameObject(name);
        emptyObject.transform.position = position;
        emptyObject.transform.rotation = rotation;

        return emptyObject;
    }

    public static GameObject InstantiateEmptyObectWithSpriteRenderer(string name, Vector3 position, Quaternion rotation) {
        GameObject emptyObject = new GameObject(name);
        emptyObject.transform.position = position;
        emptyObject.transform.rotation = rotation;

        emptyObject.AddComponent<SpriteRenderer>();

        return emptyObject;
    }

    public enum ObjectTags {
        ENEMY,
        ENEMY_SHADOW,
        PLAYER,
        UNKNOW
    }

    public static ObjectTags GetTag(string tag) {
        switch(tag) {
            case "Enemy": {
                return ObjectTags.ENEMY;
            }

            case "EnemyShadow": {
                return ObjectTags.ENEMY_SHADOW;
            }

            case "Player": {
                return ObjectTags.PLAYER;
            }

            default: {
                return ObjectTags.UNKNOW;
            }
        }
    }
}
