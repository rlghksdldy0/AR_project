using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using WebSocketSharp;
using UnityEngine.UI;

public class GpsTest4 : MonoBehaviour
{
	// 서버 API를 Json으로 가져오고 리스트에 저장해서 광고판 오브젝트를 뿌리고 실시간 위치조정하는 코드
	JsonTextReader ARListJson;

	public List<string> ARList_pid = new List<string>(); // 값을 저장할 리스트 생성
	public List<string> ARList_xCrdnt = new List<string>();
	public List<string> ARList_yCrdnt = new List<string>();
	public List<string> ARList_Name = new List<string>();
	public List<string> ARList_category = new List<string>();
	public List<string> ARList_addrStreet = new List<string>();

	//GPSManager gpsmanager;

	double add_constant = 0;
	double multyple_constant = 0.01f;



	GameObject[] Shop = new GameObject[10];
	public GameObject object1;

	int index_num = 0;
	int i = 0;
	int a = 0;
	float[] Divice_GPS_lat = new float[100]; // 광고판의 유니티상 위치가 될 배열
	float[] Divice_GPS_lon = new float[100];
	bool shop_at = true;

	string strMsg;
	public string lat;
	public string lng;
	bool islat = false;
	bool islng = false;


	private WebSocket m_WebSocket;

	[SerializeField] Text test1;
	string aaa;
	string jsonGPS;

	double[] test = new double[10];
	float[] test2 = new float[10];




	public void request_stationListCaching() //프로그램 실행 후 최초 1회 정류장 리스트 캐싱
	{ // 사이트에서 값 가져옴
		using (WebClient wc = new WebClient())
		{


			var json = new WebClient().DownloadString("http://gotouch.iptime.org:18162/arad/api/ad/List.do?lat=37.545822110628954&lng=127.05014945585616"); // 서버에서 json 리스트 다운
			string busjson = json.ToString(); // 다운받은 데이터 스트링으로 변환

			ARListJson = new JsonTextReader(new StringReader(busjson));


			bool isspid = false; //키와 벨류값을 구분할 bool값
			bool isxCrdnt = false;
			bool isyCrdnt = false;
			bool isName = false;
			bool iscategory = false;
			bool isaddrStreet = false;

			bool isowner_name = false;
			bool isowner_pid = false;

			ARList_pid.Clear();
			ARList_xCrdnt.Clear();
			ARList_yCrdnt.Clear();
			ARList_Name.Clear();
			ARList_category.Clear();

			//float[] Divice_GPS_lat = new float[10]; // 광고판의 유니티상 위치가 될 배열
			//float[] Divice_GPS_lon = new float[10]; 


			while (ARListJson.Read())
			{
				if (ARListJson.Value != null) //키와 밸류가 표시됨 (키 : TokenType이 PropertyName, 밸류 : TokenType이 String)
				{

					if (ARListJson.TokenType.ToString() == "PropertyName") // 항목 구분
					{
						if (ARListJson.Value.ToString() == "lng") { isyCrdnt = true; } // 지정된 항목이 있으면 true
						if (ARListJson.Value.ToString() == "pid" && isowner_pid == false) { isspid = true; }
						if (ARListJson.Value.ToString() == "category") { iscategory = true; }
						if (ARListJson.Value.ToString() == "name" && isowner_name == false) { isName = true; }
						if (ARListJson.Value.ToString() == "addrStreet") { isaddrStreet = true; }
						if (ARListJson.Value.ToString() == "lat") { isxCrdnt = true; }
						if (ARListJson.Value.ToString() == "owner") { isowner_name = true; isowner_pid = true; }


					}


					if (ARListJson.TokenType.ToString() == "String") // 밸류값
					{
						if (isspid) { ARList_pid.Add(ARListJson.Value.ToString()); isspid = false; } // 지정된 항목의 데이터를 저장하고 false
						if (isxCrdnt) { ARList_xCrdnt.Add(ARListJson.Value.ToString()); isxCrdnt = false; }
						if (isyCrdnt) { ARList_yCrdnt.Add(ARListJson.Value.ToString()); isyCrdnt = false; }
						if (isName) { ARList_Name.Add(ARListJson.Value.ToString()); isName = false; }
						if (iscategory) { ARList_category.Add(ARListJson.Value.ToString()); iscategory = false; }
						if (isaddrStreet) { ARList_addrStreet.Add(ARListJson.Value.ToString()); isaddrStreet = false; }
					}
					/*if (ARList_yCrdnt[index_num] != null)
					{
						if (float.Parse(ARList_xCrdnt[i]) - 0.05f < gpsmanager.lon && float.Parse(ARList_xCrdnt[i]) + 0.05f > gpsmanager.lon && float.Parse(ARList_yCrdnt[i]) - 0.05f < gpsmanager.lat && float.Parse(ARList_yCrdnt[i]) + 0.05f > gpsmanager.lat) // 오브젝트 생성을 위한 범위값 지정
						{
							if (shop_at == true) // 하나의 광고를 생성하기 위한 bool값
							{
								Divice_GPS_lat[i] = req_to_real(gpsmanager.lat) - req_to_real(double.Parse(ARList_yCrdnt[i])); // 유니티상의 y좌표 계산
								Divice_GPS_lon[i] = req_to_real(gpsmanager.lon) - req_to_real(double.Parse(ARList_xCrdnt[i])); // 유니티상의 x좌표 계산
								Shop[i] = (GameObject)Instantiate(object1, new Vector3(Divice_GPS_lon[i], 0f, Divice_GPS_lat[i]), Quaternion.identity); // i값에 따른 오브젝트 생성
								Shop[i].name = "Shop" + i + 1; // 생성된 오브젝트에 구분할 이름 부여
								if (i < 10) { i++; }
								shop_at = false; // 광고판 생성후 flase
							}

						}
						else // 광고판 생성범위를 벗어났을 경우 true
						{
							shop_at = true;
						}
					}*/
				}
				else
				{
					if (isowner_pid && isowner_name)
					{
						if (ARListJson.TokenType.ToString() == "EndObject")
						{
							isowner_name = false;
							isowner_pid = false;
						}
					}
				}


			}

		}


	}

