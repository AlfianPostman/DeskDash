using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class NpcController : MonoBehaviour
{
    int lastAttackID = -1;

    HealthSystem hs;
    [HideInInspector] public Animator anim;
    NavMeshAgent agent;
    SimpleFlash flash;
    Shake shake;
    Rigidbody rb;
    Collider col;

    public MeshRenderer bubble;
    private Transform playerTarget;
    ColliderController colController;

    bool isStunned = false;
    bool isCaptured = false;

    [SerializeField] int healthPoint;

    [SerializeField] TMP_Text text;

    //Navigation
    Vector3 destinedPoint;
    bool walkPointSet;
    [SerializeField] float range;
    [SerializeField] LayerMask groundLayer;

    void Start()
    {
        hs = new HealthSystem(healthPoint);
        anim = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        flash = GetComponent<SimpleFlash>();
        shake = GameObject.FindGameObjectWithTag("ScreenShake").GetComponent<Shake>();

        anim.SetBool("isMoving", true);
    }

    void Update()
    {
        Behaviour();
    }

    void Behaviour()
    {
        if (isStunned) 
        {
            walkPointSet = false;

            if (isCaptured)
            {
                transform.position = playerTarget.position;
            }

            anim.SetBool("isMoving", false);
        }
        else
        {
            anim.SetBool("isMoving", true);

            if(agent.velocity.magnitude < 0.15f || !walkPointSet)
            {
                SearchForDestination();
            }
            
            if(walkPointSet)
            {
                agent.SetDestination(destinedPoint);
            }
        }

        if(Vector3.Distance(transform.position, destinedPoint) < 2)
        {
            walkPointSet = false;
        }
    }

    void SearchForDestination()
    {
        float z = Random.Range(-range, range);
        float x = Random.Range(-range, range);

        destinedPoint = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

        if(Physics.Raycast(destinedPoint, Vector3.down, groundLayer)) walkPointSet = true;
    }

    public void TakeDamage(float amount, string damageSource, string attackName, int attackID, Vector3 launchDirection)
    {
        if (lastAttackID != attackID)
        {
            flash.Flash();
            lastAttackID = attackID;

            DamagedProcess(amount, damageSource, attackName, launchDirection);
        }
    }

    void DamagedProcess(float amount, string damageSource, string attackName, Vector3 launchDirection)
    {
        hs.Damage(amount);
        shake.CamShake();
        Debug.Log("This " + this.gameObject.name + " take " + amount + " damage. From " + damageSource + "'s " + attackName);

        if (isStunned)
        {
            rb.AddForce(launchDirection * 100f, ForceMode.Impulse);
        }

        if(hs.health <= 0 && !isStunned) StartCoroutine(DyingProcess());
    }

    public void Captured(Transform PlayerScript, Transform target)
    {
        colController = PlayerScript.GetComponent<ColliderController>();

        if (isStunned)
        {
            isCaptured = true;
            playerTarget = target;
        }
    }

    public void Throw(Vector3 direction)
    {
        Debug.Log("aaa");
        rb.AddForce(direction * 1000f, ForceMode.Impulse);
    }

    public IEnumerator DyingProcess() 
    {
        text.text = "Stunned!";
        isStunned = true;
        bubble.enabled = true;
        col.enabled = false;
        rb.useGravity = false;

        agent.ResetPath();
        destinedPoint = Vector3.zero;

        yield return new WaitForSecondsRealtime(10f);

        colController?.ObjectDropped();

        isCaptured = false;
        isStunned = false;
        bubble.enabled = false;
        col.enabled = true;
        rb.useGravity = true;

        // hs.Heal(healthPoint);
        text.text = "";
    }
}
