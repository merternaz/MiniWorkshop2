using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using Firebase;
using Firebase.Extensions;
using Firebase.Database;
using System;

public class auth : MonoBehaviour
{
    // Start is called before the first frame update
    public static DatabaseReference userReferance,itemReference;
    public bool conn;
    public Text conn_status,loginStatus,regStatus,statusBS;
    public Transform regPage, loginPage;
    public InputField businessname, pw, mailR,usernameR, businessnameR, pwR;
    public Toggle rememberMe;
    public static string USER_ID, bsName;
    public static int levelDAY,workerBlue,workerWhite;
    public static bool repeatBS;
    public static float CashMoney,stockCAP,sectorEXP,custEXP;
    public Animator transition;
    DependencyStatus dpStatus = DependencyStatus.UnavailableOther;
   
    void Start()
    {
        userReferance = FirebaseDatabase.DefaultInstance.GetReference("users");
        itemReference = FirebaseDatabase.DefaultInstance.GetReference("items");
        StartCoroutine(connect());
       // Kontrol1();
    }

     void Awake()
    {
        ReadData();
    }

    private void ITEMS_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        var data = e.Snapshot.Value as Dictionary<string, object>; // items ağacı içindeki kayıtlar = stone,bricks,wood....
        int x = 0;
        foreach(var item in data)
        {
            
            var item_detail = item.Value as Dictionary<string, object>; // stone,bricks içindeki özellikleri alır 
            Debug.Log(x + " " + item.Key + "/" + item_detail["cost"] + "/" + item_detail["type"]); // cost ve type özellikleri tanımlı
            
        
            x++;
        }

