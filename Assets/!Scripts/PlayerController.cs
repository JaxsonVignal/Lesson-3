using UnityEngine;
// Gives access to EventSystem, used to check if a finger is on a UI button
using UnityEngine.EventSystems;
// Gives access to List, used to collect the UI elements under a tap
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float throwForce = 15f;
    [SerializeField] private Rigidbody ballRb;

    [SerializeField] float rangeX = .5f;

    [SerializeField] private Transform arrowVisual;
    [SerializeField] private Animator animator;
    // Reference to SoundManager so the throw can play a sound
    [SerializeField] private SoundManager soundManager;
    // Parent object of the on-screen left and right buttons
    [SerializeField] private GameObject mobileControls;

    // Direction set by the on-screen buttons: -1 left, 1 right, 0 still
    private float mobileDirection = 0f;

    private Vector3 ballOffset;

    private bool wasBallThrown = false;

    private bool isAiming = false;

    [SerializeField] private GameObject[] ballPrefabs;
    [SerializeField] private Transform spawnPos;

    private GameObject currentBall;

    private void Start()
    {
        spawnBall();

        // Mobile builds: show the on-screen buttons
#if UNITY_ANDROID || UNITY_IOS
        mobileControls.SetActive(true);
        // Every other platform: hide them
#else
        mobileControls.SetActive(false);
#endif
    }

    void Update()
    {
        if (wasBallThrown)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                spawnBall();
            }

            return;
        }

        // Space on PC, or a tap off the buttons on mobile
        if (getThrowInput())
        {
            if (!isAiming)
            {
                startAiming();
            }

            else
            {
                throwBall();
                return;
            }
        }

        if (!isAiming)
        {
            moveArrow();
        }

        followArrow();
    }

    void moveArrow()
    {
        // Holds the move direction for this frame
        float horizontalInput;

        // Mobile builds: use the direction set by the on-screen buttons
#if UNITY_ANDROID || UNITY_IOS
        horizontalInput = mobileDirection;
        // Every other platform: use the arrow keys or A and D
#else
        horizontalInput = Input.GetAxis("Horizontal");
#endif

        transform.Translate(Vector3.right * horizontalInput * moveSpeed * Time.deltaTime);

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -rangeX, rangeX);
        transform.position = pos;
    }

    void followArrow()
    {
        currentBall.transform.position = transform.position + ballOffset;
    }

    void startAiming()
    {
        isAiming = true;
        animator.SetBool("Aiming", true);
    }

    void throwBall()
    {
        wasBallThrown = true;
        isAiming = false;

        Vector3 throwDirection = arrowVisual.up;

        throwDirection.y = 0f;
        throwDirection.Normalize();

        animator.SetBool("Aiming", false);

        ballRb.isKinematic = false;

        ballRb.AddForce(throwDirection * throwForce, ForceMode.Impulse);

        // Play the throw sound as the ball leaves
        soundManager.PlaySound("Throw");
    }

    void spawnBall()
    {
        if (currentBall != null)
        {
            Destroy(currentBall);
        }

        int index = Random.Range(0, ballPrefabs.Length);

        currentBall = Instantiate(ballPrefabs[index], spawnPos.position, Quaternion.identity);

        ballRb = currentBall.GetComponent<Rigidbody>();
        ballRb.isKinematic = true;

        ballOffset = currentBall.transform.position - transform.position;

        wasBallThrown = false;
    }

    public void StartThrow()
    {
        spawnBall();
    }

    // Called by the on-screen buttons' Event Triggers: -1 or 1 on Pointer Down, 0 on Pointer Up
    public void SetMoveDirection(float direction)
    {
        // Store the direction so moveArrow uses it every frame until the button is released
        mobileDirection = direction;
    }

    // Returns true on the frame the player asks to aim or throw
    bool getThrowInput()
    {
        // Mobile builds: check for a new tap
#if UNITY_ANDROID || UNITY_IOS
        // In the Unity Editor: treat a mouse click as a tap, so it can be tested without a phone
#if UNITY_EDITOR
        // Left mouse button pressed this frame
        if (Input.GetMouseButtonDown(0))
        {
            // Ignore clicks on the on-screen buttons
            return !isOverUI(Input.mousePosition);
        }

        // No click this frame
        return false;
        // On a real phone: check for a new tap
#else
        // No finger on the screen this frame
        if (Input.touchCount == 0)
        {
            return false;
        }

        // The first finger touching the screen
        Touch touch = Input.GetTouch(0);

        // Only count the moment the finger touches down, not while it is held
        if (touch.phase != TouchPhase.Began)
        {
            return false;
        }

        // Ignore taps on the on-screen buttons so moving does not also throw
        return !isOverUI(touch.position);
#endif
        // Every other platform: check the space bar
#else
        // True on the frame Space is pressed
        return Input.GetKeyDown(KeyCode.Space);
#endif
    }

    // Returns true if any UI element (like a move button) is under this screen position
    bool isOverUI(Vector2 screenPosition)
    {
        // Describe a pointer at the tap position
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = screenPosition;

        // Ask every canvas which UI elements are under that position
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        // Anything found means the tap landed on UI
        return results.Count > 0;
    }
}