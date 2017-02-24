using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mistral.UniDialogue;
using SQLite4Unity3d;

public class TestCustomSQL : MonoBehaviour 
{
	private SQLiteConnection _connection;
	
	private void Awake ()
	{
		string _dbPath = string.Format(@"Assets/StreamingAssets/{0}", "Dialogue.db");

		_connection = new SQLiteConnection(_dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
		
		Debug.Log(_connection.Query<DialogueEntry1>("SELECT * FROM DialogueEntry")[0].Actor);
	}
}
