using UnityEngine;
using System.Collections;

public class ToggleAutoSetGroup : MonoBehaviour
{
	public static int currentToggleValue = 0;

	void Awake()
	{
		SetToggleGroupData();
	}
	/// <summary>
	/// 当前界面已有的Toggle按钮动态设置Group值
	/// </summary>
	public void SetToggleGroupData()
	{
		GameObject oldParent = null;
		GameObject currentParent = null;
		bool ifSetTableValue = false;
		foreach (Transform temp in transform.GetComponentsInChildren<Transform>())
		{
			if (temp.transform.GetComponent<UIToggle>())
			{
				if (oldParent != null)
				{
					currentParent = temp.transform.parent.gameObject;
					if(currentParent == oldParent)
					{
						temp.transform.GetComponent<UIToggle>().group = currentToggleValue;
						if(temp.transform.GetComponent<UITabLabelColor>())
						{
							ifSetTableValue = true;
							var value = currentToggleValue + 1;
							temp.transform.GetComponent<UITabLabelColor>().group = value;
						}
					}
					else
					{
						if(ifSetTableValue)
						{
							currentToggleValue++;
							ifSetTableValue = false;
						}
						oldParent = temp.transform.parent.gameObject;
						currentToggleValue++;
						temp.transform.GetComponent<UIToggle>().group = currentToggleValue;
						if (temp.transform.GetComponent<UITabLabelColor>())
						{
							var tableValue = currentToggleValue + 1;
							temp.transform.GetComponent<UITabLabelColor>().group = tableValue;
						}
					}
					continue;
				}
				currentToggleValue++;
				oldParent = temp.transform.parent.gameObject;
				temp.transform.GetComponent<UIToggle>().group = currentToggleValue;
				if (temp.transform.GetComponent<UITabLabelColor>())
				{
					var tableValue = currentToggleValue + 1;
					temp.transform.GetComponent<UITabLabelColor>().group = tableValue;
				}
				
			}
		}
	}
	/// <summary>
	/// 动态生成的Toggle按钮设置Group值
	/// </summary>
	/// <returns></returns>
	public static int getCurrentToggleGroup()
	{
		currentToggleValue++;
		var value = currentToggleValue;
		return value;
	}
}
