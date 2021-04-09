using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Events;

public class PlayerInputHandler : MonoBehaviour
{
    private float movementInput = 0;

    [SerializeField] private float jumpHeight = 2;

    private Rigidbody2D rb;

    [SerializeField] private int jumpCount = 1;

    [SerializeField] private float movementSpeed = 10;

    [SerializeField][Range(0,1)] private float midairMovementSpeed=1;

    public UnityEvent Shoot;

    public UnityEvent Flip;

    private bool facingRight;
    public bool FacingRight
    {
        get
        {
            return facingRight;
        }
        private set
        {
            if (facingRight != value)
            {
                Flip.Invoke();
                facingRight = value;
            }
        }
    }

    private void OnEnable()
    {
        //initialize local variables
        rb = GetComponent<Rigidbody2D>();
    }

    private PlayerControls input;

    public void SetupInput(InputDevice device)
    {
        InputDevice[] deviceArray = new InputDevice[1];
        deviceArray[0] = device;
        SetupInput(deviceArray);
    }

    public void SetupInput(InputDevice[] devices)
    {
        input = new PlayerControls();
        input.devices = new ReadOnlyArray<InputDevice>(devices);
        input.Gameplay.Jump.performed += ctx => Jump();
        input.Gameplay.Shoot.performed += ctx => Shoot.Invoke();
        input.Gameplay.Enable();
    }

    public void Jump()
    {
        if (jumpsSinceGrounded < jumpCount)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            jumpsSinceGrounded += 1;
        }
    }

    private int jumpsSinceGrounded = 0;

    public LayerMask resetJumpLayers;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (((1 << coll.gameObject.layer) & resetJumpLayers) != 0)
        {
            jumpsSinceGrounded = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (((1 << coll.gameObject.layer) & resetJumpLayers) != 0)
        {
            //leaving ground is considered jumping
            if ( jumpsSinceGrounded == 0 ) jumpsSinceGrounded += 1;
        }
    }

    private void FixedUpdate()
    {
        // reading input and setting horizontal velocity
        movementInput = input.Gameplay.Move.ReadValue<float>();

        if (movementInput > 0)
            FacingRight = true;
        else if (movementInput < 0)
            FacingRight = false;

        if (jumpsSinceGrounded == 0)
            rb.velocity = new Vector2(movementInput * movementSpeed, rb.velocity.y);
        else
            movementInput = Mathf.Lerp(rb.velocity.x/movementSpeed, movementInput, midairMovementSpeed);
            rb.velocity = new Vector2(movementInput * movementSpeed, rb.velocity.y);
    }

    private void Start()
    {
        if (input == null) //on debug mode
        {
            SetupInput(InputSystem.devices.ToArray());
            ActionCamera.instance.targets = new List<Transform> { transform };
        }
    }
}
