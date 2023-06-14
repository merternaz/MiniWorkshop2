using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using Firebase.Auth;
using UnityEngine.UI;
using Firebase;
using System;

public class infobar : MonoBehaviour
{
    public Text bsNameTXT, cashTXT;
    public static float cash;
    // Start is called before the first frame update   

    private void Awake()
    {
        RetrieveData();
    }
    public void RetrieveData()
    {
        auth.userReferance.Child(auth.USER_ID).ValueChanged += BusinessInfo;
    }

    private void BusinessInfo(object sender,ValueChangedEventArgs args)
    {
        
        var userKeys = args.Snapshot.Value as Dictionary<string, object>; // özel unique ID katmanı

        cashTXT.text = userKeys["cash"].ToString();
        bsNameTXT.text= userKeys["businessname"].ToString();
        cash = (float)Convert.ToDouble(userKeys["cash"].ToString());
        auth.userReferance.Child(auth.USER_ID).ValueChanged -= BusinessInfo;
    }
}
