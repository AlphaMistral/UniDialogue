﻿//
// Copyright (c) 20015-2017 Jingping Yu.
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
        Conversation,
        Execution,
        Condition,
        Content
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
        /// <value>The I.</value>
		[PrimaryKey]
		public int ID { get; set; }

        /// <summary>
        /// Gets the type of the entry.
        /// </summary>
        /// <returns>The entry type.</returns>
        public abstract EntryType GetEntryType ();
	}

	public class ConversationEntry : DialogueDBEntry
	{
        /// <summary>
        /// The name of the Conversation.
        /// </summary>
        public string ConversationName { get; set; }

        /// <summary>
        /// The ID of the first Entry. Could be ExecutionEntry, ConditionEntry or ContentEntry. 
        /// </summary>
        public int FirstEntryID { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Mistral.UniDialogue.ConversationEntry"/> class.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="cn">Cn.</param>
        /// <param name="nid">Nid.</param>
        public ConversationEntry (int id, string cn, int nid)
        {
            ID = id;
            ConversationName = cn;
            FirstEntryID = nid;
        }

        public override EntryType GetEntryType ()
        {
            return EntryType.Conversation;
        }
	}

	public class ExecutionEntry : DialogueDBEntry
	{
        /// <summary>
        /// The YCode to be Executed. 
        /// </summary>
        /// <value>The execution code.</value>
        public string ExecutionCode { get; set; }

        /// <summary>
        /// The ID of the Next Entry. Could be ExecutionEntry, ConditionEntry of ContentEntry. 
        /// </summary>
        /// <value>The next entry I.</value>
        public int NextEntryID { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Mistral.UniDialogue.ExecutionEntry"/> class.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="ec">Ec.</param>
        /// <param name="nid">Nid.</param>
        public ExecutionEntry (int id, string ec, int nid)
        {
            ID = id;
			ExecutionCode = ec;
            NextEntryID = nid;
        }

        public override EntryType GetEntryType ()
        {
            return EntryType.Execution;
        }
	}

	public class ConditionEntry : DialogueDBEntry
	{
        /// <summary>
        /// The Condition of Execution this Entry. 
        /// </summary>
        /// <value>The condition code.</value>
        public string ConditionCode { get; set; }

        /// <summary>
        /// The ID of the entry to enter when the condition is met. 
        /// </summary>
        public int SuccessID { get; set; }

        /// <summary>
        /// If the condition is failed, move to the next ConditionEntry. 
        /// </summary>
        public int NextConditionID { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Mistral.UniDialogue.ConditionEntry"/> class.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="cc">Cc.</param>
        /// <param name="sid">Sid.</param>
        /// <param name="nid">Nid.</param>
        public ConditionEntry (int id, string cc, int sid, int nid)
        {
            ID = id;
            ConditionCode = cc;
            SuccessID = sid;
            NextConditionID = nid;
        }

        public override EntryType GetEntryType ()
        {
            return EntryType.Condition;
        }
	}

    public class ContentEntry : DialogueDBEntry
    {
        /// <summary>
        /// The Actors who say the content. In the case of multiple actors, split the actors using '#'. 
        /// </summary>
        public string Actor { get; set; }

        /// <summary>
        /// The content being told. 
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// The ID of the next Entry. Could be ExecutionEntry, COnditionEntry or ContentEntry. 
        /// </summary>
        public int NextEntryID { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Mistral.UniDialogue.ContentEntry"/> class.
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <param name="a">The alpha component.</param>
		/// <param name="c">C.</param>
		/// <param name="nid">Nid.</param>
        public ContentEntry (int id, string a, string c, int nid)
        {
            ID = id;
            Actor = a;
            Content = c;
            NextEntryID = nid;
        }

        public override EntryType GetEntryType ()
        {
            return EntryType.Content;
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
		/// Initialize an empty database into a UniDialogue Database. 
		/// </summary>
		/// <param name="_dbName">Db name.</param>
		public static bool InitializeUniDialogueDB (string _dbName)
		{
			if (File.Exists(streamingPath + _dbName))
			{
				SQLiteConnection _connection = new SQLiteConnection(streamingPath + _dbName, SQLiteOpenFlags.ReadWrite);
				
				///Drop Old Tables. 
				_connection.DropTable<ConversationEntry>();
				_connection.DropTable<ContentEntry>();
				_connection.DropTable<ConditionEntry>();
				_connection.DropTable<ExecutionEntry>();
				
				///Create New Tables
				_connection.CreateTable<ConversationEntry>();
				_connection.CreateTable<ContentEntry>();
				_connection.CreateTable<ConditionEntry>();
				_connection.CreateTable<ExecutionEntry>();
				
				return true;
			}
			else
			{
				Debug.Log("Unable to Initialized the database! The indicated file is not found! ");
				return false;
			}
		}

		#endregion
	}

	/// <summary>
	/// The Dialogue Database Manager. Used Upon a Connection. 
	/// </summary>
	public class DialogueDBManager
	{
		#region Public Variables



		#endregion
	}

	/// <summary>
	/// The Connection to a UniDialogueDatabase. 
	/// </summary>
	public class DialogueDBConnection
	{
		#region Public Variables


		#endregion
	}
}