        itemReference.ValueChanged -= ITEMS_ValueChanged;


    }

    void ReadData()
    {
      //  FirebaseDatabase.DefaultInstance.GetReference("items").ValueChanged += ITEMS_ValueChanged;
      //  userReferance.ValueChanged += USERS_ValueChanged;
    }

    private void USERS_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        var userID = e.Snapshot.Value as Dictionary<string, object>;
        foreach(var users in userID)
        {
            var user_details = users.Value as Dictionary<string, object>;
            Debug.Log("users=" + users.Key + "/" + user_details["businessname"] + "/" + user_details["cash"]);
        }
        userReferance.ValueChanged -= USERS_ValueChanged;
    }

    private void LOGIN(object sender, ValueChangedEventArgs args)
    {
        Debug.Log("LOGIN_EVENT");
        var userKeys = args.Snapshot.Value as Dictionary<string, object>; // özel unique ID katmanı
        string mobileID_current = SystemInfo.deviceUniqueIdentifier;
        bool userFinded = false;
        foreach(var usr_property in userKeys) // USERID içinde anahtar değerler içinde
        {
            var values = usr_property.Value as Dictionary<string, object>; //her anahtar için değer al

            if(values["password"].ToString()==pw.text && values["businessname"].ToString() == businessname.text) //psw anahtarının değeri==textbox değeri ve businessname anahtar değeri == textbox değeri
            {
                USER_ID = usr_property.Key;
                userFinded = true;
                CashMoney = (float)Convert.ToDouble(values["cash"].ToString());
                stockCAP= (float)Convert.ToDouble(values["stockCapacity"].ToString());
                levelDAY= Convert.ToInt32(values["level"].ToString());
                custEXP= (float)Convert.ToDouble(values["customerExperiance"].ToString());
                sectorEXP = (float)Convert.ToDouble(values["sectorExperiance"].ToString());
                workerWhite= Convert.ToInt32(values["workersWhite"].ToString());//beyaz yaka sayısı
                workerBlue = Convert.ToInt32(values["workersBlue"].ToString());//mavi yaka sayısı
                break;
            }

            
        }

        if (!userFinded)
        {
            Debug.LogWarning("Kullanıcı bulunamadı !");
        }
        else
        {
            Debug.LogWarning("Kullanıcı bulundu @@@");

            //SceneManager.LoadScene(1);

            StartCoroutine(loadSceneTrans(1));

            
        }

        userReferance.ValueChanged -= LOGIN;

    }

    IEnumerator loadSceneTrans(int sceneIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(sceneIndex);

    }

    public void RegPage()// KAYIT SAYFASINI GETİR
    {
        loginPage.transform.gameObject.SetActive(false);
        regPage.transform.gameObject.SetActive(true);

    }
    public void LoginPage()// LOGİN SAYFASINI GETİR
    {
        loginPage.transform.gameObject.SetActive(true);
        regPage.transform.gameObject.SetActive(false);

    }

    public void LoginEvent()
    {
        userReferance.ValueChanged += LOGIN;
    }

    public void RegisterEvent()// KAYIT OL
    {
        bsName = businessnameR.text;
        database db=new database();
        
        Debug.LogWarning("repeatBS1111"+ repeatBS);
        
        if (!repeatBS)
        {
            db.RegisterUser(businessnameR.text, usernameR.text, mailR.text, pwR.text);
            regStatus.text = "KAYIT OK";//database.statusReg
        }
        else
        {
            regStatus.text = "KAYIT MEVCUT!";//database.statusReg
        }
        

    }
  /*  public void BS_InputChange()
    {
        
        FirebaseDatabase.DefaultInstance.GetReference("users").ValueChanged += ScanRegisterInfo;

        Debug.LogWarning("repeatBS" + repeatBS);
    }*/

    private void ScanRegisterInfo(object sender, ValueChangedEventArgs args)
    {
        repeatBS = false;
        var userKeys = args.Snapshot.Value as Dictionary<string, object>; // users>özel unique ID katmanı
        string mobileID_current = SystemInfo.deviceUniqueIdentifier;
        bool userFinded = false;
        statusBS.text = "kullanılabilir";
        statusBS.color = Color.green;
        foreach (var usr_property in userKeys) // katman içindeki tip ve değerler... name,pw...
        {
            var values = usr_property.Value as Dictionary<string, object>;

            if (values["businessname"].ToString() == businessnameR.text)
            {
                Debug.LogWarning(values["businessname"].ToString() + "@@@" + businessnameR.text);
                repeatBS = true;
                statusBS.text = "kayıt var";
                statusBS.color = Color.red;
                break;
            }
            


        }

       // FirebaseDatabase.DefaultInstance.GetReference("users").ValueChanged -= ScanRegisterInfo;
    }

    private IEnumerator connect()
    {
        var tasks = FirebaseApp.CheckAndFixDependenciesAsync();//.CheckAndFixDependenciesAsync(); // Database erişimi için task oluştur
        while (!tasks.IsCompleted)
        {
            yield return null;
        }

        if (tasks.IsCanceled || tasks.IsFaulted)
        {
            Debug.LogError("Database Hatası:" + tasks.Exception);
        }

        var dependencyStatus = tasks.Result;

        if (dependencyStatus == DependencyStatus.Available)
        {
           // userReferance = FirebaseDatabase.DefaultInstance.GetReference("users"); //FirebaseDatabase.DefaultInstance.GetReference("users");
            Debug.Log("Bağlantı kuruldu..LOGIN");
            conn = true;
            

            if (conn) // bağlantı kurulduktan sonra user bilgilerini çek
            {
                conn_status.text = "CONNECTION OK...";
                conn_status.color = Color.green;
            }

        }
        else
        {
            Debug.LogError("Bağlantı SAĞLANAMADI!");
            conn_status.text = "NO CONNECTION FROM SERVER...";
            conn_status.color = Color.red;
        }
    }


    void Kontrol1()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(tasks =>
        {
            dpStatus = tasks.Result;
            if (dpStatus == DependencyStatus.Available)
            {
                //KOntrol 2
                Kontrol2();
            }
            else
            {
                Debug.LogError(
              "Could not resolve all Firebase dependencies: " + dpStatus);
            }
        });
    }

    void Kontrol2() // firebase senkronizasyon
    {
        FirebaseApp app = FirebaseApp.DefaultInstance; //FirebaseDatabase.DefaultInstance.GetReference("users");
        Debug.Log("Bağlantı kuruldu..KONTROL2");
        //conn = true;

        //Kontrol3
        Kontrol3();
    }

    void Kontrol3()
    {
        FirebaseDatabase.DefaultInstance.GetReference("users").GetValueAsync().ContinueWith(task=>
        {
            if (task.IsFaulted)
            {
                Debug.Log("HATA KONTROL3");
            }else if (task.IsCompleted)
            {
                Debug.Log("COMPLETED .. KONTROL3");

                DataSnapshot snp = task.Result;
                Debug.Log("Bağlantı kuruldu..KONTROL3");
                conn = true;
                conn_status.text = "CONNECTION OK...";
                conn_status.color = Color.green;
                Debug.Log("SNAPPS:" + snp.Key + "/" + snp.Value.ToString());
                foreach (var t in snp.Children)
                {
                    Debug.Log("SNAPPS:" + t.Key + "/"+t.Value.ToString());
                }
            }
        }); //FirebaseDatabase.DefaultInstance.GetReference("users");
        
    }
}
