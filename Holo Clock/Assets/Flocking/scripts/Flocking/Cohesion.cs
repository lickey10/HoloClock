using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cohesion : iRule
{
   public int minDist   { get; set; }
    public int maxDist  { get; set; }
    public int scalar   { get; set; }
    
    public Cohesion( )
    {
       
    }

    //create an empty vector to store the result of the flocking rule.
    public Vector3 getResult( List<Boid> boids, int current )
    {
        Vector3 result      = Vector3.zero;
        int count           = 0;
        for ( int i = 0; i < boids.Count; ++i )
        {
            //don't do anything for self.
            if ( i != current )
            {
                Boid b              = boids[current];
                Boid other          = boids[i];
                Vector3 otherPos    = other.position;
                //get the vector between the current boid and the neighbor boid.
                Vector3 dif         = otherPos - b.position;

                //get the squared magnitude. Only update the velocity if the magnitude is bigger than the minimum distance and smaller than the maximum distance.
                float dist          = Vector3.SqrMagnitude( dif );
                if ( dist <= maxDist * maxDist && dist >= minDist * minDist )
                {
                    //normalize the difference and add it to the result.
                    result          += otherPos;
                    count++;
                }
            }
        }

        if ( count > 0 )
        {
            //get the average 
            result /= count;
            Vector3 dir = result - boids[current].position;
            return dir.normalized * scalar;
        }
        return result;
    }
}