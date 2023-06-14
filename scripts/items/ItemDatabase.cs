using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static List<Item> items = new List<Item>();


    public void Awake()
    {
        CreateItemDatabase();
        Debug.LogWarning(GetItem(1).stats["volume"]); // "id=0" olan itemin "volume" özellğini getirir

    }

    public static Item GetItem(int id)
    {

        return items.Find(x => x.id == id);
    }
    public static Item GetItem(string name)
    {
        return items.Find(x => x.name == name);

    }

    public static Item GetItemByType(ItemType itemTyp)
    {
        return items.Find(x => x.iType == itemTyp);

    }

    void CreateItemDatabase()
    {
        items = new List<Item>() // componentler 0 ise hammaddenin kendisi. Alt ürün aranmaz. Diğerleri id,% oran seklindedir
        {
            new Item(1,"wood",3.75f,ItemType.Wood,new Dictionary<string, float>{{"volume",1.5f},{"leadtime",30f } },new Dictionary<int, float>{ {1,1f } }),//0,0f
            new Item(2,"woodplate",7.75f,ItemType.Wood,new Dictionary<string, float>{{"volume",1.9f},{"leadtime",144f }},new Dictionary<int, float>{ {7,1f } }),
            new Item(3,"bricks",10.75f,ItemType.Soil,new Dictionary<string, float>{{"volume",0.85f},{"leadtime",144f }},new Dictionary<int, float>{ {6,0.25f },{ 8, 0.75f } }),
            new Item(4,"iron",5.55f,ItemType.Metal,new Dictionary<string, float>{{"volume",1f},{"leadtime",144f }},new Dictionary<int, float>{ {4,1f } }),//0,0f
            new Item(5,"steelplate",16.85f,ItemType.Metal,new Dictionary<string, float>{{"volume",1.6f},{"leadtime",144f }},new Dictionary<int, float>{ {4,1f } }),
            new Item(6,"sand",2.1f,ItemType.Soil,new Dictionary<string, float>{{"volume",0.3f},{"leadtime",72f }},new Dictionary<int, float>{ {6,1f } }), //0,0f
            new Item(7,"log",5.65f,ItemType.Wood,new Dictionary<string, float>{{"volume",1f},{"leadtime",65f }},new Dictionary<int, float>{ {1,1.2f } }),
            new Item(8,"stone",4.1f,ItemType.Soil,new Dictionary<string, float>{{"volume",0.6f},{"leadtime",144f }},new Dictionary<int, float>{ {8,1f } }),//0,0f
            new Item(9,"paper",7.2f,ItemType.Wood,new Dictionary<string, float>{{"volume",0.8f},{"leadtime",144f }},new Dictionary<int, float>{ {7,15f } }),
            new Item(10,"nut",7.2f,ItemType.Metal,new Dictionary<string, float>{{"volume",0.15f},{"leadtime",144f }},new Dictionary<int, float>{ {4,0.3f } }),
            new Item(11,"screw",7.2f,ItemType.Metal,new Dictionary<string, float>{{"volume",0.2f},{"leadtime",144f }},new Dictionary<int, float>{ {4,0.5f } }),
            new Item(12,"cotton",7.2f,ItemType.Textile,new Dictionary<string, float>{{"volume",0.88f},{"leadtime",288f }},new Dictionary<int, float>{ {12,1f } }),
            new Item(13,"fabric_blue",7.2f,ItemType.Textile,new Dictionary<string, float>{{"volume",1f},{"leadtime",200f }},new Dictionary<int, float>{ {12,1.15f } }),
            new Item(14,"Wool",20.27f,ItemType.Textile,new Dictionary<string, float>{{"volume",0.65f},{"leadtime",144f }},new Dictionary<int, float>{ {14,1f } }),
            new Item(15,"Acrylic",13.6f,ItemType.Textile,new Dictionary<string, float>{{"volume",0.75f},{"leadtime",344f }},new Dictionary<int, float>{ {15,1f } }),
            new Item(16,"Yarn Ball",13.6f,ItemType.Textile,new Dictionary<string, float>{{"volume",0.75f},{"leadtime",144f }},new Dictionary<int, float>{ {14,0.75f },{15,0.25f } }),

        };
    }
}
