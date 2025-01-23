using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NpcController : MonoBehaviour
{
    [HideInInspector] public Animator anim;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void FixedUpdate()
    {

    }

    void DebugModeInput()
    {
        
    }

    public IEnumerator Dead() 
    {
        yield return new WaitForSeconds(1f);
    }
}
