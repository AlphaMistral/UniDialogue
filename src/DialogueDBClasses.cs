//
// Copyright (c) 2015-2017 Jingping Yu.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using SQLite4Unity3d;

namespace Mistral.UniDialogue
{
    /// <summary>
    /// The types of the entries. 
    /// </summary>
    public enum  EntryType
    {
        Conversation = 0,
        Execution = 2,
        Condition = 3,
        Content = 1,
		Extension = 4
    }

    ///Why are all the variables in capital? Because of SQLite4Unity3d. 
    
    /// <summary>
    /// The Base Abstract Class for all DialogueEntries. 
    /// </summary>
	public abstract class DialogueDBEntry
	{
        /// <summary>
        /// The Primary ID Key for the Entry. If set to 0, it means End. 
        /// </summary>
        /// <value>The ID.</value>
		[PrimaryKey]
		public int ID { get; protected set; }

        /// <summary>
        /// Gets the type of the entry.
        /// </summary>
        /// <returns>The entry type.</returns>
		public abstract EntryType GetEntryType();
		
		/// <summary>
		/// Gets the Next ID. For Extension Entry a -2 is returned. 
		/// </summary>
		/// <returns>The next I.</returns>
		public abstract int GetNextID ();
		
		public DialogueDBEntry ()
		{
			ID = -1;
		}
	}

	public class ConversationDBEntry : DialogueDBEntry
	{
        /// <summary>
        /// The name of the Conversation.
        /// </summary>
		[Unique]
        public string ConversationName { get; private set; }

        /// <summary>
        /// The ID of the first Entry. Could be ExecutionDBEntry, ConditionDBEntry or ContentDBEntry. 
        /// </summary>
        public int FirstEntryID { get; private set; }
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Mistral.UniDialogue.ConversationDBEntry"/> class.
		/// </summary>
		/// <param name="cn">Cn.</param>
		/// <param name="nid">Nid.</param>
		public ConversationDBEntry (string cn, int nid) : base ()
		{
			ConversationName = cn;
			FirstEntryID = nid;
		}
		
        /// <summary>
        /// Initializes a new instance of the <see cref="Mistral.UniDialogue.ConversationDBEntry"/> class.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="cn">Cn.</param>
        /// <param name="nid">Nid.</param>
        public ConversationDBEntry (int id, string cn, int nid)
        {
            ID = id;
            ConversationName = cn;
            FirstEntryID = nid;
        }
		
		/// <summary>
		/// Default Constructor
		/// </summary>
		public ConversationDBEntry () : base()
		{
			
		}
		
        public override EntryType GetEntryType ()
        {
            return EntryType.Conversation;
        }
		
		/// <summary>
		/// For Conversation Entries, the id of the first content is returned. 
		/// </summary>
		/// <returns>The next I.</returns>
		public override int GetNextID ()
		{
			return FirstEntryID;
		}
	}

	public class ExecutionDBEntry : DialogueDBEntry
	{
        /// <summary>
        /// The YCode to be Executed. 
        /// </summary>
        /// <value>The execution code.</value>
        public string ExecutionCode { get; private set; }

        /// <summary>
        /// The ID of the Next Entry. Could be ExecutionDBEntry, ConditionDBEntry of ContentDBEntry. 
        /// </summary>
        /// <value>The next entry I.</value>
        public int NextEntryID { get; private set; }

		public ExecutionDBEntry (string ec, int nid) : base ()
		{
			ExecutionCode = ec;
			NextEntryID = nid;
		}
		
        /// <summary>
        /// Initializes a new instance of the <see cref="Mistral.UniDialogue.ExecutionDBEntry"/> class.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="ec">Ec.</param>
        /// <param name="nid">Nid.</param>
        public ExecutionDBEntry (int id, string ec, int nid)
        {
            ID = id;
			ExecutionCode = ec;
            NextEntryID = nid;
        }
		
		/// <summary>
		/// Default Constructor
		/// </summary>
		public ExecutionDBEntry () : base()
		{
			
		}
		
        public override EntryType GetEntryType ()
        {
            return EntryType.Execution;
        }
		
		public override int GetNextID()
		{
			return NextEntryID;
		}
	}

	public class ConditionDBEntry : DialogueDBEntry
	{
        /// <summary>
        /// The Condition of Execution this Entry. 
        /// </summary>
        /// <value>The condition code.</value>
        public string ConditionCode { get; private set; }

