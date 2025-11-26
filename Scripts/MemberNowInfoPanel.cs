using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemberNowInfoPanel : MonoBehaviour
{
	private string[] Data_A;

	private string[] Data_B;

	private string[] ShenfenID;

	private string ShenFenName;

	private string GongMingID;

	private string JueWeiID;

	private string ShengYu;

	private string MeiLiNum;

	private string bodyData;

	private int AllGongZi;

	private string StateID;

	private string StateTime;

	private string isZuZhang;

	private string GongMingName;

	private string FuMuName;

	private string HunYinState;

	private string HuaiYunNum;

	private string TiliNum;

	private string SkillLv;

	private string XuePai;

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
		initShow();
	}

	private void OnEnable()
	{
		if (Mainload.MemberNowIndex_Enter >= 0)
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
			base.transform.Find("Info").Find("Data_SY").GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("Data_GM").GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("Data_TL").GetComponent<Text>()
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
			base.transform.Find("Info").Find("DataB_FL").Find("TipA")
				.GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("DataB_FL").Find("Num")
				.GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("DataB_SF").Find("Text")
				.GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("DataB_HT").Find("Text")
				.GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("DataB_HY").Find("Text")
				.GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("DataB_ZT").Find("Text")
				.GetComponent<Text>()
				.fontSize = 16;
		}
		else
		{
			base.transform.Find("Info").Find("Data_SY").GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("Data_GM").GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("Data_TL").GetComponent<Text>()
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
			base.transform.Find("Info").Find("DataB_FL").Find("TipA")
				.GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("DataB_FL").Find("Num")
				.GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("DataB_SF").Find("Text")
				.GetComponent<Text>()
				.fontSize = 13;
			base.transform.Find("Info").Find("DataB_HT").Find("Text")
				.GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("DataB_HY").Find("Text")
				.GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("DataB_ZT").Find("Text")
				.GetComponent<Text>()
				.fontSize = 15;
		}
	}

	private void initShow()
	{
		base.transform.Find("Info").Find("DataB_FL").Find("TipA")
			.GetComponent<Text>()
			.text = AllText.Text_UIA[1102][Mainload.SetData[4]];
	}

	private void OnEnableData()
	{
		Data_A = Mainload.Member_now[Mainload.MemberNowIndex_Enter][4].Split('|');
		Data_B = new string[9]
		{
			Mainload.Member_now[Mainload.MemberNowIndex_Enter][5],
			Mainload.Member_now[Mainload.MemberNowIndex_Enter][6],
			Mathf.FloorToInt(float.Parse(Mainload.Member_now[Mainload.MemberNowIndex_Enter][7])).ToString(),
			Mathf.FloorToInt(float.Parse(Mainload.Member_now[Mainload.MemberNowIndex_Enter][8])).ToString(),
			Mathf.FloorToInt(float.Parse(Mainload.Member_now[Mainload.MemberNowIndex_Enter][9])).ToString(),
			Mathf.FloorToInt(float.Parse(Mainload.Member_now[Mainload.MemberNowIndex_Enter][10])).ToString(),
			Mainload.Member_now[Mainload.MemberNowIndex_Enter][11],
			Mainload.Member_now[Mainload.MemberNowIndex_Enter][27],
			Mainload.Member_now[Mainload.MemberNowIndex_Enter][34]
		};
		ShenfenID = Mainload.Member_now[Mainload.MemberNowIndex_Enter][12].Split('|')[0].Split('@');
		GongMingID = Mainload.Member_now[Mainload.MemberNowIndex_Enter][13];
		JueWeiID = Mainload.Member_now[Mainload.MemberNowIndex_Enter][14];
		ShengYu = Mathf.FloorToInt(float.Parse(Mainload.Member_now[Mainload.MemberNowIndex_Enter][16])).ToString();
		MeiLiNum = Mainload.Member_now[Mainload.MemberNowIndex_Enter][20];
		bodyData = Mainload.Member_now[Mainload.MemberNowIndex_Enter][21];
		AllGongZi = int.Parse(Mainload.AllShenFenData[int.Parse(ShenfenID[0])][int.Parse(ShenfenID[1])][3]) + int.Parse(Mainload.AllJueWeiData[int.Parse(JueWeiID.Split('|')[0])][0]);
		StateID = Mainload.Member_now[Mainload.MemberNowIndex_Enter][15];
		StateTime = Mainload.Member_now[Mainload.MemberNowIndex_Enter][18];
		isZuZhang = Mainload.Member_now[Mainload.MemberNowIndex_Enter][22];
		HuaiYunNum = Mainload.Member_now[Mainload.MemberNowIndex_Enter][25];
		HunYinState = Mainload.Member_now[Mainload.MemberNowIndex_Enter][26];
		TiliNum = Mainload.Member_now[Mainload.MemberNowIndex_Enter][30];
		SkillLv = Mainload.Member_now[Mainload.MemberNowIndex_Enter][33];
		XuePai = Mainload.Member_now[Mainload.MemberNowIndex_Enter][40];
		if (GongMingID == "-1")
		{
			GongMingName = AllText.Text_UIA[1057][Mainload.SetData[4]];
		}
		else
		{
			GongMingName = AllText.Text_All_GongMing[int.Parse(GongMingID)][Mainload.SetData[4]];
		}
		FuMuName = "null";
		for (int i = 0; i < Mainload.Member_now.Count; i++)
		{
			if (!(Mainload.Member_now[i][2] != "null"))
			{
				continue;
			}
			string[] array = Mainload.Member_now[i][2].Split('|');
			for (int j = 0; j < array.Length; j++)
			{
				if (array[j] == Mainload.Member_now[Mainload.MemberNowIndex_Enter][0])
				{
					FuMuName = AllText.Text_UIA[520][Mainload.SetData[4]].Replace("@", Mainload.Member_now[i][4].Split('|')[0]);
					i = Mainload.Member_now.Count;
					break;
				}
			}
		}
		for (int k = 0; k < Mainload.Member_qu.Count; k++)
		{
			if (!(Mainload.Member_qu[k][3] != "null"))
			{
				continue;
			}
			string[] array2 = Mainload.Member_qu[k][3].Split('|');
			for (int l = 0; l < array2.Length; l++)
			{
				if (array2[l] == Mainload.Member_now[Mainload.MemberNowIndex_Enter][0])
				{
					if (FuMuName == "null")
					{
						FuMuName = AllText.Text_UIA[1343][Mainload.SetData[4]] + AllText.Text_UIA[1234][Mainload.SetData[4]].Replace("@", Mainload.Member_qu[k][2].Split('|')[0]);
					}
					else
					{
						FuMuName += AllText.Text_UIA[1234][Mainload.SetData[4]].Replace("@", Mainload.Member_qu[k][2].Split('|')[0]);
					}
					k = Mainload.Member_qu.Count;
					break;
				}
			}
		}
	}

	private void OnEnableShow()
	{
		base.transform.Find("Name").GetComponent<Text>().text = Data_A[0];
		if (FuMuName == "null")
		{
			base.transform.Find("TipA").GetComponent<Text>().text = AllText.Text_UIA[1103][Mainload.SetData[4]].Replace("$", Data_A[1]);
		}
		else if (Data_A[4] == "0")
		{
			base.transform.Find("TipA").GetComponent<Text>().text = AllText.Text_UIA[1104][Mainload.SetData[4]].Replace("@", FuMuName).Replace("$", Data_A[1]);
		}
		else
		{
			base.transform.Find("TipA").GetComponent<Text>().text = AllText.Text_UIA[1105][Mainload.SetData[4]].Replace("@", FuMuName).Replace("$", Data_A[1]);
		}
		for (int i = 0; i < base.transform.Find("IconShow").childCount; i++)
		{
			Object.Destroy(base.transform.Find("IconShow").GetChild(i).gameObject);
		}
		GameObject obj = Object.Instantiate(MemberIconShow);
		obj.name = Mainload.MemberNowIndex_Enter.ToString();
		obj.transform.GetComponent<PerLiHuiBig>().ShowID = 0;
		obj.transform.GetComponent<PerLiHuiBig>().isShowInfo = false;
		obj.transform.SetParent(base.transform.Find("IconShow"));
		obj.transform.localScale = new Vector3(1f, 1f, 1f);
		obj.transform.localPosition = new Vector3(0f, 0f, 0f);
		base.transform.Find("Info").Find("Data_SY").GetComponent<Text>()
			.text = AllText.Text_UIA[1061][Mainload.SetData[4]].Replace("@", ShengYu);
		base.transform.Find("Info").Find("Data_GM").GetComponent<Text>()
			.text = AllText.Text_UIA[1056][Mainload.SetData[4]].Replace("@", GongMingName);
		base.transform.Find("Info").Find("Data_NL").GetComponent<Text>()
			.text = AllText.Text_UIA[1063][Mainload.SetData[4]].Replace("@", Data_B[1]);
		base.transform.Find("Info").Find("Data_body").GetComponent<Text>()
			.text = AllText.Text_UIA[1064][Mainload.SetData[4]].Replace("@", bodyData);
		base.transform.Find("Info").Find("Data_TL").GetComponent<Text>()
			.text = AllText.Text_UIA[1100][Mainload.SetData[4]].Replace("@", TiliNum);
		base.transform.Find("Info").Find("Data_XH").GetComponent<Text>()
			.text = AllText.Text_UIA[1065][Mainload.SetData[4]].Replace("@", AllText.Text_AllLike[int.Parse(Data_A[9])][Mainload.SetData[4]]);
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
			.text = AllText.Text_UIA[1068][Mainload.SetData[4]].Replace("@", Data_B[2]);
		base.transform.Find("Info").Find("Data_Wu").GetComponent<Text>()
			.text = AllText.Text_UIA[1069][Mainload.SetData[4]].Replace("@", Data_B[3]);
		base.transform.Find("Info").Find("Data_Shang").GetComponent<Text>()
			.text = AllText.Text_UIA[1070][Mainload.SetData[4]].Replace("@", Data_B[4]);
		base.transform.Find("Info").Find("Data_Yi").GetComponent<Text>()
			.text = AllText.Text_UIA[1071][Mainload.SetData[4]].Replace("@", Data_B[5]);
		base.transform.Find("Info").Find("Data_ML").GetComponent<Text>()
			.text = AllText.Text_UIA[1072][Mainload.SetData[4]].Replace("@", MeiLiNum);
		base.transform.Find("Info").Find("Data_XY").GetComponent<Text>()
			.text = AllText.Text_UIA[1073][Mainload.SetData[4]].Replace("@", Data_A[7]);
		base.transform.Find("Info").Find("Data_XF").GetComponent<Text>()
			.text = AllText.Text_UIA[1074][Mainload.SetData[4]].Replace("@", Data_B[6]);
		base.transform.Find("Info").Find("Data_JM").GetComponent<Text>()
			.text = AllText.Text_UIA[1075][Mainload.SetData[4]].Replace("@", Data_B[7]);
		base.transform.Find("Info").Find("Data_PX").GetComponent<Text>()
			.text = AllText.Text_UIA[1076][Mainload.SetData[4]].Replace("@", AllText.Text_PinXing[int.Parse(Data_B[0])][Mainload.SetData[4]]);
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
		if (AllGongZi > 0)
		{
			base.transform.Find("Info").Find("DataB_FL").gameObject.SetActive(value: true);
			base.transform.Find("Info").Find("DataB_FL").Find("Num")
				.GetComponent<Text>()
				.text = AllText.Text_UIA[117][Mainload.SetData[4]].Replace("@", AllGongZi.ToString());
		}
		else
		{
			base.transform.Find("Info").Find("DataB_FL").gameObject.SetActive(value: false);
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
			if (Mainload.Member_now[Mainload.MemberNowIndex_Enter][23] != "null")
			{
				string[] array = Mainload.Member_now[Mainload.MemberNowIndex_Enter][23].Split('|');
				for (int l = 0; l < array.Length; l++)
				{
					if (int.Parse(Mainload.Member_now[Mainload.MemberNowIndex_Enter][6]) >= int.Parse(Mainload.AllBuffData[int.Parse(array[l].Split('@')[0])][1]))
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
			List<string> shuXingCiTiao = FormulaData.GetShuXingCiTiao(int.Parse(Data_B[1]), float.Parse(Data_B[2]), float.Parse(Data_B[3]), float.Parse(Data_B[4]), float.Parse(Data_B[5]), float.Parse(Data_B[7]), float.Parse(MeiLiNum), float.Parse(Data_A[7]), float.Parse(ShengYu), float.Parse(Data_A[5]), int.Parse(Data_A[2]), int.Parse(Data_A[3]), int.Parse(Data_B[8]));
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
