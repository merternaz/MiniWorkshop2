using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using System;
using System.Linq;

public class simulation : MonoBehaviour
{
    public Text orderCount, dayCalender,jobCount;
    public Transform backGRND, slot1_tf, slot2_tf, slot3_tf;
    
    database db = new database();
   // machineDatabase mDB = new machineDatabase();
    private int machinesID;
    public static Dictionary<int, string> machinesUsableList = new Dictionary<int, string>();
    public static List<int> machinesUsebleIDs = new List<int>();
    public static List<float> dailyExportCost = new();
    public static List<float> dailyExportEarns = new();
    public static List<float> dailyPurchasings = new();
    public static List<float> dailyEnergyCosts = new();
    public static float totalProfit,dailyExpCosts, dailyEarns, dailyPurchase,dailyWarehouseCost,dailyEnergyCost,dailyWorkman;
    public static int dayCounter=0;
    public Text dailyReport,dailyExpReport;
    public static float timer; // bu veri firebase user altında tutulmalı. her oyuna başladığında kaldığı yerden devam edecek
    public static float InvoiceTimerEx;// faturalandırma süresi. Günlük sfırlanacak, gün sonunda kasadan düşecek
    string macName;
    public static DatabaseReference userReferance, machineRef, machineRef_slot1, machineRef_slot2, machineRef_slot3;
    public static float dayLength = 14f;//1440f 1 gün dakika cinsinden uzun geldiği için ayarlanabilir
    public string cashReport,expReport;
    private void Awake()
    {
        userReferance = FirebaseDatabase.DefaultInstance.GetReference("users");
        //  machineRef = FirebaseDatabase.DefaultInstance.GetReference("users").Child(auth.USER_ID).Child("machines");
        machineRef_slot1 = FirebaseDatabase.DefaultInstance.GetReference("users").Child(auth.USER_ID).Child("machines").Child("slots1");
        machineRef_slot2 = FirebaseDatabase.DefaultInstance.GetReference("users").Child(auth.USER_ID).Child("machines").Child("slots2");
        machineRef_slot3 = FirebaseDatabase.DefaultInstance.GetReference("users").Child(auth.USER_ID).Child("machines").Child("slots3");

        SearchMachine("all");

        Debug.Log("SAHNE AÇILDI");
        dayCalender.text = (auth.levelDAY).ToString("#.");
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        InvoiceTimerEx += Time.deltaTime;
        
        //dayCalender.text = ((timer / dayLength) + 1).ToString("#.");
        orderCount.text = customerOrder.ordersList.Count.ToString();//orderDatabase.counter.ToString(
        jobCount.text = JobDatabase.jobCollection.Count.ToString();

        if (InvoiceTimerEx >= dayLength)//Gün sonunda Fatura hesabı yap
        {
            WarehouseInvoiceCost(auth.stockCAP, stockMenu.warehouseUsingCapacity);// depo maliyet faturalandırma
            //günlük işçilik giderleri faturalandırma 
            
            dailyPurchase = SumInvoiceType(dailyPurchasings);// günlük satınalma
            dailyEarns = SumInvoiceType(dailyExportEarns);//günlük kazanç miktarıda toplanabilir. Sadece görsel olarak (cüzdana etkisi olmayacak.)
            dailyExpCosts = SumInvoiceType(dailyExportCost);//günlük sevkyat maliyetide toplanabilir.Sadece görsel olarak (cüzdana etkisi olmayacak.)
            dailyEnergyCost = SumEnergyCosts(dailyEnergyCosts);//günlük enerji giderleri faturalandırma
            dailyWorkman = workersFunctions.workmanship / 26;// 26 iş günü için günlük işçilik
            totalProfit = dailyEarns - dailyExpCosts - dailyEnergyCost - dailyPurchase-dailyWarehouseCost-dailyWorkman;



            if (totalProfit > 0)
            {
                cashReport = string.Format("<color=red>Purchase:{0:0.#}</color>\n<color=green>Earns:{1:0.#}</color>" +
                "\n<color=red>Exports:{2:0.#}</color>\n<color=red>Warehouse:{3:0.#}</color>\n<color=red>Workman:{4:0.#}</color>" +
                "\n-------------------------" +
                "\n<color=green>Total Profit:</color>\n<color=green>+{5:0.#}</color>", dailyPurchase, dailyEarns,
                dailyExpCosts, dailyWarehouseCost,dailyWorkman,totalProfit);
            }
            else
            {
                cashReport = string.Format("<color=red>Purchase:{0}</color>\n<color=green>Earns:{1}</color>" +
                "\n<color=red>Exports:{2}</color>\n<color=red>Warehouse:{3}</color>\n<color=red>Workman:{4}</color>" +
                "\n-------------------------" +
                "\n<color=red>Total Profit:</color>\n<color=red>{5}</color>", dailyPurchase, dailyEarns,
                dailyExpCosts, dailyWarehouseCost, dailyWorkman, totalProfit);
            }

            expReport = string.Format("Customer Experience: {0}\nSector Experiance: {1}",ExportLogParameter.currentCustomerExp,
                ExportLogParameter.currentSectorExp);

            dailyExpReport.text = expReport;
            dailyReport.text = cashReport;                  
            dayCalender.text = (auth.levelDAY+dayCounter).ToString("#.");


            dailyExportCost.Clear();
            dailyExportEarns.Clear();
            dailyPurchasings.Clear();
            dailyEnergyCosts.Clear();
            InvoiceTimerEx = 0;// bu noktada depodaki ürünlerin %1-2 azalt her gün yada günlük giderleri artır kapasite fazlasına göre
            totalProfit = 0;
            dayCounter++;//gün artır
            StartCoroutine(db.UpdateDayLevel(auth.USER_ID, auth.levelDAY + dayCounter));// mevcut gün sayısını güncelle kaydet
        }

        if (selectingSlot.updateBackground)//makina satınalma yapıldığında ekranı güncelle (buyMachine > selectingSlot)
        {

            Debug.Log("selectingSlot.updateBackground==" + selectingSlot.updateBackground+"//"+ selectingSlot.slotName);

            switch (selectingSlot.slotName)
            {
                case "slots1":
                    if (slot1_tf.childCount != 0)
                    {
                        for (int i = 0; i < slot1_tf.childCount; i++)
                        {
                            Destroy(slot1_tf.GetChild(i).gameObject);
                        }

                    }
                    
                    LoadMachine(slot1_tf, buyMachine.name);
                    SearchMachine(selectingSlot.slotName);

                    break;
                case "slots2":
                    if (slot2_tf.childCount != 0)
                    {
                        for (int i = 0; i < slot2_tf.childCount; i++)
                        {
                            Destroy(slot2_tf.GetChild(i).gameObject);
                        }

                    }
                    LoadMachine(slot2_tf, buyMachine.name);
                    SearchMachine(selectingSlot.slotName);

                    break;
                case "slots3":
                    if (slot3_tf.childCount != 0)
                    {
                        for (int i = 0; i < slot3_tf.childCount; i++)
                        {
                            Destroy(slot3_tf.GetChild(i).gameObject);
                        }

                    }
                    LoadMachine(slot3_tf, buyMachine.name);
                    SearchMachine(selectingSlot.slotName);
                    break;
            }
           // SearchMachine();// arkaplanı güncelleme fonksyonu. Resimleri değiştir.
            //buyMachine.updateBackground = false;
            selectingSlot.updateBackground = false;
        }
    }

