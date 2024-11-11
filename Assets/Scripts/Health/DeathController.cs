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

                LoadNext("VictoryScreen");

                break;
            }

            case Utils.ObjectTags.PLAYER: {
                deadObject.GetComponent<ShipAnimatorController>()?.SetAnimatorIsDestroyedValue(true);
                
                var playerShips = GameObject.FindGameObjectsWithTag("Player");

                // Флаг, который отслеживает, все ли объекты уничтожены
                bool allDestroyed = true;

                // Если объекты с тегом "Player" существуют
                if (playerShips.Length > 0) {
                    foreach (var playerShip in playerShips) {
                        // Проверяем состояние isDestroyed у каждого объекта
                        bool isDestroyed = playerShip.GetComponent<Animator>()?.GetBool("isDestroyed") ?? true;

                        // Если хотя бы один объект не уничтожен, устанавливаем флаг в false и выходим из цикла
                        if (!isDestroyed) {
                            allDestroyed = false;
                            break;
                        }
                    }
                }

                // Если нет объектов с тегом "Player" или все они уничтожены, вызываем LoadNext
                if (playerShips.Length == 0 || allDestroyed) {
                    LoadNext("DefeatScreen");
                }

                break;
            }

            default: {
                break;
            }
        }
    }

    private static void LoadNext(string sceneName) {
        var levelChanger = GameObject.FindGameObjectWithTag("LevelChanger")?.GetComponent<LevelChanger>();

        if (levelChanger) {
            PlayerPrefs.SetString("nextScene", sceneName);

            levelChanger.ChangeLevel();
        }
    }
}
