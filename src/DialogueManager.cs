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
	/// DialogueEntry
	/// </summary>
	public class ContentEntry
	{
		/// <summary>
		/// The actors of this entry of content.
		/// Please note that if more than one actor present,
		/// the string should be splitted by '#'. 
		/// </summary>
		public string Actors{ get; private set; }
		
		/// <summary>
		/// The Content of the Dialogue.
		/// </summary>
		public string Content { get; private set; }
		
		public int ID { get; private set; }
		
		public int NextID { get; private set; }
		
		public ContentEntry (int id, int nid, string a, string c)
		{
			ID = id;
			NextID = nid;
			Actors = a;
			Content = c;
		}
	}
	
	/// <summary>
	/// A READ-ONLY Dialogue Manager. Singleton! 
	/// Currently, Condition Nodes and Execution Nodes are IGNORED. 
	/// </summary>
	public class DialogueManager : MonoBehaviour
	{
		#region Singleton
		
		/// <summary>
		/// Singleton of the Dialogue Manager. 
		/// </summary>
		public static DialogueManager instance;
		
		#endregion
		
		///These Variables are for the DialogueManger Rather than the instance. 
		#region Static Variables
		
		private static DialogueDBManager dbManager;
		
		private static ConversationDBEntry currentConversation;
		
		private static DialogueDBEntry currentEntry;
		
		#endregion
		
		#region Constants
		
		
		
		#endregion
		
		#region Public Variables
		
		/// <summary>
		/// The name of the Database to use.
		/// Required! 
		/// </summary>
		public string dbName;
		
		#endregion
		
		#region MonoBehaviours
		
		/// <summary>
		/// For Insuring Singleton and Initializing. 
		/// </summary>
		private void Awake ()
		{
			if (instance == null)
			{
				instance = this;
			}
			else if (instance != this)
			{
				DestroyImmediate(this);
			}
		}
		
		#endregion
		
		#region Public Methods
		
		/// <summary>
		/// Establish a new Connection to an Indicated Database. 
		/// Please don't do this frequently! 
		/// A GC is forced to make sure that the database connection is released! 
		/// </summary>
		/// <param name="_dbName">Db name.</param>
		public static void EstablishNewConnection (string _dbName)
		{
			instance.dbName = _dbName;
			dbManager.SwitchConnection(_dbName, SQLiteOpenFlags.ReadOnly);
			currentEntry = null;
		}
		
		/// <summary>
		/// Invokes a new Conversation By the Indicated ID. 
		/// Not Frequently used. Unless you know exactly the ID of the conversation. 
		/// </summary>
		/// <param name="id">Identifier.</param>
		public static void InvokeNewConversation (int id)
		{
			List<ConversationDBEntry> cdbe = dbManager._connection.Query<ConversationDBEntry>("SELECT * FROM ConversationDBEntry WHERE ID = " + id);
			if (cdbe == null || cdbe.Count == 0)
			{
				currentConversation = null;
				Debug.Log("Warning : The indicated ID does not exist at all! ");
			}
			currentConversation = cdbe[0];
			currentEntry = null;
		}
		
		/// <summary>
		/// Invokes a new Conversation By the Name. 
		/// </summary>
		/// <param name="name">Name.</param>
		public static void InvokeNewConversation (string name)
		{
			List<ConversationDBEntry> cdbe = dbManager._connection.Query<ConversationDBEntry>("SELECT * FROM ConversationDBEntry WHERE ConversationName = " + name);
			if (cdbe == null || cdbe.Count == 0)
			{
				currentConversation = null;
				Debug.Log("Warning : The indicated Name does not exist at all! ");
			}
			currentConversation = cdbe[0];
			currentEntry = null;
		}
		
		public static ContentEntry GetCurrentContent ()
		{
			if (currentEntry == null || currentEntry.GetEntryType() != EntryType.Content)
				return null;
			
			return EncodeContentEntry(currentEntry as ContentDBEntry);
		}
		
		public static List<ContentEntry> GetAllContents ()
		{
			if (currentEntry == null)
				return null;
			List<ContentEntry> ret = new List<ContentEntry>();
			
			if (currentEntry.GetEntryType() == EntryType.Content)
			{
				ret.Add(GetCurrentContent());
			}
			else if (currentEntry.GetEntryType() == EntryType.Extension)
			{
				List<int> ids = dbManager.GetExtensionList(currentEntry as ExtensionDBEntry);
				///Please note currently we ignore condition entries and execution entries! 
				for (int i = 0, imax = ids.Count; i < imax; i++)
				{
					int id = ids[i];
					ContentDBEntry dbEntry = dbManager.GetEntryByID(id) as ContentDBEntry;
					ret.Add(EncodeContentEntry(dbEntry));
				}
			}
			
			return ret;
		}
		
		public static void MoveToNextEntry (ContentEntry entry)
		{
			int id = entry.NextID;
			
			if (id == -1)
				currentEntry = null;
			else
				currentEntry = dbManager.GetEntryByID(entry.NextID);
		}
		
		#endregion
		
		#region Private Methods
		
		private static void Initialize ()
		{
			currentEntry = null;
			dbManager = new DialogueDBManager(instance.dbName, SQLiteOpenFlags.ReadOnly);
		}
		
		private static ContentEntry EncodeContentEntry (ContentDBEntry entry)
		{
			return new ContentEntry(entry.ID, entry.NextEntryID, entry.Actor, entry.Content);
		}
		
		#endregion
	}
}
