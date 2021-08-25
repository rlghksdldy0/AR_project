using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using UnityEngine.UI;
public class objecttest2 : MonoBehaviour
{
    // 생성된 오브젝트의 이미지, 텍스트를 표출하는 코드
    JsonTextReader ARListJson;

    List<string> ARList_pid = new List<string>();
    List<string> ARList_xCrdnt = new List<string>();
    List<string> ARList_yCrdnt = new List<string>();
    List<string> ARList_Name = new List<string>();
    List<string> ARList_category = new List<string>();
    List<string> ARList_addrStreet = new List<string>();

    [SerializeField] Text test1; // 텍스트 표출
    [SerializeField] Text test2;
    int i = 0;
    public Image imagecast_shop;

    GpsTest4 gpstest4;

    double add_constant = 0;
    double multyple_constant = 0.01f;

    private void request_ARListCaching() //프로그램 실행 후 최초 1회 정류장 리스트 캐싱
    { // 사이트에서 값 가져옴
        using (WebClient wc = new WebClient())
        {


            var json = new WebClient().DownloadString("http://gotouch.iptime.org:18162/arad/api/ad/List.do?lat=37.545822110628954&lng=127.05014945585616");
            string busjson = json.ToString();

            ARListJson = new JsonTextReader(new StringReader(busjson));


            bool isspid = false;
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

            while (ARListJson.Read())
            {
                if (ARListJson.Value != null) //키와 밸류가 표시됨 (키 : TokenType이 PropertyName, 밸류 : TokenType이 String)
                {

                    if (ARListJson.TokenType.ToString() == "PropertyName")
                    {
                        if (ARListJson.Value.ToString() == "lng") { isyCrdnt = true; }
                        if (ARListJson.Value.ToString() == "pid" && isowner_pid == false) { isspid = true; }
                        if (ARListJson.Value.ToString() == "category") { iscategory = true; }
                        if (ARListJson.Value.ToString() == "name" && isowner_name == false) { isName = true; }
                        if (ARListJson.Value.ToString() == "addrStreet") { isaddrStreet = true; }
                        if (ARListJson.Value.ToString() == "lat") { isxCrdnt = true; }
                        if (ARListJson.Value.ToString() == "owner") { isowner_name = true; isowner_pid = true; }

                    }


                    if (ARListJson.TokenType.ToString() == "String") // 밸류값
                    {
                        if (isspid) { ARList_pid.Add(ARListJson.Value.ToString()); isspid = false; }
                        if (isxCrdnt) { ARList_xCrdnt.Add(ARListJson.Value.ToString()); isxCrdnt = false; }
                        if (isyCrdnt) { ARList_yCrdnt.Add(ARListJson.Value.ToString()); isyCrdnt = false; }
                        if (isName) { ARList_Name.Add(ARListJson.Value.ToString()); isName = false; }
                        if (iscategory) { ARList_category.Add(ARListJson.Value.ToString()); iscategory = false; }
                        if (isaddrStreet) { ARList_addrStreet.Add(ARListJson.Value.ToString()); isaddrStreet = false; }
                    }
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

    void SetPosition()
    {
        float nowLat = (float)((double.Parse(gpstest4.lat) * 500000f) - double.Parse(ARList_xCrdnt[i]) * 500000f);
        float nowLng = (float)((double.Parse(gpstest4.lng) * 500000f) - double.Parse(ARList_yCrdnt[i]) * 500000f);

        double scale_1 = Math.Sqrt(Math.Abs(nowLat) + Math.Abs(nowLng)) * multyple_constant + add_constant;
        float scale_2 = (float)scale_1;

        transform.position = new Vector3(nowLat, 0, nowLng);
        transform.localScale = new Vector3(1, 1, 1);
    }

    private void Start()
    {
        gpstest4 = GameObject.Find("GameObject").GetComponent<GpsTest4>();

        request_ARListCaching();
        object_test();
        InvokeRepeating("SetPosition", 0.1f, 0.1f);
    }
    void object_test()
    {


        Debug.Log(i);
        for (int a = 1; a < 100; a++)
        {
            if (this.gameObject.name == "Shop" + i) // 오브젝트 이름으로 광고 구분
            {


                //fileDownload("http://gotouch.iptime.org:18160/anydev/svc/data/file/arad/content/ad/1629644436179-0700.png", @"D:\New Unity Project\testt" + i + ".png"); // 절대경로 저장


                //fileDownload("http://gotouch.iptime.org:18160/anydev/svc/data/file/arad/content/ad/1629792267386-1420.png", Application.persistentDataPath + "test" + i + ".png"); // 안드로이드 리소스폴더에 저장
                string path_cat = Application.persistentDataPath + "test" + i + ".png"; // 다운받은 사진 불러오기
                                                                                        // string path_cat = @"D:\New Unity Project\" + "testt" + i + ".png"; // 다운받은 사진 불러오기
                Texture2D texture_cat = new Texture2D(0, 0);
                byte[] byteTexture_cat = System.IO.File.ReadAllBytes(path_cat); // 불러온 사진 byte값으로 변환

                texture_cat = new Texture2D(110, 110); // 크기지정
                texture_cat.LoadImage(byteTexture_cat); // 이미지로 로드
                imagecast_shop.sprite = Sprite.Create(texture_cat, new Rect(0, 0, 110, 110), new Vector2()); // sprite형식으로 표출



                test1.text = ARList_Name[i].ToString(); // 리스트로 불러온 텍스트값 표출
                test2.text = ARList_category[i].ToString();

            }
            else
            {
                i++; // 해당하는 번호가 아닐시 번호상승
            }

        }
    }
    public void fileDownload(string url, string path) // 웹에서 이미지를 다운받아 저장하기 위한 함수

    {

        try

        {

            WebClient webClient = new WebClient();

            webClient.DownloadFile(url, path);

        }
        catch (Exception e)

        {

            Console.WriteLine(e);

            Console.ReadLine();

        }

    }
    /*
    // 이미지 다운받고 적용
    int i = 0;
    public Image imagecast_shop;


    private Sprite[] sprites;

    void Start()
    {
        object_test();
    }

    void object_test()
    {
        while (true)
        {
            if (this.gameObject.name == "shop" + i)
            {

                fileDownload("http://psycure.ipdisk.co.kr:8088/cat" + i, @"C:\Users\PCR3\Desktop\aaa\New Unity Project\Assets\Resources\shop" + i + ".png");
                string path_cat = @"C:\Users\PCR3\Desktop\aaa\New Unity Project\Assets\Resources\shop" + i + ".png";
                Texture2D texture_cat = new Texture2D(0, 0);
                byte[] byteTexture_cat = System.IO.File.ReadAllBytes(path_cat);

                texture_cat = new Texture2D(1920, 1275);
                texture_cat.LoadImage(byteTexture_cat);
                imagecast_shop.sprite = Sprite.Create(texture_cat, new Rect(0, 0, 1920, 1275), new Vector2());

                break;
            }
            else
            {
                i++;
            }
        }
    }
    public void fileDownload(string url, string path)

    {

        try

        {

            WebClient webClient = new WebClient();

            webClient.DownloadFile(url, path);

        }
        catch (Exception e)

        {

            Console.WriteLine(e);

            Console.ReadLine();

        }

    }

    */
}