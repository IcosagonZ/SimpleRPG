using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D playerRigidBody;

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

    void Awake()
    {
        moveAction = inputActionsAsset.FindActionMap("Player").FindAction("Move");
        jumpAction = inputActionsAsset.FindActionMap("Player").FindAction("Jump");
    }

    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
    }

    void OnEnable() { moveAction.Enable(); }
    void OnDisable() { moveAction.Disable(); }

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
        playerRigidBody.linearVelocity = new Vector2(moveVector.x*moveSpeed, playerRigidBody.linearVelocity.y);
    }
}
