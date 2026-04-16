using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    Rigidbody2D rb;
    Vector2 input;

    [Header("Rotation")]
    [SerializeField] GameObject firepoint;
    [SerializeField] bool rotationEnabled;
    [SerializeField] float rotationIntensity;
    [SerializeField] float rotationSpeed;
    Vector3 firepointLeftPos;
    Vector3 firepointRightPos;
    Vector3 firepointDefaultPos;

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
        // Setting imaginary most left and most right points for firepoint
        firepointDefaultPos = firepoint.transform.localPosition;
        firepointLeftPos = firepointDefaultPos + new Vector3(-rotationIntensity, 0, 0);
        firepointRightPos = firepointDefaultPos + new Vector3(rotationIntensity, 0, 0);
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

        // Moving the firepoint to left or right point depending on the input
        if (Input.GetAxis("Horizontal") > 0) 
        {
            firepoint.transform.localPosition = Vector3.Lerp(firepointDefaultPos, firepointRightPos, Input.GetAxis("Horizontal"));
        }
        else 
        {
            firepoint.transform.localPosition = Vector3.Lerp(firepointDefaultPos, firepointLeftPos, Input.GetAxis("Horizontal"));
        }

        // Making the spaceship always be facing the firepoint
        transform.LookAt(firepoint.transform);
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
