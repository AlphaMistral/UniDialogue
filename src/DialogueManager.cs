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
		#region Public Variables
		
		/// <summary>
		/// Singleton of the Dialogue Manager. 
		/// </summary>
		public static DialogueManager instance;
		
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
	}
}
