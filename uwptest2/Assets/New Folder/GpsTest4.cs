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
	// ���� API�� Json���� �������� ����Ʈ�� �����ؼ� ������ ������Ʈ�� �Ѹ��� �ǽð� ��ġ�����ϴ� �ڵ�
	JsonTextReader ARListJson;

	public List<string> ARList_pid = new List<string>(); // ���� ������ ����Ʈ ����
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
	float[] Divice_GPS_lat = new float[100]; // �������� ����Ƽ�� ��ġ�� �� �迭
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




	public void request_stationListCaching() //���α׷� ���� �� ���� 1ȸ ������ ����Ʈ ĳ��
	{ // ����Ʈ���� �� ������
		using (WebClient wc = new WebClient())
		{


			var json = new WebClient().DownloadString("http://gotouch.iptime.org:18162/arad/api/ad/List.do?lat=37.545822110628954&lng=127.05014945585616"); // �������� json ����Ʈ �ٿ�
			string busjson = json.ToString(); // �ٿ���� ������ ��Ʈ������ ��ȯ

			ARListJson = new JsonTextReader(new StringReader(busjson));


			bool isspid = false; //Ű�� �������� ������ bool��
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

			//float[] Divice_GPS_lat = new float[10]; // �������� ����Ƽ�� ��ġ�� �� �迭
			//float[] Divice_GPS_lon = new float[10]; 


			while (ARListJson.Read())
			{
				if (ARListJson.Value != null) //Ű�� ����� ǥ�õ� (Ű : TokenType�� PropertyName, ��� : TokenType�� String)
				{

					if (ARListJson.TokenType.ToString() == "PropertyName") // �׸� ����
					{
						if (ARListJson.Value.ToString() == "lng") { isyCrdnt = true; } // ������ �׸��� ������ true
						if (ARListJson.Value.ToString() == "pid" && isowner_pid == false) { isspid = true; }
						if (ARListJson.Value.ToString() == "category") { iscategory = true; }
						if (ARListJson.Value.ToString() == "name" && isowner_name == false) { isName = true; }
						if (ARListJson.Value.ToString() == "addrStreet") { isaddrStreet = true; }
						if (ARListJson.Value.ToString() == "lat") { isxCrdnt = true; }
						if (ARListJson.Value.ToString() == "owner") { isowner_name = true; isowner_pid = true; }


					}


					if (ARListJson.TokenType.ToString() == "String") // �����
					{
						if (isspid) { ARList_pid.Add(ARListJson.Value.ToString()); isspid = false; } // ������ �׸��� �����͸� �����ϰ� false
						if (isxCrdnt) { ARList_xCrdnt.Add(ARListJson.Value.ToString()); isxCrdnt = false; }
						if (isyCrdnt) { ARList_yCrdnt.Add(ARListJson.Value.ToString()); isyCrdnt = false; }
						if (isName) { ARList_Name.Add(ARListJson.Value.ToString()); isName = false; }
						if (iscategory) { ARList_category.Add(ARListJson.Value.ToString()); iscategory = false; }
						if (isaddrStreet) { ARList_addrStreet.Add(ARListJson.Value.ToString()); isaddrStreet = false; }
					}
					/*if (ARList_yCrdnt[index_num] != null)
					{
						if (float.Parse(ARList_xCrdnt[i]) - 0.05f < gpsmanager.lon && float.Parse(ARList_xCrdnt[i]) + 0.05f > gpsmanager.lon && float.Parse(ARList_yCrdnt[i]) - 0.05f < gpsmanager.lat && float.Parse(ARList_yCrdnt[i]) + 0.05f > gpsmanager.lat) // ������Ʈ ������ ���� ������ ����
						{
							if (shop_at == true) // �ϳ��� ���� �����ϱ� ���� bool��
							{
								Divice_GPS_lat[i] = req_to_real(gpsmanager.lat) - req_to_real(double.Parse(ARList_yCrdnt[i])); // ����Ƽ���� y��ǥ ���
								Divice_GPS_lon[i] = req_to_real(gpsmanager.lon) - req_to_real(double.Parse(ARList_xCrdnt[i])); // ����Ƽ���� x��ǥ ���
								Shop[i] = (GameObject)Instantiate(object1, new Vector3(Divice_GPS_lon[i], 0f, Divice_GPS_lat[i]), Quaternion.identity); // i���� ���� ������Ʈ ����
								Shop[i].name = "Shop" + i + 1; // ������ ������Ʈ�� ������ �̸� �ο�
								if (i < 10) { i++; }
								shop_at = false; // ������ ������ flase
							}

						}
						else // ������ ���������� ����� ��� true
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

	int req_to_real(double n) // gps�� ����Ƽ�� ǥ���ϱ����� ����
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

			//Debug.Log($"{((WebSocket)sender).Url}���� + ������ : {e.Data}�� ��.");
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
