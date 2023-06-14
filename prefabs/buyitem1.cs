using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using Firebase.Auth;
using UnityEngine.UI;
using Firebase;
public class buyitem1 : MonoBehaviour
{
    public int mat_id;
    
    public static bool inWH = false;
    private Transform ImportPanel;
    public GameObject ImportObject;
    public InputField quantity;
    //public static bool hasItem = false;
    public float WH_qty,TotalCost,Wallet,LeadTime;
    public bool Ordered = false;
    public string mat_name;
    float t = 0;

    private void Start()
    {
        ImportPanel = GameObject.FindGameObjectWithTag("ImportScreen").GetComponent<Transform>();
        
    }
    void Update()
    {
       /* if (Ordered)
        {
            t += Time.deltaTime;
            Debug.LogWarning("ÜRÜN YOLDA="+Ordered+"/"+t+"/"+LeadTime);
            if (LeadTime - t <= 0)
            {
               // BuyEventFunction(mat_name, mat_id);
                t = 0;
                Debug.LogWarning("ÜRÜN GELDİ=" + LeadTime+"/"+Ordered);
                Ordered = false;
            }
        }*/
    }
    private void GetWarehouseQty(object sender, ValueChangedEventArgs args)
    {
        var whKeys = args.Snapshot.Value as Dictionary<string, object>; // özel unique ID katmanı
       
        

        foreach (var wh_property in args.Snapshot.Children) // katman içindeki tip ve değerler
        {

            

            if (wh_property.Key.Equals(mat_id.ToString()))
            {

                // this.gameObject.transform.GetComponentInParent<matPropertys>().inWarehouse = true; // itemi işaretle varsa 
                inWH = true;
                var values = wh_property.Value as Dictionary<string, object>;

                Debug.LogWarning("wh=" + wh_property.Key+"/whQTY=" + values["quantity"]+"/");
                WH_qty = (float)Convert.ToDouble(values["quantity"].ToString()); // AMBAR MİKTARI
                break;
            }
            else
            {
                WH_qty = 0;
                inWH = false;
            }
        }
        // FirebaseDatabase.DefaultInstance.GetReference("users").Child(auth.USER_ID).Child("warehouse").ValueChanged -= GetWarehouseQty;
        auth.userReferance.Child(auth.USER_ID).Child("warehouse").ValueChanged -= GetWarehouseQty;
        //OnDisable();
    }

    /// <summary>
    /// Item satınalma menüsünde InputField alanına değer girildiğinde çalışır. Ambarı kontrol eder
    /// </summary>
    public void EntryQty()
    {
        mat_id = this.gameObject.transform.GetComponentInParent<matPropertys>().id; // alım yapılacak ürün ID = butonun ait olduğu ailenin classındaki id
                                                                                    // FirebaseDatabase.DefaultInstance.GetReference("users").Child(auth.USER_ID).Child("warehouse").ValueChanged += GetWarehouseQty; // item ID var/yok kontrol
        auth.userReferance.Child(auth.USER_ID).Child("warehouse").ValueChanged += GetWarehouseQty; // item ID var/yok kontrol

        //Debug.LogWarning("HAS=" + this.gameObject.transform.GetComponentInParent<matPropertys>().inWarehouse); // Ambarda var  ise TRUE,yok ise FALSE işaretini bilgi verir

    }

    public void AddIncomingList(int itemid,float leadtime,float qty,bool inWarehouse,float warehouseQTY)
    {
        
        GameObject go = Instantiate(ImportObject, ImportPanel) as GameObject;
        ImportObjectProperty iop = go.GetComponent<ImportObjectProperty>();
        string name= ItemDatabase.GetItem(itemid).name;
        iop.itemId = itemid;
        iop.itemName = ItemDatabase.GetItem(itemid).name;
        iop.qty = qty;
        iop.WH_qty = warehouseQTY;
        iop.inWH = inWarehouse;
        iop.itemImage.sprite= Resources.Load<Sprite>("items/" + name);
        iop.remTime= ItemDatabase.GetItem(itemid).stats["leadtime"];
        iop.describe.text = string.Format("{0} is transporting,{1} arriving time",name,leadtime);
    }

