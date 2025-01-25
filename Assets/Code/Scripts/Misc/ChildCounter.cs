using UnityEngine;

public class ChildCounter : MonoBehaviour
{
    GameHandler gh;
    private int enemyCount = 0;

    private void Start() 
    {
        gh = GetComponentInParent<GameHandler>();
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object has the "Enemy" tag
        if (other.CompareTag("Enemy"))
        {
            enemyCount++;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the object leaving has the "Enemy" tag
        if (other.CompareTag("Enemy"))
        {
            enemyCount--;
        }
    }

    void Update()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, 5f); // Radius of 5
        int enemyCount = 0;

        foreach (Collider col in enemies)
        {
            if (col.CompareTag("Enemy"))
            {
                enemyCount++;
                gh.currentSoap = enemyCount;
            }
        }
    }
}
