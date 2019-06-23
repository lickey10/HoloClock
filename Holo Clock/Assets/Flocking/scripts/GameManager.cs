using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Threading;

public class GameManager : MonoBehaviour 
{   
    /// <summary>
    /// The manager that handles the updates in the UI sliders and buttons.
    /// </summary>
    public UiManager        uiManager;

    /// <summary>
    /// a list of boids is used to update the velocity of gameObjects that are subscribed in the list using flocking.
    /// </summary>
    private List<Boid>      _boids;

    /// <summary>
    /// a reference to the boids list that other classes can use to add, remove or loop through the boids.
    /// </summary>
    public List<Boid>       boids { get { return _boids; } }

    public int speed        { get; set; }

    /// <summary>
    /// The three elements that determine the velocity of boids.
    /// </summary>
    private Alignment   _alignment;
    private Cohesion    _cohesion;
    private Separation  _separation;

	void Awake( ) 
    {
        _boids                  = new List<Boid>( );
        _alignment              = new Alignment( );
        _cohesion               = new Cohesion( );
        _separation             = new Separation( );

        extractXmlValues( readDataStorageXml( "dataStorage.xml" ) );
        //get the values of the flocking rules from the uiManager.
    }

	// Update is called once per frame
	void Update( )
    {
        new Thread( ( ) =>
        {
            //run through all boids.
            for ( int i = _boids.Count - 1; i >= 0; --i )
            {

                Boid b         = _boids[i];
                //get the boids current velocity.
                Vector3 velocity    = b.velocity;

                //add the influences of neighboring boids to the velocity.
                velocity += _alignment.getResult( _boids, i );
                velocity += _cohesion.getResult( _boids, i );
                velocity += _separation.getResult( _boids, i );

                //normalize the velocity and make sure that the boids new velocity is updated.
                velocity.Normalize( );
                b.velocity = velocity;
                              
                b.lookat    = b.position + velocity;
            }
        } ).Start( );

        for ( int i = _boids.Count - 1; i >= 0; --i )
        {
            //update the boids position in the mainthread.
            _boids[i].transform.position += _boids[i].velocity * Time.deltaTime * speed;           
        }
    }

    /// <summary>
    /// update the flocking rule values from the uiManager.
    /// </summary>
    public void setFlockingRuleValues( )
    { 
        speed                   = uiManager.speed;
        _alignment.minDist      = uiManager.alignmentMinDist;
        _alignment.maxDist      = uiManager.alignmentMaxDist;
        _alignment.scalar       = uiManager.alignmentScale;

        _cohesion.minDist       = uiManager.cohesionMinDist;
        _cohesion.maxDist       = uiManager.cohesionMaxDist;
        _cohesion.scalar        = uiManager.cohesionScale;

        _separation.minDist     = uiManager.separationMinDist;
        _separation.maxDist     = uiManager.separationMaxDist;
        _separation.scalar      = uiManager.separationScale;
    }

     public XmlParsedInformation readDataStorageXml( string fileName )
    {
        //get the xml-file from the xmlreadWrite class.
        XmlDocument doc                     = XmlReadWrite.getInstance( ).readXmlDocument( fileName );
        XmlParsedInformation rootNode       = XmlReadWrite.getInstance( ).parseXmlDocument( doc );
        return rootNode;
    }

    //gets the parsed infrormation from the default and dataStorage files and parses it to retrieve the flocking rule values.
    public void extractXmlValues( XmlParsedInformation rootNode )
    { 
        //get the list of flockingRules and the speed parameter.
        List<XmlParsedInformation> flockingRules    = rootNode.getElementsByName( "flockingRule" );
        XmlParsedInformation speedElement           = rootNode.getFirstElementByName( "speed" );

        speed   = parseAttribute( "speed", "value", speedElement.getAttribute( "value" ) );

        //parse the flocking rules and set the values.
        foreach ( XmlParsedInformation element in flockingRules )
        {
            string elementName                      = element.getAttribute( "name" );

            switch ( elementName )
            {
                case "Alignment":
                    _alignment.minDist                = parseAttribute( name, "minDist", element.getAttribute( "minDist" ) );
                    _alignment.maxDist                = parseAttribute( name, "maxDist", element.getAttribute( "maxDist" ) );
                    _alignment.scalar                 = parseAttribute( name, "scalar", element.getAttribute( "scalar" ) );
                    break;
                case "Cohesion":
                    _cohesion.minDist                 = parseAttribute( name, "minDist", element.getAttribute( "minDist" ) );
                    _cohesion.maxDist                 = parseAttribute( name, "maxDist", element.getAttribute( "maxDist" ) );
                    _cohesion.scalar                  = parseAttribute( name, "scalar", element.getAttribute( "scalar" ) );
                    break;
                case "Separation":
                    _separation.minDist               = parseAttribute( name, "minDist", element.getAttribute( "minDist" ) );
                    _separation.maxDist               = parseAttribute( name, "maxDist", element.getAttribute( "maxDist" ) );
                    _separation.scalar                = parseAttribute( name, "scalar", element.getAttribute( "scalar" ) );
                    break;                    
            }            
        }
    }

    private int parseAttribute( string name, string key, string value )
    {
        try
        {
            return int.Parse( value );
        }
        catch ( Exception exc )
        {
            throw new Exception( "could not parse value : " + value + " in element " + name + ". Attribute : " + key + ". " + exc );
        }
    }
}
