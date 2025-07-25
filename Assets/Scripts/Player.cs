using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D playerRigidBody;

    public Animator playerAnimator;
    public SpriteRenderer playerSpriteRenderer;
    public InputActionAsset inputActionsAsset;

    private InputAction moveAction;
    private InputAction jumpAction;

    private Vector2 moveVector;

    public float moveSpeed = 3f;
    public float jumpForce = 5f;

    public Transform groundTransform;
    public float groundRadius = 0.2f;
    public LayerMask groundLayer;

    private bool isGrounded;

    //private int logTimer=0;

    void Awake()
    {
        moveAction = inputActionsAsset.FindActionMap("Player").FindAction("Move");
        jumpAction = inputActionsAsset.FindActionMap("Player").FindAction("Jump");
    }

    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
    }
    void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        moveVector = moveAction.ReadValue<Vector2>();

        isGrounded = Physics2D.OverlapCircle(groundTransform.position, groundRadius, groundLayer);

        if(jumpAction.triggered && isGrounded)
        {
            playerRigidBody.AddForce(Vector2.up*jumpForce, ForceMode2D.Impulse);
        }
        //playerRigidBody.AddForce(moveVector, ForceMode2D.Impulse);
    }

    void FixedUpdate()
    {
        playerRigidBody.linearVelocity = new Vector2(moveVector.x*moveSpeed, moveVector.y*moveSpeed);
        playerAnimator.SetFloat("Speed_X", Mathf.Abs(playerRigidBody.linearVelocity.x));
        playerAnimator.SetFloat("Speed_Y", playerRigidBody.linearVelocity.y);

        if(playerRigidBody.linearVelocity.magnitude<0.0001f)
        {
            playerAnimator.SetBool("Moving", false);
        }
        else
        {
            playerAnimator.SetBool("Moving", true);
        }

        if(playerRigidBody.linearVelocity.x>0)
        {
            playerSpriteRenderer.flipX=false;
        }
        else if(playerRigidBody.linearVelocity.x<0)
        {
            playerSpriteRenderer.flipX=true;
        }
        /*
        if(logTimer>60)
        {
            Debug.Log($"Moving:{playerAnimator.GetBool("Moving")}");
            Debug.Log($"Speed X:{playerAnimator.GetFloat("Speed_X")}");
            Debug.Log($"Speed Y:{playerAnimator.GetFloat("Speed_Y")}");
            logTimer = 0;
        }
        else
        {
            logTimer++;
        }
        */
    }
}
