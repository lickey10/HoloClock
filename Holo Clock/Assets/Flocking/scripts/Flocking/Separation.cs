using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Separation : iRule
{
    public int minDist  { get; set; }
    public int maxDist  { get; set; }
    public int scalar   { get; set; }
    
    public Separation( )
    {
        
    }
        
    public Vector3 getResult( List<Boid> boids, int current )
    {
        Vector3 result      = Vector3.zero;
        int count           = 0;
        for ( int i = 0; i < boids.Count; ++i )
        {
            //don't do anything for self.
            if ( i != current )
            {
                //get the boid from the loop.
                Boid b          = boids[current];
                Boid other      = boids[i];
                Vector3 bPos    = b.position;
                Vector3 dif     = bPos - other.position;

                float dist      = Vector3.Magnitude( dif );
                if ( dist <= maxDist && dist >= minDist )
                {
                    dif.Normalize( );
                    result      += dif / dist;
                    count++;
                }
            }
        }

        if ( count > 0 )
        {
            result /= count;
            result.Normalize( );
            return result * scalar;
        }
        return result;
    }
}