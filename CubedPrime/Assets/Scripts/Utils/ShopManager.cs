using Scripts;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public item[] items;

    public int itemIndex;
    private item itemSelected;


    public void NextItem()
    {
        itemIndex++;
        if (itemIndex >= items.Length)
        {
            itemIndex = 0;
        }

        itemSelected = items[itemIndex];
    }
    
    public void BuyItem()
    {
        if (itemSelected.price <= Inventory.money)
        {
            Inventory.money -= itemSelected.price;
            itemSelected.owned = true;
            return;
        }

        Debug.Log("not enough money");
    }
}
