using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class playerController : MonoBehaviour
{
    public Transform cam;

    public float sensitivity = 5f;

    public float maxSpeed = 4f;
    public float maxRunSpeed = 10f;

    public float maxAcceleration = 35f;
    public float maxAirAcceleration = 20f;

    public float jumpPower = 10f;
    public bool grounded;
    public bool running;

    Rigidbody rb;

    Vector3 direction;
    Vector3 desiredVelocity;
    Vector3 velocity;

    float cameraX;
    float cameraY;

    float maxSpeedChange = 35f;
    float acceleration;
    float friction;
    float speed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        checkGrounded(collision);
        getFriction(collision);
    }
    private void OnCollisionStay(Collision collision)
    {
        checkGrounded(collision);
        getFriction(collision);
    }
    private void OnCollisionExit(Collision collision)
    {
        friction = 0;
        grounded = false;
    }

    void getFriction(Collision col)
    {
        friction = col.collider.sharedMaterial.dynamicFriction;
    }

    void checkGrounded(Collision col)
    {
        for (int i = 0; i < col.contactCount; i++)
        {
            float normal = col.GetContact(i).normal.y;
            grounded |= normal >= 0.9f;
        }
    }

    private void Update()
    {
        speed = running ? maxRunSpeed : maxSpeed;
        direction = transform.forward * Input.GetAxisRaw("Vertical") + transform.right * Input.GetAxisRaw("Horizontal");
        desiredVelocity = direction * Mathf.Max(speed - friction);

        cameraY = Input.GetAxisRaw("Mouse X") * sensitivity;
        cameraX += Input.GetAxisRaw("Mouse Y") * sensitivity;
        cameraX = Mathf.Clamp(cameraX, -90f, 90f);

        cam.localEulerAngles = new Vector3(-cameraX, 0f, 0f);
        transform.Rotate(Vector3.up * cameraY);

        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
        running = Input.GetKey(KeyCode.LeftShift) && grounded;
    }
    private void FixedUpdate()
    {
        velocity = rb.velocity;
        acceleration = grounded ? maxAcceleration : maxAirAcceleration;
        maxSpeedChange = acceleration * Time.deltaTime;

        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);

        rb.velocity = velocity;
    }
}