    void BuyEventFunction(string name,int id)
    {
        mat_id = id;
        string mat_name = name;
        float totalCost = 0f;
        float wallet = 0f;
        foreach (KeyValuePair<int, float> components in ItemDatabase.GetItem(mat_id).comp) // Material ID ye göre anahtar değerleri alacak component içerisinden
        {
            int comp_id = components.Key; // bileşen itemID
            float percent = components.Value;//bileşen item oranı

            if (comp_id != 0) // bileşen_id 0 değilse 1 den fazla bileşeni vardır
            {
                float cost = ItemDatabase.GetItem(comp_id).cost * Convert.ToInt32(quantity.text); // bileşenler içinden id ye ait cost bul ve miktarla çarp
                totalCost += cost;
                TotalCost = totalCost;
            }
            else
            {
                float cost = ItemDatabase.GetItem(mat_id).cost * Convert.ToInt32(quantity.text);
                totalCost = cost;
                TotalCost = totalCost;
            }

            Debug.LogWarning("ID=" + comp_id + "/" + percent);
        }



        if (inWH) //DEPODA item var ise üstüne güncelleme yap -- Ambarda var  ise TRUE,yok ise FALSE işaretini bilgi verir
        {                                                                               //this.gameObject.transform.GetComponentInParent<matPropertys>().inWarehouse
            WH_qty += (float)Convert.ToInt32(quantity.text);//güncel miktar+depo
            UpdateItemQuantity(auth.USER_ID, mat_id, WH_qty);
            Debug.LogWarning("updateWH:" + this.gameObject.transform.GetComponentInParent<matPropertys>().inWarehouse + "/" + WH_qty);
        }
        else
        {
            InsertWarehouse(auth.USER_ID, mat_id, mat_name, (float)Convert.ToDouble(quantity.text));
            Debug.LogWarning("insertWH:" + this.gameObject.transform.GetComponentInParent<matPropertys>().inWarehouse + "/" + WH_qty);

        }

        wallet = infobar.cash - totalCost;
        UpdateCashWallet(auth.USER_ID, wallet);
        simulation.dailyPurchasings.Add(totalCost);//faturaya ekle
        
    }



    public void Buy()
    {
        slidingpage spage=new slidingpage();
        mat_id = this.gameObject.transform.GetComponentInParent<matPropertys>().id; // alım yapılacak ürün ID = butonun ait olduğu ailenin classındaki id
        mat_name = this.gameObject.transform.GetComponentInParent<matPropertys>().name.text;
        LeadTime = ItemDatabase.GetItem(mat_id).stats["leadtime"];
        Debug.Log("LEAD TIME=" + LeadTime);
        Ordered = true;
        AddIncomingList(mat_id, LeadTime, (float)Convert.ToInt32(quantity.text),inWH,WH_qty);
        /*
        float totalCost = 0f;
        float wallet = 0f;
        foreach (KeyValuePair<int, float> components in ItemDatabase.GetItem(mat_id).comp) // Material ID ye göre anahtar değerleri alacak component içerisinden
        {
            int comp_id = components.Key; // bileşen itemID
            float percent = components.Value;//bileşen item oranı

            if (comp_id != 0) // bileşen_id 0 değilse 1 den fazla bileşeni vardır
            {
                float cost = ItemDatabase.GetItem(comp_id).cost * Convert.ToInt32(quantity.text); // bileşenler içinden id ye ait cost bul ve miktarla çarp
                totalCost += cost;
                TotalCost = totalCost;
            }
            else
            {
                float cost = ItemDatabase.GetItem(mat_id).cost * Convert.ToInt32(quantity.text);
                totalCost = cost;
                TotalCost = totalCost;
            }
            
            Debug.LogWarning("ID=" + comp_id + "/" + percent);
        }

        

        if (inWH) //DEPODA item var ise üstüne güncelleme yap -- Ambarda var  ise TRUE,yok ise FALSE işaretini bilgi verir
        {                                                                               //this.gameObject.transform.GetComponentInParent<matPropertys>().inWarehouse
            WH_qty += (float)Convert.ToInt32(quantity.text);//güncel miktar+depo
            UpdateItemQuantity(auth.USER_ID, mat_id, WH_qty);
            Debug.LogWarning("updateWH:" + this.gameObject.transform.GetComponentInParent<matPropertys>().inWarehouse + "/"+WH_qty);
        }
        else
        {
            InsertWarehouse(auth.USER_ID, mat_id, mat_name, (float)Convert.ToDouble(quantity.text));
            Debug.LogWarning("insertWH:" + this.gameObject.transform.GetComponentInParent<matPropertys>().inWarehouse + "/" + WH_qty);

        }

        wallet = infobar.cash - totalCost;
        UpdateCashWallet(auth.USER_ID, wallet);
        spage.CloseMatPanel();*/
    }

    public void InsertWarehouse(string userid, int itemid, string itemname, float quantity)
    {
        warehouse wh = new warehouse(itemid, itemname, quantity);
        string json = JsonUtility.ToJson(wh);
        
        Debug.Log("json=" + json);
        simulation.userReferance.Child(userid).Child("warehouse").Child(itemid.ToString()).SetRawJsonValueAsync(json);
    }

    public void UpdateItemQuantity(string userid, int item_id, float qty) // IEnumerator Item miktarını güncelle . UserID verilmeli. Loginden sonra ID yi tut
    {

        //DatabaseReference userReferance = FirebaseDatabase.DefaultInstance.GetReference("users");
        var DBTask = simulation.userReferance.Child(userid).Child("warehouse").Child(item_id.ToString()).Child("quantity").SetValueAsync(qty);


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
}
