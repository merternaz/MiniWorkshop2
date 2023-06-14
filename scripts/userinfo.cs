using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class userinfo 
{

    public string businessname;
    public string username;
    public string email;
    public string password;
    public string mobileID;
    public int level;
    public float cash;
    public float stockCapacity;
    public float customerExperiance;
    public float sectorExperiance;
    public int workersWhite,workersBlue; // beyaz ve mavi yaka işçi sayısı--işçi olmadan makina çalışmayacak. 
    public float productivityRatio, workmanshipDaily;

    public userinfo(string business,string usr,string mail,string pw)
    {
        this.businessname = business;
        this.username = usr;
        this.email = mail;
        this.password = pw;
        this.mobileID =SystemInfo.deviceUniqueIdentifier; ;
        this.level = 1;
        this.cash = 10000f;
        this.stockCapacity = 500f;
        this.customerExperiance = 0f;
        this.sectorExperiance = 0f;
        this.workersWhite = 0;
        this.workersBlue = 0;
        this.workmanshipDaily = 0;//günlük işçilik gideri
        this.productivityRatio = 0;// üretim etkisi (çalışanların)
    }
    
}
