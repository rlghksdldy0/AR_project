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
    { // 사이트에서 값 가져옴
        using (WebClient wc = new WebClient())
        {
            //busjson으로 파싱한 json값이 string형태로 저장됨
            var json = new WebClient().DownloadString("http://gotouch.iptime.org:18152/changwonbus/api/demo/RouteList.do");
            string busjson = json.ToString();

            JsonTextReader busjsonRead = new JsonTextReader(new StringReader(busjson));

            string busValueListTemp = "";
            string busKeyListTemp = "";

            while (busjsonRead.Read())
            {


                if (busjsonRead.Value != null) //키와 밸류가 표시됨 (키 : TokenType이 PropertyName, 밸류 : TokenType이 String)
                {
                    //Debug.Log(busjsonRead.Value);

                    if (busjsonRead.TokenType.ToString() == "PropertyName") // 키 텍스트에 표시
                    {
                        if (!(busjsonRead.Value.ToString() == "result") && !(busjsonRead.Value.ToString() == "routes")) // 버스정보가 나오기 전에 맨 처음 나오는 루트 키 등은 제외
                        {
                            string busKeyTempKey = busjsonRead.Value.ToString();
                            busKeyListTemp += busKeyTempKey + "\n";



                        }
                    }


                    if (busjsonRead.TokenType.ToString() == "String") // 밸류 텍스트에 표시
                    {
                        string busKeyTempValue = busjsonRead.Value.ToString();
                        busValueListTemp += busKeyTempValue + "\n";


                    }



                }
                else
                {
                    Debug.Log(busjsonRead.TokenType); //StartObject, EndObject, StartArray, EndArray

                    if (busjsonRead.TokenType.ToString() == "StartObject") // " { " 중괄호가 시작됐을 때 
                    {
                        Debug.Log("하나 시작");
                    }

                    if (busjsonRead.TokenType.ToString() == "EndObject")
                    {
                        busKeyList.Add(busKeyListTemp);
                        busValueList.Add(busValueListTemp);

                        busValueListTemp = "";
                        busKeyListTemp = "";

                    }

                }// if (busjsonRead.Value != null) else 끝부분



            }//처음 while (busjsonRead.Read()) 끝부분
        }

        Debug.Log("사이트에서 정보를 받아옵니다...");
    }


    private void textOutput(int busNum) // 검색한 버스 번호를 화면에 뿌려줌
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
