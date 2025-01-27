using UnityEngine;
using System.Collections;

public class AttackHitbox : MonoBehaviour
{
    Collider col;

    [ReadOnly, SerializeField]
    string attackName;

    [ReadOnly, SerializeField]
    string damageSource;
    
    [ReadOnly, SerializeField]
    float weaponDamage;

    int attackID;

    Vector3 launchDir;

    public void GetDamageDetail(float value, string source, string name, int attackIdVal, Vector3 launchDirection)
    {
        weaponDamage = value;
        damageSource = source;
        attackName = name;
        attackID = attackIdVal;
        launchDir = launchDirection;
    }

    private void OnTriggerEnter(Collider col) {
        if (col.CompareTag("Enemy"))
        {
            col.GetComponent<NpcController>()?.TakeDamage(weaponDamage, damageSource, attackName, attackID, launchDir);
        }
    }    
}
