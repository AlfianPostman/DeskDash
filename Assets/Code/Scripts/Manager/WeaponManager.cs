using UnityEngine;
using System.Collections;

public class WeaponManager : MonoBehaviour
{
    [Header("Weapon Detail")]
    public string weaponName;
    public float weaponDamage;
    private int attackID = 0;

    [Header("References")]
    [SerializeField]
    private GameObject weaponContainer;
    [ReadOnly, SerializeField]
    public GameObject weaponObj;
    Weapon weaponScript;

    [HideInInspector] public PlayerController pc;
    [HideInInspector] public Animator wpAnim;
    
    
    void Awake()
    {
        GetWeaponObject();
    }
    
    void Start()
    {
        pc = GetComponent<PlayerController>();
        // ac = GetComponent<AnimationController>();
        if(weaponObj != null) wpAnim = weaponObj.GetComponent<Animator>();    
    }

    void GetWeaponObject()
    {
        if(weaponContainer.transform.childCount > 0 && weaponContainer != null)
        {
            weaponObj = weaponContainer.transform.GetChild(0).gameObject;
            weaponScript = weaponObj.GetComponent<Weapon>();
            wpAnim = weaponObj.GetComponent<Animator>();
        }
        else
        {
            Debug.Log("No weapon attached");
        }
    }

    public virtual void Attack()
    {
        if(weaponObj != null) StartCoroutine(AttackProcess());
    }

    public void AttachWeapon(Transform obj)
    {
        if(weaponObj == null)
        {
            Debug.Log("Set "+ obj.name + " as weapon container child");
            obj.SetParent(weaponContainer.transform);

            GetWeaponObject();

            obj.localPosition = Vector3.zero;
            obj.localRotation = Quaternion.Euler(Vector3.zero);
        }
    }

    public void DetachWeapon()
    {
        Debug.Log("aa");
        weaponObj.transform.SetParent(null);
        weaponObj = null;
    }

    public virtual IEnumerator AttackProcess() 
    {
        int currentComboIndex = 0; // Tracks the current attack in the combo
        float comboResetTime = weaponScript.attackDuration[currentComboIndex] / 2; // Time to reset the combo if no input is received
        bool comboActive = true; // To manage the combo state

        pc.canAttack = false;

        while (comboActive)
        {
            // Change the hitbox damage based on combo index attack damage
            weaponScript.InitAttackDetail(weaponScript.attackDamage[currentComboIndex], pc.gameObject.name, "Attack"+(currentComboIndex+1), attackID);

            attackID++;


            // Play the animation for the current combo step
            wpAnim.SetTrigger("Attack" + (currentComboIndex + 1));
            pc.anim.SetBool("isAttacking", true);

            // Wait for the attack duration of the current step
            yield return new WaitForSeconds(comboResetTime + (comboResetTime/2));

            // Wait for the next input within the combo reset time
            float timer = 0f;
            float timeStopped = 0f;
            bool nextAttackTriggered = false;

  
            while (timer < comboResetTime)
            {
                if (pc.attackButton)
                {
                    timeStopped = timer;
                    nextAttackTriggered = true;
                    break;
                }

                timer += Time.deltaTime;
                yield return null;
            }

            if (nextAttackTriggered)
            {
                currentComboIndex++;

                // Loop back to the start of the combo if the index exceeds the combo length
                if (currentComboIndex >= 2)
                {
                    currentComboIndex = 0;
                    yield return new WaitForSeconds(comboResetTime);
                }
            }
            else
            {
                comboActive = false; // End the combo if no input is received in time
            }
        }

        pc.anim.SetBool("isAttacking", false);
        pc.canAttack = true;
    }
}
