using UnityEngine;
using System.Collections;

public class ColliderController : MonoBehaviour
{
    Collider col;
    PlayerController pc;
    WeaponManager wm;
    public Transform carryTarget;
    
    private void Start() {
        pc = GetComponent<PlayerController>();
        wm = GetComponent<WeaponManager>();
    }

    private void OnTriggerEnter(Collider col) {
        if (col.CompareTag("Weapon"))
        {
            Transform _col = col.transform;
            wm.AttachWeapon(_col);
        }
    }    

    private void OnTriggerStay(Collider col) {
        if (col.CompareTag("NPCInteract"))
        {
            if(pc.pickUpButton)
            {
                Debug.Log("am taking");
                col.GetComponentInParent<NpcController>()?.Captured(carryTarget);
            }
        }
    }    
}
