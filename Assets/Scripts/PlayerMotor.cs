using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private const float LaneDistance = 2.5f;
    private const float TurnSpeed = 0.05f;
    
    //
    private bool isRunning = false;
    
    // Animation
    private Animator anim;
    
    // Movement
    private CharacterController controller;
    private float jumpForce = 6.0f; // ---------Add Jump force  4.0
    private float gravity = 12.0f;
    private float verticalVelocity;
    private int desiredLane = 1; // 0 = Left, 1 = middle, 2 = Right
    
    // Speed modifier
    private float originalSpeed = 7.0f;
    private float speed;
    private float speedIncreaseLastTick;
    private float speedIncreaseTime = 2.5f; // ----- to 7.0f
    private float speedIncreaseAmount = 0.1f;
    
    private void Start()
    {
        speed = originalSpeed;
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        // Tap to Start Game
        if(!isRunning)
            return;

        // Speed Increase
        if (Time.time - speedIncreaseLastTick > speedIncreaseTime)
        {
            speedIncreaseLastTick = Time.time;
            speed += speedIncreaseAmount;
            
            // Change Modifier Score Text
            GameManager.Instance.UpdateModifier(speed - originalSpeed);
        }
        
        // Gather the inputs on which lane we should be
        // Move Left
        if (MobileInput.Instance.SwipeLeft)
        {
            Debug.Log("Left---");
            MoveLane(false);
        }

        // Move Right
        if (MobileInput.Instance.SwipeRight)
        {
            Debug.Log("Right---");
            MoveLane(true);
        }
        
        // Calculate where we should be in the future
        Vector3 targetPos = transform.position.z * Vector3.forward;
        
        if (desiredLane == 0)
            targetPos += Vector3.left * LaneDistance;
        else if(desiredLane == 2)
            targetPos += Vector3.right * LaneDistance;
        
        // Calculate Move Delta
        Vector3 moveVector = Vector3.zero;
        moveVector.x = (targetPos - transform.position).normalized.x * speed;

        bool isGrounded = IsGrounded();
        anim.SetBool("Grounded", isGrounded);
        
        // Calculate Y
        if (IsGrounded())
        {
            verticalVelocity = 0.1f;

            // Jump
            if (MobileInput.Instance.SwipeUp)
            {
                anim.SetTrigger("Jump");
                verticalVelocity = jumpForce;
     //           Debug.Log("Jumping Up");
            }
            
            // Slide
            else if (MobileInput.Instance.SwipeDown)
            {
                StartSliding();
                Invoke("StopSliding", 1.0f); // Stop Sliding after 1 sec
            }
        }
        else
        {
            verticalVelocity -= (gravity * Time.deltaTime);
            
            // Fast Falling Mechanic
            if (MobileInput.Instance.SwipeDown)
            {
                verticalVelocity = -jumpForce;
                Debug.Log("Down | Sliding");
            }
        }
        
        moveVector.y = verticalVelocity;
        moveVector.z = speed;
        
        // Move the Player
        controller.Move(moveVector * Time.deltaTime);
        
        // Rotate player to where he is going
        Vector3 dir = controller.velocity;
        dir.y = 0;
        transform.forward = Vector3.Lerp(transform.forward,dir, TurnSpeed);
    }

    private void StartSliding()
    {
        anim.SetBool("Sliding", true);
        controller.height /= 2;
        controller.center = new Vector3(controller.center.x, controller.center.y / 2, controller.center.z);
    }
    
    private void StopSliding()
    {
        anim.SetBool("Sliding", false);
        controller.height *= 2;
        controller.center = new Vector3(controller.center.x, controller.center.y * 2, controller.center.z);
    }

    private void MoveLane(bool goingRight)
    {
        desiredLane += (goingRight) ? 1 : -1;
        desiredLane = Mathf.Clamp(desiredLane, 0, 2);
    }

    private bool IsGrounded()
    {
        Ray groundRay = new Ray(
            new Vector3(
                controller.bounds.center.x,
                controller.bounds.center.y - controller.bounds.extents.y + 0.2f,
                controller.bounds.center.z),
            Vector3.down);
        
        Debug.DrawRay(groundRay.origin, groundRay.direction, Color.cyan, 1.0f);

        return (Physics.Raycast(groundRay, 0.2f + 0.1f));
    }

    public void StartRunning()
    {
        isRunning = true;
        anim.SetTrigger("StartRunning");
    }

    private void Crash()
    {
        anim.SetTrigger("Death");
        isRunning = false;
        GameManager.Instance.GameOver();
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        switch (hit.gameObject.tag)
        {
            case "Obstacle":
                Crash();
                break;
        }
    }
}
