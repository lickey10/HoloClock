using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface iRule 
{
    int minDist { get; set; }
    int maxDist { get; set; }
    int scalar  { get; set; }
    Vector3 getResult( List<Boid> boids, int current );
}