using UnityEngine;
using System.Collections;

public class Boid : MonoBehaviour 
{
    public Vector3 position { get; set; }
    public Vector3 velocity { set; get; }
    public Vector3 lookat   { get; set; }

    void Update( )
    {
        //we cannot request the transform in a thread other than the main thread, so we set the position here.
        position    = transform.position;
        transform.LookAt( lookat );
    }
}
