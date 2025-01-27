using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Character Setting")]
    [Space(10)]
    public float baseSpeed = 3f;
    public float runSpeed = 5f;
    public float turnSmoothTime;
    public float dashForce = 1000f;
    public float dashCooldown = 2f;
    float turnSmoothVelocity;
    float moveSpeed = 0f;
    bool canDash = true;
    bool isDashing = false;

    bool isFacingRight = true;
    
    // Hotkey
    bool dashButton;
    bool restartButton;
    [HideInInspector] public bool throwButton;
    [HideInInspector] public bool attackButton;
    [HideInInspector] public bool pickUpButton;

    [HideInInspector] public bool ableToMove = false;
    [HideInInspector] public bool canAttack = true;
    Vector2 input;

    [Space(20)]
    [Header("References")]
    [Space(10)]
    public Transform cam;

    [Space(20)]
    [Header("Debug Mode")]
    [Space(10)]
    [SerializeField]
    bool DebugMode = false;

    Rigidbody rb;
    CameraTarget ct;
    [HideInInspector] public Animator anim;

    // Manager Reference
    PlayerSpriteController ps;
    WeaponManager wm;
    public GameHandler gh;

    public Animator hand;
    public Transform handTarget;
    public Transform originalPosition;
    public bool startSceneTriggered = false;
    public bool startScene = true;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();

        rb = GetComponent<Rigidbody>();
        ps = GetComponent<PlayerSpriteController>();
        ct = GetComponent<CameraTarget>();
        wm = GetComponent<WeaponManager>();

        rb.useGravity = false;
    }

    void FixedUpdate()
    {
        MyInputs();

        if(startSceneTriggered)
        {
            StartCoroutine(StartSceneProccess());
        }

        if(!startScene)
        {
            Movement();
        }
        else
        {
        transform.position = handTarget.position;
        }
    }

    void MyInputs() 
    {
        if (DebugMode) DebugModeInput();

        if (ableToMove)
        {
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            input = Vector2.ClampMagnitude(input, 1);
            dashButton = Input.GetKey(KeyCode.LeftShift);
            attackButton = Input.GetKey(KeyCode.Mouse0);
            pickUpButton = Input.GetKey(KeyCode.E);
            throwButton = Input.GetKey(KeyCode.Q);
            restartButton = Input.GetKey(KeyCode.Escape);

            if(restartButton) SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            // anim.SetFloat("speed", input.magnitude);

            if (attackButton && canAttack) {
                wm.Attack();
            }
        }
    }

    void DebugModeInput()
    {
        if(Input.GetKey(KeyCode.G) && canAttack && wm.weaponObj != null) {
            wm.DetachWeapon();
        }
    }

    void Movement() 
    {
        moveSpeed = runSpeed;
        Vector3 direction = new Vector3(input.x, 0f, input.y).normalized;

        Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(transform.position, forward, Color.green);

        // Normalize the camera angle
        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward = camForward.normalized;
        camRight = camRight.normalized;

        // Controlling the rotation based on camera angle
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            
            if(canAttack) transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

        // Control Sprite
        if(isFacingRight && input.x < 0f || !isFacingRight && input.x > 0f)
        {
            isFacingRight = !isFacingRight;
            ps.Flip();
        }

        // Camera target offset
        if(input.x != 0f || input.y != 0f)
        {
            ct.Moving(true);
            anim.SetBool("isMoving", true);
        }
        else
        {
            ct.Moving(false);
            anim.SetBool("isMoving", false);
        }

        if (dashButton && canDash && !isDashing) 
        {
            StartCoroutine(Dash());
        }

        // Start Moving
        if (ableToMove)
        {
            Vector3 yVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            rb.linearVelocity = input.x * camRight * moveSpeed + input.y * camForward * moveSpeed + yVelocity;
        }
    }

    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        // Get the dash direction (forward or based on input)
        Vector3 dashDirection = transform.forward;

        // Apply the dash force
        rb.AddForce(dashDirection * dashForce, ForceMode.Impulse);

        // Wait for the dash duration
        yield return new WaitForSeconds(.2f);

        // Stop the dash (optional, depending on how you want to control the motion)
        rb.linearVelocity = Vector3.zero;

        isDashing = false;

        // Wait for the cooldown
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    public void StartGame()
    {
        startSceneTriggered = true;
    }

    IEnumerator StartSceneProccess()
    {
        hand.SetTrigger("Start");

        yield return new WaitForSeconds(3f);

        gh.isTimerRunning = true;
        rb.useGravity = true;
        startScene = false;
        startSceneTriggered = false;
    }
}
