using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Firebase.Database;
using System;
using TMPro;

public class attachToMac : MonoBehaviour,IPointerClickHandler
{
    /// <summary>
    /// hangi makinanın butonuna basıldıysa , o makinayı sahnede bulup ANIMATORUNU başlatmalı ve üretim simulasyonu başlamalı
    /// hangi iş no ve sip no çalışıyorsa o kayıtları güncellemeli
    /// </summary>
    int itemid, orderid;
    float stockQty = 0f, orderQty;
    bool isEnough = false;
    public GameObject floatingText;

    public List<int> ItemIdInWarehouse = new List<int>();
    public List<float> ItemqtyInWarehouse = new List<float>();
    public void OnPointerClick(PointerEventData eventData)
    {
        string machineName;
        machineName = this.GetComponentInChildren<Text>().text;
        auth.userReferance.Child(auth.USER_ID).Child("warehouse").ValueChanged += DepodakiMateryal;

        Transform machineList = this.gameObject.GetComponentInParent<Transform>();        
        Transform jobList = machineList.transform.parent.parent.GetComponentInParent<Transform>();//JobList panelini yakala
        //JobList paneli içindeki scriptte olan sipariş bilgileri al
        int jobID = jobList.GetComponent<JobObjectScript>().jid;
        int orderID = jobList.GetComponent<JobObjectScript>().orderid;
        string jName= jobList.GetComponent<JobObjectScript>().jname;
        float qty= jobList.GetComponent<JobObjectScript>().qty;
        string itemname= jobList.GetComponent<JobObjectScript>().orderName.text;
        int itemID = ItemDatabase.GetItem(itemname).id;


        RequirementMaterials(itemID, qty); // alt ürünleri kontrol et

        if (workersFunctions.currentWorkerCount>0)//mavi yaka var ise makina çalıştırılabilir. Yoksa olmaz
        {
            if (isEnough)//eğer hammadde yeterli ise Makinanın Çalışma Bilgilerini (runningJobsInfo) , iş emri olarak eşle
            {
                GameObject.FindGameObjectWithTag(machineName).GetComponent<runningJobsInfo>().jobID = jobID;
                GameObject.FindGameObjectWithTag(machineName).GetComponent<runningJobsInfo>().orderID = orderID;
                GameObject.FindGameObjectWithTag(machineName).GetComponent<runningJobsInfo>().orderQty = qty;
                GameObject.FindGameObjectWithTag(machineName).GetComponent<runningJobsInfo>().productQty = 0;
                GameObject.FindGameObjectWithTag(machineName).GetComponent<runningJobsInfo>().itemID = itemID;

                Debug.Log("SEÇİLEN İŞ:" + jobID + "/" + orderID + "/" + jName + "/" + qty);
                GameObject.FindGameObjectWithTag(machineName).GetComponent<machineInfo>().GetAnimatorStart();// animasyonu başlat
            }
            else
            {
                
                Debug.Log("MALZEME YETERSİİZ . SATINALMA YAPIN");
            }
        }
        else
        {
            if (floatingText)
            {
               // FloatingTextForInfo("YETERLİ MAVİ YAKA PERSONELİ YOK");
            }
            Debug.Log("YETERLİ MAVİ YAKA PERSONELİ YOK "+workersFunctions.currentWorkerCount.ToString());
        }
        
        
    }

    void FloatingTextForInfo(string InfoText)
    {
        //  if (this.transform.Find("FloatingText") == null)
        //  {
        GameObject go = Instantiate(floatingText, this.transform.position, Quaternion.identity, this.transform) as GameObject;// makina transformunu algılamadı
        go.transform.GetChild(0).GetChild(0).transform.GetComponent<TMP_Text>().text = InfoText;
        //go.transform.GetChild(0).GetChild(0).transform.GetComponent<TMP_Text>().po
        // }

        // Debug.Log("MESHTEX=" + go.transform.GetChild(0).GetChild(0).transform.GetComponent<TMP_Text>().text);
    }

    /// <summary>
    /// Makinada çalışmadan önce hammadde kontrolünü yapıp ona göre ileryelecek. Hammadde kadarda üretilebilir veya 
    /// üretime izin vermez. Önce hammadde tedariği sağlanır
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void DepodakiMateryal(object sender, ValueChangedEventArgs args)
    {
        var whKeys = args.Snapshot.Value as Dictionary<string, object>; // özel unique ID katmanı
                                                                        // orderid = this.gameObject.GetComponentInParent<JobObjectScript>().orderid;
                                                                        //itemid = orderDatabase.GetCollectionOrderId(orderid).itemid;
                                                                        //itemid = orderDatabase.GetCollectionOrderId(orderid).itemid;
                                                                        // orderQty = this.gameObject.GetComponentInParent<JobObjectScript>().qty;
        ItemIdInWarehouse = new List<int>();
        ItemqtyInWarehouse = new List<float>();
        foreach (var wh_property in args.Snapshot.Children) // katman içindeki tip ve değerler
        {


            

                //   this.gameObject.transform.GetComponentInParent<matPropertys>().inWarehouse = true; // itemi işaretle varsa 

                var values = wh_property.Value as Dictionary<string, object>;

                stockQty = (float)Convert.ToDouble(values["quantity"].ToString()); // AMBAR MİKTARI
                itemid = Convert.ToInt32(values["item_id"].ToString());

                ItemIdInWarehouse.Add(itemid);
                ItemqtyInWarehouse.Add(stockQty);

               // break;
            
        }

        auth.userReferance.Child(auth.USER_ID).Child("warehouse").ValueChanged -= DepodakiMateryal;

    }

    public void RequirementMaterials(int itemID, float qty)
    {
        float totalCost = 0f, quantity;
        int x = 0;
        foreach (KeyValuePair<int, float> components in ItemDatabase.GetItem(itemID).comp) // Material ID ye göre anahtar değerleri alacak component içerisinden
        {
            int comp_id = components.Key; // bileşen itemID
            float percent = components.Value;//bileşen item oranı

            
            foreach(int id in ItemIdInWarehouse)
            {
                if (comp_id == id)
                {
                    if (percent * qty > ItemqtyInWarehouse[x])
                    {
                        isEnough = false;
                    }
                    else
                    {
                        isEnough = true;
                    }
                }
                x++;
            }           
           

            
        }
    }
}