        /// <summary>
        /// The ID of the entry to enter when the condition is met. 
        /// </summary>
        public int SuccessID { get; private set; }

        /// <summary>
        /// If the condition is failed, move to the next ConditionDBEntry. 
        /// </summary>
        public int NextConditionID { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Mistral.UniDialogue.ConditionDBEntry"/> class.
		/// </summary>
		/// <param name="cc">Cc.</param>
		/// <param name="sid">Sid.</param>
		/// <param name="nid">Nid.</param>
		public ConditionDBEntry (string cc, int sid, int nid) : base ()
		{
			ConditionCode = cc;
			SuccessID = sid;
			NextConditionID = nid;
		}
		
        /// <summary>
        /// Initializes a new instance of the <see cref="Mistral.UniDialogue.ConditionDBEntry"/> class.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="cc">Cc.</param>
        /// <param name="sid">Sid.</param>
        /// <param name="nid">Nid.</param>
        public ConditionDBEntry (int id, string cc, int sid, int nid)
        {
            ID = id;
            ConditionCode = cc;
            SuccessID = sid;
            NextConditionID = nid;
        }
		
		/// <summary>
		/// Default Constructor
		/// </summary>
		public ConditionDBEntry () : base()
		{
			
		}
		
        public override EntryType GetEntryType ()
        {
            return EntryType.Condition;
        }
		
		public override int GetNextID ()
		{
			return NextConditionID;
		}
	}
	
	public class ExtensionDBEntry : DialogueDBEntry
	{
		/// <summary>
		/// The IDs of the next entries that are concatenated into a single string. 
		/// </summary>
		/// <value>The I ds.</value>
		public string IDs { get; private set; }
		
		public ExtensionDBEntry () : base ()
		{
			
		}
		
		public ExtensionDBEntry (string ids)
		{
			IDs = ids;
		}
		
		public ExtensionDBEntry (int id, string ids)
		{
			ID = id;
			IDs = ids;
		}
		
		public override EntryType GetEntryType ()
		{
			return EntryType.Extension;
		}
		
		/// <summary>
		/// An Extension Entry never has a next id! 
		/// </summary>
		/// <returns>The next I.</returns>
		public override int GetNextID ()
		{
			return -1;
		}
	}

    public class ContentDBEntry : DialogueDBEntry
    {
        /// <summary>
        /// The Actors who say the content. In the case of multiple actors, split the actors using '#'. 
        /// </summary>
        public string Actor { get; private set; }

        /// <summary>
        /// The content being told. 
        /// </summary>
        public string Content { get; private set; }

