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
		
		ConversationDBEntry con1 = new ConversationDBEntry("asd1", -1);
		ConversationDBEntry con2 = new ConversationDBEntry("asd2", -1);
		ConversationDBEntry con3 = new ConversationDBEntry("asd3", -1);
		ConversationDBEntry con4 = new ConversationDBEntry("asd4", -1);
		ConversationDBEntry con5 = new ConversationDBEntry("asd5", -1);
		
		ContentDBEntry coe1 = new ContentDBEntry("fff1", "ccc1", -1);
		ContentDBEntry coe2 = new ContentDBEntry("fff2", "ccc2", -1);
		ContentDBEntry coe3 = new ContentDBEntry("fff3", "ccc3", -1);
		ContentDBEntry coe4 = new ContentDBEntry("fff4", "ccc4", -1);
		ContentDBEntry coe5 = new ContentDBEntry("fff5", "ccc5", -1);
		
		ExecutionDBEntry exe1 = new ExecutionDBEntry("eee1", -1);
		ExecutionDBEntry exe2 = new ExecutionDBEntry("eee2", -1);
		ExecutionDBEntry exe3 = new ExecutionDBEntry("eee3", -1);
		ExecutionDBEntry exe4 = new ExecutionDBEntry("eee4", -1);
		ExecutionDBEntry exe5 = new ExecutionDBEntry("eee5", -1);
		
		ConditionDBEntry cod1 = new ConditionDBEntry("ddd1", -1, -1);
		ConditionDBEntry cod2 = new ConditionDBEntry("ddd2", -1, -1);
		ConditionDBEntry cod3 = new ConditionDBEntry("ddd3", -1, -1);
		ConditionDBEntry cod4 = new ConditionDBEntry("ddd4", -1, -1);
		ConditionDBEntry cod5 = new ConditionDBEntry("ddd5", -1, -1);
		
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
		
		List<ExecutionDBEntry> list = _connection.Query<ExecutionDBEntry>("SELECT *, MAX(ID) FROM ExecutionDBEntry;");
		foreach (ExecutionDBEntry ee in list)
		{
			Debug.Log(ee.ExecutionCode);
		}
	}
}
