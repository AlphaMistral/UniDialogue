using UnityEngine;
using System.Collections;
using Mistral.UniDialogue;
using System.Collections.Generic;

public class TestScanner : MonoBehaviour 
{
	public string testString;
	public List<YCodeNode> nodes;
	
	private void Start ()
	{
		StartCoroutine(Safe ());
	}
	
	private IEnumerator Safe ()
	{
		nodes = YCodeScanner.AnalyzeSentence(testString);

		foreach (YCodeNode node in nodes)
		{
			Debug.Log(node);
		}
		
		yield return null;
	}
}
