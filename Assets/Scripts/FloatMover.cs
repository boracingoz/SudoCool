using UnityEngine;

public class FloatMover : MonoBehaviour
{
    [Header("Move Settings")]
    public float amplitude = 1f;      
    public float speed = 1f;
    public bool randomizePhase = true;

    private Vector3 startPos;
    private Vector3 axis;
    private float phase;

    void Awake()
    {
        
        startPos = transform.position;

        axis = Random.onUnitSphere;
        axis.z = 0; 

        phase = randomizePhase ? Random.Range(0f, Mathf.PI * 2f) : 0f;
    }

    void Update()
    {
        float t = Time.time * speed + phase;

        Vector3 offset = axis * (Mathf.Sin(t) * amplitude);

        transform.position = startPos + offset;
    }
}
