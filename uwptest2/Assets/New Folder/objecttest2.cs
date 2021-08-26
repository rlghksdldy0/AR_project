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
    // ������ ������Ʈ�� �̹���, �ؽ�Ʈ�� ǥ���ϴ� �ڵ�
    JsonTextReader ARListJson;

    List<string> ARList_pid = new List<string>();
    List<string> ARList_xCrdnt = new List<string>();
    List<string> ARList_yCrdnt = new List<string>();
    List<string> ARList_Name = new List<string>();
    List<string> ARList_category = new List<string>();
    List<string> ARList_addrStreet = new List<string>();

    [SerializeField] Text test1; // �ؽ�Ʈ ǥ��
    [SerializeField] Text test2;
    int i = 0;
    public Image imagecast_shop;
    public string imageUrl = "http://gotouch.iptime.org:18160/anydev/svc/data/file/arad/content/ad/1629951680659-1739.png";
    GpsTest4 gpstest4;

    double add_constant = 0;
    double multyple_constant = 0.01f;

    public GameObject AR_prefab;

    private void request_ARListCaching() //���α׷� ���� �� ���� 1ȸ ������ ����Ʈ ĳ��
    { // ����Ʈ���� �� ������
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
                if (ARListJson.Value != null) //Ű�� ����� ǥ�õ� (Ű : TokenType�� PropertyName, ��� : TokenType�� String)
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


                    if (ARListJson.TokenType.ToString() == "String") // �����
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
        if (float.Parse(ARList_xCrdnt[i]) - 0.002f < double.Parse(gpstest4.lat) && float.Parse(ARList_xCrdnt[i]) + 0.002f > double.Parse(gpstest4.lat) && float.Parse(ARList_yCrdnt[i]) - 0.002f < double.Parse(gpstest4.lng) && float.Parse(ARList_yCrdnt[i]) + 0.002f > double.Parse(gpstest4.lng))
        {
            AR_prefab.SetActive(true);
        }
        else
        {
            AR_prefab.SetActive(false);
        }
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
            if (this.gameObject.name == "Shop" + i) // ������Ʈ �̸����� ���� ����
            {


                //fileDownload("http://gotouch.iptime.org:18160/anydev/svc/data/file/arad/content/ad/1629644436179-0700.png", @"D:\New Unity Project\testt" + i + ".png"); // ������ ����


                //fileDownload("http://gotouch.iptime.org:18160/anydev/svc/data/file/arad/content/ad/1629792267386-1420.png", Application.persistentDataPath + "test" + i + ".png"); // �ȵ���̵� ���ҽ������� ����
                //string path_cat = Application.persistentDataPath + "test" + i + ".png"; // �ٿ���� ���� �ҷ�����
                // string path_cat = @"D:\New Unity Project\" + "testt" + i + ".png"; // �ٿ���� ���� �ҷ�����
                //Texture2D texture_cat = new Texture2D(0, 0);
                //byte[] byteTexture_cat = System.IO.File.ReadAllBytes(path_cat); // �ҷ��� ���� byte������ ��ȯ

                //texture_cat = new Texture2D(110, 110); // ũ������
                //texture_cat.LoadImage(byteTexture_cat); // �̹����� �ε�
                //imagecast_shop.sprite = Sprite.Create(texture_cat, new Rect(0, 0, 110, 110), new Vector2()); // sprite�������� ǥ��
                StartCoroutine("WebDownload");


                test1.text = ARList_Name[i].ToString(); // ����Ʈ�� �ҷ��� �ؽ�Ʈ�� ǥ��
                test2.text = ARList_category[i].ToString();

            }
            else
            {
                i++; // �ش��ϴ� ��ȣ�� �ƴҽ� ��ȣ���
            }

        }
    }
    public void fileDownload(string url, string path) // ������ �̹����� �ٿ�޾� �����ϱ� ���� �Լ�

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
    // �̹��� �ٿ�ް� ����
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

    IEnumerator WebDownload()
    {
        WWW webimage = new WWW(imageUrl);
        yield return webimage;
        imagecast_shop.material.mainTexture = webimage.texture;
    }
}