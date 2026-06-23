using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using PinePie.SimpleJoystick;
[RequireComponent(typeof(Rigidbody2D))]



public class PlayerController2D : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float baseMoveSpeed = 6f;
    public float moveSpeed;
    Rigidbody2D rb;
    Vector2 input;
[Header("Mobile")]
public JoystickController joystick;
    [Header("Rotation")]
    [SerializeField] bool rotationEnabled;
    [SerializeField] float rotationAngle;
    [SerializeField] float rotationSpeed = 1f;
    float angleLeft;
    float angleRight;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        SetFirepointPositions();

        // setting the speed
        moveSpeed = baseMoveSpeed;
    }

    void SetFirepointPositions()
    {
        // Setting most left and most right angle values
        angleLeft = angleLeft + rotationAngle;
        angleRight = angleRight - rotationAngle;
    }

    void Update()
    {
        MoveInput();
        RotateShip();
    }

    void RotateShip()
    {
        // checking if rotation is enabled 
        if (rotationEnabled == false) return;

        // calculating rotation
        //float currentAngle = (input.x > 0) ? angleRight * input.x : angleLeft * input.x;
        float currentAngle = rotationAngle * -input.x;
        if (input.x == 0) currentAngle = 0;

        // rotating the spaceship
        Quaternion targetRotation = Quaternion.Euler(0, 0, currentAngle);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

   void MoveInput()
{
    float horizontal = Input.GetAxis("Horizontal");
    float vertical = Input.GetAxis("Vertical");

    if (joystick != null)
    {
        horizontal += joystick.InputDirection.x;
        vertical += joystick.InputDirection.y;
    }

    input = new Vector2(horizontal, vertical);
    input = Vector2.ClampMagnitude(input, 1f);
}

    void FixedUpdate()
    {
        Movement();
    }

    public void ModifySpeed(float speedCoof = 1f)
    {
        moveSpeed = baseMoveSpeed * speedCoof;
    }

    void Movement()
    {
        transform.Translate(input.x * moveSpeed, input.y * moveSpeed, 0f, Space.World);
    }
}
