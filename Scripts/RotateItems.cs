using UnityEngine;

public class RotateItems : MonoBehaviour
{
    public float rotationSpeed = 0.5f;
    public float floatAmplitude = 0.25f;
    public float floatSpeed = 2f;

    Vector3 startPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up *rotationSpeed * Time.deltaTime, Space.World);

        float yOffset = Mathf.Sin(Time.time * floatSpeed) *floatAmplitude;
        transform.position = startPos + Vector3.up * yOffset;
    }
}
