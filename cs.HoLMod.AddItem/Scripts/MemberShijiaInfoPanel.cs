using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemberShijiaInfoPanel : MonoBehaviour
{
	private string[] Data_A;

	private string[] Data_B;

	private string[] ShenfenID;

	private string GongMingID;

	private string JueWeiID;

	private string ShengYu;

	private string ShenFenName;

	private string FuMuName;

	private string StateID;

	private string StateTime;

	private string bodyNum;

	private string MeiLiNum;

	private string GongMingName;

	private string HunYinState;

	private string HuaiYunNum;

	private string isZuZhang;

	private string HaoGanNum;

	private string ShijiaName;

	private string SkillLv;

	private string XuePai;

	private int indexA;

	private int indexB;

	private GameObject MemberIconShow;

	private GameObject PerCiTiao;

	private GameObject jueweiShowA;

	private void Awake()
	{
		jueweiShowA = (GameObject)Resources.Load("PerJueWei");
		PerCiTiao = (GameObject)Resources.Load("PerCiTiao");
		MemberIconShow = (GameObject)Resources.Load("PerLiHuiBig");
	}

	private void Start()
	{
		initSize();
	}

	private void OnEnable()
	{
		if (Mainload.MemberShijiaIndex_Enter != "null")
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
			base.transform.Find("Info").Find("Data_GM").GetComponent<Text>()
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
			base.transform.Find("Info").Find("Data_XP").GetComponent<Text>()
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
			base.transform.Find("Info").Find("Data_GM").GetComponent<Text>()
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
			base.transform.Find("Info").Find("Data_XP").GetComponent<Text>()
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
				.fontSize = 13;
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
		indexA = int.Parse(Mainload.MemberShijiaIndex_Enter.Split('|')[0]);
		indexB = int.Parse(Mainload.MemberShijiaIndex_Enter.Split('|')[1]);
		Data_A = Mainload.Member_other[indexA][indexB][2].Split('|');
		Data_B = new string[7]
		{
			Mainload.Member_other[indexA][indexB][3],
			Mathf.FloorToInt(float.Parse(Mainload.Member_other[indexA][indexB][4])).ToString(),
			Mathf.FloorToInt(float.Parse(Mainload.Member_other[indexA][indexB][5])).ToString(),
			Mathf.FloorToInt(float.Parse(Mainload.Member_other[indexA][indexB][6])).ToString(),
			Mathf.FloorToInt(float.Parse(Mainload.Member_other[indexA][indexB][7])).ToString(),
			Mainload.Member_other[indexA][indexB][8],
			Mainload.Member_other[indexA][indexB][22]
		};
		ShenfenID = Mainload.Member_other[indexA][indexB][9].Split('|')[0].Split('@');
		GongMingID = Mainload.Member_other[indexA][indexB][10];
		JueWeiID = Mainload.Member_other[indexA][indexB][11];
		ShengYu = Mathf.FloorToInt(float.Parse(Mainload.Member_other[indexA][indexB][17])).ToString();
		StateID = Mainload.Member_other[indexA][indexB][16];
		StateTime = Mainload.Member_other[indexA][indexB][18];
		MeiLiNum = Mainload.Member_other[indexA][indexB][19];
		bodyNum = Mainload.Member_other[indexA][indexB][20];
		HunYinState = Mainload.Member_other[indexA][indexB][21];
		HuaiYunNum = Mainload.Member_other[indexA][indexB][12];
		isZuZhang = Mainload.Member_other[indexA][indexB][23];
		if (Mainload.SetData[4] == 0 && float.Parse(Mainload.Member_other[indexA][indexB][15]) > 0f)
		{
			HaoGanNum = float.Parse(Mainload.Member_other[indexA][indexB][15]).ToString("f1");
		}
		else
		{
			HaoGanNum = float.Parse(Mainload.Member_other[indexA][indexB][15]).ToString("f0");
		}
		SkillLv = Mainload.Member_other[indexA][indexB][25];
		XuePai = Mainload.Member_other[indexA][indexB][30];
		FuMuName = "null";
		for (int i = 0; i < Mainload.Member_other[indexA].Count; i++)
		{
			if (Mainload.Member_other[indexA][indexB][2].Split('|')[9] == Mainload.Member_other[indexA][i][0])
			{
				FuMuName = AllText.Text_UIA[520][Mainload.SetData[4]].Replace("@", Mainload.Member_other[indexA][i][2].Split('|')[0]);
				break;
			}
		}
		for (int j = 0; j < Mainload.Member_Other_qu[indexA].Count; j++)
		{
			if (!(Mainload.Member_Other_qu[indexA][j][23] != "null"))
			{
				continue;
			}
			string[] array = Mainload.Member_Other_qu[indexA][j][23].Split('|');
			for (int k = 0; k < array.Length; k++)
			{
				if (array[k] == Mainload.Member_other[indexA][indexB][0])
				{
					if (FuMuName == "null")
					{
						FuMuName = AllText.Text_UIA[1343][Mainload.SetData[4]] + AllText.Text_UIA[1234][Mainload.SetData[4]].Replace("@", Mainload.Member_Other_qu[indexA][j][2].Split('|')[0]);
					}
					else
					{
						FuMuName += AllText.Text_UIA[1234][Mainload.SetData[4]].Replace("@", Mainload.Member_Other_qu[indexA][j][2].Split('|')[0]);
					}
					j = Mainload.Member_Other_qu[indexA].Count;
					break;
				}
			}
		}
		if (GongMingID == "-1")
		{
			GongMingName = AllText.Text_All_GongMing[0][Mainload.SetData[4]];
		}
		else
		{
			GongMingName = AllText.Text_All_GongMing[int.Parse(GongMingID)][Mainload.SetData[4]];
		}
		ShijiaName = FormulaData.GetShijiaName(indexA, isNeibu: false);
	}

	private void OnEnableShow()
	{
		base.transform.Find("Name").GetComponent<Text>().text = "@".Replace("@", Data_A[0]);
		if (FuMuName != "null")
		{
			base.transform.Find("TipA").gameObject.SetActive(value: true);
			if (Data_A[4] == "0")
			{
				base.transform.Find("TipA").GetComponent<Text>().text = AllText.Text_UIA[1058][Mainload.SetData[4]].Replace("@", ShijiaName).Replace("$", FuMuName);
			}
			else
			{
				base.transform.Find("TipA").GetComponent<Text>().text = AllText.Text_UIA[1059][Mainload.SetData[4]].Replace("@", ShijiaName).Replace("$", FuMuName);
			}
		}
		else
		{
			base.transform.Find("TipA").GetComponent<Text>().text = ShijiaName;
		}
		for (int i = 0; i < base.transform.Find("IconShow").childCount; i++)
		{
			Object.Destroy(base.transform.Find("IconShow").GetChild(i).gameObject);
		}
		GameObject obj = Object.Instantiate(MemberIconShow);
		obj.name = indexB.ToString();
		obj.transform.GetComponent<PerLiHuiBig>().ShowID = 5;
		obj.transform.GetComponent<PerLiHuiBig>().ShijiaIndex = indexA;
		obj.transform.GetComponent<PerLiHuiBig>().isShowInfo = false;
		obj.transform.SetParent(base.transform.Find("IconShow"));
		obj.transform.localScale = new Vector3(1f, 1f, 1f);
		obj.transform.localPosition = new Vector3(0f, 0f, 0f);
		base.transform.Find("Info").Find("Data_SY").GetComponent<Text>()
			.text = AllText.Text_UIA[1061][Mainload.SetData[4]].Replace("@", ShengYu);
		base.transform.Find("Info").Find("Data_GX").GetComponent<Text>()
			.text = AllText.Text_UIA[1062][Mainload.SetData[4]].Replace("@", HaoGanNum);
		base.transform.Find("Info").Find("Data_GM").GetComponent<Text>()
			.text = AllText.Text_UIA[1056][Mainload.SetData[4]].Replace("@", GongMingName);
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
		if (XuePai != "0")
		{
			base.transform.Find("Info").Find("Data_XP").gameObject.SetActive(value: true);
			base.transform.Find("Info").Find("Data_XP").GetComponent<Text>()
				.text = AllText.AllXuePaiText[int.Parse(XuePai)][Mainload.SetData[4]];
		}
		else
		{
			base.transform.Find("Info").Find("Data_XP").gameObject.SetActive(value: false);
		}
		for (int j = 0; j < base.transform.Find("JueWeiShow").childCount; j++)
		{
			Object.Destroy(base.transform.Find("JueWeiShow").GetChild(j).gameObject);
		}
		if (JueWeiID != "0|0")
		{
			GameObject gameObject = Object.Instantiate(jueweiShowA);
			gameObject.transform.Find("Text").GetComponent<Text>().text = AllText.Text_AllJueWei[int.Parse(JueWeiID.Split('|')[0])][Mainload.SetData[4]].Replace("@", AllText.Text_AllFengdi[int.Parse(JueWeiID.Split('|')[1])][Mainload.SetData[4]]);
			gameObject.transform.SetParent(base.transform.Find("JueWeiShow"));
			gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
		}
		if (ShenfenID[0] == "0")
		{
			base.transform.Find("Info").Find("DataB_SF").gameObject.SetActive(value: false);
		}
		else if (ShenfenID[0] == "5")
		{
			base.transform.Find("Info").Find("DataB_SF").gameObject.SetActive(value: true);
			string text = AllText.Text_AllShenFen[int.Parse(ShenfenID[0])][int.Parse(ShenfenID[1])][Mainload.SetData[4]].Split('|')[int.Parse(ShenfenID[2])].Replace("Provincial ", "");
			string text2 = "null";
			if (int.Parse(ShenfenID[3]) >= 0)
			{
				text2 = AllText.Text_City[int.Parse(ShenfenID[3])][Mainload.SetData[4]].Split('~')[0];
			}
			if (int.Parse(ShenfenID[4]) >= 0)
			{
				text2 = ((Mainload.SetData[4] != 0) ? AllText.Text_City[int.Parse(ShenfenID[3])][Mainload.SetData[4]].Split('~')[1].Split('|')[int.Parse(ShenfenID[4])] : (text2 + AllText.Text_City[int.Parse(ShenfenID[3])][Mainload.SetData[4]].Split('~')[1].Split('|')[int.Parse(ShenfenID[4])]));
			}
			ShenFenName = text.Replace("@", text2.Replace(" Province", ""));
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
		if (Data_A[4] == "0")
		{
			if (HuaiYunNum == "-1")
			{
				if (isZuZhang == "1")
				{
					base.transform.Find("Info").Find("DataB_HT").Find("Text")
						.GetComponent<Text>()
						.text = AllText.Text_UIA[1077][Mainload.SetData[4]];
					base.transform.Find("Info").Find("DataB_HT").Find("Text")
						.GetComponent<Text>()
						.color = new Color(1f, 1f, 0.667f, 1f);
				}
				else
				{
					base.transform.Find("Info").Find("DataB_HT").Find("Text")
						.GetComponent<Text>()
						.text = AllText.Text_UIA[1078][Mainload.SetData[4]];
					base.transform.Find("Info").Find("DataB_HT").Find("Text")
						.GetComponent<Text>()
						.color = new Color(1f, 1f, 1f, 0.3f);
				}
			}
			else if (HuaiYunNum == "10")
			{
				base.transform.Find("Info").Find("DataB_HT").Find("Text")
					.GetComponent<Text>()
					.text = AllText.Text_UIA[1327][Mainload.SetData[4]];
				base.transform.Find("Info").Find("DataB_HT").Find("Text")
					.GetComponent<Text>()
					.color = new Color(1f, 1f, 0.667f, 1f);
			}
			else
			{
				base.transform.Find("Info").Find("DataB_HT").Find("Text")
					.GetComponent<Text>()
					.text = AllText.Text_UIA[1079][Mainload.SetData[4]].Replace("@", (10 - int.Parse(HuaiYunNum)).ToString());
				base.transform.Find("Info").Find("DataB_HT").Find("Text")
					.GetComponent<Text>()
					.color = new Color(1f, 1f, 0.667f, 1f);
			}
		}
		else if (isZuZhang == "0")
		{
			base.transform.Find("Info").Find("DataB_HT").Find("Text")
				.GetComponent<Text>()
				.text = AllText.Text_UIA[1078][Mainload.SetData[4]];
			base.transform.Find("Info").Find("DataB_HT").Find("Text")
				.GetComponent<Text>()
				.color = new Color(1f, 1f, 1f, 0.3f);
		}
		else
		{
			base.transform.Find("Info").Find("DataB_HT").Find("Text")
				.GetComponent<Text>()
				.text = AllText.Text_UIA[1077][Mainload.SetData[4]];
			base.transform.Find("Info").Find("DataB_HT").Find("Text")
				.GetComponent<Text>()
				.color = new Color(1f, 1f, 0.667f, 1f);
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
		for (int k = 0; k < base.transform.Find("AllCiTiao").Find("Viewport").Find("Content")
			.childCount; k++)
		{
			Object.Destroy(base.transform.Find("AllCiTiao").Find("Viewport").Find("Content")
				.GetChild(k)
				.gameObject);
			}
			int num = 0;
			if (Mainload.Member_other[indexA][indexB][29] != "null")
			{
				string[] array = Mainload.Member_other[indexA][indexB][29].Split('|');
				for (int l = 0; l < array.Length; l++)
				{
					if (int.Parse(Mainload.Member_other[indexA][indexB][3]) >= int.Parse(Mainload.AllBuffData[int.Parse(array[l].Split('@')[0])][1]))
					{
						GameObject gameObject2 = Object.Instantiate(PerCiTiao);
						gameObject2.transform.Find("Text").GetComponent<Text>().text = AllText.Text_AllBuff[int.Parse(array[l].Split('@')[0])][Mainload.SetData[4]];
						gameObject2.transform.SetParent(base.transform.Find("AllCiTiao").Find("Viewport").Find("Content"));
						gameObject2.transform.localScale = new Vector3(1f, 1f, 1f);
						gameObject2.transform.localPosition = new Vector3(-95 + 190 * (num % 2), -20 - 30 * Mathf.FloorToInt((float)num / 2f), 0f);
						num++;
					}
				}
			}
			List<string> shuXingCiTiao = FormulaData.GetShuXingCiTiao(int.Parse(Data_B[0]), float.Parse(Data_B[1]), float.Parse(Data_B[2]), float.Parse(Data_B[3]), float.Parse(Data_B[4]), float.Parse(Data_B[6]), float.Parse(MeiLiNum), float.Parse(Data_A[7]), float.Parse(ShengYu), float.Parse(Data_A[5]), int.Parse(Data_A[2]), int.Parse(Data_A[3]), 0);
			for (int m = 0; m < shuXingCiTiao.Count; m++)
			{
				GameObject obj2 = Object.Instantiate(PerCiTiao);
				obj2.transform.Find("Text").GetComponent<Text>().text = shuXingCiTiao[m];
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
