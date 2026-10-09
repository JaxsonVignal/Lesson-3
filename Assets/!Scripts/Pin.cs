using UnityEngine;

public class Pin : MonoBehaviour
{
    [SerializeField] private float fallenAngleThreshold = 10f;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private Rigidbody rb;
    private bool isFallen = false;

    private void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        rb = GetComponent<Rigidbody>();
    }


    //method to check if each pin is going to fall each frame if it has we set is fallen = true
    public bool HasFallen()
    {
        //recording the angle of our pins so that we can check if the angle the pin is currently at should not allow it to stand 
        float angle = Quaternion.Angle(startRotation, transform.rotation);
        if (angle > fallenAngleThreshold)
        {
            isFallen = true;
        }
        return isFallen;
    }


    //method to reset each pin after a new frame is started 
    public void ResetPin()
    {
        //setting the pin game object to true to bring the model back into the scene 
        gameObject.SetActive(true);

        //set is fallen to false so pin forgets the last frame 
        isFallen = false;

        //reset rb values so that theres no forces left over from last throw
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = startPosition;
        transform.rotation = startRotation;
    }
}