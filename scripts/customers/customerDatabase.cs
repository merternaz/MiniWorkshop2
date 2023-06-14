using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class customerDatabase : MonoBehaviour
{
    public static List<customer> cust = new List<customer>();
   

    public void Awake()
    {
        CustomerList();
    }

    public static customer GetCustomer(int id)
    {
        return cust.Find(x => x.cust_id == id);
    }

    public static customer GetCustomer(ItemType sectorType)
    {
        return cust.Find(x => x.sector == sectorType);
    }

    void CustomerList()
    {
        cust = new List<customer>()
        {

            new customer(1,ItemType.Wood,"HD HOME",new Dictionary<string, int>{{"min",3 },{"max",5 } }),
            new customer(2,ItemType.Metal,"MG METAL FACTORY",new Dictionary<string, int>{{"min",3 },{"max",7 } }),
            new customer(3,ItemType.Wood,"GREEN FURNITURE",new Dictionary<string, int>{{"min",3 },{"max",5 } }),
            new customer(4,ItemType.Wood,"LUX WOOD CO.",new Dictionary<string, int>{{"min",2 },{"max",4 } }),
            new customer(5,ItemType.Wood,"BAM-BOO INC.",new Dictionary<string, int>{{"min",6 },{"max",9 } }),
            new customer(6,ItemType.Metal,"IRONISH INC.",new Dictionary<string, int>{{"min",2 },{"max",7 } }),
            new customer(7,ItemType.Textile,"WOOLMANN TEXTILE",new Dictionary<string, int>{{"min",2 },{"max",7 } }),
            new customer(8,ItemType.Textile,"MULTI-KNIT TEX",new Dictionary<string, int>{{"min",2 },{"max",7 } }),
            new customer(9,ItemType.Textile,"MERCHAN-DYER TEX",new Dictionary<string, int>{{"min",2 },{"max",7 } }),
            new customer(10,ItemType.Textile,"OVERLOOK TEXTILE",new Dictionary<string, int>{{"min",2 },{"max",7 } }),
            new customer(11,ItemType.Textile,"ACRYL TEXTILE",new Dictionary<string, int>{{"min",2 },{"max",7 } }),


    };
    }
}
