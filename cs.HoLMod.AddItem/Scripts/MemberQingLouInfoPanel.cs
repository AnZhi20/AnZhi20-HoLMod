using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemberQingLouInfoPanel : MonoBehaviour
{
	private int SceneIndex;

	private string[] Data_A;

	private string[] Data_B;

	private string BodyNum;

	private string MeiLiNum;

	private string SkillLv;

	private GameObject MemberIconShow;

	private GameObject PerCiTiao;

	private void Awake()
	{
		PerCiTiao = (GameObject)Resources.Load("PerCiTiao");
		MemberIconShow = (GameObject)Resources.Load("PerLiHuiBig");
	}

	private void Start()
	{
		initSize();
	}

	private void OnEnable()
	{
		if (Mainload.MemberQingLouIndex_Enter >= 0)
		{
			OnEnableData();
			OnEnableShow();
		}
		else
		{
			base.gameObject.SetActive(value: false);
		}
	}

	private void initSize()
	{
		if (Mainload.SetData[4] == 0)
		{
			base.transform.Find("Info").Find("Data_NL").GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("Data_body").GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("Data_XH").GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("Data_JN").GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("Data_TF").GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("Data_Wen").GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("Data_Wu").GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("Data_Shang").GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("Data_Yi").GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("Data_ML").GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("Data_XY").GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("Data_PX").GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("ShenFen").Find("Text")
				.GetComponent<Text>()
				.fontSize = 16;
		}
		else
		{
			base.transform.Find("Info").Find("Data_NL").GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("Data_body").GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("Data_XH").GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("Data_JN").GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("Data_TF").GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("Data_Wen").GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("Data_Wu").GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("Data_Shang").GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("Data_Yi").GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("Data_ML").GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("Data_XY").GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("Data_PX").GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("ShenFen").Find("Text")
				.GetComponent<Text>()
				.fontSize = 15;
		}
	}

	private void OnEnableData()
	{
		SceneIndex = int.Parse(Mainload.SceneID.Split('|')[1]);
		Data_A = Mainload.Member_Qinglou[SceneIndex][Mainload.MemberQingLouIndex_Enter][3].Split('|');
		Data_B = new string[5]
		{
			Mainload.Member_Qinglou[SceneIndex][Mainload.MemberQingLouIndex_Enter][4],
			Mainload.Member_Qinglou[SceneIndex][Mainload.MemberQingLouIndex_Enter][5],
			Mainload.Member_Qinglou[SceneIndex][Mainload.MemberQingLouIndex_Enter][6],
			Mainload.Member_Qinglou[SceneIndex][Mainload.MemberQingLouIndex_Enter][7],
			Mainload.Member_Qinglou[SceneIndex][Mainload.MemberQingLouIndex_Enter][8]
		};
		BodyNum = Mainload.Member_Qinglou[SceneIndex][Mainload.MemberQingLouIndex_Enter][11];
		MeiLiNum = Mainload.Member_Qinglou[SceneIndex][Mainload.MemberQingLouIndex_Enter][10];
		SkillLv = Mainload.Member_Qinglou[SceneIndex][Mainload.MemberQingLouIndex_Enter][15];
	}

	private void OnEnableShow()
	{
		base.transform.Find("Name").GetComponent<Text>().text = Data_A[0];
		for (int i = 0; i < base.transform.Find("IconShow").childCount; i++)
		{
			Object.Destroy(base.transform.Find("IconShow").GetChild(i).gameObject);
		}
		GameObject obj = Object.Instantiate(MemberIconShow);
		obj.name = Mainload.MemberQingLouIndex_Enter.ToString();
		obj.transform.GetComponent<PerLiHuiBig>().cityID = SceneIndex;
		obj.transform.GetComponent<PerLiHuiBig>().ShowID = 7;
		obj.transform.GetComponent<PerLiHuiBig>().isShowInfo = false;
		obj.transform.SetParent(base.transform.Find("IconShow"));
		obj.transform.localScale = new Vector3(1f, 1f, 1f);
		obj.transform.localPosition = new Vector3(0f, 0f, 0f);
		base.transform.Find("Info").Find("Data_NL").GetComponent<Text>()
			.text = AllText.Text_UIA[1063][Mainload.SetData[4]].Replace("@", Data_B[0]);
		base.transform.Find("Info").Find("Data_body").GetComponent<Text>()
			.text = AllText.Text_UIA[1064][Mainload.SetData[4]].Replace("@", BodyNum);
		base.transform.Find("Info").Find("Data_XH").GetComponent<Text>()
			.text = AllText.Text_UIA[1065][Mainload.SetData[4]].Replace("@", AllText.Text_AllLike[int.Parse(Data_A[1])][Mainload.SetData[4]]);
		if (Mainload.SetData[4] == 1)
		{
			if (Data_A[6] == "0")
			{
				base.transform.Find("Info").Find("Data_JN").GetComponent<Text>()
					.text = AllText.Text_UIA[1066][Mainload.SetData[4]].Replace("@", AllText.Text_AllMemberSkill[int.Parse(Data_A[6])][Mainload.SetData[4]]).Replace("$", SkillLv);
			}
			else
			{
				base.transform.Find("Info").Find("Data_JN").GetComponent<Text>()
					.text = AllText.Text_UIA[1066][Mainload.SetData[4]].Replace("@", AllText.Text_AllMemberSkill[int.Parse(Data_A[6])][Mainload.SetData[4]].Substring(0, 2).ToUpper()).Replace("$", SkillLv);
			}
			if (Data_A[2] == "0")
			{
				base.transform.Find("Info").Find("Data_TF").GetComponent<Text>()
					.text = AllText.Text_UIA[1067][Mainload.SetData[4]].Replace("@", AllText.Text_AllTianFu[int.Parse(Data_A[2])][Mainload.SetData[4]]).Replace("$", Data_A[3]);
			}
			else
			{
				base.transform.Find("Info").Find("Data_TF").GetComponent<Text>()
					.text = AllText.Text_UIA[1067][Mainload.SetData[4]].Replace("@", AllText.Text_AllTianFu[int.Parse(Data_A[2])][Mainload.SetData[4]].Substring(0, 2).ToUpper()).Replace("$", Data_A[3]);
			}
		}
		else
		{
			base.transform.Find("Info").Find("Data_JN").GetComponent<Text>()
				.text = AllText.Text_UIA[1066][Mainload.SetData[4]].Replace("@", AllText.Text_AllMemberSkill[int.Parse(Data_A[6])][Mainload.SetData[4]]).Replace("$", SkillLv);
			base.transform.Find("Info").Find("Data_TF").GetComponent<Text>()
				.text = AllText.Text_UIA[1067][Mainload.SetData[4]].Replace("@", AllText.Text_AllTianFu[int.Parse(Data_A[2])][Mainload.SetData[4]]).Replace("$", Data_A[3]);
		}
		base.transform.Find("Info").Find("Data_Wen").GetComponent<Text>()
			.text = AllText.Text_UIA[1068][Mainload.SetData[4]].Replace("@", Data_B[1]);
		base.transform.Find("Info").Find("Data_Wu").GetComponent<Text>()
			.text = AllText.Text_UIA[1069][Mainload.SetData[4]].Replace("@", Data_B[2]);
		base.transform.Find("Info").Find("Data_Shang").GetComponent<Text>()
			.text = AllText.Text_UIA[1070][Mainload.SetData[4]].Replace("@", Data_B[3]);
		base.transform.Find("Info").Find("Data_Yi").GetComponent<Text>()
			.text = AllText.Text_UIA[1071][Mainload.SetData[4]].Replace("@", Data_B[4]);
		base.transform.Find("Info").Find("Data_ML").GetComponent<Text>()
			.text = AllText.Text_UIA[1072][Mainload.SetData[4]].Replace("@", MeiLiNum);
		base.transform.Find("Info").Find("Data_XY").GetComponent<Text>()
			.text = AllText.Text_UIA[1073][Mainload.SetData[4]].Replace("@", Data_A[7]);
		base.transform.Find("Info").Find("Data_PX").GetComponent<Text>()
			.text = AllText.Text_UIA[1076][Mainload.SetData[4]].Replace("@", AllText.Text_PinXing[int.Parse(Data_A[8])][Mainload.SetData[4]]);
		base.transform.Find("Info").Find("ShenFen").Find("Text")
			.GetComponent<Text>()
			.text = AllText.Text_UIA[1119][Mainload.SetData[4]];
		for (int j = 0; j < base.transform.Find("AllCiTiao").Find("Viewport").Find("Content")
			.childCount; j++)
		{
			Object.Destroy(base.transform.Find("AllCiTiao").Find("Viewport").Find("Content")
				.GetChild(j)
				.gameObject);
			}
			int num = 0;
			if (Mainload.Member_Qinglou[SceneIndex][Mainload.MemberQingLouIndex_Enter][17] != "null")
			{
				string[] array = Mainload.Member_Qinglou[SceneIndex][Mainload.MemberQingLouIndex_Enter][17].Split('|');
				for (int k = 0; k < array.Length; k++)
				{
					if (int.Parse(Mainload.Member_Qinglou[SceneIndex][Mainload.MemberQingLouIndex_Enter][4]) >= int.Parse(Mainload.AllBuffData[int.Parse(array[k].Split('@')[0])][1]))
					{
						GameObject gameObject = Object.Instantiate(PerCiTiao);
						gameObject.transform.Find("Text").GetComponent<Text>().text = AllText.Text_AllBuff[int.Parse(array[k].Split('@')[0])][Mainload.SetData[4]];
						gameObject.transform.SetParent(base.transform.Find("AllCiTiao").Find("Viewport").Find("Content"));
						gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
						gameObject.transform.localPosition = new Vector3(-95 + 190 * (num % 2), -20 - 30 * Mathf.FloorToInt((float)num / 2f), 0f);
						num++;
					}
				}
			}
			List<string> shuXingCiTiao = FormulaData.GetShuXingCiTiao(int.Parse(Data_B[0]), float.Parse(Data_B[1]), float.Parse(Data_B[2]), float.Parse(Data_B[3]), float.Parse(Data_B[4]), -1f, float.Parse(MeiLiNum), float.Parse(Data_A[7]), -1f, float.Parse(Data_A[5]), int.Parse(Data_A[2]), int.Parse(Data_A[3]), 0);
			for (int l = 0; l < shuXingCiTiao.Count; l++)
			{
				GameObject obj2 = Object.Instantiate(PerCiTiao);
				obj2.transform.Find("Text").GetComponent<Text>().text = shuXingCiTiao[l];
				obj2.transform.SetParent(base.transform.Find("AllCiTiao").Find("Viewport").Find("Content"));
				obj2.transform.localScale = new Vector3(1f, 1f, 1f);
				obj2.transform.localPosition = new Vector3(-95 + 190 * (num % 2), -20 - 30 * Mathf.FloorToInt((float)num / 2f), 0f);
				num++;
			}
			base.transform.Find("AllCiTiao").Find("Viewport").Find("Content")
				.GetComponent<RectTransform>()
				.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30 + 30 * Mathf.CeilToInt((float)num / 2f));
			base.transform.Find("ChuShen").GetComponent<Text>().text = AllText.Text_UIA[1304][Mainload.SetData[4]].Replace("@", FormulaData.GetShijiaName(int.Parse(Data_A[9]), isNeibu: false));
		}
	}
