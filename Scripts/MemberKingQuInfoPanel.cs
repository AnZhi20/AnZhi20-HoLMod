using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemberKingQuInfoPanel : MonoBehaviour
{
	private GameObject PerLiHuiA;

	private GameObject PerCiTiao;

	private string[] Data_A;

	private string[] Data_B;

	private string[] ShenFen;

	private string PeiOuName;

	private string ShengYu;

	private string ShijiaName;

	private string StateID;

	private string StateTime;

	private string bodyNum;

	private string MeiLiNum;

	private string HuaiYunNum;

	private string HaoGanNum;

	private string TipA;

	private string TipB;

	private string TipC;

	private string skillLv;

	private string FuQiRelax;

	private string FengHao;

	private bool isPeiOuLive;

	private void Awake()
	{
		PerCiTiao = (GameObject)Resources.Load("PerCiTiao");
		PerLiHuiA = (GameObject)Resources.Load("PerLiHuiBig");
	}

	private void Start()
	{
		initSize();
	}

	private void OnEnable()
	{
		if (Mainload.MemberKingQuIndex_Enter >= 0)
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
			base.transform.Find("Info").Find("DataB_HT").Find("Text")
				.GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("DataB_ZT").Find("Text")
				.GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("Data_FQ").GetComponent<Text>()
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
				.fontSize = 13;
			base.transform.Find("Info").Find("DataB_HT").Find("Text")
				.GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("DataB_ZT").Find("Text")
				.GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("Data_FQ").GetComponent<Text>()
				.fontSize = 15;
		}
	}

	private void OnEnableData()
	{
		Mainload.PropSelected = new List<List<int>>();
		Data_A = Mainload.Member_King_qu[Mainload.MemberKingQuIndex_Enter][2].Split('|');
		Data_B = new string[7]
		{
			Mainload.Member_King_qu[Mainload.MemberKingQuIndex_Enter][3],
			Mathf.FloorToInt(float.Parse(Mainload.Member_King_qu[Mainload.MemberKingQuIndex_Enter][4])).ToString(),
			Mathf.FloorToInt(float.Parse(Mainload.Member_King_qu[Mainload.MemberKingQuIndex_Enter][5])).ToString(),
			Mathf.FloorToInt(float.Parse(Mainload.Member_King_qu[Mainload.MemberKingQuIndex_Enter][6])).ToString(),
			Mathf.FloorToInt(float.Parse(Mainload.Member_King_qu[Mainload.MemberKingQuIndex_Enter][7])).ToString(),
			Mainload.Member_King_qu[Mainload.MemberKingQuIndex_Enter][8],
			Mainload.Member_King_qu[Mainload.MemberKingQuIndex_Enter][17]
		};
		ShengYu = Mathf.FloorToInt(float.Parse(Mainload.Member_King_qu[Mainload.MemberKingQuIndex_Enter][10])).ToString();
		StateID = Mainload.Member_King_qu[Mainload.MemberKingQuIndex_Enter][9];
		StateTime = Mainload.Member_King_qu[Mainload.MemberKingQuIndex_Enter][11];
		MeiLiNum = Mainload.Member_King_qu[Mainload.MemberKingQuIndex_Enter][15];
		bodyNum = Mainload.Member_King_qu[Mainload.MemberKingQuIndex_Enter][16];
		FengHao = Mainload.Member_King_qu[Mainload.MemberKingQuIndex_Enter][26];
		HuaiYunNum = Mainload.Member_King_qu[Mainload.MemberKingQuIndex_Enter][13];
		ShenFen = Mainload.Member_King_qu[Mainload.MemberKingQuIndex_Enter][18].Split('|')[0].Split('@');
		if (Mainload.SetData[4] == 0 && float.Parse(Mainload.Member_King_qu[Mainload.MemberKingQuIndex_Enter][12]) > 0f)
		{
			HaoGanNum = float.Parse(Mainload.Member_King_qu[Mainload.MemberKingQuIndex_Enter][12]).ToString("f1");
		}
		else
		{
			HaoGanNum = float.Parse(Mainload.Member_King_qu[Mainload.MemberKingQuIndex_Enter][12]).ToString("f0");
		}
		skillLv = Mainload.Member_King_qu[Mainload.MemberKingQuIndex_Enter][20];
		FuQiRelax = float.Parse(Mainload.Member_King_qu[Mainload.MemberKingQuIndex_Enter][21].Split('|')[0]).ToString("f0");
		if (Data_A[4] == "0")
		{
			TipA = AllText.Text_UIA[1106][Mainload.SetData[4]];
			TipB = AllText.Text_UIA[1087][Mainload.SetData[4]];
			TipC = AllText.Text_UIA[1096][Mainload.SetData[4]];
		}
		else
		{
			TipA = AllText.Text_UIA[1085][Mainload.SetData[4]];
			TipB = AllText.Text_UIA[1088][Mainload.SetData[4]];
			TipC = AllText.Text_UIA[1097][Mainload.SetData[4]];
		}
		isPeiOuLive = false;
		PeiOuName = AllText.Text_AllShenFen[int.Parse(ShenFen[0])][int.Parse(ShenFen[1])][Mainload.SetData[4]].Split('|')[int.Parse(ShenFen[2])];
		for (int i = 0; i < Mainload.Member_King.Count; i++)
		{
			if (Data_A[9] == Mainload.Member_King[i][0])
			{
				isPeiOuLive = true;
				PeiOuName = AllText.Text_UIA[1107][Mainload.SetData[4]].Replace("@", Mainload.Member_King[i][2].Split('|')[0]).Replace("$", TipA).Replace("~", AllText.Text_AllShenFen[int.Parse(ShenFen[0])][int.Parse(ShenFen[1])][Mainload.SetData[4]].Split('|')[int.Parse(ShenFen[2])]);
				break;
			}
		}
		string newValue = FormuAct.ShijiaName_AllQu(int.Parse(Data_A[1]), Data_A[0]);
		if (int.Parse(Data_A[1]) >= 0)
		{
			ShijiaName = AllText.Text_UIA[1089][Mainload.SetData[4]].Replace("@", newValue).Replace("$", TipB);
		}
		else if (Data_A[1] == "-1")
		{
			ShijiaName = AllText.Text_UIA[1090][Mainload.SetData[4]].Replace("~", newValue).Replace("@", Data_A[11]).Replace("$", TipB);
		}
		else if (Data_A[1] == "-2")
		{
			ShijiaName = AllText.Text_UIA[1089][Mainload.SetData[4]].Replace("@", newValue).Replace("$", TipB);
		}
		else if (Data_A[1] == "-100")
		{
			int num = int.Parse(Mainload.Member_King_qu[Mainload.MemberKingQuIndex_Enter][0].Substring(Mainload.Member_King_qu[Mainload.MemberKingQuIndex_Enter][0].Length - 1)) % 4;
			newValue = AllText.Text_UIA[1176][Mainload.SetData[4]].Split('|')[num];
			ShijiaName = AllText.Text_UIA[1089][Mainload.SetData[4]].Replace("@", newValue).Replace("$", TipB);
		}
		else if (Data_A[1] == "-3")
		{
			ShijiaName = AllText.Text_UIA[1091][Mainload.SetData[4]].Replace("$", TipC);
		}
		else if (Data_A[1] == "-4")
		{
			ShijiaName = AllText.Text_UIA[1092][Mainload.SetData[4]].Replace("$", TipC);
		}
		else if (Data_A[1] == "-5")
		{
			ShijiaName = AllText.Text_UIA[1093][Mainload.SetData[4]].Replace("$", TipC);
		}
		else if (Data_A[1] == "-6")
		{
			ShijiaName = AllText.Text_UIA[1094][Mainload.SetData[4]].Replace("$", TipC);
		}
		else if (Data_A[1] == "-7")
		{
			ShijiaName = AllText.Text_UIA[1095][Mainload.SetData[4]].Replace("$", TipC);
		}
	}

	private void OnEnableShow()
	{
		base.transform.Find("Name").GetComponent<Text>().text = "@".Replace("@", Data_A[0]);
		base.transform.Find("TipA").GetComponent<Text>().text = ShijiaName;
		for (int i = 0; i < base.transform.Find("IconShow").childCount; i++)
		{
			Object.Destroy(base.transform.Find("IconShow").GetChild(i).gameObject);
		}
		GameObject obj = Object.Instantiate(PerLiHuiA);
		obj.name = Mainload.MemberKingQuIndex_Enter.ToString();
		obj.transform.GetComponent<PerLiHuiBig>().ShowID = 1;
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
			.text = AllText.Text_UIA[1065][Mainload.SetData[4]].Replace("@", AllText.Text_AllLike[int.Parse(Data_A[10])][Mainload.SetData[4]]);
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
		base.transform.Find("Info").Find("Data_FQ").GetComponent<Text>()
			.text = AllText.Text_UIA[1233][Mainload.SetData[4]].Replace("@", FuQiRelax);
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
		base.transform.Find("Info").Find("DataB_SF").Find("Text")
			.GetComponent<Text>()
			.text = PeiOuName;
		base.transform.Find("Info").Find("Data_FQ").gameObject.SetActive(isPeiOuLive);
		if (HuaiYunNum == "-1")
		{
			if (Data_A[4] == "0")
			{
				base.transform.Find("Info").Find("DataB_HT").Find("Text")
					.GetComponent<Text>()
					.text = AllText.Text_UIA[1098][Mainload.SetData[4]];
				base.transform.Find("Info").Find("DataB_HT").Find("Text")
					.GetComponent<Text>()
					.color = new Color(1f, 1f, 1f, 0.3f);
			}
			else
			{
				base.transform.Find("Info").Find("DataB_HT").Find("Text")
					.GetComponent<Text>()
					.text = AllText.Text_UIA[1099][Mainload.SetData[4]];
				base.transform.Find("Info").Find("DataB_HT").Find("Text")
					.GetComponent<Text>()
					.color = new Color(1f, 1f, 0.667f, 1f);
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
		if (FengHao != "null" && ShenFen[1] == "8")
		{
			base.transform.Find("Info").Find("DataB_FH").gameObject.SetActive(value: true);
			base.transform.Find("Info").Find("DataB_FH").Find("Text")
				.GetComponent<Text>()
				.text = FengHao;
		}
		else
		{
			base.transform.Find("Info").Find("DataB_FH").gameObject.SetActive(value: false);
		}
		for (int j = 0; j < base.transform.Find("AllCiTiao").Find("Viewport").Find("Content")
			.childCount; j++)
		{
			Object.Destroy(base.transform.Find("AllCiTiao").Find("Viewport").Find("Content")
				.GetChild(j)
				.gameObject);
			}
			int num = 0;
			if (Mainload.Member_King_qu[Mainload.MemberKingQuIndex_Enter][24] != "null")
			{
				string[] array = Mainload.Member_King_qu[Mainload.MemberKingQuIndex_Enter][24].Split('|');
				for (int k = 0; k < array.Length; k++)
				{
					if (int.Parse(Mainload.Member_King_qu[Mainload.MemberKingQuIndex_Enter][3]) >= int.Parse(Mainload.AllBuffData[int.Parse(array[k].Split('@')[0])][1]))
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
