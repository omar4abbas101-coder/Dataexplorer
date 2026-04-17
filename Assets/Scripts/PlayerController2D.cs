using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    Rigidbody2D rb;
    Vector2 input;

    [Header("Rotation")]
    [SerializeField] bool rotationEnabled;
    [SerializeField] float rotationAngle;
    float angleLeft;
    float angleRight;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        SetFirepointPositions();
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
        float currentAngle = (input.x > 0) ? angleRight * input.x : angleLeft * -input.x;

        // rotating the spaceship
        transform.eulerAngles = new Vector3(0, 0, currentAngle);
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

    void Movement()
    {
        rb.velocity = input * moveSpeed;
    }
}
