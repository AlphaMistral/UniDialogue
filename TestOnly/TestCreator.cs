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
		
		ConversationEntry con1 = new ConversationEntry("asd1", -1);
		ConversationEntry con2 = new ConversationEntry("asd2", -1);
		ConversationEntry con3 = new ConversationEntry("asd3", -1);
		ConversationEntry con4 = new ConversationEntry("asd4", -1);
		ConversationEntry con5 = new ConversationEntry("asd5", -1);
		
		ContentEntry coe1 = new ContentEntry("fff1", "ccc1", -1);
		ContentEntry coe2 = new ContentEntry("fff2", "ccc2", -1);
		ContentEntry coe3 = new ContentEntry("fff3", "ccc3", -1);
		ContentEntry coe4 = new ContentEntry("fff4", "ccc4", -1);
		ContentEntry coe5 = new ContentEntry("fff5", "ccc5", -1);
		
		ExecutionEntry exe1 = new ExecutionEntry("eee1", -1);
		ExecutionEntry exe2 = new ExecutionEntry("eee2", -1);
		ExecutionEntry exe3 = new ExecutionEntry("eee3", -1);
		ExecutionEntry exe4 = new ExecutionEntry("eee4", -1);
		ExecutionEntry exe5 = new ExecutionEntry("eee5", -1);
		
		ConditionEntry cod1 = new ConditionEntry("ddd1", -1, -1);
		ConditionEntry cod2 = new ConditionEntry("ddd2", -1, -1);
		ConditionEntry cod3 = new ConditionEntry("ddd3", -1, -1);
		ConditionEntry cod4 = new ConditionEntry("ddd4", -1, -1);
		ConditionEntry cod5 = new ConditionEntry("ddd5", -1, -1);
		
		_dbManager.InsertEntry(con1);
		_dbManager.InsertEntry(con2);
		_dbManager.InsertEntry(con3);
		_dbManager.InsertEntry(con4);
		_dbManager.InsertEntry(con5);
		_dbManager.InsertEntry(coe1);
		_dbManager.InsertEntry(coe2);
		_dbManager.InsertEntry(coe3);
		_dbManager.InsertEntry(coe4);
		_dbManager.InsertEntry(coe5);
		_dbManager.InsertEntry(exe1);
		_dbManager.InsertEntry(exe2);
		_dbManager.InsertEntry(exe3);
		_dbManager.InsertEntry(exe4);
		_dbManager.InsertEntry(exe5);
		_dbManager.InsertEntry(cod1);
		_dbManager.InsertEntry(cod2);
		_dbManager.InsertEntry(cod3);
		_dbManager.InsertEntry(cod4);
		_dbManager.InsertEntry(cod5);
		
		List<ExecutionEntry> list = _connection.Query<ExecutionEntry>("SELECT *, MAX(ID) FROM ExecutionEntry;");
		foreach (ExecutionEntry ee in list)
		{
			Debug.Log(ee.ExecutionCode);
		}
	}
}
