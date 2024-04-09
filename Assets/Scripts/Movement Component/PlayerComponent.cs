using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerComponent : MonoBehaviour
{
    [Header("Animation Setting:")]
    [SerializeField ,Tooltip("write the name of the jump animation")] 

    string animDash = "Dash";
    [SerializeField, Tooltip("write the name of the Walk animation")] 
    string animWalk = "Walk";    
    [SerializeField, Tooltip("write the name of the Idle animation")] 
    string animIdle = "Idle";

    string currentAnim = "";


    Animator anim;
    private PlayerInputs input = null;

    private Vector2 moveValue;

    [HideInInspector]
    public bool isGrounded = true;
    [HideInInspector]
    public bool canJump = true;


    Rigidbody rb; // player's rigid body

    [Header("set the speed:")]
    public float playerSpeed;

    [Header("Dash values:")]
    public float dashSpeed = 0.1f;
    public float dashDuration = 0.5f;

    public float dashCooldown = 1f;
    bool isDashing = false;
    bool canDash = true;

    private void Awake()
    {
        input = new PlayerInputs();
        rb = GetComponent<Rigidbody>();
        TryGetComponent(out anim);
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += OnHorizontal;
        input.Player.Movement.canceled += OnHorizontalCancelled;
        input.Player.Dash.started += OnDash;
    }
    private void OnDisable()
    {
        input.Player.Movement.performed -= OnHorizontal;
        input.Player.Movement.canceled -= OnHorizontalCancelled; 
        input.Player.Dash.started -= OnDash;
        input.Disable();
    }

    private void Update()
    {
        if (isDashing) return;

        if (input.Player.Jump.WasPressedThisFrame())
            canJump = true;
        else
            canJump = false;

        if(moveValue == Vector2.zero)
        {
            ChangeAnimation(animIdle);
        }
    }

    private void FixedUpdate()
    {
        if (isDashing) return;
        rb.velocity = playerSpeed * Time.fixedDeltaTime * moveValue.normalized;
    }

    private void OnHorizontal(InputAction.CallbackContext value)
    {
        moveValue = value.ReadValue<Vector2>();
        //if (moveValue.x > 0)
        //{
        //    Quaternion rot = transform.rotation;
        //    rot= Quaternion.AngleAxis(180,Vector3.up);
        //    transform.rotation = rot;
        //}
        //else if(moveValue.x < 0)
        //{
        //    Quaternion rot = transform.rotation;
        //    rot = Quaternion.AngleAxis(180, -Vector3.up);
        //    transform.rotation = rot;
        //}

        ChangeAnimation(animWalk);
    }

    private void OnHorizontalCancelled(InputAction.CallbackContext value)
    {
        moveValue = Vector2.zero;
    }

    
    //private void OnCollisionEnter(Collision collision)
    //{
    //}


    #region Dash functions:
    void OnDash(InputAction.CallbackContext value)
    {
        if (canDash == false || rb.velocity == Vector3.zero) return;

        Debug.Log(":1>");
        gameObject.layer = 10;

        ChangeAnimation(animDash);
        StartCoroutine(Dashing());
    }

    IEnumerator Dashing()
    {
        isDashing = true;
        canDash = false;
        rb.velocity = moveValue.normalized * dashSpeed;
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        gameObject.layer = 6;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    #endregion


    void ChangeAnimation(string animation)
    {
        if (!anim)
            return;

        if (currentAnim != animation)
        {
            currentAnim = animation;
            anim.CrossFade(animation,0.2f);
        }
    }
}
