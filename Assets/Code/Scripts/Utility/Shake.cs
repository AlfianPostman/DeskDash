using UnityEngine;

public class Shake : MonoBehaviour {
    
    Animator anim;

    private void Start() 
    {
        anim = GetComponent<Animator>();   
    }

    private void Update() 
    {
        if(Input.GetKey(KeyCode.K))
        {
            anim.SetTrigger("Shake");
        }
    }

    public void CamShake()
    {
        Debug.Log("ASDASDASD");
        anim.SetTrigger("Shake");
    }
}