    void WarehouseInvoiceCost(float WHCAP,float usingCAP)// ürünlerin günlük depolama maliyetini kasadan düşecek
    {
        float x= usingCAP / WHCAP; // kullanm katsayısı (1den küçükeşit ise belirlenen sabit maliyet, aksi halde katla)
        float rentCostWH = 150f;
        float invoiceCost;
        if (x <= 1) // eğer ambar miktarını aşmıyorsa sabit kira maliyeti
        {
            invoiceCost = rentCostWH;
        }
        else // eğer aşıyorsa kira bedeli x aşım oranı
        {
            invoiceCost = rentCostWH * x;
        }
        dailyWarehouseCost = invoiceCost;
        float wallet = infobar.cash - dailyWarehouseCost; // ekrandaki kasadan düş
        db.UpdateCashWallet(auth.USER_ID, wallet); // cüzdanı güncelle

    }

    public float SumInvoiceType(List<float> recordedList)
    {
        float sum=0;

        sum=(float)recordedList.Sum(); //System.Linq özelliği ile geldi

       /* for(int i = 0; i < recordedList.Count; i++)
        {
            sum += recordedList[i];
        }*/

        return (float)sum;
    }

    public float SumEnergyCosts(List<float> energyCostList)
    {
        float sum = 0;
        sum = (float)energyCostList.Sum();
        float wallet = infobar.cash - sum;
        db.UpdateCashWallet(auth.USER_ID, wallet);
        return (float)sum;
    }

    public void SearchMachine(string slotname)
    {
        //userReferance.Child(auth.USER_ID).Child("machines").ValueChanged += ScanMachineInfo;
        // machineRef.ValueChanged += ScanMachineInfo;
        machinesUsebleIDs.Clear();// her çalışmada temizlemeli (RunJob kısmında tekrarlı buton oluşmaması için)

        if (slotname == "slots1")
        {
            machineRef_slot1.ValueChanged += ScanMachineInfo_1;//1.slotu ara

        }
        else if (slotname == "slots2")
        {
            machineRef_slot2.ValueChanged += ScanMachineInfo_2;

        }
        else if (slotname == "slots3")
        {
            machineRef_slot3.ValueChanged += ScanMachineInfo_3;

        }
        else
        {
            machineRef_slot1.ValueChanged += ScanMachineInfo_1;
            machineRef_slot2.ValueChanged += ScanMachineInfo_2;
            machineRef_slot3.ValueChanged += ScanMachineInfo_3;

        }
    }


