using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Auth;
using UnityEngine.UI;
using Firebase;
using UnityEngine.EventSystems;
using System;

public class database : MonoBehaviour
{
    public DatabaseReference userReferance;
    public bool userRepeated = false;
    public static string statusReg;
    void Start()
    {
        StartCoroutine(connect());
    }

    public void InsertMachines(string userid,int macid,string macname,float price,float speed,ItemType mType,float energy,string slot)
    {   //(string userid,int macid,string macname,float price,float speed,ItemType mType,float energy)
        userReferance = FirebaseDatabase.DefaultInstance.GetReference("users");
        machineData md = new machineData(macid,macname,price,speed,mType,energy);
        string json = JsonUtility.ToJson(md);
        
        //string userKey = userReferance.Push().Key;
        Debug.Log("json=" + json);
        userReferance.Child(userid).Child("machines").Child(slot).SetRawJsonValueAsync(json);

       /* if (slot == "slot1")
        {
            userReferance.Child(userid).Child("machines").Child("slot1").SetRawJsonValueAsync(json);
        }
        else if (slot == "slot2")
        {
            userReferance.Child(userid).Child("machines").Child("slot2").SetRawJsonValueAsync(json);

        }
        else
        {
            userReferance.Child(userid).Child("machines").Child("slot3").SetRawJsonValueAsync(json);

        }*/

    }

    public IEnumerator UpdateMachines(string userid, int macid, string macname, float price, float speed, ItemType mType,float energy,string slot) // Item miktarını güncelle . UserID verilmeli. Loginden sonra ID yi tut
    {
        // bu alanda machine script oluşturup değerleri ona atayop JSON setRaw şeklinde tek seferde update olabilir
        string mTip = Enum.GetName(typeof(ItemType), mType);

        //ItemType.
        DatabaseReference userReferance = FirebaseDatabase.DefaultInstance.GetReference("users");
        var DBTask = userReferance.Child(userid).Child("machines").Child(slot).Child("machine_id").SetValueAsync(macid);
        var DBTask1 = userReferance.Child(userid).Child("machines").Child(slot).Child("machine_name").SetValueAsync(macname);
        var DBTask2 = userReferance.Child(userid).Child("machines").Child(slot).Child("machine_cost").SetValueAsync(price);
        var DBTask3 = userReferance.Child(userid).Child("machines").Child(slot).Child("machine_speed").SetValueAsync(speed);
        var DBTask4 = userReferance.Child(userid).Child("machines").Child(slot).Child("machineType").SetValueAsync((int)mType);//mType
        var DBTask5 = userReferance.Child(userid).Child("machines").Child(slot).Child("energy_consumption").SetValueAsync(energy);
        var DBTask6= userReferance.Child(userid).Child("machines").Child(slot).Child("macTip").SetValueAsync(mTip);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
        yield return new WaitUntil(predicate: () => DBTask1.IsCompleted);
        yield return new WaitUntil(predicate: () => DBTask2.IsCompleted);
        yield return new WaitUntil(predicate: () => DBTask3.IsCompleted);
        yield return new WaitUntil(predicate: () => DBTask4.IsCompleted);
        yield return new WaitUntil(predicate: () => DBTask5.IsCompleted);
        yield return new WaitUntil(predicate: () => DBTask6.IsCompleted);

        if (DBTask.Exception != null && DBTask1.Exception != null && DBTask2.Exception != null && DBTask3.Exception != null && DBTask4.Exception != null)
        {
            Debug.LogWarning("Makina Kayıt hatası");
        }
        else
        {
            Debug.LogWarning("Makina Güncellendi");
        }
    }

    public IEnumerator UpdateDayLevel(string userid,int day)
    {
        DatabaseReference userReferance = FirebaseDatabase.DefaultInstance.GetReference("users");
        var DBTask = userReferance.Child(userid).Child("level").SetValueAsync(day);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
        if (DBTask.Exception != null)
        {
            Debug.LogWarning("Gün Kayıt hatası");
        }
        else
        {
            Debug.LogWarning("Gün Güncellendi");
        }

    }

