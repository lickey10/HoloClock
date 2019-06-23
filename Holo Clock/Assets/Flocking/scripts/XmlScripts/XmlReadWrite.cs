using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using System;
using System.Collections.Generic;

/// <summary>
/// A tree-structured class that represents the xml element tree.
/// </summary>
public class XmlParsedInformation
{
    //the name of the element as gotten from the XML file.
    public string elementName   { get; set; }

    //the text between the element tags as gotten from the XML file. If no text was found, it is empty ( "" ).
    public string text          { get; set; }

    //the collection of children as gotten from the XML file, represented as a list of XmlParsedInformation.
    private List<XmlParsedInformation>  _children;

    //the collection of attributes as gotten from the XML file. Use getAttribute and getAttributes functions to gain access of the attributes by their name.
    private Dictionary<string, string>  _attributes;

    public XmlParsedInformation( )
    {
        _children = new List<XmlParsedInformation>( );
        _attributes = new Dictionary<string, string>( );
        text        = "";
    }

    public void addAttribute( String key, String value )
    {
        _attributes.Add( key, value );
    }
        
    /// <summary>
    /// Get a specific attribute by its XML name.
    /// </summary>
    /// <param name="key">the name of the attribute</param>
    /// <returns>the attribute if it can be found. Otherwise throws an exception.</returns>
    public String getAttribute( String key )
    {
        string value = "";
        try
        {
            value = _attributes[key];
        } catch ( Exception exc )
        {
            throw new Exception( "could not find this attribute name. " + exc.Message );
        }
        return value;
    }

    /// <summary>
    /// Get the first element by their XML name from the list of child nodes and potentially their childnodes.
    /// </summary>
    /// <param name="name"></param>
    /// <returns>the element as an XmlParsedInformation instance if it can be found. Otherwise throws an exception.</returns>
    public XmlParsedInformation getFirstElementByName( string name )
    {
        List<XmlParsedInformation> elements = getElementsByName( name );
        if ( elements.Count > 0 )
        {
            return elements[0];
        }
        throw new Exception( "could not find XML element with name " + name );
    }

    /// <summary>
    /// Gets a list of elements by their XML name from the list of child nodes and ptentially their childnodes.
    /// </summary>
    /// <param name="name"></param>
    /// <returns>a collection of xmlParsedInformation instances. This list is empty if no elements can be found.</returns>
    public List<XmlParsedInformation> getElementsByName( string name )
    {
        List<XmlParsedInformation> children = new List<XmlParsedInformation>( );
        findChildrenWithName( this, name, ref children );
        return children;
    }

    //find elements with the specific name. This is a recursive function.
    private void findChildrenWithName( XmlParsedInformation current, string name, ref List<XmlParsedInformation> children )
    {
        if ( current.elementName == name )
        {
            children.Add( current );
        }
        foreach ( XmlParsedInformation child in current.getChildren( ) )
        {
            findChildrenWithName( child, name, ref children );
        }
    }

    /// <summary>
    /// gets the attributes of this XML element represented by a string, string keyValue dictionary.
    /// </summary>
    /// <returns>a dictionary containing attributes as gotten from the XML file.</returns>
    public Dictionary<String, String> getAttributes( )
    {
        return _attributes;
    }

    public void addChild( XmlParsedInformation value )
    {
        _children.Add( value );
    }

    public List<XmlParsedInformation> getChildren( )
    {
        return _children;
    }
};

public class XmlReadWrite
{
    private static XmlReadWrite _instance;

    /// <summary>
    /// gets a singleton instance of this class.
    /// </summary>
    /// <returns></returns>
    public static XmlReadWrite getInstance( )
    {
        if ( _instance == null )
        {
            _instance = new XmlReadWrite( );
        }
        return _instance;
    }

    /// <summary>
    /// Reads a file from the Resources folder and returns it as an XML file.
    /// </summary>
    /// <param name="fileName">this is the filename of the xml file, without the .xml extension, so if your fileName is monsters.xml, the filename string should be monsters. 
    /// Also note that this file must be located in the Resources folder.</param>
    /// <returns></returns>
    public XmlDocument readXmlDocument( string fileName )
    {
        //remove the ".xml" extension
        fileName    = fileName.ToLower( );
        while ( fileName.Contains( ".xml" ) )
        {
            int index   = fileName.IndexOf( ".xml" );
            fileName    = fileName.Remove( index, 4 );
        }
        XmlDocument xmldoc = new XmlDocument( );
        try
        {
            TextAsset textAsset = ( TextAsset )Resources.Load( fileName );
            xmldoc.LoadXml( textAsset.text );
        }
        catch ( IOException ioExc )
        {
            throw new IOException( "Could not read this file. Make sure that it is an XML file and located in the Assets/Resources folder. Also make sure spelling is correct and the .xml file extension is omitted. " + ioExc.Message );
        }
        catch ( Exception exc )
        {
            throw new Exception( "Something went wrong reading the XML file. " + exc.Message );
        }
        return xmldoc;
    }

    /// <summary>
    /// parses a given xml file and returns the rootnode.
    /// </summary>
    /// <param name="xmlDoc"></param>
    /// <returns>the rootnode containing the xml file.</returns>
    public XmlParsedInformation parseXmlDocument( XmlDocument xmlDoc )
    {
        XmlElement rootNode = xmlDoc.DocumentElement;
        return parseXmlDocument( rootNode );
    }

    /// <summary>
    /// creates a new element for every child in the rootnode. 
    /// It then proceeds to create an xmlParsedInformation class where it stores that info in a tree.
    /// </summary>
    /// <param name="current"></param>
    /// <returns>returns the rootnode when it is parsed.</returns>
    private XmlParsedInformation parseXmlDocument( XmlElement current )
    {
        XmlAttributeCollection attributes   = current.Attributes;
        XmlParsedInformation xmlinfo        = new XmlParsedInformation( );
        xmlinfo.elementName                 = current.Name;
        foreach ( XmlAttribute attr in attributes )
        {
            xmlinfo.addAttribute( attr.Name, attr.Value );
        }

        //loop through every node in the current element
        foreach ( XmlNode node in current.ChildNodes )
        {
            //if the node itself is an element, we do the same thing as for the root element, until all child elements have been parsed.
            if ( node.NodeType == XmlNodeType.Element )
            {
                XmlElement element  = ( XmlElement )node;
                xmlinfo.addChild( parseXmlDocument( element ) );
            }
            else if ( node.NodeType == XmlNodeType.Text )
            {
                xmlinfo.text    = node.InnerText;
            }
        }
        return xmlinfo;
    }

    /// <summary>
    /// saves an XML document to the Resources folder with the given fileName.
    /// </summary>
    /// <param name="document">the xmlDocument that will be stored.</param>
    /// <param name="fileName">the filename.</param>
    public void saveXmlInfo( XmlDocument document, string fileName )
    {
        if ( !fileName.ToLower( ).Contains( ".xml" ) )
        { 
            fileName += ".xml";
        }

        string path = null;
        #if UNITY_EDITOR
            path = "Assets/Flocking/Resources/" + fileName;
        #endif

        using ( FileStream fs = new FileStream( path, FileMode.Create ) )
        {
            using ( StreamWriter writer = new StreamWriter( fs ) )
            {
                writer.Write( document.OuterXml );
            }
        }
        #if UNITY_EDITOR
                UnityEditor.AssetDatabase.Refresh( );
        #endif
    }
}