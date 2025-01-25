using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerHealth : MonoBehaviour 
{
    SimpleFlash flash;
    Shake shake;
    PlayerController pc;
    HealthSystem hs;
    public Slider slider;
    public GameHandler gh;

    public int healthPoint = 100;

    private void Start() 
    {
        hs = new HealthSystem(healthPoint);
        pc = GetComponent<PlayerController>();

        flash = GetComponent<SimpleFlash>();
        shake = GameObject.FindGameObjectWithTag("ScreenShake").GetComponent<Shake>();
    }

    private void Update() 
    {
        slider.value = hs.health;
    }

    public void TakeDamage(float amount, string damageSource)
    {
        flash.Flash();
        StartCoroutine(DamagedProcess(amount, damageSource));
    }

    IEnumerator DamagedProcess(float amount, string damageSource)
    {
        hs.Damage(amount);
        shake.CamShake();
        Debug.Log("This " + this.gameObject.name + " take " + amount + " damage. From " + damageSource);
        yield return new WaitForSeconds(.2f);

        if(hs.health <= 0) StartCoroutine(Dead());
    }

    public IEnumerator Dead() 
    {
        pc.ableToMove = false;
        yield return new WaitForSeconds(1f);
        Debug.Log("am dedddd");

        gh.Losing();
    }
}