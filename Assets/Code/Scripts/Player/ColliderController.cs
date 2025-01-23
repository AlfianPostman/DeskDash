using UnityEngine;
using System.Collections;

public class ColliderController : MonoBehaviour
{
    Collider col;
    PlayerController pc;
    WeaponManager wm;
    
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
}
