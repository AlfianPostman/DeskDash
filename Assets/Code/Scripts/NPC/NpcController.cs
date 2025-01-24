using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class NpcController : MonoBehaviour
{
    int lastAttackID = -1;

    NavMeshAgent agent;
    HealthSystem hs;
    bool isStunned = false;
    bool isCaptured = false;

    SimpleFlash flash;
    Shake shake;

    [SerializeField] int healthPoint;

    [SerializeField] TMP_Text text;

    //Navigation
    Vector3 destinedPoint;
    bool walkPointSet;
    [SerializeField] float range;
    [SerializeField] LayerMask groundLayer;

    [HideInInspector] public Animator anim;

    public MeshRenderer bubble;

    private Transform playerTarget;

    void Start()
    {
        hs = new HealthSystem(healthPoint);
        anim = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();

        flash = GetComponent<SimpleFlash>();
        shake = GameObject.FindGameObjectWithTag("ScreenShake").GetComponent<Shake>();
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
        }
        else
        {
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

        if(hs.health <= 0) StartCoroutine(DyingProcess());
    }

    public void Captured(Transform target)
    {
        isCaptured = true;
        Debug.Log("Captured");
        playerTarget = target;
    }

    public IEnumerator DyingProcess() 
    {
        text.text = "Stunned!";
        isStunned = true;
        bubble.enabled = true;
       
        agent.ResetPath();
        destinedPoint = Vector3.zero;

        yield return new WaitForSecondsRealtime(10f);
        
        isCaptured = false;
        isStunned = false;

        hs.Heal(healthPoint);
        text.text = "";

        bubble.enabled = false;
    }
}
