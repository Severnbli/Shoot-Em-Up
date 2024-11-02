using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathController : MonoBehaviour
{
    public static void Death(GameObject deadObject) {
        
        Utils.ObjectTags objectTag = Utils.GetTag(deadObject.tag);

        switch (objectTag) {
            case Utils.ObjectTags.ENEMY: {
                deadObject.GetComponent<ExplosionController>()?.StartExplosion();

                // Анимация взрыва у тени
                GameObject.FindGameObjectWithTag("EnemyShadow")?.GetComponent<ExplosionController>()?.StartExplosion();

                SceneManager.LoadScene("RepeatScreen");

                break;
            }

            case Utils.ObjectTags.ENEMY_SHADOW: {
                deadObject.GetComponent<ExplosionController>()?.StartExplosion();

                break;
            }

            case Utils.ObjectTags.PLAYER: {
                deadObject.GetComponent<ExplosionController>()?.StartExplosion();

                if (GameObject.FindGameObjectsWithTag("Player").Length == 0) {
                    SceneManager.LoadScene("RepeatScreen");
                }
                
                break;
            }

            default: {
                break;
            }
        }
    }
}
