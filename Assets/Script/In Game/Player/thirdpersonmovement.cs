using UnityEngine;

public class thirdpersonmovement : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    private float speed = 6f;
    private float gravity = -9.81f;
    private Vector3 velocity;
    private float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    [SerializeField] Transform cam;
    [SerializeField] Transform groundcheck;
    private float groundDistance = 0.4f;
    private float jumpHeight = 3f;
    [SerializeField] LayerMask groundmask;
    bool isGrounded;

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundcheck.position, groundDistance, groundmask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        if(direction.magnitude >= 0.1f)
        {
            float targatAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targatAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targatAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed *Time.deltaTime);
        }
        if (Input.GetButtonDown("Jump")&& isGrounded) 
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
