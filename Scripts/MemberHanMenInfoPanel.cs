using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemberHanMenInfoPanel : MonoBehaviour
{
	private string[] Data_A;

	private string[] Data_B;

	private string[] ShenfenID;

	private string GongMingID;

	private string JueWeiID;

	private string ShengYu;

	private string ShenFenName;

	private string bodyNum;

	private string MeiLiNum;

	private string GongMingName;

	private string HaoGanNum;

	private string skillLv;

	private string XuePai;

	private int MemberIndex;

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
		if (Mainload.MemberHanMenIndex_Enter >= 0)
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
		}
	}

	private void OnEnableData()
	{
		MemberIndex = Mainload.MemberHanMenIndex_Enter;
		Data_A = Mainload.Member_Hanmen[MemberIndex][2].Split('|');
		Data_B = new string[7]
		{
			Mainload.Member_Hanmen[MemberIndex][3],
			Mathf.FloorToInt(float.Parse(Mainload.Member_Hanmen[MemberIndex][4])).ToString(),
			Mathf.FloorToInt(float.Parse(Mainload.Member_Hanmen[MemberIndex][5])).ToString(),
			Mathf.FloorToInt(float.Parse(Mainload.Member_Hanmen[MemberIndex][6])).ToString(),
			Mathf.FloorToInt(float.Parse(Mainload.Member_Hanmen[MemberIndex][7])).ToString(),
			Mainload.Member_Hanmen[MemberIndex][8],
			Mainload.Member_Hanmen[MemberIndex][22]
		};
		ShenfenID = Mainload.Member_Hanmen[MemberIndex][9].Split('|')[0].Split('@');
		GongMingID = Mainload.Member_Hanmen[MemberIndex][10];
		JueWeiID = Mainload.Member_Hanmen[MemberIndex][11];
		ShengYu = Mathf.FloorToInt(float.Parse(Mainload.Member_Hanmen[MemberIndex][17])).ToString();
		MeiLiNum = Mainload.Member_Hanmen[MemberIndex][19];
		bodyNum = Mainload.Member_Hanmen[MemberIndex][20];
		if (Mainload.SetData[4] == 0 && float.Parse(Mainload.Member_Hanmen[MemberIndex][15]) > 0f)
		{
			HaoGanNum = float.Parse(Mainload.Member_Hanmen[MemberIndex][15]).ToString("f1");
		}
		else
		{
			HaoGanNum = float.Parse(Mainload.Member_Hanmen[MemberIndex][15]).ToString("f0");
		}
		skillLv = Mainload.Member_Hanmen[MemberIndex][24];
		XuePai = Mainload.Member_Hanmen[MemberIndex][25];
		if (GongMingID == "-1")
		{
			GongMingName = AllText.Text_All_GongMing[0][Mainload.SetData[4]];
		}
		else
		{
			GongMingName = AllText.Text_All_GongMing[int.Parse(GongMingID)][Mainload.SetData[4]];
		}
	}

	private void OnEnableShow()
	{
		base.transform.Find("Name").GetComponent<Text>().text = Data_A[0];
		if (Data_A[4] == "0")
		{
			base.transform.Find("TipA").GetComponent<Text>().text = AllText.Text_UIA[1115][Mainload.SetData[4]];
		}
		else
		{
			base.transform.Find("TipA").GetComponent<Text>().text = AllText.Text_UIA[1116][Mainload.SetData[4]];
		}
		for (int i = 0; i < base.transform.Find("IconShow").childCount; i++)
		{
			Object.Destroy(base.transform.Find("IconShow").GetChild(i).gameObject);
		}
		GameObject obj = Object.Instantiate(MemberIconShow);
		obj.name = MemberIndex.ToString();
		obj.transform.GetComponent<PerLiHuiBig>().ShowID = 15;
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
					.text = AllText.Text_UIA[1066][Mainload.SetData[4]].Replace("@", AllText.Text_AllMemberSkill[int.Parse(Data_A[6])][Mainload.SetData[4]]).Replace("$", skillLv);
			}
			else
			{
				base.transform.Find("Info").Find("Data_JN").GetComponent<Text>()
					.text = AllText.Text_UIA[1066][Mainload.SetData[4]].Replace("@", AllText.Text_AllMemberSkill[int.Parse(Data_A[6])][Mainload.SetData[4]].Substring(0, 2).ToUpper()).Replace("$", skillLv);
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
				.text = AllText.Text_UIA[1066][Mainload.SetData[4]].Replace("@", AllText.Text_AllMemberSkill[int.Parse(Data_A[6])][Mainload.SetData[4]]).Replace("$", skillLv);
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
		for (int k = 0; k < base.transform.Find("AllCiTiao").Find("Viewport").Find("Content")
			.childCount; k++)
		{
			Object.Destroy(base.transform.Find("AllCiTiao").Find("Viewport").Find("Content")
				.GetChild(k)
				.gameObject);
			}
			int num = 0;
			if (Mainload.Member_Hanmen[MemberIndex][26] != "null")
			{
				string[] array = Mainload.Member_Hanmen[MemberIndex][26].Split('|');
				for (int l = 0; l < array.Length; l++)
				{
					if (int.Parse(Mainload.Member_Hanmen[MemberIndex][3]) >= int.Parse(Mainload.AllBuffData[int.Parse(array[l].Split('@')[0])][1]))
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
