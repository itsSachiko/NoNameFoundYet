using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerComponent : MonoBehaviour
{
    [Header("Animation Setting:")]
    [SerializeField, Tooltip("write the name of the jump animation")]

    string animDash = "DASH";
    [SerializeField, Tooltip("write the name of the Walk animation")]
    string animWalk = "WALK";
    [SerializeField, Tooltip("write the name of the Idle animation")]
    string animIdle = "IDLE";
    [SerializeField, Tooltip("write the name of the Shooting animation")]
    string animRanged = "RANGED";
    [SerializeField, Tooltip("write the name of the Melee animation")]
    string animMelee = "MELEE";
    [SerializeField, Tooltip("write the name of the Melee animation")]
    string animDeath = "DEATH";

    string currentAnim = "";



    [SerializeField] float camPosY = 18;
    [SerializeField] Animator anim;
    [SerializeField] SpriteRenderer spriteRenderer;
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

    public static Action onShoot;
    public static Action onSwing;
    public static Action onAnimEnd;

    private void Awake()
    {
        input = new PlayerInputs();
        rb = GetComponent<Rigidbody>();

    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += OnHorizontal;
        input.Player.Movement.canceled += OnHorizontalCancelled;
        input.Player.Dash.started += OnDash;
        onShoot += ShootAnim;
        onSwing += SwingAnim;
        onAnimEnd += OnAnimEnd;
    }

    private void OnDisable()
    {
        input.Player.Movement.performed -= OnHorizontal;
        input.Player.Movement.canceled -= OnHorizontalCancelled;
        input.Player.Dash.started -= OnDash;
        input.Disable();

        onShoot -= ShootAnim;
        onSwing -= SwingAnim;
        onAnimEnd -= OnAnimEnd;
    }

    private void ShootAnim()
    {
        CheckMousePos();
        ChangeAnimation(animRanged);
    }

    private void SwingAnim()
    {
        CheckMousePos();
        ChangeAnimation(animMelee);
    }

    void CheckMousePos()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.y += camPosY;

        if (transform.position.x < pos.x)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }

    void OnAnimEnd()
    {
        Debug.Log(currentAnim);
        ChangeAnimation(animIdle);
    }

    private void Update()
    {
        if (isDashing) return;

        if (input.Player.Jump.WasPressedThisFrame())
            canJump = true;
        else
            canJump = false;

        if (currentAnim != animWalk && currentAnim != animIdle)
        {
            CheckMousePos();
            return;
        }

        if (moveValue == Vector2.zero)
        {
            ChangeAnimation(animIdle);
        }
        else
        {
            if (moveValue.x > 0)
            {
                spriteRenderer.flipX = false;
            }
            else if (moveValue.x < 0)
            {
                spriteRenderer.flipX = true;
            }
            ChangeAnimation(animWalk);
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
        if (moveValue.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveValue.x < 0)
        {
            spriteRenderer.flipX = true;
        }

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

        if (currentAnim != animation /*|| animation == animIdle*/)
        {
            anim.CrossFade(animation, 0f);
            currentAnim = animation;
        }
    }
}
