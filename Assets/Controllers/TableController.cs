using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Controllers.Objects;
using System;

public class TableController : MonoBehaviour
{

    public PlayerController Player;
    public Canvas CanvasInventory;
    public Item[] TableInventory = new Item[6];
    public Image[] IconInventory = new Image[6];
    public Text[] TextInventory = new Text[6];
    public Item[] ItemRecipes1 = new Item[8];
    public Item[] ItemRecipes2 = new Item[8];
    public Sprite SpriteWood;
    public Sprite SpriteRock;
    public Sprite SpriteBush;
    public Sprite SpriteWater;
    public Sprite SpriteHammer;
    public Sprite SpriteTable;
    public Sprite SpritePickaxe;
    public Sprite SpriteAxe;
    public Sprite SpriteHome;
    public Sprite SpriteWell;
    public int SelectIndex = -1;

    // Start is called before the first frame update
    void Start()
    {
        
        ClearInventory();
        CanvasInventory.gameObject.SetActive(false);
        ItemRecipes1[(int)ItemType.Home].type = (int)ItemType.Tree;
        ItemRecipes2[(int)ItemType.Home].type = (int)ItemType.Rock;
        ItemRecipes1[(int)ItemType.Home].amount = 50;
        ItemRecipes2[(int)ItemType.Home].amount = 50;
        ItemRecipes1[(int)ItemType.Well].type = (int)ItemType.Tree;
        ItemRecipes2[(int)ItemType.Well].type = (int)ItemType.Rock;
        ItemRecipes1[(int)ItemType.Well].amount = 25;
        ItemRecipes2[(int)ItemType.Well].amount = 25;

    }

