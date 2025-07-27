using System.Collections.Generic;
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
    private InputAction attackAction;

    private Vector2 moveVector;

    public float moveSpeed = 3f;

    private bool isAttack;

    //private int logTimer=0;
    public float attackRange = 0.5f;
    public int attackDamage = 10;
    public LayerMask enemyLayers;

    public List<string> inventory = new List<string>();

    void Awake()
    {
        moveAction = inputActionsAsset.FindActionMap("Player").FindAction("Move");
        jumpAction = inputActionsAsset.FindActionMap("Player").FindAction("Jump");
        attackAction = inputActionsAsset.FindActionMap("Player").FindAction("Attack");
    }

    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
        attackAction.Enable();
    }
    void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
        attackAction.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        moveVector = moveAction.ReadValue<Vector2>();
        isAttack = attackAction.triggered;
        playerAnimator.SetBool("Attack", isAttack);
        if (isAttack)
        {
            Attack();
        }

        //playerRigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    void FixedUpdate()
    {
        playerRigidBody.linearVelocity = new Vector2(moveVector.x * moveSpeed, moveVector.y * moveSpeed);

        playerAnimator.SetFloat("Speed_X", Mathf.Abs(playerRigidBody.linearVelocity.x));
        playerAnimator.SetFloat("Speed_Y", playerRigidBody.linearVelocity.y);

        if (playerRigidBody.linearVelocity.magnitude < 0.0001f)
        {
            playerAnimator.SetBool("Moving", false);
        }
        else
        {
            playerAnimator.SetBool("Moving", true);
        }

        if (playerRigidBody.linearVelocity.x > 0)
        {
            playerSpriteRenderer.flipX = false;
        }
        else if (playerRigidBody.linearVelocity.x < 0)
        {
            playerSpriteRenderer.flipX = true;
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

    void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayers);
        if(hitEnemies!=null)
        {
            foreach (Collider2D enemy in hitEnemies)
            {
                if(enemy!=null)
                {
                    enemy.GetComponent<Enemy>().TakeDamage(attackDamage, this);
                }
            }
        }
    }

    public void AddInventory(string objectName)
    {
        inventory.Add(objectName);
        Debug.Log($"Added {objectName}");
    }
}
