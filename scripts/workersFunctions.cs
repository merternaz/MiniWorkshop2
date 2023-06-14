using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase;
using System;
using Firebase.Extensions;
using Firebase.Database;


public class workersFunctions : MonoBehaviour
{
    public static DatabaseReference userDataInfo;
    public Text mngCount, workerCount;
    public float minimum_wage = 300f;
    private float minSkill = 1;
    private float maxSkill = 100;
    private int foundedSkill;
    public static float acceptedWage,totalSkill, skillPointW, skillPointM, new_wage_W, new_wage_M, productRatio,workmanship;
    public static int workerCapFromDatabase,currentWorkerCount,currentManagerCount;
    public Text mng_skill,wrk_skill, mng_Wage,wrk_Wage;
    //1 beyaz yaka - 10 mavi yaka kotası artırır. 
    // oyun başlangıcında 10 mavi yaka alınabilir. sonrası için 1 beyaz yaka alınmalı . +10 mavi yaka daha kotası açılır.
    // beyaz yaka maliyetlidir. mavi yaka üretim hızını artırır.

    private void Start()
    {
        userDataInfo = FirebaseDatabase.DefaultInstance.GetReference("users").Child(auth.USER_ID );
        userDataInfo.ValueChanged += GetWorkersInfo;

    }
    public void SearchWorker()
    {
        skillPointW = UnityEngine.Random.Range(minSkill, maxSkill);
        new_wage_W = minimum_wage * (100 + skillPointW) / 100;// mavi yaka ücreti
        productRatio = skillPointW / 1000;// üretim hızını artırsın
        wrk_skill.text = skillPointW.ToString("#.#");
        wrk_Wage.text = new_wage_W.ToString("#.#");
        userDataInfo.ValueChanged += GetWorkersInfo;
    }


    public void SearchManager()
    {
        skillPointM = UnityEngine.Random.Range(minSkill, maxSkill);
        new_wage_M = minimum_wage * (200 + skillPointM) / 100;// beyaz yaka ücreti
        int workerCap =+ 10;
        mng_skill.text = skillPointM.ToString("#.#");
        mng_Wage.text = new_wage_M.ToString("#.#");
        userDataInfo.ValueChanged += GetWorkersInfo;
    }

    /// <summary>
    /// Kabul edilen yönetici için maliyeti öde , +10 Worker kapasitesi artır, Database güncelle
    /// </summary>
    public void AcceptManager() 
    {
        database db = new database();
        workerCapFromDatabase = (currentManagerCount+1) * 10; // her beyaz yaka +10 mavi yaka 
        //currentWorkerCount = auth.workerBlue;
        userDataInfo.ValueChanged += GetWorkersInfo;
        //currentManagerCount = auth.workerWhite;

        totalSkill = ((totalSkill * ((currentWorkerCount) + currentManagerCount+1)) + skillPointM/1000) / ((currentWorkerCount) + currentManagerCount+1);//üretim efektine yeni etkisi
        //workmanship = ((workmanship * ((currentWorkerCount) + currentManagerCount + 1)) + new_wage_M);// /((currentWorkerCount) + currentManagerCount+1);//günlük işçilik yeni etkisi
        workmanship = (workmanship + new_wage_M);
        StartCoroutine(SetManagersInfo(currentManagerCount + 1, workmanship, totalSkill));
        mngCount.text = (currentManagerCount + 1).ToString();
        workerCount.text = (currentWorkerCount).ToString() + "(" + workerCapFromDatabase.ToString() + ")";
        float wallet = infobar.cash - new_wage_M;
        db.UpdateCashWallet(auth.USER_ID, wallet);

        skillPointM = 0;
        new_wage_M = 0;
        mng_skill.text = "";
        mng_Wage.text = "";
    }

    public void FireManager()// Yönetici eksiltme
    {
        database db = new database();
        workerCapFromDatabase = (currentManagerCount -1) * 10; // her beyaz yaka +10 mavi yaka 
        userDataInfo.ValueChanged += GetWorkersInfo;
        if (currentManagerCount > 0)
        {
            totalSkill = (totalSkill - (totalSkill / ((currentWorkerCount) + currentManagerCount)));//üretim efektine yeni etkisi
                                                                                                    //workmanship = ((workmanship * ((currentWorkerCount) + currentManagerCount + 1)) + new_wage_M);// /((currentWorkerCount) + currentManagerCount+1);//günlük işçilik yeni etkisi
            workmanship = (workmanship - minimum_wage * 2); // toplam maliyet olarak hesaplanıyor
            StartCoroutine(SetManagersInfo(currentManagerCount - 1, workmanship, totalSkill));
            mngCount.text = (currentManagerCount - 1).ToString();
            workerCount.text = (currentWorkerCount - 1).ToString() + "(" + workerCapFromDatabase.ToString() + ")";
            float wallet = infobar.cash - minimum_wage * 2;//çıkartırken 1 maaş öde
            db.UpdateCashWallet(auth.USER_ID, wallet);

            skillPointM = 0;
            new_wage_M = 0;
            mng_skill.text = "";
            mng_Wage.text = "";

        }
        else
        {
            Debug.LogWarning("ÇIKARILACAK YÖNETİCİ BULUNMAMAKTADIR");

        }
    }

