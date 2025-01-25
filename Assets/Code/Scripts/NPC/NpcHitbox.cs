using UnityEngine;

public class NpcHitbox : MonoBehaviour 
{

    Collider col;

    [ReadOnly, SerializeField]
    string damageSource;
    
    [SerializeField]
    float attackDamage;

    private void Start() 
    {
        damageSource = transform.parent.name;
    }

    private void OnTriggerEnter(Collider col) 
    {
        if (col.CompareTag("Player"))
        {
            col.GetComponent<PlayerHealth>()?.TakeDamage(attackDamage, damageSource);
        }
    }  
}