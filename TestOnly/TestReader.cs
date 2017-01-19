using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Xml.Serialization;

public class TestReader : MonoBehaviour 
{
	#region Public Static Methods

	public static void Load ()
	{
		TextAsset asset = Resources.Load<TextAsset> ("Fuck");

		Debug.Log (asset.text);

		//XmlSerializer serializer = new XmlSerializer ();

		//StringReader reader = new StringReader (asset.text);
	}

	#endregion


	#region MonoBehaviours

	private void Start ()
	{
		Load ();
	}

	#endregion
}
