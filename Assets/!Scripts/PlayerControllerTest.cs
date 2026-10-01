using UnityEngine;                                   // Access to Unity classes

public class PlayerControllerTest: MonoBehaviour        // Script attached to the Arrow root object
{
    // NEW: Inspector heading
    [Header("Movement")]                             // Groups the next fields under a heading
    // END NEW
    public float moveSpeed = 5f;                     // Arrow slide speed in units per second
    public float xRange = 3f;                        // Left/right limit for the arrow (the constraint)

    // NEW: Inspector heading
    [Header("Throwing")]
    // END NEW
    public float throwForce = 15f;                   // Strength of the throw impulse
    public Transform arrowVisual;                    // The swinging object; its green (up) axis is the throw direction
    public Animator animator;                        // The Animator that plays Idle and Aiming

    // NEW: spawning fields
    [Header("Spawning")]
    public GameObject[] ballPrefabs;                 // All the ball prefab variants to pick from randomly
    public Transform spawnPoint;                     // Empty child object marking where new balls appear

    private GameObject currentBall;                  // The ball currently in play
    // END NEW

    // NEW: ballRb is now private, filled in by code when a ball spawns
    private Rigidbody ballRb;                        // The current ball's Rigidbody
    // END NEW
    private Vector3 ballOffset;                      // Gap between the arrow and the current ball
    private bool wasBallThrown = false;              // True after a throw until the next ball spawns
    private bool isAiming = false;                   // True while the arrow is swinging

    void Start()                                     // Runs once at the start
    {
        // NEW: replaces the old offset and isKinematic lines
        SpawnBall();                                 // Create the first ball
        // END NEW
    }

    void Update()                                    // Runs every frame
    {
        if (wasBallThrown)                           // The ball has been thrown and is flying
        {
            // NEW: R spawns the next ball
            if (Input.GetKeyDown(KeyCode.R))         // Player presses R to get the next ball
            {
                SpawnBall();                         // Spawn a new random ball and reset state
            }
            // END NEW
            return;                                  // Skip the rest of Update while the ball is in flight
        }

        if (Input.GetKeyDown(KeyCode.Space))         // Space pressed this frame
        {
            if (!isAiming)                           // First press
            {
                StartAiming();                       // Start the swinging animation
            }
            else                                     // Second press
            {
                ThrowBall();                         // Throw in the arrow's current direction
                return;                              // End this frame's Update early
            }
        }

        if (!isAiming)                               // Only allow sliding when not aiming
        {
            MoveArrow();                             // Slide and clamp the arrow
        }
        FollowArrow();                               // Keep the ball attached to the arrow
    }

    void MoveArrow()                                 // Moves the arrow left/right within the lane
    {
        float horizontalInput = Input.GetAxis("Horizontal");   // Read left/right input (-1 to 1)

        // Slide the arrow along its right direction, scaled by speed and frame time
        transform.Translate(Vector3.right * horizontalInput * moveSpeed * Time.deltaTime);

        Vector3 pos = transform.position;            // Copy the position to a local variable
        pos.x = Mathf.Clamp(pos.x, -xRange, xRange); // Constrain x so the arrow stays on the lane
        transform.position = pos;                    // Apply the clamped position
    }

    void FollowArrow()                               // Keeps the carried ball attached to the arrow
    {
        // NEW: now moves currentBall (the spawned ball) instead of the scene ball
        currentBall.transform.position = transform.position + ballOffset;   // Arrow position plus the stored gap
        // END NEW
    }

    void StartAiming()                               // Begins the aiming phase
    {
        isAiming = true;                             // Remember we are aiming
        animator.SetBool("Aiming", true);            // Tell the Animator to play the Aiming animation
    }

    void ThrowBall()                                 // Launches the ball
    {
        wasBallThrown = true;                        // Stop following the arrow
        isAiming = false;                            // Leave the aiming phase

        Vector3 throwDirection = arrowVisual.up;     // Capture the green (Y) axis direction before the animator changes state
        // If the green axis points backward, use -arrowVisual.up instead
        // If it points sideways, use arrowVisual.right or -arrowVisual.right

        throwDirection.y = 0f;                       // Remove any up/down part so the throw stays level on the lane
        throwDirection.Normalize();                  // Rescale to length 1 so the force is consistent

        animator.SetBool("Aiming", false);           // Return the Animator to Idle

        ballRb.isKinematic = false;                  // Turn physics back on so the ball can be pushed and roll
        ballRb.AddForce(throwDirection * throwForce, ForceMode.Impulse);   // Shoot the ball in one instant burst
    }

    // NEW: creates a new random ball at the spawn point
    void SpawnBall()                                 // Creates a new random ball at the spawn point
    {
        if (currentBall != null)                     // If there is an old ball still in the scene
        {
            Destroy(currentBall, 3f);                // Remove it after 3 seconds so it does not pile up
        }

        int index = Random.Range(0, ballPrefabs.Length);   // Pick a random array index (max is exclusive for ints)

        // Create a copy of the chosen prefab at the spawn point with no rotation
        currentBall = Instantiate(ballPrefabs[index], spawnPoint.position, Quaternion.identity);

        ballRb = currentBall.GetComponent<Rigidbody>();    // Grab the new ball's Rigidbody
        ballRb.isKinematic = true;                   // Carried, not simulated, until thrown

        ballOffset = currentBall.transform.position - transform.position;   // Recalculate the gap for this new ball

        wasBallThrown = false;                       // We are carrying a ball again
    }
    // END NEW
}