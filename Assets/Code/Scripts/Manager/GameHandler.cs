using UnityEngine;

public class GameHandler : MonoBehaviour
{
    HealthSystem hs;

    void Start()
    {
        hs = new HealthSystem(69);

        Debug.Log("hp:" + hs.maxHealth);
    }
}
