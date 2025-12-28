using UnityEngine;

public class BulletTracer : MonoBehaviour
{
    private LineRenderer bulletTracer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        bulletTracer = GetComponent<LineRenderer>();
    }

    public void Draw(Vector3 start, Vector3 end)
    {
        bulletTracer.SetPosition(0, start);
        bulletTracer.SetPosition(1, end);

        Destroy(gameObject, 0.02f);        
    }
}
