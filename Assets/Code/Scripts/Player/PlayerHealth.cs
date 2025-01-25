using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerHealth : MonoBehaviour 
{
    SimpleFlash flash;
    Shake shake;
    PlayerController pc;
    HealthSystem hs;
    
    public int healthPoint = 100;

    int lastAttackID;

    private void Start() 
    {
        hs = new HealthSystem(healthPoint);
        pc = GetComponent<PlayerController>();

        flash = GetComponent<SimpleFlash>();
        shake = GameObject.FindGameObjectWithTag("ScreenShake").GetComponent<Shake>();
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

        if(hs.health <= 0) StartCoroutine(Dead());
    }

    public IEnumerator Dead() 
    {
        pc.ableToMove = false;
        yield return new WaitForSeconds(1f);
    }
}