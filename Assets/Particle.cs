using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public Vector3 position;
    public Vector3 velocity;
    public Vector3 acceleration;
    public int mass;
    public int r;
    public Particle other;
    // Start is called before the first frame update
    void Start()
    {
        // position = new Vecto
        position = new Vector3(0,0,0);
        velocity = new Vector3(Random.Range(1,20), Random.Range(1,20), 0);
        velocity = velocity * Random.Range(2,6);
        acceleration = new Vector3(0,0,0);
        mass = Random.Range(2,6);
        r = (int)Mathf.Sqrt(mass)* 3;
        this.transform.position = new Vector3(position.x, position.y, 0);
        this.transform.localScale *= r+2;
    }
    public void ApplyForce(Vector3 force)
    {
        Vector3 f = force;
        f /= mass;
        acceleration += f;
    }
    // Update is called once per frame
    void Update()
    {
        velocity += acceleration* Time.deltaTime;
        position+= velocity* Time.deltaTime;
        transform.position = position;
        acceleration = Vector3.zero;
        Collide(other);
        BounceEdges();
    }
    void BounceEdges()
    {
        // Assuming the screen edges are defined by Camera bounds
        Camera cam = Camera.main;
        float screenWidth = cam.orthographicSize * cam.aspect;
        float screenHeight = cam.orthographicSize;

        // Bounce on x-axis
        if (position.x > screenWidth - r)
        {
            position.x = screenWidth - r;
            velocity.x *= -1;
        }
        else if (position.x < -screenWidth + r)
        {
            position.x = -screenWidth + r;
            velocity.x *= -1;
        }

        // Bounce on y-axis
        if (position.y > screenHeight - r)
        {
            position.y = screenHeight - r;
            velocity.y *= -1;
        }
        else if (position.y < -screenHeight + r)
        {
            position.y = -screenHeight + r;
            velocity.y *= -1;
        }
    }
    public void Collide(Particle other)
    {
        Vector3 impactVector = other.position - position;
        float d = impactVector.magnitude;
        if (d < r+ other.r)
        {
            // Push the particles out so that they are not overlapping
            float overlap = d - (r+ other.r);
            Vector3 dir = impactVector.normalized * (overlap * 0.5f);
            position += dir;
            other.position -= dir;

            // Correct the distance
            d = r+ other.r;
            impactVector = impactVector.normalized * d;

            float mSum = mass + other.mass;
            Vector3 vDiff = other.velocity - velocity;
            // Particle A (this)
            float num = Vector3.Dot(vDiff, impactVector);
            float den = mSum * d * d;
            Vector3 deltaVA = impactVector * (2 * other.mass * num / den);
            velocity += deltaVA;
            // Particle B (other)
            Vector3 deltaVB = impactVector * (-2 * mass * num / den);
            other.velocity += deltaVB;
        }
    }
}
