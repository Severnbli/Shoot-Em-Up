
using System;
using System.Linq;
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

    public static float GetRightBoundPlayerShipPosX() {
        var playerShips = GameObject.FindGameObjectsWithTag("Player");
        if (playerShips.Length == 0) return -Mathf.Infinity;
        
        return playerShips.Max(ship => ship.transform.position.x);
    }

    public static float GetLeftBoundPlayerShipPosX() {
        var playerShips = GameObject.FindGameObjectsWithTag("Player");
        if (playerShips.Length == 0) return Mathf.Infinity;

        return playerShips.Min(ship => ship.transform.position.x);
    }

    [Serializable]
    public class InputKey
    {
        [SerializeField] private KeyCode _key;
        [SerializeField] private HandlerType _handlerType;

        public KeyCode GetKey() {
            return _key;
        } 

        public HandlerType GetHandlerType() {
            return _handlerType;
        }

        public bool IsEventTrigger() {
            switch(_handlerType) {
                case HandlerType.GET_KEY: {
                    return Input.GetKey(_key);
                }

                case HandlerType.GET_KEY_UP: {
                    return Input.GetKeyUp(_key);
                }

                case HandlerType.GET_KEY_DOWN: {
                    return Input.GetKeyDown(_key);
                }

                default: {
                    return false;
                }
            }
        }
    }

    public enum HandlerType {
        GET_KEY,
        GET_KEY_UP,
        GET_KEY_DOWN
    }

    public static void CopyAudioSourceProperties(AudioSource from, AudioSource to) {
        if (from != null && to != null) {
            to.clip = from.clip;
            to.volume = from.volume;
            to.outputAudioMixerGroup = from.outputAudioMixerGroup;
        }
    }
}