    private void ScanMachineInfo_1(object sender, ValueChangedEventArgs args)
    {
        var machines = args.Snapshot.Value as Dictionary<string, object>; // users>userID>özel unique ID katmanı
        machinesID = Convert.ToInt32(machines["machine_id"].ToString());//yoksa 0 id ile gelir , var ise makina ID ile gelir
        foreach (var m in args.Snapshot.Children) // 5 parametre içeriyor. 5 kez içerde döner
        {
            //  var values=m.Value as Dictionary<string, object>;
            if (m.Key.ToString() == "machine_id") // 1 sefer dönecek
            {
                machinesUsebleIDs.Add(Convert.ToInt32(machines["machine_id"].ToString()));

            }
            Debug.Log("machine_id:" + machines["machine_id"].ToString() + "/" + m.Value.ToString());
        }
        Debug.Log("SAHİP OLUNAN MAKİNA_Slot1:" + machinesID + "/");// + machinesUsableList[1]); //        Debug.Log("SAHİP OLUNAN MAKİNA11:" + machinesID+"/"+ machinesUsableList[1]);


        // bu fonksyonda çoklu slota yerleştirme olmalı. ID 1 den fazla ise onları da yerleştir.
        if (machinesID != 0)
        {
            macName = machineDatabase.GetMachine(machinesID).machine_name;

            LoadMachine(slot1_tf, macName);


        }
        // machineRef.ValueChanged -= ScanMachineInfo;
        machineRef_slot1.ValueChanged -= ScanMachineInfo_1;

    }
    private void ScanMachineInfo_2(object sender, ValueChangedEventArgs args)
    {
        // machinesUsebleIDs = new List<int>();
        var machines = args.Snapshot.Value as Dictionary<string, object>; // users>userID>özel unique ID katmanı
        machinesID = Convert.ToInt32(machines["machine_id"].ToString());//yoksa 0 id ile gelir , var ise makina ID ile gelir
        foreach (var m in args.Snapshot.Children) // 5 parametre içeriyor. 5 kez içerde döner
        {
            //  var values=m.Value as Dictionary<string, object>;
            if (m.Key.ToString() == "machine_id") // 1 sefer dönecek
            {
                machinesUsebleIDs.Add(Convert.ToInt32(machines["machine_id"].ToString()));

            }
            Debug.Log("machine_id:" + machines["machine_id"].ToString() + "/" + m.Value.ToString());
        }
        Debug.Log("SAHİP OLUNAN MAKİNA_Slot2:" + machinesID + "/");// + machinesUsableList[1]); //        Debug.Log("SAHİP OLUNAN MAKİNA11:" + machinesID+"/"+ machinesUsableList[1]);


        // bu fonksyonda çoklu slota yerleştirme olmalı. ID 1 den fazla ise onları da yerleştir.
        if (machinesID != 0)
        {
            macName = machineDatabase.GetMachine(machinesID).machine_name;

            LoadMachine(slot2_tf, macName);


        }
        // machineRef.ValueChanged -= ScanMachineInfo;
        machineRef_slot2.ValueChanged -= ScanMachineInfo_2;

    }
    private void ScanMachineInfo_3(object sender, ValueChangedEventArgs args)
    {
        // machinesUsebleIDs = new List<int>();
        var machines = args.Snapshot.Value as Dictionary<string, object>; // users>userID>özel unique ID katmanı
        machinesID = Convert.ToInt32(machines["machine_id"].ToString());//yoksa 0 id ile gelir , var ise makina ID ile gelir
        foreach (var m in args.Snapshot.Children) // 5 parametre içeriyor. 5 kez içerde döner
        {
            //  var values=m.Value as Dictionary<string, object>;
            if (m.Key.ToString() == "machine_id") // 1 sefer dönecek
            {
                machinesUsebleIDs.Add(Convert.ToInt32(machines["machine_id"].ToString()));

            }
            Debug.Log("machine_id:" + machines["machine_id"].ToString() + "/" + m.Value.ToString());
        }
        Debug.Log("SAHİP OLUNAN MAKİNA_Slot3:" + machinesID + "/");// + machinesUsableList[1]); //        Debug.Log("SAHİP OLUNAN MAKİNA11:" + machinesID+"/"+ machinesUsableList[1]);

        

        // bu fonksyonda çoklu slota yerleştirme olmalı. ID 1 den fazla ise onları da yerleştir.
        if (machinesID != 0)
        {
            macName = machineDatabase.GetMachine(machinesID).machine_name;
            LoadMachine(slot3_tf, macName);


        }
        // machineRef.ValueChanged -= ScanMachineInfo;
        machineRef_slot3.ValueChanged -= ScanMachineInfo_3;

    }

    private void LoadMachine(Transform background,string objectName)
    {
        GameObject obj = Resources.Load<GameObject>("machines/" + objectName);
        Instantiate(obj, background);


    }
}



