using UnityEngine;

public class MovementDemo : MonoBehaviour
{
    // Seconds counter
    private float secondTimer = 0f;
    private int secondsPassed = 0;

    // Movement
    [SerializeField] private bool isMoving = true;   // Toggle this in the Inspector during Play mode
    [SerializeField] private float speed = 2f;
    [SerializeField] private Vector3 direction = Vector3.right;

    // Direction reversal
    [SerializeField] private float reverseInterval = 5f;   // Seconds before the direction flips
    private float reverseTimer = 0f;

    // Color
    [SerializeField] private Color movingColor = Color.red;
    private Renderer objRenderer;
    private Color originalColor;

    void Start()
    {
        objRenderer = GetComponent<Renderer>();

        // Save the starting color so we can switch back to it when stopped
        originalColor = objRenderer.material.color;
    }

    void Update()
    {
        CountSeconds();

        if (isMoving)
        {
            Move();
            CheckReverse();
        }

        UpdateColor();
    }

    // Print how many seconds have passed, once per second
    void CountSeconds()
    {
        secondTimer += Time.deltaTime;

        if (secondTimer >= 1f)
        {
            secondTimer -= 1f;   // Subtract instead of resetting to 0 so leftover time is not lost
            secondsPassed++;
            Debug.Log("Seconds passed: " + secondsPassed);
        }
    }

    // Move the object using its transform
    void Move()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    // Flip to the opposite direction every reverseInterval seconds
    void CheckReverse()
    {
        reverseTimer += Time.deltaTime;

        if (reverseTimer >= reverseInterval)
        {
            reverseTimer = 0f;
            direction = -direction;
            Debug.Log("Direction reversed!");
        }
    }

    // Red while moving, original color while stopped
    void UpdateColor()
    {
        if (isMoving)
        {
            objRenderer.material.color = movingColor;
        }
        else
        {
            objRenderer.material.color = originalColor;
        }
    }
}