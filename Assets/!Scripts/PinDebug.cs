using UnityEngine;

public class PinDebug : MonoBehaviour


{

    //vars
    [SerializeField]private int pointValue = 1;
    private float KockdownSpeed = 1.2f;
    private bool isStanding = true;
    private string pinLabel = "this is a test";


    private Vector3 startPos;
    private Vector2 endPos = new Vector2 (0, 0);
    private Quaternion startRot;
    public Color fallCol = new Color(1f, 0f, 0f, 1f);

    private Rigidbody rb;
    private Renderer rend;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();

        startPos = transform.position;
        startRot = transform.rotation;

        if (rb == null)
        {
            Debug.Log("No rb found add rb");
        }

        Debug.Log("game obj is active");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("script is active");
    }

    // Update is called once per frame
    void Update()
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

    private void FixedUpdate()
    {
        if(isStanding && rb != null)
        {
            float currentSpeed = rb.linearVelocity.magnitude;

            if(currentSpeed > KockdownSpeed)
            {
                PinFellOver();
            }
        }
    }

    private void PinFellOver()
    {
        if(!isStanding)
        {
            return;
        }

        isStanding = false;

        if(rend != null)
        {
            rend.material.color = fallCol;

        }

        Debug.Log("Pin fell over!");

    }
}