        /// <summary>
        /// The ID of the next Entry. Could be ExecutionDBEntry, ConditionDBEntry or ContentDBEntry. 
        /// </summary>
        public int NextEntryID { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Mistral.UniDialogue.ContentDBEntry"/> class.
		/// </summary>
		/// <param name="a">The alpha component.</param>
		/// <param name="c">C.</param>
		/// <param name="nid">Nid.</param>
		public ContentDBEntry (string a, string c, int nid) : base ()
		{
			Actor = a;
			Content = c;
			NextEntryID = nid;
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Mistral.UniDialogue.ContentDBEntry"/> class.
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <param name="a">The alpha component.</param>
		/// <param name="c">C.</param>
		/// <param name="nid">Nid.</param>
        public ContentDBEntry (int id, string a, string c, int nid)
        {
            ID = id;
            Actor = a;
            Content = c;
            NextEntryID = nid;
        }

		/// <summary>
		/// Default Constrcutor
		/// </summary>
		public ContentDBEntry () : base()
		{
			
		}
		
        public override EntryType GetEntryType ()
        {
            return EntryType.Content;
        }
		
		public override int GetNextID ()
		{
			return NextEntryID;
		}
    }

	/// <summary>
	/// The Administrator of UniDialogue. In charge of creating and deleting database. 
	/// Singleton! 
	/// </summary>
	public static class DialogueDBAdmin
	{
		#region Public Static Variables

		public static string streamingPath
		{
			///DEBUG: Compiler Options are now toggled. With these options exist the MonoDevelop can't function appropriately ... 
			get
			{
//#if UNITY_EDITOR
				return @"Assets/StreamingAssets/";
//#elif UNITY_ANDROID
//				return "jar:file://" + Application.dataPath + "!/assets/";
//#elif UNITY_IOS
//				return Application.dataPath + "/Raw/";
//#else
//				return Application.dataPath + "/StreamingAssets/";
//#endif
			}
		}

		#endregion

		#region Public Static Methods
		
		/// <summary>
		/// Delete the indicated database file. 
		/// </summary>
		/// <returns><c>true</c>, if database was deleted, <c>false</c> otherwise.</returns>
		/// <param name="_dbName">Db name.</param>
		public static bool DeleteDatabase (string _dbName)
		{
			if (File.Exists(streamingPath + _dbName))
			{
				File.Delete(streamingPath + _dbName);
				
				Debug.Log("The Indicated Database has been successfully Removed! ");
				
				return true;
			}
			else
			{
				Debug.Log("Deleting Database Failed! File not Found! ");
				
				return false;
			}
		}
		
		/// <summary>
		/// Creates a new empty database. Do not use the database immediately after this function! 
		/// </summary>
		/// <param name="_dbName">Db name.</param>
		public static bool CreateDatabase (string _dbName)
		{
			if (File.Exists(streamingPath + _dbName))
			{
				Debug.Log("Unable to Create the Database! A file with the same name has already been created! ");
				
				return false;
			}
			else
			{
				SQLiteConnection _connection = new SQLiteConnection(streamingPath + _dbName, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
				
				return true;
			}
		}
		
		/// <summary>
		/// Create a new UniDilaougeDatabase. 
		/// </summary>
		/// <param name="_dbName">Db name.</param>
		public static bool CreateUniDialogueDB (string _dbName)
		{
			if (!CreateDatabase(_dbName))
				return false;
			
			InitializeUniDialogueDB(_dbName);
			
			return true;
		}
		
		/// <summary>
		/// Initialize a database into a UniDialogue Database. 
		/// Warning: Won't Drop tables other than the ones used by UniDialogue. And meanwhile all the tables that UniDialogue uses
		/// will be dropped. This operation is not revorable. 
		/// </summary>
		/// <param name="_dbName">Db name.</param>
		public static bool InitializeUniDialogueDB (string _dbName)
		{
			if (File.Exists(streamingPath + _dbName))
			{
				SQLiteConnection _connection = new SQLiteConnection(streamingPath + _dbName, SQLiteOpenFlags.ReadWrite);
				
				///Drop Old Tables. 
				_connection.DropTable<ConversationDBEntry>();
				_connection.DropTable<ContentDBEntry>();
				_connection.DropTable<ConditionDBEntry>();
				_connection.DropTable<ExecutionDBEntry>();
				
				///Create New Tables
				_connection.CreateTable<ConversationDBEntry>();
				_connection.CreateTable<ContentDBEntry>();
				_connection.CreateTable<ConditionDBEntry>();
				_connection.CreateTable<ExecutionDBEntry>();
				
				return true;
			}
			else
			{
				Debug.Log("Unable to Initialized the database! The indicated file is not found! ");
				return false;
			}
		}
		
		/// <summary>
		/// Copy the Database to the indicated path. 
		/// </summary>
		/// <returns><c>true</c>, if up database was backed, <c>false</c> otherwise.</returns>
		/// <param name="_originDB">Origin D.</param>
		/// <param name="_targetPath">Target path.</param>
		public static bool BackUpDatabase (string _originDB, string _targetPath)
		{
			if (!File.Exists(streamingPath + _originDB))
			{
				Debug.Log("The dabatase to backup does not exist. ");
				return false;
			}
			
			if (File.Exists(_targetPath + _originDB))
			{
				File.Delete(_targetPath + _originDB);
			}
			
			File.Copy(streamingPath + _originDB, _targetPath + _originDB);
			
			return true;
		}
		
		#endregion
	}

	/// <summary>
	/// The Dialogue Database Manager. Used Based on a Connection. 
	/// </summary>
	public class DialogueDBManager
	{
		#region Public Variables

		/// <summary>
		/// The Connection to the UniDialogueDB. 
		/// </summary>
		public DialogueDBConnection _connection;

		#endregion
		
		#region Public Attributes
		
		public int NextContentID
		{
			get
			{
				return nextContentID;
			}
			
			private set
			{
				nextContentID = value;
			}
		}
		
		public int NextExecutionID
		{
			get
			{
				return nextExecutionID;
			}
			
			private set
			{
				nextExecutionID = value;
			}
		}
		
		public int NextConditionID
		{
			get
			{
				return nextConditionID;
			}
			
			private set
			{
				nextConditionID = value;
			}
		}
		
		public int NextConversationID
		{
			get
			{
				return nextConversationID;
			}
			
			private set
			{
				nextConversationID = value;
			}
		}
		
		public int NextExtensionID
		{
			get
			{
				return nextExtensionID;
			}

			private set
			{
				nextExtensionID = value;
			}
		}
		
		#endregion
		
		#region Private Variables
		
		private int nextContentID;
		
		private int nextExecutionID;
		
		private int nextConditionID;
		
		private int nextConversationID;
		
		private int nextExtensionID;
		
		#endregion 
		
		#region Constructors
		
		public DialogueDBManager ()
		{
			
		}
		
		public DialogueDBManager (string dbName, SQLiteOpenFlags flag)
		{
			_connection = new DialogueDBConnection(dbName, flag);
			Update();
		}
		
		public DialogueDBManager (DialogueDBConnection con)
		{
			_connection = con;
			
			Update();
		}
		
		#endregion
		
		#region Insert Methods
		
		/// <summary>
		/// Inserts an entry into the database. 
		/// This Method is For Test Only! 
		/// </summary>
		/// <param name="entry">Entry.</param>
		public void InsertEntry (DialogueDBEntry entry)
		{
			EntryType eType = entry.GetEntryType();
			
			int newID;
			
			switch (eType)
			{
				case EntryType.Conversation:
					newID = NextConversationID;
					break;
					
				case EntryType.Condition:
					newID = NextConditionID;
					break;
					
				case EntryType.Content:
					newID = NextContentID;
					break;
					
				case EntryType.Execution:
					newID = NextExecutionID;
					break;
				case EntryType.Extension:
					newID = NextExtensionID;
					break;
				default:
					newID = 0;
					break;
			}
			
			//entry.ID = newID;
			
			try
			{
				_connection.Insert(entry);
			}
			catch (SQLiteException sex)
			{
				Debug.Log("Entry is not inserted. An error has occured. Check the constraints of the database scheme. "
					+ "The Result is: " + sex.Result
					+ "The Message is: " + sex.Message);
				throw;
			}
			
			switch (eType)
			{
				case EntryType.Conversation:
					NextConversationID++;
					break;
					
				case EntryType.Condition:
					NextConditionID += 10;
					break;
					
				case EntryType.Content:
					NextContentID += 10;
					break;
					
				case EntryType.Execution:
					NextExecutionID += 10;
					break;
				case EntryType.Extension:
					NextExtensionID += 10;
					break;
				default:
					break;
			}
		}
		
		//These Methods are not welcomed to use ... However in test mode or some situations they could be pretty handy ...
		//Recommended Replacement : Safe insert. Just to make the codes more beautiful. 
		
		public void InsertConversationDBEntry (string cname, int startID)
		{
			ConversationDBEntry toInsert = new ConversationDBEntry(NextConversationID, cname, startID);
			try
			{
				_connection.Insert(toInsert);
			}
			catch (SQLiteException sex)
			{
				Debug.Log("Entry is not inserted. An error has occured. Check the constraints of the database scheme. "
					+ "The Result is: " + sex.Result
					+ "The Message is: " + sex.Message);
				throw;
			}
			NextConversationID++;
		}
		
		public void InsertContentDBEntry (string aname, string cname, int nextID)
		{
			ContentDBEntry toInsert = new ContentDBEntry(NextContentID, aname, cname, nextID);
			try
			{
				_connection.Insert(toInsert);
			}
			catch (SQLiteException sex)
			{
				Debug.Log("Entry is not inserted. An error has occured. Check the constraints of the database scheme. "
					+ "The Result is: " + sex.Result
					+ "The Message is: " + sex.Message);
				throw;
			}
			NextContentID += 10;
		}
		
		public void InsertExecutionDBEntry (string ycode, int nextID)
		{
			ExecutionDBEntry toInsert = new ExecutionDBEntry(NextExecutionID, ycode, nextID);
			try
			{
				_connection.Insert(toInsert);
			}
			catch (SQLiteException sex)
			{
				Debug.Log("Entry is not inserted. An error has occured. Check the constraints of the database scheme. "
					+ "The Result is: " + sex.Result
					+ "The Message is: " + sex.Message);
				throw;
			}
			NextExecutionID += 10;
		}
		
		public void InsertConditionDBEntry (string ycode, int sucID, int nextID)
		{
			ConditionDBEntry toInsert = new ConditionDBEntry(NextConditionID, ycode, sucID, nextID);
			try
			{
				_connection.Insert(toInsert);
			}
			catch (SQLiteException sex)
			{
				Debug.Log("Entry is not inserted. An error has occured. Check the constraints of the database scheme. "
					+ "The Result is: " + sex.Result
					+ "The Message is: " + sex.Message);
				throw;
			}
			NextConditionID += 10;
		}
		
		public void InsertExtensionDBEntry (string ids)
		{
			ExtensionDBEntry toInsert = new ExtensionDBEntry(NextExtensionID, ids);
			try
			{
				_connection.Insert(toInsert);
			}
			catch (SQLiteException sex)
			{
				Debug.Log("Entry is not inserted. An error has occured. Check the constraints of the database scheme. "
					+ "The Result is: " + sex.Result
					+ "The Message is: " + sex.Message);
				throw;
			}
			NextExtensionID += 10;
		}
		
		//These Methods are not welcomed to use ... However in test mode or some situations they could be pretty handy ...
		
		#endregion 
		
		#region Public Methods
		
		/// <summary>
		/// Updates the Manager's Varirables
		/// </summary>
		public void Update ()
		{
			///Get the Rows with maximum ID. 
			ConversationDBEntry _maxConversationDBEntry = _connection.Query<ConversationDBEntry>("SELECT *, MAX(ID) FROM ConversationDBEntry")[0];
			ContentDBEntry _maxContentDBEntry = _connection.Query<ContentDBEntry>("SELECT *, MAX(ID) FROM ContentDBEntry")[0];
			ExecutionDBEntry _maxExecutionDBEntry = _connection.Query<ExecutionDBEntry>("SELECT *, MAX(ID) FROM ExecutionDBEntry")[0];
			ConditionDBEntry _maxConditionDBEntry = _connection.Query<ConditionDBEntry>("SELECT *, MAX(ID) FROM ConditionDBEntry")[0];
			ExtensionDBEntry _maxExtensionDBEntry = _connection.Query<ExtensionDBEntry>("SELECT *, MAX(ID) FROM ExtensionDBEntry")[0];

			///And then set the IDs to the Manager. If a table is empty, then set the start ID. 
			if (_maxConversationDBEntry.ID != 0)
				NextConversationID = _maxConversationDBEntry.ID + 1;
			else
				NextConversationID = 1;

			if (_maxContentDBEntry.ID != 0)
				NextContentID = _maxContentDBEntry.ID + 10;
			else
				NextContentID = 1;

			if (_maxExecutionDBEntry.ID != 0)
				NextExecutionID = _maxExecutionDBEntry.ID + 10;
			else
				NextExecutionID = 2;

			if (_maxConditionDBEntry.ID != 0)
				NextConditionID = _maxConditionDBEntry.ID + 10;
			else
				NextConditionID = 3;

			if (_maxExtensionDBEntry.ID != 0)
				NextExtensionID = _maxExtensionDBEntry.ID + 10;
			else
				NextExtensionID = 4;
		}
		
		public void SwitchConnection (DialogueDBConnection con)
		{
			_connection.Disconnect();
			_connection = con;
		}
		
		public void SwitchConnection (string dbName, SQLiteOpenFlags flag)
		{
			_connection.Disconnect();
			_connection.EstablishConnection(dbName, flag);
		}
		
		/// <summary>
		/// Get the DialogueEntryByID. 
		/// </summary>
		/// <returns>The entry by I.</returns>
		/// <param name="id">Identifier.</param>
		public DialogueDBEntry GetEntryByID (int id)
		{
			EntryType type = (EntryType) (id % 10);

			DialogueDBEntry ret;

			switch (type)
			{
				case EntryType.Content: 
					ret = _connection.Query<ContentDBEntry>("SELECT * FROM ContentDBEntry WHERE ID = " + id)[0];
					break;
				case EntryType.Condition:
					ret = _connection.Query<ConditionDBEntry>("SELECT * FROM ConditionDBEntry WHERE ID = " + id)[0];
					break;
				case EntryType.Execution:
					ret = _connection.Query<ExecutionDBEntry>("SELECT * FROM ExecutionDBEntry WHERE ID = " + id)[0];
					break;
				case EntryType.Extension:
					ret = _connection.Query<ExtensionDBEntry>("SELECT * FROM ExtensionDBEntry WHERE ID = " + id)[0];
					break;
				default :
					ret = null;
					break;
			}

			return ret;
		}

		/// <summary>
		/// Returns the Next DialogueDBEntry of the Selected Entry.
		/// If the entry is an Extension Entry, or it is the end of the entry chain, a null would be returned. 
		/// And the corresponding warning message would be displayed. 
		/// </summary>
		/// <returns>The next entry.</returns>
		/// <param name="entry">Entry.</param>
		public DialogueDBEntry GetNextEntry (DialogueDBEntry entry)
		{
			int id = entry.GetNextID();
			if (id == -1)
			{
				if (entry.GetEntryType() == EntryType.Extension)
				{
					Debug.Log("Warning - You are trying to get the Next Entry of an extension entry! ");
					return null;
				}
				else
				{
					Debug.Log("This Entry does not have a next ID. ");
					return null;
				}
			}
			return GetEntryByID (id);
		}
		
		public List<int> GetExtensionList (ExtensionDBEntry entry)
		{
			List<int> ret = new List<int>();
			
			string[] strs = entry.IDs.Split('#');
			
			foreach (string str in strs)
			{
				ret.Add(int.Parse (str));
			}
			
			return ret;
		}
		
		#endregion
	}

	/// <summary>
	/// The Connection to a UniDialogueDatabase. 
	/// Wraps the SQLiteConnection to make sure that the low-level API is not exposed. 
	/// Always be ready to change the model level. 
	/// </summary>
	public class DialogueDBConnection
	{
		#region Public Variables


		#endregion
		
		#region Private Variables
		
		/// <summary>
		/// The Connection to the SQLite DB. 
		/// </summary>
		private static SQLiteConnection _connection = null;
		
		#endregion
		
		#region Constructors
		
		public DialogueDBConnection ()
		{
			_connection = null;
		}
		
		public DialogueDBConnection (string _dbName, SQLiteOpenFlags authentication)
		{
			EstablishConnection(_dbName, authentication);
		}
		
		#endregion
		
		#region Connection Methods
		
		/// <summary>
		/// Establishes a new connection to the indicated database. 
		/// </summary>
		/// <param name="_dbName">Db name.</param>
		/// <param name="authentication">Authentication.</param>
		public bool EstablishConnection (string _dbName, SQLiteOpenFlags authentication)
		{
			_connection = null;
			
			if (!File.Exists(DialogueDBAdmin.streamingPath + _dbName))
			{
				Debug.Log("Failed to Establish the Connection! The indicated Database does not exist! ");
				return false;
			}
			
			_connection = new SQLiteConnection(DialogueDBAdmin.streamingPath + _dbName, authentication);
			
			Debug.Log("The Connection has been established successfully! ");
			
			return true;
		}
		
		/// <summary>
		/// Disconnects from the current database. 
		/// </summary>
		public void Disconnect ()
		{
			_connection.Close ();
			GC.Collect();
			Debug.Log("Disconnected! ");
		}
		
		#endregion
		
		#region SQL Methods
		
		public List<T> Query<T> (string query, params object[] args) where T : new() 
		{
			return _connection.Query<T> (query, args);
		}
		
		/// <summary>
		/// Safe Insert will never cause an interruption. Mostly used during runtime.
		/// </summary>
		/// <returns>The insert.</returns>
		/// <param name="obj">Object.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public int SafeInsert (object obj)
		{
			try
			{
				int x = _connection.Insert(obj);
				return x;
			}
			catch (SQLiteException sex)
			{
				Debug.Log("An Error is Occured While Trying to Insert Data. However in Safe Mode the Error is Ignored. "
					+ "The result is: " + sex.Result + "The Message is: " + sex.Message
				);
				return 0;
			}
		}
		
		/// <summary>
		/// The same with SQLite4Unity Insert Function. 
		/// Possibly Trigger an error and stops a MonoBehaviour. 
		/// </summary>
		/// <param name="obj">Object.</param>
		public int Insert (object obj)
		{
			return _connection.Insert(obj);
		}
		
		#endregion
	}
}
