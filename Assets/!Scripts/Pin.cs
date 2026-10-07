using UnityEngine;

public class Pin : MonoBehaviour
{
    [SerializeField] private float fallenAngleThreshold = 10f;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private Rigidbody rb;

    private void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        rb = GetComponent<Rigidbody>();
    }

    public bool HasFallen()
            {
    float angle = Quaternion.Angle(startRotation, transform.rotation);
    bool isFallen = angle > fallenAngleThreshold;
            return isFallen;
        }

    public void ResetPin()
    {
    rb.linearVelocity = Vector3.zero;
    rb.angularVelocity = Vector3.zero;
    transform.position = startPosition;
    transform.rotation = startRotation;
       }
}