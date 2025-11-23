using UnityEngine;
using UnityEngine.UI;

public class XiQuInfoPanel : MonoBehaviour
{
	private void Start()
	{
		InitSize();
	}

	private void OnEnable()
	{
		OnEnableShow();
	}

	private void OnEnableShow()
	{
		base.transform.Find("Name").GetComponent<Text>().text = AllText.Text_AllXiQu[Mainload.XiQuID_Enter][Mainload.SetData[4]].Split('|')[0];
		base.transform.Find("Info").GetComponent<Text>().text = AllText.Text_AllXiQu[Mainload.XiQuID_Enter][Mainload.SetData[4]].Split('|')[1];
	}

	private void InitSize()
	{
		if (Mainload.SetData[4] == 0)
		{
			base.transform.Find("Info").GetComponent<Text>().fontSize = 18;
		}
		else
		{
			base.transform.Find("Info").GetComponent<Text>().fontSize = 16;
		}
	}
}
