using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GX;

public class TableTest : MonoBehaviour
{
	IEnumerator Start()
	{
		yield return this.StartCoroutine(ArchiveManager.LoadTextAsync("Table/WorldLevelBase.pbt", text =>
		{
			Debug.Log(text);
		}));
	}
}

