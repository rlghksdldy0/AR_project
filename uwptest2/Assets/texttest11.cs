using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;


public class texttest11 : MonoBehaviour
{
    // Start is called before the first frame update

    public Text busKey;
    public Text busValue;
    public InputField busNum;

    List<string> busKeyList = new List<string>();
    List<string> busValueList = new List<string>();



    private void request()
    { // ����Ʈ���� �� ������
        using (WebClient wc = new WebClient())
        {
            //busjson���� �Ľ��� json���� string���·� �����
            var json = new WebClient().DownloadString("http://gotouch.iptime.org:18152/changwonbus/api/demo/RouteList.do");
            string busjson = json.ToString();

            JsonTextReader busjsonRead = new JsonTextReader(new StringReader(busjson));

            string busValueListTemp = "";
            string busKeyListTemp = "";

            while (busjsonRead.Read())
            {


                if (busjsonRead.Value != null) //Ű�� ����� ǥ�õ� (Ű : TokenType�� PropertyName, ��� : TokenType�� String)
                {
                    //Debug.Log(busjsonRead.Value);

                    if (busjsonRead.TokenType.ToString() == "PropertyName") // Ű �ؽ�Ʈ�� ǥ��
                    {
                        if (!(busjsonRead.Value.ToString() == "result") && !(busjsonRead.Value.ToString() == "routes")) // ���������� ������ ���� �� ó�� ������ ��Ʈ Ű ���� ����
                        {
                            string busKeyTempKey = busjsonRead.Value.ToString();
                            busKeyListTemp += busKeyTempKey + "\n";



                        }
                    }


                    if (busjsonRead.TokenType.ToString() == "String") // ��� �ؽ�Ʈ�� ǥ��
                    {
                        string busKeyTempValue = busjsonRead.Value.ToString();
                        busValueListTemp += busKeyTempValue + "\n";


                    }



                }
                else
                {
                    Debug.Log(busjsonRead.TokenType); //StartObject, EndObject, StartArray, EndArray

                    if (busjsonRead.TokenType.ToString() == "StartObject") // " { " �߰�ȣ�� ���۵��� �� 
                    {
                        Debug.Log("�ϳ� ����");
                    }

                    if (busjsonRead.TokenType.ToString() == "EndObject")
                    {
                        busKeyList.Add(busKeyListTemp);
                        busValueList.Add(busValueListTemp);

                        busValueListTemp = "";
                        busKeyListTemp = "";

                    }

                }// if (busjsonRead.Value != null) else ���κ�



            }//ó�� while (busjsonRead.Read()) ���κ�
        }

        Debug.Log("����Ʈ���� ������ �޾ƿɴϴ�...");
    }


    private void textOutput(int busNum) // �˻��� ���� ��ȣ�� ȭ�鿡 �ѷ���
    {
        busKey.GetComponent<Text>().text = busKeyList[busNum];
        busValue.GetComponent<Text>().text = busValueList[busNum];
    }






    void Start()
    {
        request();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            textOutput(int.Parse(busNum.text) - 1);
        }
    }
}