    public void AcceptWorker()
    {
        database db = new database();
        workerCapFromDatabase = (currentManagerCount) * 10; // her beyaz yaka +10 mavi yaka  (auth.workerWhite + 1) * 10
        //currentWorkerCount = auth.workerBlue;
        userDataInfo.ValueChanged += GetWorkersInfo;
        //currentManagerCount = auth.workerWhite;

        if (currentWorkerCount + 1 <= workerCapFromDatabase)// alınan işçi kapasiteyi aşmıyor ise 
        {
            totalSkill=((totalSkill*((currentWorkerCount+1)+currentManagerCount))+skillPointW/1000)/((currentWorkerCount + 1) + currentManagerCount);//üretim efektine yeni etkisi
            //workmanship = ((workmanship * ((currentWorkerCount + 1) + currentManagerCount)) + new_wage_W); // /((currentWorkerCount + 1) + currentManagerCount);//günlük işçilik yeni etkisi
            workmanship = (workmanship + new_wage_W);
            StartCoroutine(SetWorkersInfo(currentWorkerCount + 1,workmanship,totalSkill));
            workerCount.text = (currentWorkerCount + 1).ToString()+" ("+workerCapFromDatabase.ToString()+")";
            float wallet = infobar.cash - new_wage_W;// 1 maaş girişte ödenir
            db.UpdateCashWallet(auth.USER_ID, wallet);//kasayı günceller
        }
        else
        {
            Debug.LogWarning("İŞÇİ KAPASİTESİ DOLU. YÖNETİCİ ALINIZ");
        }

        skillPointW = 0;
        new_wage_W = 0;
        wrk_skill.text = "";
        wrk_Wage.text="";

    }

    public void FireWorker()
    {
        database db = new database();
        workerCapFromDatabase = (currentManagerCount) * 10; // her beyaz yaka +10 mavi yaka  (auth.workerWhite + 1) * 10
        userDataInfo.ValueChanged += GetWorkersInfo;

        if (currentWorkerCount > 0)
        {
            if (currentWorkerCount - 1 >= workerCapFromDatabase)// alınan işçi kapasiteyi aşmıyor ise 
            {
                totalSkill = (totalSkill - (totalSkill / ((currentWorkerCount) + currentManagerCount)));//üretim efektine yeni etkisi
                                                                                                        //workmanship = ((workmanship * ((currentWorkerCount + 1) + currentManagerCount)) + new_wage_W); // /((currentWorkerCount + 1) + currentManagerCount);//günlük işçilik yeni etkisi
                workmanship = (workmanship - minimum_wage);
                StartCoroutine(SetWorkersInfo(currentWorkerCount - 1, workmanship, totalSkill));
                workerCount.text = (currentWorkerCount - 1).ToString() + " (" + workerCapFromDatabase.ToString() + ")";
                float wallet = infobar.cash - minimum_wage;// 1 maaş çıkışta ödenir
                db.UpdateCashWallet(auth.USER_ID, wallet);//kasayı günceller
            }
            else
            {
                Debug.LogWarning("İŞÇİ KAPASİTESİ DOLU. YÖNETİCİ ÇIKARINIZ");
            }

            skillPointW = 0;
            new_wage_W = 0;
            wrk_skill.text = "";
            wrk_Wage.text = "";
        }
        else
        {
            Debug.LogWarning("ÇIKARILACAK İŞÇİ BULUNMAMAKTADIR");

        }


    }

    private void GetWorkersInfo(object sender,ValueChangedEventArgs e)
    {
        var data =e.Snapshot.Value as Dictionary<string, object>;

       
        currentWorkerCount =Convert.ToInt32(data["workersBlue"].ToString());// mavi yaka sayısı
        currentManagerCount = Convert.ToInt32(data["workersWhite"].ToString());//beyaz yaka sayısı
        totalSkill= (float)Convert.ToDouble(data["productivityRatio"].ToString());// üretim efekti
        workmanship= (float)Convert.ToDouble(data["workmanshipDaily"].ToString());//işçilik günlük ortalaması
        mngCount.text = (currentManagerCount).ToString();
        workerCount.text = (currentWorkerCount).ToString() + " (" + workerCapFromDatabase.ToString() + ")";
        userDataInfo.ValueChanged -= GetWorkersInfo;
    }

    /// <summary>
    /// İşçi sayısını set eder (Yeni alım sonrası)
    /// </summary>
    /// <param name="workerCount"></param>
    /// <returns></returns>
    private IEnumerator SetWorkersInfo(int workerCount,float workmanCost, float productivity)
    {
        DatabaseReference userReferance = FirebaseDatabase.DefaultInstance.GetReference("users");

        var task = userDataInfo.Child("workersBlue").SetValueAsync(workerCount);
        var task_workman = userDataInfo.Child("workmanshipDaily").SetValueAsync(workmanCost);
        var task_productivity = userDataInfo.Child("productivityRatio").SetValueAsync(productivity);

        yield return new WaitUntil(predicate: () => task.IsCompleted);
    }

    /// <summary>
    /// Yönetici sayısını set eder. (Yeni alım sonrası)
    /// </summary>
    /// <param name="workerCount"></param>
    /// <returns></returns>
    private IEnumerator SetManagersInfo(int workerCount,float workmanCost,float productivity)
    {
        DatabaseReference userReferance = FirebaseDatabase.DefaultInstance.GetReference("users");

        var task = userDataInfo.Child("workersWhite").SetValueAsync(workerCount);
        var task_workman= userDataInfo.Child("workmanshipDaily").SetValueAsync(workmanCost);
        var task_productivity= userDataInfo.Child("productivityRatio").SetValueAsync(productivity);

        yield return new WaitUntil(predicate: () => task.IsCompleted);
    }
}
