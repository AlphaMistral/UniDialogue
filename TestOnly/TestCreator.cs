using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mistral.UniDialogue;
using SQLite4Unity3d;

public class TestCreator : MonoBehaviour 
{

	private void Start ()
	{
		//DialogueDBAdmin.DeleteDatabase("Fucker.db");
		//DialogueDBAdmin.CreateUniDialogueDB("Fucker.db");
		//SQLiteConnection _connection = new SQLiteConnection("Assets/StreamingAssets/Fucker.db", SQLiteOpenFlags.ReadWrite);
		DialogueDBConnection _connection = new DialogueDBConnection("Fucker.db", SQLiteOpenFlags.ReadWrite);
		ExecutionEntry entry = new ExecutionEntry(123, "asdfawfewqfasdfsadf", 5123);
		ExecutionEntry entry2 = new ExecutionEntry(1231, "asdfawfewqfasdfsadf", 5123);
		ExecutionEntry entry3 = new ExecutionEntry(1232, "asdfawfewqfasdfsadf", 5123);
		ExecutionEntry entry4 = new ExecutionEntry(1233, "asdfawfewqfasdfsadf", 5123);
		ExecutionEntry entry5 = new ExecutionEntry(1234, "asdfawfewqfasdfsadf", 5123);
		ExecutionEntry entry6 = new ExecutionEntry(1235, "asdfawfewqfasdfsadf", 5123);
		ExecutionEntry entry7 = new ExecutionEntry(1236, "asdfawfewqfasdfsadf", 5123);
		ExecutionEntry entry8 = new ExecutionEntry(1237, "asdfawfewqfasdfsadf", 5123123);
		List<ExecutionEntry> list = _connection.Query<ExecutionEntry>("SELECT *, MAX(ID) FROM ExecutionEntry;");
		foreach (ExecutionEntry ee in list)
		{
			Debug.Log(ee.NextEntryID);
		}
	}
}
