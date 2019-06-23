using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class UiManager : MonoBehaviour 
{
    //sliders
    public Slider                   slider_speed;

    public Slider                   slider_alignmentMinDist;
    public Slider                   slider_alignmentMaxDist;
    public Slider                   slider_alignmentScale;

    public Slider                   slider_cohesionMinDist;
    public Slider                   slider_cohesionMaxDist;
    public Slider                   slider_cohesionScale;

    public Slider                   slider_separationMinDist;
    public Slider                   slider_separationMaxDist;
    public Slider                   slider_separationScale;

    private XmlParsedInformation    _dataStorage;
    private XmlParsedInformation    _defaults;

    public int speed                { get; private set; }

    public int alignmentMinDist     { get; private set; }
    public int alignmentMaxDist     { get; private set; }
    public int alignmentScale       { get; private set; }

    public int cohesionMinDist      { get; private set; }
    public int cohesionMaxDist      { get; private set; }
    public int cohesionScale        { get; private set; }

    public int separationMinDist    { get; private set; }
    public int separationMaxDist    { get; private set; }
    public int separationScale      { get; private set; }

	// Use this for initialization
	void Awake( ) 
    {
        _dataStorage  = readDataStorageXml( "dataStorage.xml" );
        _defaults     = readDataStorageXml( "default" );

        extractXmlValues( _dataStorage );
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
                    alignmentMinDist                = parseAttribute( name, "minDist", element.getAttribute( "minDist" ) );
                    alignmentMaxDist                = parseAttribute( name, "maxDist", element.getAttribute( "maxDist" ) );
                    alignmentScale                  = parseAttribute( name, "scalar", element.getAttribute( "scalar" ) );
                    break;
                case "Cohesion":
                    cohesionMinDist                 = parseAttribute( name, "minDist", element.getAttribute( "minDist" ) );
                    cohesionMaxDist                 = parseAttribute( name, "maxDist", element.getAttribute( "maxDist" ) );
                    cohesionScale                   = parseAttribute( name, "scalar", element.getAttribute( "scalar" ) );
                    break;
                case "Separation":
                    separationMinDist               = parseAttribute( name, "minDist", element.getAttribute( "minDist" ) );
                    separationMaxDist               = parseAttribute( name, "maxDist", element.getAttribute( "maxDist" ) );
                    separationScale                 = parseAttribute( name, "scalar", element.getAttribute( "scalar" ) );
                    break;                    
            }            
        }
        //set the sliders current values.
        slider_speed.value              = speed;

        slider_alignmentMinDist.value   = alignmentMinDist;
        slider_alignmentMaxDist.value   = alignmentMaxDist;
        slider_alignmentScale.value     = alignmentScale;

        slider_cohesionMinDist.value    = cohesionMinDist;
        slider_cohesionMaxDist.value    = cohesionMaxDist;
        slider_cohesionScale.value      = cohesionScale;

        slider_separationMinDist.value  = separationMinDist;
        slider_separationMaxDist.value  = separationMaxDist;
        slider_separationScale.value    = separationScale;
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
    
    public void saveChanges( )
    {
        //create an xml document that will be stored.
        XmlDocument doc             = new XmlDocument( );

        XmlElement rootNode         = doc.CreateElement( "dataStorage" );
        XmlElement flockingRules    = doc.CreateElement( "flockingRules" );
        XmlElement alignment        = createFlockingRuleElement( doc, "Alignment", alignmentMinDist, alignmentMaxDist, alignmentScale );
        XmlElement cohesion         = createFlockingRuleElement( doc, "Cohesion", cohesionMinDist, cohesionMaxDist, cohesionScale );
        XmlElement separation       = createFlockingRuleElement( doc, "Separation", separationMinDist, separationMaxDist, separationScale );
        XmlElement speedElement     = doc.CreateElement( "speed" );

        speedElement.SetAttribute( "value", speed.ToString( ) );

        doc.AppendChild( rootNode );        
        rootNode.AppendChild( flockingRules );
        rootNode.AppendChild( speedElement );
        flockingRules.AppendChild( alignment );
        flockingRules.AppendChild( cohesion );
        flockingRules.AppendChild( separation );      

        XmlReadWrite.getInstance( ).saveXmlInfo( doc, "dataStorage" );
    }

    private XmlElement createFlockingRuleElement( XmlDocument doc, string name, int minDist, int maxDist, int scale )
    {
        XmlElement element  = doc.CreateElement( "flockingRule" );

        element.SetAttribute( "name", name );
        element.SetAttribute( "minDist", minDist.ToString( ) );
        element.SetAttribute( "maxDist", maxDist.ToString( ) );
        element.SetAttribute( "scalar", scale.ToString( ) );
        element.SetAttribute( "type", "flockingRule" );

        return element;
    }

    void Update( )
    {
        gameObject.GetComponent<GameManager>( ).setFlockingRuleValues( );
    }

    public void resetToDefaults( )
    {
        extractXmlValues( _defaults );
    }

    public void updateSpeed( float newValue )
    {
        speed               = ( int )newValue;
    }

    public void updateAlignmentMinDist( float newValue )
    {
        alignmentMinDist    = ( int )newValue;
    }

    public void updateAlignmentMaxDist( float newValue )
    {
        alignmentMaxDist    = ( int )newValue;
    }

    public void updateAlignmentScale( float newValue )
    {
        alignmentScale      = ( int )newValue;
    }

    public void updateCohesionMinDist( float newValue )
    {
        cohesionMinDist     = ( int )newValue;
    }

    public void updateCohesionMaxDist( float newValue )
    {
        cohesionMaxDist     = ( int )newValue;
    }

    public void updateCohesionScale( float newValue )
    {
        cohesionScale       = ( int )newValue;
    }

    public void updateSeparationMinDist( float newValue )
    {
        separationMinDist   = ( int )newValue;
    }

    public void updateSeparationMaxDist( float newValue )
    {
        separationMaxDist   = ( int )newValue;
    }

    public void updateSepatationScale( float newValue )
    {
        separationScale     = ( int )newValue;
    }
}
