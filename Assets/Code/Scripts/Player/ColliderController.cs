using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColliderController : MonoBehaviour
{
    Collider col;
    PlayerController pc;
    WeaponManager wm;
    public Transform carryTarget;
    private Transform heldObject;

    bool isCarrying = false;
    
    public string pickupTag = "NPCInteract"; // Tag for objects that can be picked up
    private List<GameObject> nearbyObjects = new List<GameObject>(); // List of nearby objects

    private void Start() 
    {
        pc = GetComponent<PlayerController>();
        wm = GetComponent<WeaponManager>();
    }

    private void OnTriggerEnter(Collider col) 
    {
        if (col.CompareTag("Weapon"))
        {
            Transform _col = col.transform;
            wm.AttachWeapon(_col);
        }

        if (col.CompareTag("NPCInteract"))
        {
            nearbyObjects.Add(col.gameObject);
        }

    }

    private void OnTriggerStay(Collider col) 
    {
        
        if (col.CompareTag("Bucket"))
        {
            if (pc.pickUpButton && isCarrying)
            {
                Debug.Log(heldObject);
                Transform _pos = col.GetComponentInParent<Bucket>().containerTarget;
                
                heldObject.GetComponentInParent<NpcController>()?.SuccessfullyCaptured(_pos);
            }
        }
    }

    void StoreObjectInBucket()
    {

    }

    private void OnTriggerExit(Collider col) 
    {
        if (col.CompareTag("NPCInteract"))
        {
            nearbyObjects.Remove(col.gameObject);
        }
    }

    void Update()
    {
        if (pc.pickUpButton && !isCarrying)
        {
            PickupClosestObject();
        }

        if (pc.throwButton && heldObject != null)
        {
            heldObject.GetComponentInParent<NpcController>()?.Throw(pc.transform.forward);
        }
    }

    void PickupClosestObject()
    {
        if (nearbyObjects.Count == 0) return;

        GameObject closestObject = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject obj in nearbyObjects)
        {
            if (obj == null) continue; // Skip null references (in case an object is destroyed)

            float distance = Vector3.Distance(transform.position, obj.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestObject = obj;
            }
        }

        // Call the pickup function on the closest object
        if (closestObject != null)
        {
            isCarrying = true;
            heldObject = closestObject.transform.parent;

            closestObject.GetComponentInParent<NpcController>()?.Captured(this.transform, carryTarget);
            nearbyObjects.Remove(closestObject);
        }
    }

    public void ObjectDropped()
    {
        isCarrying = false;
        heldObject = null;
    }
}
