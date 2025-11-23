using UnityEngine;
using UnityEngine.UI;

public class PropInfoPanel : MonoBehaviour
{
	private void Start()
	{
		initSize();
		initShow();
	}

	private void OnEnable()
	{
		OnEnableData();
	}

	private void Update()
	{
		UpdateData();
	}

	private void initSize()
	{
		if (Mainload.SetData[4] == 0)
		{
			base.transform.Find("InfoShow").Find("Prices").Find("Tip")
				.GetComponent<Text>()
				.fontSize = 18;
			base.transform.Find("InfoShow").Find("Prices").Find("Num")
				.GetComponent<Text>()
				.fontSize = 18;
			base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
				.fontSize = 18;
			base.transform.Find("InfoShow").Find("Name").GetComponent<Text>()
				.fontSize = 20;
		}
		else
		{
			base.transform.Find("InfoShow").Find("Prices").Find("Tip")
				.GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("InfoShow").Find("Prices").Find("Num")
				.GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("InfoShow").Find("Name").GetComponent<Text>()
				.fontSize = 16;
		}
	}

	private void initShow()
	{
		base.transform.Find("InfoShow").Find("Prices").Find("Tip")
			.GetComponent<Text>()
			.text = AllText.Text_UIA[280][Mainload.SetData[4]];
	}

	private void OnEnableData()
	{
		Mainload.PropData_Enter = "null";
	}