    // Update is called once per frame
    void Update()
    {

        //Enables the inventory object.
        if (CanvasInventory.gameObject.activeSelf == true)
        {

            //Checks if the player is opening inventory.
            if (Input.GetKeyDown(KeyCode.I))
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                CanvasInventory.gameObject.SetActive(false);
                SelectIndex = -1;

            }

        }

    }

    protected void ClearInventory()
    {

        //Closes the inventory screen.
        CanvasInventory.gameObject.SetActive(false);

        //Clears all the slots in the inventory.
        foreach (var icon in IconInventory)
        {
            icon.gameObject.SetActive(false);
        }
        foreach (var text in TextInventory)
        {
            text.gameObject.SetActive(false);
        }

    }

    protected void AddInventory(int type, int amount)
    {

        //Gets the index of the item object.
        var index = Array.FindIndex(TableInventory, item => (item.type == type));

        //Gets a counter variable.
        int counter = 0;

        //Checks if the inventory contains that item.
        if (index == -1)
        {

            //Searches for a free slot in the inventory.
            foreach (var item in TableInventory)
            {

                //Checks if the item is empty.
                if (item.type == -1)
                {

                    //Sets the item and amount of the slot.
                    TableInventory[counter].type = type;
                    TableInventory[counter].amount = amount;
                    break;

                }

                //Increments the counter.
                counter++;

            }

        }
        else
        {

            //Sets the item and amount of the slot.
            TableInventory[index].type = type;
            TableInventory[index].amount += amount;

        }

        //Updates the inventory.
        UpdateInventory();

    }

    public void UpdateInventory()
    {

        //Gets a counter variable.
        int counter = 0;

        //Updates the visuals in the inventory.
        foreach (var item in TableInventory)
        {

            //Checks if the item is valid.
            if ((item.type != -1) && (item.amount >= 1))
            {

                //Gets the sprite for the icon.
                switch (item.type)
                {

                    case (int)ItemType.Tree:
                        IconInventory[counter].sprite = SpriteWood;
                        break;

                    case (int)ItemType.Rock:
                        IconInventory[counter].sprite = SpriteRock;
                        break;

                    case (int)ItemType.Bush:
                        IconInventory[counter].sprite = SpriteBush;
                        break;

                    case (int)ItemType.Table:
                        IconInventory[counter].sprite = SpriteTable;
                        break;

                    case (int)ItemType.Pickaxe:
                        IconInventory[counter].sprite = SpritePickaxe;
                        break;

                    case (int)ItemType.Axe:
                        IconInventory[counter].sprite = SpriteAxe;
                        break;

                    case (int)ItemType.Home:
                        IconInventory[counter].sprite = SpriteHome;
                        break;

                    case (int)ItemType.Well:
                        IconInventory[counter].sprite = SpriteWell;
                        break;

                }

                //Sets the item text to the item amount.
                TextInventory[counter].text = item.amount.ToString();

                //Sets the items to be active.
                IconInventory[counter].gameObject.SetActive(true);
                TextInventory[counter].gameObject.SetActive(true);

            }
            else
            {

                //Disables the item and text.
                item.type = -1;
                item.amount = 0;
                IconInventory[counter].gameObject.SetActive(false);
                TextInventory[counter].gameObject.SetActive(false);

            }

            //Increments the counter.
            counter++;

        }

    }

    public void MoveItem(int slot)
    {

        //Checks if the selected item is not set.
        if (Player.SelectIndex != -1)
        {

            //Checks if the slot contains that item.
            if (TableInventory[slot].type == Player.PlayerInventory[Player.SelectIndex].type)
            {

                //Adds the item to the slot amount.
                TableInventory[slot].amount += Player.PlayerInventory[Player.SelectIndex].amount;
                Player.PlayerInventory[Player.SelectIndex].amount = 0;
                Player.PlayerInventory[Player.SelectIndex].type = 0;
                UpdateInventory();
                Player.UpdateInventory();

                //Resets the selected index.
                SelectIndex = -1;
                Player.SelectIndex = -1;

            }
            else
            {

                //Gets the item from the selected slot.
                Item temp = new Item();
                temp.type = Player.PlayerInventory[Player.SelectIndex].type;
                temp.amount = Player.PlayerInventory[Player.SelectIndex].amount;

                //Replaces the item values of the selected slot.
                Player.PlayerInventory[Player.SelectIndex].type = TableInventory[slot].type;
                Player.PlayerInventory[Player.SelectIndex].amount = TableInventory[slot].amount;

                //Replaces the item values of the selected slot.
                TableInventory[slot].type = temp.type;
                TableInventory[slot].amount = temp.amount;

                //Resets the selected index.
                SelectIndex = -1;
                Player.SelectIndex = -1;

                //Updates the inventory.
                UpdateInventory();
                Player.UpdateInventory();

            }

        }
        else if(SelectIndex == -1)
        {
            SelectIndex = slot;
        }
        else
        {

            //Checks if the slot contains that item.
            if (TableInventory[slot].type == TableInventory[SelectIndex].type)
            {

                //Adds the item to the slot amount.
                TableInventory[slot].amount += TableInventory[SelectIndex].amount;
                TableInventory[SelectIndex].amount = 0;
                TableInventory[SelectIndex].type = 0;
                UpdateInventory();

                //Resets the selected index.
                SelectIndex = -1;

            }
            else
            {

                //Gets the item from the selected slot.
                Item temp = new Item();
                temp.type = TableInventory[SelectIndex].type;
                temp.amount = TableInventory[SelectIndex].amount;

                //Replaces the item values of the selected slot.
                TableInventory[SelectIndex].type = TableInventory[slot].type;
                TableInventory[SelectIndex].amount = TableInventory[slot].amount;

                //Replaces the item values of the selected slot.
                TableInventory[slot].type = temp.type;
                TableInventory[slot].amount = temp.amount;

                //Resets the selected index.
                SelectIndex = -1;

                //Updates the inventory.
                UpdateInventory();

            }

        }

    }

    public void CraftItem(int itemtype)
    {

        //Gets the item recipe variables.
        var type1 = ItemRecipes1[itemtype].type;
        var type2 = ItemRecipes2[itemtype].type;
        var amount1 = ItemRecipes1[itemtype].amount;
        var amount2 = ItemRecipes2[itemtype].amount;

        //Gets the indexes of the item types in the inventory.
        var index1 = Array.FindIndex(TableInventory, item => (item.type == type1));
        var index2 = Array.FindIndex(TableInventory, item => (item.type == type2));

        //Checks if both indexes are valid.
        if ((index1 != -1) && (index2 != -1))
        {

            //Checks if the amount of the items are enough.
            if ((TableInventory[index1].amount >= amount1) && (TableInventory[index2].amount >= amount2))
            {

                //Removes the resource values.
                TableInventory[index1].amount -= amount1;
                TableInventory[index2].amount -= amount2;

                //Adds the new item to the player inventory.
                AddInventory(itemtype, 1);

            }
        }
    }
}
