using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float baseMoveSpeed = 6f;
    public float moveSpeed;
    Rigidbody2D rb;
    Vector2 input;

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
        // I switched GetAxisRaw to GetAxis (and removed "Snap" from Axis settings) to get the gradual start and stop to the ship movement
        // To make the delay in movement bigger or smaller you can play around with "Gravity" and "Sensitivity" variables in Axis settings in Edit > Project Settings > Input Manager > Axis > Vertical / Horizontal
        // Increasing gravity will make the ship stop faster
        // Increasing Sensitivity will make the ship accelerate faster

        input = new Vector2(
            Input.GetAxis("Horizontal"), 
            Input.GetAxis("Vertical")
        );

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
