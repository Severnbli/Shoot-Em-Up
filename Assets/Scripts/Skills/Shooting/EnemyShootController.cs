public class EnemyShootController : ShootController
{
    void Start()
    {
        StartCoroutine(RepeatUsing());   
    }
}
