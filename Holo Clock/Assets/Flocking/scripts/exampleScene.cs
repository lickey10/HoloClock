using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class exampleScene : MonoBehaviour 
{
    private GameManager     _gameManager;

    private List<Boid>      _boids;

    /// <summary>
    /// The area in which boids will be spawned and live (if they exit the area, they will be removed from the simulation).
    /// </summary>
    public Transform        flockingArea;

    /// <summary>
    /// A collection of prefabs that will be used in the simulation.
    /// </summary>
    public GameObject[]     gameObjects;

    /// <summary>
    /// A collection of gameObjecs whos transform is used as a position where boids are spawned.
    /// </summary>
    public Transform[]     spawnPositions;

    /// <summary>
    /// A random value that will be added to the position. Together with the spawnPositions gameobject, this determindes where boids are spawned.
    /// </summary>
    public float            range;

    /// <summary>
    /// An emty gameObject that is used to group boids together.
    /// </summary>
    public Transform        collection;

    /// <summary>
    /// The width, height and depth of the flockingArea.
    /// </summary>
    private int _width { get { return ( int )flockingArea.localScale.x; } }
    private int _height { get { return ( int )flockingArea.localScale.y; } }
    private int _depth { get { return ( int )flockingArea.localScale.z; } }

    /// <summary>
    /// The x, y and z positions of the flockingArea.
    /// </summary>
    private int _posX { get { return ( int )flockingArea.position.x; } }
    private int _posY { get { return ( int )flockingArea.position.y; } }
    private int _posZ { get { return ( int )flockingArea.position.z; } }

    /// <summary>
    /// A boolean indicating whether boids will be spawned at predetermined spawn positions or are spawned at random locations on the map (this is true if no spawnPositions are assigned.)
    /// </summary>
    private bool _useSpawnPositions;

	// Use this for initialization
	void Start( ) 
    {
        _gameManager        = gameObject.GetComponent<GameManager>( );
        _boids              = _gameManager.boids;
        _useSpawnPositions  = spawnPositions.Length > 0;

        //Coroutine spawns boids.
        StartCoroutine( spawnBoids( ) );
        if ( gameObjects.Length == 0 )
        {
            throw new Exception( "no gameObjects have been selected. Make sure that at least one gameObject is selected" );
        }

        if ( flockingArea == null )
        {
            throw new Exception( "no flockingArea is selected." );
        }

        if ( !_useSpawnPositions )
        {
            Debug.Log( "No spawn positions have been set. Boids will spawn at random locations in the map flockingArea." );
        }
	}

    /// <summary>
    /// Boids are spawned in this coroutine.
    /// </summary>
    /// <returns></returns>
    private IEnumerator spawnBoids( )
    {
        while ( true )
        {
            for ( int i = 0; i < _gameManager.speed / 100; ++i )
            {
                Vector3 position = Vector3.zero;
                if ( _useSpawnPositions )
                {
                    //make sure that if the size of any dimension is 0, boids do not spawn in that direction.
                    float x         = _width == 0 ? 0.0f : ( UnityEngine.Random.value * range * 2 ) - range;
                    float y         = _height == 0 ? 0.0f : ( UnityEngine.Random.value * range * 2 ) - range;
                    float z         = _depth == 0 ? 0.0f : ( UnityEngine.Random.value * range * 2 ) - range;

                    //get a random element from the spawnPositions array and add a random value to it.
                    int index       = ( int )( UnityEngine.Random.value * spawnPositions.Length );
                    position = spawnPositions[index].position + new Vector3( x, y, z );
                }
                else
                {
                    //if no spawnPositions were selected, spawn boids on random locations on the map.
                    position = new Vector3( ( _posX - _width / 2 ) + UnityEngine.Random.value * _width, ( _posY - _height / 2 ) + UnityEngine.Random.value * _height, ( _posZ - _depth / 2 ) + UnityEngine.Random.value * _depth );
                }
                //finally, spawn the boid.
                spawnBoid( position );
            }
            yield return new WaitForSeconds( 15 / ( _gameManager.speed + 1 ) );
        }
    }

	// Update is called once per frame
	void Update( ) 
    {        
        for ( int i = _boids.Count - 1; i >= 0; --i )
        {

            Boid b      = _boids[i];
            Vector3 pos = b.transform.position;
            //if the boid is outside of the flockingArea, remove it from theh simulation.
            if ( ( _width > 0 && ( pos.x <= _posX - _width / 2 || pos.x >= _posX + _width / 2 ) ) ||
                    ( _height > 0 && ( pos.y <= _posY - _height / 2 || pos.y >= _posY + _height / 2 ) ) ||
                    ( _depth > 0 && ( pos.z <= _posZ - _depth / 2 || pos.z >= _posZ + _depth / 2 ) ) )
            {
                //wait until the tail is at the boids current location (so it is not abruptly deleted).
                GameObject.Destroy( b.gameObject, 0.5f );
                //no longer include this boid in any calculation by removing it from the boids collection.
                _boids.RemoveAt( i );
                b = null;
            }
        }
	}

    private void spawnBoid( Vector3 position )
    {
        //get a random prefab from the collection of prefabs.
        int index               = ( int )( UnityEngine.Random.value * gameObjects.Length );
        GameObject gobj         = gameObjects[index];
        //instanciate that prefab at the specified position and add it to the boid collection.
        gobj = Instantiate( gobj, position, Quaternion.identity ) as GameObject;
        gobj.transform.parent = collection;

        //initially, move the boid to the center of the world.
        Vector3 moveTo          = flockingArea.position;
        Vector3 dir             = moveTo - position;
        //add the boid script to the gameobject and set the velocity.
        Boid b                  = gobj.AddComponent<Boid>( );
        b.velocity = dir.normalized;

        //add the boid to the collection.
        _boids.Add( b );
    }
}
