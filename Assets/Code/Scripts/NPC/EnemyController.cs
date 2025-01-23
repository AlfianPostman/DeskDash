using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    int lastAttackID = -1;

    HealthSystem hs;
    SimpleFlash flash;
    Shake shake;

    [SerializeField]
    float iFrameTime;
    [SerializeField]
    int healthPoint;

    void Start() 
    {
        hs = new HealthSystem(healthPoint);
        flash = GetComponent<SimpleFlash>();
        shake = GameObject.FindGameObjectWithTag("ScreenShake").GetComponent<Shake>();

        // Debug.Log("hp: " + hs.maxHealth + " or " + hs.GetHealthPercentage()*100 + "%");
    }

    public void TakeDamage(float amount, string damageSource, string attackName, int attackID)
    {
        if (lastAttackID != attackID)
        {
            flash.Flash();
            lastAttackID = attackID;
            StartCoroutine(DamagedProcess(amount, damageSource, attackName));
        }
    }

    IEnumerator DamagedProcess(float amount, string damageSource, string attackName)
    {
        hs.Damage(amount);
        shake.CamShake();
        Debug.Log("This " + this.gameObject.name + " take " + amount + " damage. From " + damageSource + "'s " + attackName);
        yield return new WaitForSeconds(.1f);

        if(hs.health <= 0) StartCoroutine(DyingProcess());

    }
    
    IEnumerator DyingProcess()
    {
        Debug.Log("Eouoeueggh mati aku wak ..");
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
}