	private void UpdateData()
	{
		if (Mainload.PropData_Enter != "null")
		{
			if (!base.transform.Find("InfoShow").gameObject.activeSelf)
			{
				base.transform.Find("InfoShow").gameObject.SetActive(value: true);
			}
			if (base.transform.Find("InfoShow").Find("Name").GetComponent<Text>()
				.text != AllText.Text_AllProp[int.Parse(Mainload.PropData_Enter.Split('|')[0])][Mainload.SetData[4]])
			{
				base.transform.Find("InfoShow").Find("Name").GetComponent<Text>()
					.text = AllText.Text_AllProp[int.Parse(Mainload.PropData_Enter.Split('|')[0])][Mainload.SetData[4]];
				string[] array = Mainload.AllPropdata[int.Parse(Mainload.PropData_Enter.Split('|')[0])][2].Split('|');
				base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
					.text = "";
				if (int.Parse(array[0]) > 0)
				{
					base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
						.text = AllText.Text_UIA[1123][Mainload.SetData[4]].Replace("@", array[0]);
				}
				if (int.Parse(array[1]) > 0)
				{
					base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
						.text = base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
						.text + AllText.Text_UIA[1124][Mainload.SetData[4]].Replace("@", array[1]);
				}
				if (int.Parse(array[2]) > 0)
				{
					base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
						.text = base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
						.text + AllText.Text_UIA[1125][Mainload.SetData[4]].Replace("@", array[2]);
				}
				if (int.Parse(array[3]) > 0)
				{
					base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
						.text = base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
						.text + AllText.Text_UIA[1126][Mainload.SetData[4]].Replace("@", array[3]);
				}
				if (int.Parse(array[4]) < 0)
				{
					base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
						.text = base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
						.text + AllText.Text_UIA[1127][Mainload.SetData[4]].Replace("@", array[4]);
				}
				else if (int.Parse(array[4]) > 0)
				{
					base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
						.text = base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
						.text + AllText.Text_UIA[1127][Mainload.SetData[4]].Replace("@", "+" + array[4]);
				}
				if (int.Parse(array[5]) < 0)
				{
					base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
						.text = base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
						.text + AllText.Text_UIA[1128][Mainload.SetData[4]].Replace("@", array[5]);
				}
				else if (int.Parse(array[5]) > 0)
				{
					base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
						.text = base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
						.text + AllText.Text_UIA[1128][Mainload.SetData[4]].Replace("@", "+" + array[5]);
				}
				if (int.Parse(array[6]) < 0)
				{
					base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
						.text = base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
						.text + AllText.Text_UIA[1129][Mainload.SetData[4]].Replace("@", array[6]);
				}
				else if (int.Parse(array[6]) > 0)
				{
					base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
						.text = base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
						.text + AllText.Text_UIA[1129][Mainload.SetData[4]].Replace("@", "+" + array[6]);
				}
				if (int.Parse(array[7]) < 0)
				{
					base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
						.text = base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
						.text + AllText.Text_UIA[1130][Mainload.SetData[4]].Replace("@", array[7]);
				}
				else if (int.Parse(array[7]) > 0)
				{
					base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
						.text = base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
						.text + AllText.Text_UIA[1130][Mainload.SetData[4]].Replace("@", "+" + array[7]);
				}
				if (int.Parse(array[8]) > 0)
				{
					base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
						.text = base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
						.text + AllText.Text_UIA[1131][Mainload.SetData[4]] + "\n";
				}
				if (int.Parse(array[9]) > 0)
				{
					base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
						.text = base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
						.text + AllText.Text_UIA[1075][Mainload.SetData[4]].Replace("@", "+" + array[9]) + "\n";
				}
				if (int.Parse(array[10]) > 0)
				{
					base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
						.text = base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
						.text + AllText.Text_AllMemberSkill[1][Mainload.SetData[4]] + ": +" + array[10] + "\n";
				}
				if (int.Parse(array[11]) > 0)
				{
					base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
						.text = base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
						.text + AllText.Text_AllMemberSkill[2][Mainload.SetData[4]] + ": +" + array[11] + "\n";
				}
				if (int.Parse(array[12]) > 0)
				{
					base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
						.text = base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
						.text + AllText.Text_AllMemberSkill[3][Mainload.SetData[4]] + ": +" + array[12] + "\n";
				}
				if (int.Parse(array[13]) > 0)
				{
					base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
						.text = base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
						.text + AllText.Text_AllMemberSkill[4][Mainload.SetData[4]] + ": +" + array[13] + "\n";
				}
				if (int.Parse(array[14]) > 0)
				{
					base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
						.text = base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
						.text + AllText.Text_AllMemberSkill[5][Mainload.SetData[4]] + ": +" + array[14] + "\n";
				}
				if (int.Parse(array[15]) > 0)
				{
					base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
						.text = base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
						.text + AllText.Text_AllMemberSkill[6][Mainload.SetData[4]] + ": +" + array[15] + "\n";
				}
				if (Mainload.PropData_Enter.Split('|')[0] == "75")
				{
					base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
						.text = base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
						.text + AllText.Text_UIA[1258][Mainload.SetData[4]] + "\n";
				}
				else if (Mainload.PropData_Enter.Split('|')[0] == "282")
				{
					base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
						.text = base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
						.text + AllText.Text_UIA[1259][Mainload.SetData[4]] + "\n";
				}
				else if (Mainload.PropData_Enter.Split('|')[0] == "283")
				{
					base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
						.text = base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
						.text + AllText.Text_UIA[1260][Mainload.SetData[4]] + "\n";
				}
				else if (Mainload.PropData_Enter.Split('|')[0] == "285")
				{
					base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
						.text = base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
						.text + AllText.Text_UIA[1808][Mainload.SetData[4]] + "\n";
				}
				base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
					.text = base.transform.Find("InfoShow").Find("DataShow").GetComponent<Text>()
					.text + "(" + AllText.Text_AllPropClass[int.Parse(Mainload.AllPropdata[int.Parse(Mainload.PropData_Enter.Split('|')[0])][1])][Mainload.SetData[4]] + ")";
			}
			if (base.transform.Find("InfoShow").Find("Prices").Find("Num")
				.GetComponent<Text>()
				.text != FormulaData.NumShow(Mathf.CeilToInt(float.Parse(Mainload.AllPropdata[int.Parse(Mainload.PropData_Enter.Split('|')[0])][0]) * float.Parse(Mainload.PropData_Enter.Split('|')[1]))))
			{
				base.transform.Find("InfoShow").Find("Prices").Find("Num")
					.GetComponent<Text>()
					.text = FormulaData.NumShow(Mathf.CeilToInt(float.Parse(Mainload.AllPropdata[int.Parse(Mainload.PropData_Enter.Split('|')[0])][0]) * float.Parse(Mainload.PropData_Enter.Split('|')[1])));
			}
			if (base.transform.Find("InfoShow").position != Mainload.PropInfoPanelPosi)
			{
				base.transform.Find("InfoShow").position = Mainload.PropInfoPanelPosi;
			}
		}
		else if (base.transform.Find("InfoShow").gameObject.activeSelf)
		{
			base.transform.Find("InfoShow").gameObject.SetActive(value: false);
		}
	}
}
