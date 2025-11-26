using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenKeNowInfoPanel : MonoBehaviour
{
	private string[] DataA;

	private string OldNow;

	private string ShengYu;

	private string GongZi;

	private string Meili;

	private string SkillLv;

	private string Wen;

	private string Wu;

	private string Shang;

	private string Yi;

	private string JiMou;

	private string XinQing;

	private string Tili;

	private string bodyNum;

	private string HuaiYunData;

	private float PayXInitSize;

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
		initShow();
	}

	private void OnEnable()
	{
		if (Mainload.MenkeIndex_Enter >= 0)
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
			base.transform.Find("Info").Find("Data_TL").GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("Data_NL").GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("Data_XF").GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("Data_ML").GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("Data_PX").GetComponent<Text>()
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
			base.transform.Find("Info").Find("Data_body").GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("Data_TF").GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("Data_XY").GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("Shenfen").Find("Text")
				.GetComponent<Text>()
				.fontSize = 16;
		}
		else
		{
			base.transform.Find("Info").Find("Data_SY").GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("Data_TL").GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("Data_NL").GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("Data_XF").GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("Data_ML").GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("Data_PX").GetComponent<Text>()
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
			base.transform.Find("Info").Find("Data_body").GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("Data_TF").GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("Data_XY").GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("Shenfen").Find("Text")
				.GetComponent<Text>()
				.fontSize = 15;
		}
	}

	private void initShow()
	{
		base.transform.Find("Info").Find("Coins").Find("TipA")
			.GetComponent<Text>()
			.text = AllText.Text_UIA[221][Mainload.SetData[4]];
	}

	private void OnEnableData()
	{
		DataA = Mainload.MenKe_Now[Mainload.MenkeIndex_Enter][2].Split('|');
		OldNow = Mainload.MenKe_Now[Mainload.MenkeIndex_Enter][3];
		ShengYu = Mathf.FloorToInt(float.Parse(Mainload.MenKe_Now[Mainload.MenkeIndex_Enter][11])).ToString();
		GongZi = Mainload.MenKe_Now[Mainload.MenkeIndex_Enter][18];
		SkillLv = Mainload.MenKe_Now[Mainload.MenkeIndex_Enter][16];
		Meili = Mainload.MenKe_Now[Mainload.MenkeIndex_Enter][13];
		Wen = float.Parse(Mainload.MenKe_Now[Mainload.MenkeIndex_Enter][4]).ToString("f0");
		Wu = float.Parse(Mainload.MenKe_Now[Mainload.MenkeIndex_Enter][5]).ToString("f0");
		Shang = float.Parse(Mainload.MenKe_Now[Mainload.MenkeIndex_Enter][6]).ToString("f0");
		Yi = float.Parse(Mainload.MenKe_Now[Mainload.MenkeIndex_Enter][7]).ToString("f0");
		JiMou = Mainload.MenKe_Now[Mainload.MenkeIndex_Enter][15];
		XinQing = Mainload.MenKe_Now[Mainload.MenkeIndex_Enter][8];
		Tili = Mainload.MenKe_Now[Mainload.MenkeIndex_Enter][19];
		bodyNum = Mainload.MenKe_Now[Mainload.MenkeIndex_Enter][14];
		HuaiYunData = Mainload.MenKe_Now[Mainload.MenkeIndex_Enter][17];
		if (Mainload.SetData[4] == 0)
		{
			PayXInitSize = -306f;
		}
		else
		{
			PayXInitSize = -330f;
		}
	}

	private void OnEnableShow()
	{
		base.transform.Find("Name").GetComponent<Text>().text = "@".Replace("@", DataA[0]);
		for (int i = 0; i < base.transform.Find("IconShow").childCount; i++)
		{
			Object.Destroy(base.transform.Find("IconShow").GetChild(i).gameObject);
		}
		GameObject obj = Object.Instantiate(MemberIconShow);
		obj.name = Mainload.MenkeIndex_Enter.ToString();
		obj.transform.GetComponent<PerLiHuiBig>().ShowID = 4;
		obj.transform.GetComponent<PerLiHuiBig>().isShowInfo = false;
		obj.transform.SetParent(base.transform.Find("IconShow"));
		obj.transform.localScale = new Vector3(1f, 1f, 1f);
		obj.transform.localPosition = new Vector3(0f, 0f, 0f);
		base.transform.Find("Info").Find("Data_SY").GetComponent<Text>()
			.text = AllText.Text_UIA[1061][Mainload.SetData[4]].Replace("@", ShengYu);
		base.transform.Find("Info").Find("Data_ML").GetComponent<Text>()
			.text = AllText.Text_UIA[1072][Mainload.SetData[4]].Replace("@", Meili);
		base.transform.Find("Info").Find("Data_NL").GetComponent<Text>()
			.text = AllText.Text_UIA[1063][Mainload.SetData[4]].Replace("@", OldNow);
		base.transform.Find("Info").Find("Data_TL").GetComponent<Text>()
			.text = AllText.Text_UIA[1100][Mainload.SetData[4]].Replace("@", Tili);
		base.transform.Find("Info").Find("Data_body").GetComponent<Text>()
			.text = AllText.Text_UIA[1064][Mainload.SetData[4]].Replace("@", bodyNum);
		if (Mainload.SetData[4] == 1)
		{
			if (DataA[6] == "0")
			{
				base.transform.Find("Info").Find("Data_JN").GetComponent<Text>()
					.text = AllText.Text_UIA[1066][Mainload.SetData[4]].Replace("@", AllText.Text_AllMemberSkill[int.Parse(DataA[6])][Mainload.SetData[4]]).Replace("$", SkillLv);
			}
			else
			{
				base.transform.Find("Info").Find("Data_JN").GetComponent<Text>()
					.text = AllText.Text_UIA[1066][Mainload.SetData[4]].Replace("@", AllText.Text_AllMemberSkill[int.Parse(DataA[6])][Mainload.SetData[4]].Substring(0, 2).ToUpper()).Replace("$", SkillLv);
			}
			if (DataA[2] == "0")
			{
				base.transform.Find("Info").Find("Data_TF").GetComponent<Text>()
					.text = AllText.Text_UIA[1067][Mainload.SetData[4]].Replace("@", AllText.Text_AllTianFu[int.Parse(DataA[2])][Mainload.SetData[4]]).Replace("$", DataA[3]);
			}
			else
			{
				base.transform.Find("Info").Find("Data_TF").GetComponent<Text>()
					.text = AllText.Text_UIA[1067][Mainload.SetData[4]].Replace("@", AllText.Text_AllTianFu[int.Parse(DataA[2])][Mainload.SetData[4]].Substring(0, 2).ToUpper()).Replace("$", DataA[3]);
			}
		}
		else
		{
			base.transform.Find("Info").Find("Data_JN").GetComponent<Text>()
				.text = AllText.Text_UIA[1066][Mainload.SetData[4]].Replace("@", AllText.Text_AllMemberSkill[int.Parse(DataA[6])][Mainload.SetData[4]]).Replace("$", SkillLv);
			base.transform.Find("Info").Find("Data_TF").GetComponent<Text>()
				.text = AllText.Text_UIA[1067][Mainload.SetData[4]].Replace("@", AllText.Text_AllTianFu[int.Parse(DataA[2])][Mainload.SetData[4]]).Replace("$", DataA[3]);
		}
		base.transform.Find("Info").Find("Data_Wen").GetComponent<Text>()
			.text = AllText.Text_UIA[1068][Mainload.SetData[4]].Replace("@", Wen);
		base.transform.Find("Info").Find("Data_Wu").GetComponent<Text>()
			.text = AllText.Text_UIA[1069][Mainload.SetData[4]].Replace("@", Wu);
		base.transform.Find("Info").Find("Data_Shang").GetComponent<Text>()
			.text = AllText.Text_UIA[1070][Mainload.SetData[4]].Replace("@", Shang);
		base.transform.Find("Info").Find("Data_Yi").GetComponent<Text>()
			.text = AllText.Text_UIA[1071][Mainload.SetData[4]].Replace("@", Yi);
		base.transform.Find("Info").Find("Data_JM").GetComponent<Text>()
			.text = AllText.Text_UIA[1075][Mainload.SetData[4]].Replace("@", JiMou);
		base.transform.Find("Info").Find("Data_XF").GetComponent<Text>()
			.text = AllText.Text_UIA[1074][Mainload.SetData[4]].Replace("@", XinQing);
		base.transform.Find("Info").Find("Data_PX").GetComponent<Text>()
			.text = AllText.Text_UIA[1076][Mainload.SetData[4]].Replace("@", AllText.Text_PinXing[int.Parse(DataA[8])][Mainload.SetData[4]]);
		base.transform.Find("Info").Find("Data_XY").GetComponent<Text>()
			.text = AllText.Text_UIA[1073][Mainload.SetData[4]].Replace("@", DataA[7]);
		base.transform.Find("Info").Find("Coins").Find("Num")
			.GetComponent<Text>()
			.text = AllText.Text_UIA[117][Mainload.SetData[4]].Replace("@", GongZi);
		base.transform.Find("Info").Find("Shenfen").Find("Text")
			.GetComponent<Text>()
			.text = AllText.Text_UIA[267][Mainload.SetData[4]];
		if (HuaiYunData == "-1")
		{
			base.transform.Find("Info").Find("Data_HT").gameObject.SetActive(value: false);
			base.transform.Find("Info").Find("Coins").localPosition = new Vector3(PayXInitSize, -460f, 0f);
		}
		else if (HuaiYunData == "10")
		{
			base.transform.Find("Info").Find("Data_HT").gameObject.SetActive(value: true);
			base.transform.Find("Info").Find("Coins").localPosition = new Vector3(PayXInitSize, -430f, 0f);
			base.transform.Find("Info").Find("Data_HT").Find("Text")
				.GetComponent<Text>()
				.text = AllText.Text_UIA[1327][Mainload.SetData[4]];
		}
		else
		{
			base.transform.Find("Info").Find("Data_HT").gameObject.SetActive(value: true);
			base.transform.Find("Info").Find("Coins").localPosition = new Vector3(PayXInitSize, -430f, 0f);
			base.transform.Find("Info").Find("Data_HT").Find("Text")
				.GetComponent<Text>()
				.text = AllText.Text_UIA[1079][Mainload.SetData[4]].Replace("@", (10 - int.Parse(HuaiYunData)).ToString());
		}
		for (int j = 0; j < base.transform.Find("AllCiTiao").Find("Viewport").Find("Content")
			.childCount; j++)
		{
			Object.Destroy(base.transform.Find("AllCiTiao").Find("Viewport").Find("Content")
				.GetChild(j)
				.gameObject);
			}
			int num = 0;
			if (Mainload.MenKe_Now[Mainload.MenkeIndex_Enter][21] != "null")
			{
				string[] array = Mainload.MenKe_Now[Mainload.MenkeIndex_Enter][21].Split('|');
				for (int k = 0; k < array.Length; k++)
				{
					if (int.Parse(Mainload.MenKe_Now[Mainload.MenkeIndex_Enter][3]) >= int.Parse(Mainload.AllBuffData[int.Parse(array[k].Split('@')[0])][1]))
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
			List<string> shuXingCiTiao = FormulaData.GetShuXingCiTiao(int.Parse(OldNow), float.Parse(Wen), float.Parse(Wu), float.Parse(Shang), float.Parse(Yi), float.Parse(JiMou), float.Parse(Meili), float.Parse(DataA[7]), float.Parse(ShengYu), float.Parse(DataA[5]), 0, 0, 0);
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
