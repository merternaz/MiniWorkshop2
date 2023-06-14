using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item 
{
    public int id;
    public string name;
    public float cost;
    public ItemType iType;
    public Sprite icon;
    public Dictionary<string, float> stats = new Dictionary<string, float>();
    public Dictionary<int, float> comp = new Dictionary<int, float>();


    public Item(int id, string name, float cost, ItemType type,Dictionary<string,float> stats,Dictionary<int,float> components)
    {
        this.id = id;
        this.name = name;
        this.cost = cost;
        this.iType = type;
        this.icon = Resources.Load<Sprite>("items/" + name);
        this.stats = stats;
        this.comp = components;
    }

    public Item(Item item)
    {
        this.id = item.id;
        this.name = item.name;
        this.iType = item.iType;
        this.icon = Resources.Load<Sprite>("items/" + item.name);
        this.stats = item.stats;
        this.comp = item.comp;
    }


}



public enum ItemType
{
    Wood,Metal,Raw,Soil,Textile
}
