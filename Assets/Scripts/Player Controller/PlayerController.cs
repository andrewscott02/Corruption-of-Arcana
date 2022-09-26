using UnityEngine;

/// <summary>
/// Authored & Written by @mrobertscgd
/// </summary>
namespace NecroPanda.Player
{
    public class PlayerController : MonoBehaviour
    {
        public CharacterController controller;

        public float speed = 12f;
        public float gravity = -9.81f;
        public float jumpHeight = 3f;

        public Transform groundCheck;
        public float groundDistance = 0.4f;
        public LayerMask groundMask;
        public bool paused;

        Vector3 velocity;
        bool isGrounded;

        // Update is called once per frame
        void Update()
        {
            GetInput();   
        }

        void GetInput()
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            controller.Move(move * speed * Time.deltaTime);

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                speed = speed * 2f;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                speed = speed / 2f;
            }

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
    }
}