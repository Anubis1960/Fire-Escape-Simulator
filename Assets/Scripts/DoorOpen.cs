using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public AnimationCurve openSpeedCurve = new AnimationCurve(
        new Keyframe(0, 1, 0, 0),
        new Keyframe(0.8f, 1, 0, 0),
        new Keyframe(1, 0, 0, 0)
    );

    public float openSpeedMultiplier = 2.0f;
    public float doorOpenAngle = 0.0f;

    private bool open = false;
    private bool enter = false;

    private Quaternion defaultRotation;
    private Quaternion targetRotation;

    private float openTime = 0f;

    void Start()
    {
        defaultRotation = transform.localRotation;
        targetRotation = defaultRotation;

        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = true;
        }
    }

    void Update()
    {
        if (!enter) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            open = !open;
            openTime = 0f;

            // Set the target rotation based on open state
            float targetAngle = open ? doorOpenAngle : 0f;
            Debug.Log("Target Angle: " + targetAngle);
            targetRotation = defaultRotation * Quaternion.Euler(0, targetAngle, 0);
        }

        // Only animate if we have started opening/closing
        if (openTime < 1f)
        {
            openTime += Time.deltaTime * openSpeedMultiplier * openSpeedCurve.Evaluate(openTime);
            transform.localRotation = Quaternion.Slerp(defaultRotation, targetRotation, openTime);
        }
    }

    void OnGUI()
    {
        if (enter)
        {
            GUI.Label(new Rect(Screen.width / 2 - 75, Screen.height - 100, 155, 30),
                "Press 'F' to " + (open ? "close" : "open") + " the door");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered trigger");
            enter = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enter = false;
        }
    }
}