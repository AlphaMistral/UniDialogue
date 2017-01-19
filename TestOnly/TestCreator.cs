using UnityEngine;
using System.Collections;
using Mistral.UniDialogue;
using SQLite4Unity3d;

public class TestCreator : MonoBehaviour 
{

	private void Start ()
	{
		//SQLiteConnection  _connection = new SQLiteConnection("Assets/StreamingAssets/Fucker.db", SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
		DialogueDBAdmin.CreateUniDialogueDB("Fucker.db");
	}
}