    public IEnumerator UpgradeMachines(string userid, int item_id, float qty) // Item miktarını güncelle . UserID verilmeli. Loginden sonra ID yi tut
    {

        DatabaseReference userReferance = FirebaseDatabase.DefaultInstance.GetReference("users");
        var DBTask = userReferance.Child(userid).Child("machines").Child(item_id.ToString()).Child("quantity").SetValueAsync(qty);


        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning("Miktar Kayıt hatası");
        }
        else
        {
            Debug.LogWarning("Miktar Güncellendi");
        }
    }

    public IEnumerator UpdateCustomerExperiance(string userid,float exp) // Item miktarını güncelle . UserID verilmeli. Loginden sonra ID yi tut
    {

        DatabaseReference userReferance = FirebaseDatabase.DefaultInstance.GetReference("users");
        var DBTask = userReferance.Child(userid).Child("customerExperiance").SetValueAsync(exp);
        //item_id = Convert.ToInt32(DBTask.get);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning("Exp Kayıt hatası");
        }
        else
        {
            Debug.LogWarning("Exp Güncellendi");
        }
    }

    public IEnumerator UpdateSectorExperiance(string userid, float exp) // Item miktarını güncelle . UserID verilmeli. Loginden sonra ID yi tut
    {

        DatabaseReference userReferance = FirebaseDatabase.DefaultInstance.GetReference("users");
        var DBTask = userReferance.Child(userid).Child("sectorExperiance").SetValueAsync(exp);
        //item_id = Convert.ToInt32(DBTask.get);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning("Sector Kayıt hatası");
        }
        else
        {
            Debug.LogWarning("Sector Güncellendi");
        }
    }

    public IEnumerator GetMachinesID(string userid, int item_id) // Item miktarını güncelle . UserID verilmeli. Loginden sonra ID yi tut
    {

        DatabaseReference userReferance = FirebaseDatabase.DefaultInstance.GetReference("users");
        var DBTask = userReferance.Child(userid).Child("machines").Child("machine_id").GetValueAsync();
        //item_id = Convert.ToInt32(DBTask.get);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning("Miktar Kayıt hatası");
        }
        else
        {
            Debug.LogWarning("Miktar Güncellendi");
        }
    }

    public void InsertWarehouse(string userid, int itemid, string itemname, float quantity)
    {
        userReferance = FirebaseDatabase.DefaultInstance.GetReference("users");
        warehouse wh = new warehouse(itemid, itemname, quantity);
        string json = JsonUtility.ToJson(wh);
        string name = itemname;
        //string userKey = userReferance.Push().Key;
        Debug.Log("json=" + json);
        userReferance.Child(userid).Child("warehouse").Child(itemid.ToString()).SetRawJsonValueAsync(json);
    }

    public void RegisterUser(string business, string usr, string mail, string pw)
    {
        int macid = 0;
        string macname = "";
        float price = 0;
        float speed = 0;
        ItemType mType = ItemType.Wood;
        float energy = 0;

        //user bilgileri
        userReferance = FirebaseDatabase.DefaultInstance.GetReference("users");       
        userinfo user_info = new userinfo(business, usr, mail,pw);
        string json = JsonUtility.ToJson(user_info);
        string userKey = userReferance.Push().Key;
        userReferance.Child(userKey).SetRawJsonValueAsync(json);

        //Makina slot default değerler (3 slot)
        DatabaseReference userMachineRef1 = FirebaseDatabase.DefaultInstance.GetReference("users").Child(userKey).Child("machines").Child("slots1");
        DatabaseReference userMachineRef2 = FirebaseDatabase.DefaultInstance.GetReference("users").Child(userKey).Child("machines").Child("slots2");
        DatabaseReference userMachineRef3 = FirebaseDatabase.DefaultInstance.GetReference("users").Child(userKey).Child("machines").Child("slots3");
        machineData md = new machineData(macid, macname, price, speed, mType, energy);
        string json_machine = JsonUtility.ToJson(md);
        string path1 = userMachineRef1.Push().Key;
        string path2 = userMachineRef2.Push().Key;
        string path3 = userMachineRef3.Push().Key;
        userMachineRef1.SetRawJsonValueAsync(json_machine);
        userMachineRef2.SetRawJsonValueAsync(json_machine);
        userMachineRef3.SetRawJsonValueAsync(json_machine);




    }

   

    private void ScanRegisterInfo(object sender, ChildChangedEventArgs args)
    {
        var userKeys = args.Snapshot.Value as Dictionary<string, object>; // özel unique ID katmanı
        string mobileID_current = SystemInfo.deviceUniqueIdentifier;
        bool userFinded = false;
        foreach (var usr_property in userKeys) // katman içindeki tip ve değerler
        {
            var values = usr_property.Value as Dictionary<string, object>;

            if (values["businessname"].ToString() == auth.bsName)
            {
                Debug.LogWarning(values["businessname"].ToString()+"@@@"+ auth.bsName);
                userRepeated = true;
                break;
            }


        }
    }

    public float GetItemQuantity(string userid, int item_id) //IEnumerator
    {
        DatabaseReference userReferance = FirebaseDatabase.DefaultInstance.GetReference("users");
        var quantity = userReferance.Child(userid).Child("warehouse").Child(item_id.ToString()).Child("quantity").GetValueAsync();
        float qq;
        /*  yield return new WaitUntil(predicate: () => quantity.IsCompleted);

          if (quantity.Exception != null)
          {
              Debug.LogWarning("Miktar Kayıt hatası");
          }
          else
          {
              Debug.LogWarning("Miktar Güncellendi");
          }*/
        //qq = (float)quantity.Result.Value;
        return (float)Convert.ToDouble(quantity.ToString()); //qq;
    }


    public IEnumerator UpdateItemQuantity(string userid,int item_id, float qty) // Item miktarını güncelle . UserID verilmeli. Loginden sonra ID yi tut
    {

        DatabaseReference userReferance = FirebaseDatabase.DefaultInstance.GetReference("users");
        var DBTask = userReferance.Child(userid).Child("warehouse").Child(item_id.ToString()).Child("quantity").SetValueAsync(qty);
       // stockMenu.GetStockItemInfo(item_id).itemQuantity = qty;

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning("Miktar Kayıt hatası");
        }
        else
        {
            Debug.LogWarning("Miktar Güncellendi");
        }
    }

    public IEnumerator UpdateBusinessCash(string userid,float cash) // CASH miktar güncelle . UserID verilmeli. Loginden sonra ID yi tut
    {

      //  DatabaseReference userReferance = FirebaseDatabase.DefaultInstance.GetReference("users");
        var DBTask = simulation.userReferance.Child(userid).Child("cash").SetValueAsync(cash);


        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning("Miktar Kayıt hatası");
        }
        else
        {
            Debug.LogWarning("Miktar Güncellendi");
        }
    }

    public void UpdateCashWallet(string userid, float cash) // IEnumerator Item miktarını güncelle . UserID verilmeli. Loginden sonra ID yi tut
    {

        //DatabaseReference userReferance = FirebaseDatabase.DefaultInstance.GetReference("users");
        var DBTask = simulation.userReferance.Child(userid).Child("cash").SetValueAsync(cash);


        //yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning("Miktar Kayıt hatası");
        }
        else
        {
            Debug.LogWarning("Miktar Güncellendi");
        }
    }


    private IEnumerator connect()
    {
        var tasks = FirebaseApp.CheckAndFixDependenciesAsync(); // Database erişimi için task oluştur
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
            userReferance = FirebaseDatabase.DefaultInstance.GetReference("users"); //FirebaseDatabase.DefaultInstance.GetReference("users");
            Debug.Log("Bağlantı kuruldu..DB");
            bool conn = true;

            

        }
        else
        {
            Debug.LogError("Bağlantı SAĞLANAMADI!");
            
        }
    }
}
