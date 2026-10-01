using UnityEngine;



public class PinDebugLog: MonoBehaviour
{
    // -----------------------------------------------------------
    // PRIMITIVE VARIABLES
    // -----------------------------------------------------------
    public int pointValue = 1;                     // int
    public float knockdownVelocityThreshold = 1.2f; // float
    private bool isStanding = true;                 // bool
    private string pinLabel = "Pin";                 // string

    // -----------------------------------------------------------
    // UNITY VARIABLES
    // -----------------------------------------------------------
    private Vector3 startPosition;   // x/y/z position, used to remember where the pin started
    private Vector2 uiOffset = new Vector2(10f, 5f); // x/y pair, just to demonstrate the type
    private Quaternion startRotation; // 3D rotation, used to remember starting rotation
    public Color fallenColor = new Color(1f, 0f, 0f, 1f); // red tint applied when the pin falls

    // -----------------------------------------------------------
    // COMPONENTS AS VARIABLES
    // -----------------------------------------------------------
    private Rigidbody rb;       // fetched with GetComponent in Awake()
    private Renderer rend;      // fetched with GetComponent in Awake()

    // -----------------------------------------------------------
    // Awake() - runs once, before any Start() in the scene.
    // Best place to cache component references and starting state.
    // -----------------------------------------------------------
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();

        startPosition = transform.position;

        //defines blank zeroed rotation 
        startRotation = Quaternion.identity;

        //sets rotation to the objects current rotation in the scene 
        startRotation = transform.rotation;

        if (rb == null)
        {
            Debug.LogError(pinLabel + ": No Rigidbody found! This script requires one.");
        }

        Debug.Log("Game obj active");
    }

    // -----------------------------------------------------------
    // Start() - runs once, after every Awake() has finished.
    // -----------------------------------------------------------
    private void Start()
    {
        Debug.Log("Script is active!");
    }

    // -----------------------------------------------------------
    // Update() - runs once per rendered frame.
    // Used here for a simple non-physics check: has the pin
    // tipped past a safe angle even if it never triggers the
    // velocity check below.
    // -----------------------------------------------------------
    private void Update()
    {
        if (isStanding)
        {
            float tiltAngle = Vector3.Angle(transform.up, Vector3.up);
            if (tiltAngle > 35f)
            {
                PinFellOver();
            }
        }
    }

    // -----------------------------------------------------------
    // FixedUpdate() - runs on Unity's fixed physics timestep.
    // All physics reads/writes belong here, not in Update().
    // -----------------------------------------------------------
    private void FixedUpdate()
    {
        if (isStanding && rb != null)
        {
            float currentSpeed = rb.linearVelocity.magnitude;

            if (currentSpeed > knockdownVelocityThreshold)
            {
               
                PinFellOver();

            }
        }
    }

    // -----------------------------------------------------------
    // CUSTOM METHOD - this is the actual game logic that no
    // built-in component knows how to do on its own.
    // -----------------------------------------------------------
    private void PinFellOver()
    {
        if (!isStanding)
        {
            return;
        }

        isStanding = false;

        if (rend != null)
        {
            rend.material.color = fallenColor;
        }

        Debug.Log("Pin fell over");
    }
}