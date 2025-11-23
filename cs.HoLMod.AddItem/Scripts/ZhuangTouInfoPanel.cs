using UnityEngine;
using UnityEngine.UI;

public class ZhuangTouInfoPanel : MonoBehaviour
{
	private GameObject MemberIconShow;

	private string ZTName;

	private string OldNum;

	private string ShouMingNum;

	private string Guanli;

	private string ZhongCheng;

	private string PinXing;

	private int FengdiIndex;

	private int NongZIndex;

	private int ZhongTouIndex;

	private void Awake()
	{
		MemberIconShow = (GameObject)Resources.Load("PerLiHuiBig");
	}

	private void Start()
	{
		initSize();
		initShow();
	}

	private void OnEnable()
	{
		OnEnableData();
		OnEnableShow();
	}

	private void initSize()
	{
		if (Mainload.SetData[4] == 0)
		{
			base.transform.Find("Info").Find("Data_NL").GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("Data_GL").GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("Data_ZC").GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("Data_PX").GetComponent<Text>()
				.fontSize = 16;
			base.transform.Find("Info").Find("Shenfen").Find("Text")
				.GetComponent<Text>()
				.fontSize = 16;
		}
		else
		{
			base.transform.Find("Info").Find("Data_NL").GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("Data_GL").GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("Data_ZC").GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("Data_PX").GetComponent<Text>()
				.fontSize = 15;
			base.transform.Find("Info").Find("Shenfen").Find("Text")
				.GetComponent<Text>()
				.fontSize = 15;
		}
	}

	private void initShow()
	{
		base.transform.Find("Info").Find("Shenfen").Find("Text")
			.GetComponent<Text>()
			.text = AllText.Text_UIA[1120][Mainload.SetData[4]];
	}

	private void OnEnableData()
	{
		FengdiIndex = int.Parse(Mainload.ZhuangTouData_Enter.Split('|')[0]);
		NongZIndex = int.Parse(Mainload.ZhuangTouData_Enter.Split('|')[1]);
		ZhongTouIndex = int.Parse(Mainload.ZhuangTouData_Enter.Split('|')[2]);
		ZTName = Mainload.ZhuangTou_now[FengdiIndex][NongZIndex][ZhongTouIndex][2].Split('|')[0];
		OldNum = Mainload.ZhuangTou_now[FengdiIndex][NongZIndex][ZhongTouIndex][3];
		ShouMingNum = Mainload.ZhuangTou_now[FengdiIndex][NongZIndex][ZhongTouIndex][2].Split('|')[2];
		Guanli = Mainload.ZhuangTou_now[FengdiIndex][NongZIndex][ZhongTouIndex][4];
		ZhongCheng = Mainload.ZhuangTou_now[FengdiIndex][NongZIndex][ZhongTouIndex][5];
		PinXing = Mainload.ZhuangTou_now[FengdiIndex][NongZIndex][ZhongTouIndex][2].Split('|')[3];
	}

	private void OnEnableShow()
	{
		base.transform.Find("Name").GetComponent<Text>().text = ZTName;
		base.transform.Find("Info").Find("Data_NL").GetComponent<Text>()
			.text = AllText.Text_UIA[1063][Mainload.SetData[4]].Replace("@", OldNum);
		base.transform.Find("Info").Find("Data_GL").GetComponent<Text>()
			.text = AllText.Text_UIA[1121][Mainload.SetData[4]].Replace("@", Guanli);
		base.transform.Find("Info").Find("Data_ZC").GetComponent<Text>()
			.text = AllText.Text_UIA[1122][Mainload.SetData[4]].Replace("@", ZhongCheng);
		base.transform.Find("Info").Find("Data_PX").GetComponent<Text>()
			.text = AllText.Text_UIA[1076][Mainload.SetData[4]].Replace("@", AllText.Text_PinXing[int.Parse(PinXing)][Mainload.SetData[4]]);
		for (int i = 0; i < base.transform.Find("IconShow").childCount; i++)
		{
			Object.Destroy(base.transform.Find("IconShow").GetChild(i).gameObject);
		}
		GameObject gameObject = Object.Instantiate(MemberIconShow);
		gameObject.name = Mainload.ZhuangTouData_Enter.Split('|')[2];
		gameObject.transform.GetComponent<PerLiHuiBig>().FengdiIndex = int.Parse(Mainload.ZhuangTouData_Enter.Split('|')[0]);
		gameObject.transform.GetComponent<PerLiHuiBig>().NongZIndex = int.Parse(Mainload.ZhuangTouData_Enter.Split('|')[1]);
		gameObject.transform.GetComponent<PerLiHuiBig>().ShowID = 13;
		gameObject.transform.GetComponent<PerLiHuiBig>().isShowInfo = false;
		gameObject.transform.SetParent(base.transform.Find("IconShow"));
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
	}
}