	int req_to_real(double n) // gps를 유니티에 표현하기위한 계산식
	{
		double a = (n * 10);
		return (int)(a);
	}
	void out_Info()
	{

		if (Shop[a] != null)
		{
			//Shop[a].transform.position = new Vector3(Divice_GPS_lat[a], 0, Divice_GPS_lon[a]);
			Shop[a].transform.position = new Vector3(Divice_GPS_lat[a], 0, Divice_GPS_lon[a]);
			a++;
			if (a == 10)
			{
				a = 0;
			}
		}


		if (ARList_yCrdnt[index_num] != null)
		{
			//if (float.Parse(ARList_xCrdnt[i]) - 500f < double.Parse(lng) && float.Parse(ARList_xCrdnt[i]) + 500f > double.Parse(lng) && float.Parse(ARList_yCrdnt[i]) - 500f < double.Parse(lat) && float.Parse(ARList_yCrdnt[i]) + 500f > double.Parse(lat))
			//{
			if (shop_at == true)
			{
				Divice_GPS_lat[i] = (float)((double.Parse(lng) * 100000f) - ((double.Parse(ARList_yCrdnt[i]) * 100000f)));
				//Debug.Log(req_to_real(float.Parse(lat) - float.Parse(ARList_yCrdnt[i])));
				//Divice_GPS_lon[i] = req_to_real(double.Parse(lng) - double.Parse(ARList_xCrdnt[i]));
				Divice_GPS_lon[i] = (float)((double.Parse(lat) * 100000f) - ((double.Parse(ARList_xCrdnt[i]) * 100000f)));
				//Shop[i] = (GameObject)Instantiate(object1, new Vector3(Divice_GPS_lon[i], 0f, Divice_GPS_lat[i]), Quaternion.identity);
				Shop[i] = (GameObject)Instantiate(object1, new Vector3(Divice_GPS_lat[i], 0f, Divice_GPS_lon[i]), Quaternion.identity);
				test[i] = Math.Sqrt(Math.Abs(Divice_GPS_lon[i]) + Math.Abs(Divice_GPS_lat[i])) * multyple_constant + add_constant;
				test2[i] = (float)test[i];
				Shop[i].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

				Shop[i].name = "Shop" + i;
				i++;
				
				if (i > 100)
				{
					shop_at = false;
				}
			}

			//}
			//else
			//{
			//	shop_at = true;
			//}
		}


	}

	public void Start()
	{
		//gpsmanager = GameObject.Find("ARCamera").GetComponent<GPSManager>();
		request_stationListCaching();
		InvokeRepeating("out_Info", 0.5f, 0.5f);


		m_WebSocket = new WebSocket("ws://220.117.107.150:8000/");
		m_WebSocket.Connect();

		m_WebSocket.OnMessage += (sender, e) =>
		{
			aaa = e.Data;

			//Debug.Log($"{((WebSocket)sender).Url}에서 + 데이터 : {e.Data}가 옴.");
			strMsg = e.Data;

			//Debug.Log(lat);
			//Debug.Log(lng);
			if (strMsg != null)
			{
				JsonTextReader reader = new JsonTextReader(new StringReader(strMsg));
				while (reader.Read())
				{
					if (reader.Value != null)
					{
						if (reader.TokenType.ToString() == "PropertyName")
						{
							if (reader.Value.ToString() == "lat") { islat = true; }
							if (reader.Value.ToString() == "lng") { islng = true; }
						}

						if (reader.TokenType.ToString() == "String")
						{
							if (islat)
							{
								lat = reader.Value.ToString();
								islat = false;
							}

							if (islng)
							{
								lng = reader.Value.ToString();
								islng = false;
							}

						}

					}

				}

			}

		};

	}
	public void Update()
	{


	}
}
