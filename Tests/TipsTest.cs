using UnityEngine;
using System.Collections;

public class TipsTest : MonoBehaviour 
{
	private GameObject uiroot;
	/// <summary>
	/// 漂浮文字预制
	/// </summary>
	public GameObject prefab_Tips;
	void Start()
	{
		uiroot = GameObject.Find("UI Root");
	}
	// Update is called once per frame
	void OnGUI () 
	{
		if (GUI.Button(new Rect(10, 40, 200, 40), "颜色渐变提示"))
		{
			GameObject go = Instantiate(prefab_Tips) as GameObject;
			//go.gameObject.transform.parent = uiroot.transform;
			//go.transform.localScale = Vector3.one*0.005f;
			go.transform.localPosition = Vector3.zero;
			//颜色自动渐变
			go.GetComponent<TipsFadeAcross>().showTips("这是一个文字漂浮测试!");
		}
		if (GUI.Button(new Rect(10, 100, 200, 40), "自定义颜色提示"))
		{
			GameObject go = Instantiate(prefab_Tips) as GameObject;
			go.gameObject.transform.parent = uiroot.transform;
			go.transform.localScale = Vector3.one;
			go.transform.localPosition = Vector3.zero;
			go.GetComponent<TipsFadeAcross>().showTips("这是一个自定义颜色文字漂浮测试", Color.green, true,20);
		}
		if (GUI.Button(new Rect(10, 160, 200, 40), "自定义起点提示"))
		{
			GameObject go = Instantiate(prefab_Tips) as GameObject;
			go.gameObject.transform.parent = uiroot.transform;
			go.transform.localScale = Vector3.one;
			go.transform.localPosition = Vector3.zero;
			//使用自定义颜色(上下左右的UI坐标位置) 
			// (-1,1)	(0,1)		(1,1)
			// (-1,0.5)	(0,0.5)		(1,1)
			// (-1,-1)	(0,-1)		(1,1)
			go.GetComponent<TipsFadeAcross>().showTips("这是一个自定义起点位置漂浮测试", Color.red, true, 20, new Vector3(0, -1, 0));
		}
		if (GUI.Button(new Rect(10, 220, 200, 40), "自定义运动速度提示"))
		{
			GameObject go = Instantiate(prefab_Tips) as GameObject;
			go.gameObject.transform.parent = uiroot.transform;
			go.transform.localScale = Vector3.one;
			go.transform.localPosition = Vector3.zero;
			go.GetComponent<TipsFadeAcross>().showTips("这是一个自定义运动速度文字漂浮测试", Color.red, true, 20, new Vector3(0, -1, 0),4);
		}
	}
}
