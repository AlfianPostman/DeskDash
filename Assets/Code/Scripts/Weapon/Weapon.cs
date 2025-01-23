using UnityEngine;

public class Weapon : WeaponManager
{
    [Header("Weapon Detail")]
    public float[] attackDuration = { 1.3f, 1.3f, 1.3f };
    public float comboResetTime = 1f;
    [HideInInspector] public float[] attackDamage;

    [Header("Assign all hitbox here")]
    public Transform[] hitBoxTransforms;
    private AttackHitbox[] hitBoxes;


    void Start() 
    {
        attackDamage = new float[] { weaponDamage, weaponDamage + (weaponDamage * 0.5f) };

        InitiateHitBoxScript();
    }

    void InitiateHitBoxScript()
    {
        // Initialize the array for AttackHitbox references
        hitBoxes = new AttackHitbox[hitBoxTransforms.Length];

        // Populate the AttackHitbox array
        for (int i = 0; i < hitBoxTransforms.Length; i++)
        {
            if (hitBoxTransforms[i] != null) // Ensure the Transform is assigned
            {
                hitBoxes[i] = hitBoxTransforms[i].gameObject.AddComponent<AttackHitbox>();
            }
            else
            {
                Debug.LogWarning($"HitBox Transform at index {i} is not assigned.");
            }
        }
    }

    public void InitAttackDetail(float amount, string damageSource, string attackName, int attackID)
    {
        // Initialize the array for AttackHitbox references
        hitBoxes = new AttackHitbox[hitBoxTransforms.Length];

        // Populate the AttackHitbox array
        for (int i = 0; i < hitBoxTransforms.Length; i++)
        {
            if (hitBoxTransforms[i] != null) // Ensure the Transform is assigned
            {
                hitBoxes[i] = hitBoxTransforms[i].gameObject.GetComponent<AttackHitbox>();
                if (hitBoxes[i] != null) // Ensure the AttackHitbox component exists
                {
                    hitBoxes[i].GetDamageDetail(amount, damageSource, attackName, attackID);
                }
                else
                {
                    Debug.LogWarning($"No AttackHitbox found on {hitBoxTransforms[i].name}");
                }
            }
            else
            {
                Debug.LogWarning($"HitBox Transform at index {i} is not assigned.");
            }
        }
    }
}