using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mistral.UniDialogue;
using SQLite4Unity3d;

public class TestCreator : MonoBehaviour 
{

	private void Start ()
	{
		DialogueDBAdmin.DeleteDatabase("Fucker.db");
		DialogueDBAdmin.CreateUniDialogueDB("Fucker.db");
		//SQLiteConnection _connection = new SQLiteConnection("Assets/StreamingAssets/Fucker.db", SQLiteOpenFlags.ReadWrite);
		DialogueDBConnection _connection = new DialogueDBConnection("Fucker.db", SQLiteOpenFlags.ReadWrite);
		DialogueDBManager _dbManager = new DialogueDBManager(_connection);
		
		int x = 1;
		x++;
		
		_dbManager.InsertConversationEntry("c1", -1);
		_dbManager.InsertConversationEntry("c2", -1);
		_dbManager.InsertConversationEntry("c3", -1);
		_dbManager.InsertConversationEntry("c4", -1);
		_dbManager.InsertConversationEntry("c5", -1);
		
		_dbManager.InsertContentEntry("jingping", "asdfasdf", -1);
		_dbManager.InsertContentEntry("jingping1", "asdfasdf1", -1);
		_dbManager.InsertContentEntry("jingping2", "asdfasdf2", -1);
		_dbManager.InsertContentEntry("jingping3", "asdfasdf3", -1);
		_dbManager.InsertContentEntry("jingping4", "asdfasdf4", -1);
		_dbManager.InsertContentEntry("jingping5", "asdfasdf5", -1);
		
		_dbManager.InsertConditionEntry("asduifgaiuwer", -1, -1);
		_dbManager.InsertConditionEntry("asdgsdgasb", -1, -1);
		_dbManager.InsertConditionEntry("asduisdgasfgaiuwer", -1, -1);
		_dbManager.InsertConditionEntry("123asduif13gaiuwer", -1, -1);
		_dbManager.InsertConditionEntry("at124s51uifgaiuwer", -1, -1);
		
		_dbManager.InsertExecutionEntry("xcv23", -1);
		_dbManager.InsertExecutionEntry("xcv213", -1);
		_dbManager.InsertExecutionEntry("xcv223", -1);
		_dbManager.InsertExecutionEntry("xcv233", -1);
		_dbManager.InsertExecutionEntry("xcv243", -1);
		_dbManager.InsertExecutionEntry("xcv253", -1);
		
		List<ExecutionEntry> list = _connection.Query<ExecutionEntry>("SELECT *, MAX(ID) FROM ExecutionEntry;");
		foreach (ExecutionEntry ee in list)
		{
			Debug.Log(ee.ExecutionCode);
		}
	}
}
