using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemberKingInfoPanel : MonoBehaviour
{
	private string[] Data_A;

	private string[] Data_B;

	private string[] ShenfenID;

	private string JueWeiID;

	private string ShengYu;

	private string ShenFenName;

	private string FuMuName;

	private string StateID;

	private string StateTime;

	private string bodyNum;

	private string MeiLiNum;

	private string HunYinState;

	private string HuaiYunNum;

	private string HaoGanNum;

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
		if (Mainload.MemberKingIndex_Enter >= 0)
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
			base.transform.Find("Info").Find("Data_GX").GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("Data_SY").GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("Data_NL").GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("Data_body").GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("Data_XF").GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("Data_ML").GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("Data_XY").GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("Data_XH").GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("Data_PX").GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("Data_TF").GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("Data_JN").GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("Data_Wen").GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("Data_Wu").GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("Data_Shang").GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("Data_Yi").GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("Data_JM").GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("TipA").GetComponent<Text>().fontSize = 16;
			base.transform.Find("Info").Find("DataB_SF").Find("Text")
				.GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("DataB_HY").Find("Text")
				.GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("DataB_HT").Find("Text")
				.GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("DataB_ZT").Find("Text")
				.GetComponent<Text>()
				.fontSize = 16;
		}
		else
		{
			base.transform.Find("Info").Find("Data_GX").GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("Data_SY").GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("Data_NL").GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("Data_body").GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("Data_XF").GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("Data_ML").GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("Data_XY").GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("Data_XH").GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("Data_PX").GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("Data_TF").GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("Data_JN").GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("Data_Wen").GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("Data_Wu").GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("Data_Shang").GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("Data_Yi").GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("Data_JM").GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("TipA").GetComponent<Text>().fontSize = 15;
			base.transform.Find("Info").Find("DataB_SF").Find("Text")
				.GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("DataB_HY").Find("Text")
				.GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("DataB_HT").Find("Text")
				.GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("DataB_ZT").Find("Text")
				.GetComponent<Text>()
				.fontSize = 15;
		}
	}

	private void OnEnableData()
	{
		Data_A = Mainload.Member_King[Mainload.MemberKingIndex_Enter][2].Split('|');
		Data_B = new string[7]
		{
			Mainload.Member_King[Mainload.MemberKingIndex_Enter][3],
			Mathf.FloorToInt(float.Parse(Mainload.Member_King[Mainload.MemberKingIndex_Enter][4])).ToString(),
			Mathf.FloorToInt(float.Parse(Mainload.Member_King[Mainload.MemberKingIndex_Enter][5])).ToString(),
			Mathf.FloorToInt(float.Parse(Mainload.Member_King[Mainload.MemberKingIndex_Enter][6])).ToString(),
			Mathf.FloorToInt(float.Parse(Mainload.Member_King[Mainload.MemberKingIndex_Enter][7])).ToString(),
			Mainload.Member_King[Mainload.MemberKingIndex_Enter][8],
			Mainload.Member_King[Mainload.MemberKingIndex_Enter][21]
		};
		ShenfenID = Mainload.Member_King[Mainload.MemberKingIndex_Enter][9].Split('|')[0].Split('@');
		JueWeiID = Mainload.Member_King[Mainload.MemberKingIndex_Enter][10];
		ShengYu = Mathf.FloorToInt(float.Parse(Mainload.Member_King[Mainload.MemberKingIndex_Enter][16])).ToString();
		StateID = Mainload.Member_King[Mainload.MemberKingIndex_Enter][15];
		StateTime = Mainload.Member_King[Mainload.MemberKingIndex_Enter][17];
		MeiLiNum = Mainload.Member_King[Mainload.MemberKingIndex_Enter][18];
		bodyNum = Mainload.Member_King[Mainload.MemberKingIndex_Enter][19];
		HunYinState = Mainload.Member_King[Mainload.MemberKingIndex_Enter][20];
		HuaiYunNum = Mainload.Member_King[Mainload.MemberKingIndex_Enter][11];
		if (Mainload.SetData[4] == 0 && float.Parse(Mainload.Member_King[Mainload.MemberKingIndex_Enter][14]) > 0f)
		{
			HaoGanNum = float.Parse(Mainload.Member_King[Mainload.MemberKingIndex_Enter][14]).ToString("f1");
		}
		else
		{
			HaoGanNum = float.Parse(Mainload.Member_King[Mainload.MemberKingIndex_Enter][14]).ToString("f0");
		}
		SkillLv = Mainload.Member_King[Mainload.MemberKingIndex_Enter][23];
		FuMuName = "null";
		for (int i = 0; i < Mainload.Member_King.Count; i++)
		{
			if (Mainload.Member_King[Mainload.MemberKingIndex_Enter][2].Split('|')[9] == Mainload.Member_King[i][0])
			{
				FuMuName = AllText.Text_UIA[520][Mainload.SetData[4]].Replace("@", Mainload.Member_King[i][2].Split('|')[0]);
				break;
			}
		}
		for (int j = 0; j < Mainload.Member_King_qu.Count; j++)
		{
			string[] array = Mainload.Member_King_qu[j][23].Split('|');
			for (int k = 0; k < array.Length; k++)
			{
				if (array[k] == Mainload.Member_King[Mainload.MemberKingIndex_Enter][0])
				{
					if (FuMuName == "null")
					{
						FuMuName = AllText.Text_UIA[1343][Mainload.SetData[4]] + AllText.Text_UIA[1234][Mainload.SetData[4]].Replace("@", Mainload.Member_King_qu[j][2].Split('|')[0]);
					}
					else
					{
						FuMuName += AllText.Text_UIA[1234][Mainload.SetData[4]].Replace("@", Mainload.Member_King_qu[j][2].Split('|')[0]);
					}
					j = Mainload.Member_King_qu.Count;
					break;
				}
			}
		}
	}

	private void OnEnableShow()
	{
		base.transform.Find("Name").GetComponent<Text>().text = "@".Replace("@", Data_A[0]);
		if (FuMuName != "null")
		{
			base.transform.Find("TipA").gameObject.SetActive(value: true);
			if (Data_A[4] == "0")
			{
				base.transform.Find("TipA").GetComponent<Text>().text = AllText.Text_UIA[1108][Mainload.SetData[4]].Replace("@", FuMuName);
			}
			else
			{
				base.transform.Find("TipA").GetComponent<Text>().text = AllText.Text_UIA[1109][Mainload.SetData[4]].Replace("@", FuMuName);
			}
		}
		else
		{
			base.transform.Find("TipA").GetComponent<Text>().text = AllText.Text_UIA[932][Mainload.SetData[4]];
		}
		for (int i = 0; i < base.transform.Find("IconShow").childCount; i++)
		{
			Object.Destroy(base.transform.Find("IconShow").GetChild(i).gameObject);
		}
		GameObject obj = Object.Instantiate(MemberIconShow);
		obj.name = Mainload.MemberKingIndex_Enter.ToString();
		obj.transform.GetComponent<PerLiHuiBig>().ShowID = 12;
		obj.transform.GetComponent<PerLiHuiBig>().isShowInfo = false;
		obj.transform.SetParent(base.transform.Find("IconShow"));
		obj.transform.localScale = new Vector3(1f, 1f, 1f);
		obj.transform.localPosition = new Vector3(0f, 0f, 0f);
		base.transform.Find("Info").Find("Data_SY").GetComponent<Text>()
			.text = AllText.Text_UIA[1061][Mainload.SetData[4]].Replace("@", ShengYu);
		base.transform.Find("Info").Find("Data_GX").GetComponent<Text>()
			.text = AllText.Text_UIA[1062][Mainload.SetData[4]].Replace("@", HaoGanNum);
		base.transform.Find("Info").Find("Data_NL").GetComponent<Text>()
			.text = AllText.Text_UIA[1063][Mainload.SetData[4]].Replace("@", Data_B[0]);
		base.transform.Find("Info").Find("Data_body").GetComponent<Text>()
			.text = AllText.Text_UIA[1064][Mainload.SetData[4]].Replace("@", bodyNum);
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
		base.transform.Find("Info").Find("Data_XF").GetComponent<Text>()
			.text = AllText.Text_UIA[1074][Mainload.SetData[4]].Replace("@", Data_B[5]);
		base.transform.Find("Info").Find("Data_JM").GetComponent<Text>()
			.text = AllText.Text_UIA[1075][Mainload.SetData[4]].Replace("@", Data_B[6]);
		base.transform.Find("Info").Find("Data_PX").GetComponent<Text>()
			.text = AllText.Text_UIA[1076][Mainload.SetData[4]].Replace("@", AllText.Text_PinXing[int.Parse(Data_A[8])][Mainload.SetData[4]]);
		if (ShenfenID[0] == "0")
		{
			base.transform.Find("Info").Find("DataB_SF").gameObject.SetActive(value: false);
		}
		else if (ShenfenID[0] == "5")
		{
			base.transform.Find("Info").Find("DataB_SF").gameObject.SetActive(value: true);
			string text = AllText.Text_AllShenFen[int.Parse(ShenfenID[0])][int.Parse(ShenfenID[1])][Mainload.SetData[4]].Split('|')[int.Parse(ShenfenID[2])];
			string text2 = null;
			if (int.Parse(ShenfenID[3]) >= 0)
			{
				text2 = AllText.Text_City[int.Parse(ShenfenID[3])][Mainload.SetData[4]].Split('~')[0];
			}
			if (int.Parse(ShenfenID[4]) >= 0)
			{
				text2 += AllText.Text_City[int.Parse(ShenfenID[3])][Mainload.SetData[4]].Split('~')[1].Split('|')[int.Parse(ShenfenID[4])];
			}
			ShenFenName = text.Replace("@", text2);
			base.transform.Find("Info").Find("DataB_SF").Find("Text")
				.GetComponent<Text>()
				.text = ShenFenName;
		}
		else
		{
			base.transform.Find("Info").Find("DataB_SF").gameObject.SetActive(value: true);
			ShenFenName = AllText.Text_AllShenFen[int.Parse(ShenfenID[0])][int.Parse(ShenfenID[1])][Mainload.SetData[4]].Split('|')[int.Parse(ShenfenID[2])];
			base.transform.Find("Info").Find("DataB_SF").Find("Text")
				.GetComponent<Text>()
				.text = ShenFenName;
		}
		base.transform.Find("Info").Find("DataB_HY").Find("Text")
			.GetComponent<Text>()
			.text = AllText.Text_AllHunYinState[int.Parse(HunYinState)][Mainload.SetData[4]];
		if (JueWeiID != "0|0")
		{
			base.transform.Find("Info").Find("DataB_JW").gameObject.SetActive(value: true);
			if (JueWeiID.Split('|')[0] == "4")
			{
				base.transform.Find("Info").Find("DataB_JW").Find("Text")
					.GetComponent<Text>()
					.text = AllText.Text_FanWang[int.Parse(JueWeiID.Split('|')[1])][Mainload.SetData[4]];
			}
			else
			{
				base.transform.Find("Info").Find("DataB_JW").Find("Text")
					.GetComponent<Text>()
					.text = AllText.Text_AllJueWei[int.Parse(JueWeiID.Split('|')[0])][Mainload.SetData[4]].Replace("@", AllText.Text_AllFandi[int.Parse(JueWeiID.Split('|')[1])][Mainload.SetData[4]]);
			}
		}
		else
		{
			base.transform.Find("Info").Find("DataB_JW").gameObject.SetActive(value: false);
		}
		if (HuaiYunNum == "-1")
		{
			if (ShenfenID[1] != "5")
			{
				if (JueWeiID.Split('|')[0] == "4")
				{
					base.transform.Find("Info").Find("DataB_HT").Find("Text")
						.GetComponent<Text>()
						.text = AllText.Text_UIA[1638][Mainload.SetData[4]];
				}
				else
				{
					base.transform.Find("Info").Find("DataB_HT").Find("Text")
						.GetComponent<Text>()
						.text = AllText.Text_UIA[1110][Mainload.SetData[4]];
				}
			}
			else
			{
				base.transform.Find("Info").Find("DataB_HT").Find("Text")
					.GetComponent<Text>()
					.text = AllText.Text_UIA[1112][Mainload.SetData[4]];
			}
		}
		else if (HuaiYunNum == "10")
		{
			base.transform.Find("Info").Find("DataB_HT").Find("Text")
				.GetComponent<Text>()
				.text = AllText.Text_UIA[1327][Mainload.SetData[4]];
		}
		else
		{
			base.transform.Find("Info").Find("DataB_HT").Find("Text")
				.GetComponent<Text>()
				.text = AllText.Text_UIA[1079][Mainload.SetData[4]].Replace("@", (10 - int.Parse(HuaiYunNum)).ToString());
		}
		if (StateID == "0")
		{
			base.transform.Find("Info").Find("DataB_ZT").gameObject.SetActive(value: false);
		}
		else if (Mainload.AllMemberState[int.Parse(StateID)][3] == "0")
		{
			base.transform.Find("Info").Find("DataB_ZT").gameObject.SetActive(value: true);
			base.transform.Find("Info").Find("DataB_ZT").Find("Text")
				.GetComponent<Text>()
				.text = AllText.Text_AllMemberState[int.Parse(StateID)][Mainload.SetData[4]];
			base.transform.Find("Info").Find("DataB_ZT").Find("Text")
				.GetComponent<Text>()
				.color = new Color(1f, 1f, 0.667f, 1f);
		}
		else
		{
			base.transform.Find("Info").Find("DataB_ZT").gameObject.SetActive(value: true);
			base.transform.Find("Info").Find("DataB_ZT").Find("Text")
				.GetComponent<Text>()
				.text = AllText.Text_UIA[1080][Mainload.SetData[4]].Replace("@", AllText.Text_AllMemberState[int.Parse(StateID)][Mainload.SetData[4]]).Replace("$", StateTime);
			base.transform.Find("Info").Find("DataB_ZT").Find("Text")
				.GetComponent<Text>()
				.color = new Color(1f, 1f, 0.667f, 1f);
		}
		for (int j = 0; j < base.transform.Find("AllCiTiao").Find("Viewport").Find("Content")
			.childCount; j++)
		{
			Object.Destroy(base.transform.Find("AllCiTiao").Find("Viewport").Find("Content")
				.GetChild(j)
				.gameObject);
			}
			int num = 0;
			if (Mainload.Member_King[Mainload.MemberKingIndex_Enter][26] != "null")
			{
				string[] array = Mainload.Member_King[Mainload.MemberKingIndex_Enter][26].Split('|');
				for (int k = 0; k < array.Length; k++)
				{
					if (int.Parse(Mainload.Member_King[Mainload.MemberKingIndex_Enter][3]) >= int.Parse(Mainload.AllBuffData[int.Parse(array[k].Split('@')[0])][1]))
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
			List<string> shuXingCiTiao = FormulaData.GetShuXingCiTiao(int.Parse(Data_B[0]), float.Parse(Data_B[1]), float.Parse(Data_B[2]), float.Parse(Data_B[3]), float.Parse(Data_B[4]), float.Parse(Data_B[6]), float.Parse(MeiLiNum), float.Parse(Data_A[7]), float.Parse(ShengYu), float.Parse(Data_A[5]), int.Parse(Data_A[2]), int.Parse(Data_A[3]), 0);
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
		}
	}
