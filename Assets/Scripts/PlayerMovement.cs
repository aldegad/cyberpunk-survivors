using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 10f;

    [SerializeField]
    private Transform cameraTransform;

    [SerializeField]
    private Animator animator;


    private CharacterController characterController;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // character movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 inputDirection = new Vector3(horizontalInput, 0, verticalInput);
        float inputMagnitude = Mathf.Clamp01(inputDirection.magnitude);
        
        Vector3 movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * inputDirection;
        movementDirection.Normalize();

        Vector3 velocity = movementDirection * inputMagnitude * movementSpeed;

        if (movementDirection != Vector3.zero)
        {
            animator.SetBool("IsMoving", true);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }
        
        characterController.Move(velocity * Time.deltaTime);

        // character rotate
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            Vector3 forwardDirection = raycastHit.point - transform.position;
            forwardDirection.y = 0;

            Quaternion toRotation = Quaternion.LookRotation(forwardDirection);
            transform.rotation = toRotation;
        }
        
        // charactor animation
        float velocityZ = Vector3.Dot(movementDirection, transform.forward);
        float velocityX = Vector3.Dot(movementDirection, transform.right);
        animator.SetFloat("Velocity Z", velocityZ, 0.05f, Time.deltaTime);
        animator.SetFloat("Velocity X", velocityX, 0.05f, Time.deltaTime);
    }

    public Quaternion GetRotation()
    {
        return transform.rotation;
    }
}
