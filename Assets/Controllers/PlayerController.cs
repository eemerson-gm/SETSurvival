using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Controllers.Objects;

enum ItemType
{
    Tree,
    Rock,
    Bush,
    Table,
    Pickaxe,
    Axe,
    Home,
    Well
}

public class PlayerController : MonoBehaviour
{

    //Defines the player variables.
    [Header("Player Variables")]
    [SerializeField] private Camera PlayerCamera;
    [SerializeField] private Rigidbody PlayerBody;
    [SerializeField] private float PlayerSpeed;
    [SerializeField] private float PlayerAccel;
    [SerializeField] private float PlayerJump;
    public float PlayerThirst = 100;
    [Header("Physics Variables")]
    [SerializeField] private float PlayerMoveX;
    [SerializeField] private float PlayerMoveZ;
    [SerializeField] private Vector3 PlayerDirectionX;
    [SerializeField] private Vector3 PlayerDirectionZ;
    public Canvas CanvasInventory;
    public Canvas CanvasTutorial;
    public Item[] PlayerInventory = new Item[6];
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
    public GameObject ObjectTable;
    public GameObject ObjectHome;
    public GameObject ObjectWell;
    public Image IconMaterial;
    public Slider WaterSlider;
    public TableController Table;
    public int SelectIndex = -1;
    public GameObject ObjectPickaxe;
    public GameObject ObjectAxe;
    public float ToolRotation;
    public AudioSource SoundTree;
    public AudioSource SoundRock;
    public AudioSource SoundBush;
    public AudioSource SoundWater;

    void Start()
    {

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        //Clears the player inventory.
        ClearInventory();

        //Starts the water deterioration.
        InvokeRepeating("SubtractWater", 1.0f, 1.0f);

        //Gets the crafting costs.
        ItemRecipes1[(int)ItemType.Table].type = (int)ItemType.Tree;
        ItemRecipes2[(int)ItemType.Table].type = (int)ItemType.Rock;
        ItemRecipes1[(int)ItemType.Table].amount = 25;
        ItemRecipes2[(int)ItemType.Table].amount = 25;
        ItemRecipes1[(int)ItemType.Pickaxe].type = (int)ItemType.Bush;
        ItemRecipes2[(int)ItemType.Pickaxe].type = (int)ItemType.Rock;
        ItemRecipes1[(int)ItemType.Pickaxe].amount = 20;
        ItemRecipes2[(int)ItemType.Pickaxe].amount = 10;
        ItemRecipes1[(int)ItemType.Axe].type = (int)ItemType.Bush;
        ItemRecipes2[(int)ItemType.Axe].type = (int)ItemType.Rock;
        ItemRecipes1[(int)ItemType.Axe].amount = 20;
        ItemRecipes2[(int)ItemType.Axe].amount = 10;

        //Hides the tools.
        ObjectPickaxe.SetActive(false);
        ObjectAxe.SetActive(false);

    }

    void Update()
    {

        //Checks if the player is holding a pickaxe or axe.
        switch (PlayerInventory[0].type)
        {

            case (int)ItemType.Pickaxe:
                ObjectAxe.SetActive(false);
                ObjectPickaxe.SetActive(true);
                break;

            case (int)ItemType.Axe:
                ObjectPickaxe.SetActive(false);
                ObjectAxe.SetActive(true);
                break;

            default:
                ObjectPickaxe.SetActive(false);
                ObjectAxe.SetActive(false);
                break;

        }

        //Checks if inventory is not active.
        if (CanvasInventory.gameObject.activeSelf == false)
        {

            //Gets a ray from the camera.
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            var hit = new RaycastHit();

            //Gets the game object that has been hit.
            if (Physics.Raycast(ray, out hit))
            {

                //Draws a line to the object.
                Debug.DrawLine(hit.point, transform.position, Color.red);

                //Checks the distance of the ray.
                if ((hit.distance < 5) && (CanvasTutorial.gameObject.activeSelf == false))
                {

                    //Checks if the hand inventory item is the table.
                    if ((PlayerInventory[0].type == -1) || (PlayerInventory[0].type == (int)ItemType.Tree) || (PlayerInventory[0].type == (int)ItemType.Rock) || (PlayerInventory[0].type == (int)ItemType.Bush) || (PlayerInventory[0].type == (int)ItemType.Pickaxe) || (PlayerInventory[0].type == (int)ItemType.Axe))
                    {

                        //Checks the type of material which is being collected.
                        switch (hit.transform.gameObject.tag)
                        {

                            case "Tree":
                                IconMaterial.gameObject.SetActive(true);
                                IconMaterial.sprite = SpriteWood;
                                break;

                            case "Rock":
                                IconMaterial.gameObject.SetActive(true);
                                IconMaterial.sprite = SpriteRock;
                                break;

                            case "Bush":
                                IconMaterial.gameObject.SetActive(true);
                                IconMaterial.sprite = SpriteBush;
                                break;

                            case "Well":
                            case "Water":
                                IconMaterial.gameObject.SetActive(true);
                                IconMaterial.sprite = SpriteWater;
                                break;

                            case "Table":
                                IconMaterial.gameObject.SetActive(true);
                                IconMaterial.sprite = SpriteHammer;
                                break;

                            default:
                                IconMaterial.gameObject.SetActive(false);
                                break;

                        }

                        //Checks if the shoot button has been pressed.
                        if (Input.GetMouseButtonDown(0))
                        {

                            Console.WriteLine(hit.transform.gameObject.tag);

                            //Sets the tool rotation.
                            ToolRotation = 45;

                            //Checks the type of material which is being collected.
                            switch (hit.transform.gameObject.tag)
                            {

                                case "Tree":
                                    AddInventory((int)ItemType.Tree, 1 + (4 * Convert.ToInt32(PlayerInventory[0].type == (int)ItemType.Axe)));
                                    SoundTree.Play();
                                    break;

                                case "Rock":
                                    AddInventory((int)ItemType.Rock, 1 + (4 * Convert.ToInt32(PlayerInventory[0].type == (int)ItemType.Pickaxe)));
                                    SoundRock.Play();
                                    break;

                                case "Bush":
                                    AddInventory((int)ItemType.Bush, 1);
                                    SoundBush.Play();
                                    break;

                                case "Well":
                                case "Water":
                                    ChangeWater(10.0f);
                                    SoundWater.Play();
                                    break;

                                case "Table":
                                    Cursor.visible = true;
                                    Cursor.lockState = CursorLockMode.None;
                                    hit.transform.GetChild(0).gameObject.SetActive(true);
                                    CanvasInventory.gameObject.SetActive(true);
                                    Table = hit.transform.GetComponent<TableController>();
                                    break;

                            }

                        }

                    }
                    else if (PlayerInventory[0].type == (int)ItemType.Table)
                    {

                        //Display the build icon.
                        IconMaterial.gameObject.SetActive(true);
                        IconMaterial.sprite = SpriteTable;

                        //Checks if the shoot button has been pressed.
                        if (Input.GetMouseButtonDown(0))
                        {

                            //Creates a table object at the position.
                            GameObject table = Instantiate(ObjectTable, hit.point, Quaternion.identity);
                            table.GetComponent<TableController>().Player = this;

                            //Gets the index of the item object.
                            var index = Array.FindIndex(PlayerInventory, item => (item.type == (int)ItemType.Table));
                            PlayerInventory[index].amount -= 1;
                            UpdateInventory();

                        }

                    }
                    else if (PlayerInventory[0].type == (int)ItemType.Home)
                    {

                        //Display the build icon.
                        IconMaterial.gameObject.SetActive(true);
                        IconMaterial.sprite = SpriteHome;

                        //Checks if the shoot button has been pressed.
                        if (Input.GetMouseButtonDown(0))
                        {

                            //Creates a table object at the position.
                            GameObject table = Instantiate(ObjectHome, hit.point, Quaternion.identity);

                            //Gets the index of the item object.
                            var index = Array.FindIndex(PlayerInventory, item => (item.type == (int)ItemType.Home));
                            PlayerInventory[index].amount -= 1;
                            UpdateInventory();

                        }

                    }
                    else if (PlayerInventory[0].type == (int)ItemType.Well)
                    {

                        //Display the build icon.
                        IconMaterial.gameObject.SetActive(true);
                        IconMaterial.sprite = SpriteWell;

                        //Checks if the shoot button has been pressed.
                        if (Input.GetMouseButtonDown(0))
                        {

                            //Creates a table object at the position.
                            GameObject table = Instantiate(ObjectWell, hit.point, Quaternion.identity);

                            //Gets the index of the item object.
                            var index = Array.FindIndex(PlayerInventory, item => (item.type == (int)ItemType.Well));
                            PlayerInventory[index].amount -= 1;
                            UpdateInventory();

                        }

                    }
                    else
                    {
                        IconMaterial.gameObject.SetActive(false);
                    }

                }
                else
                {
                    IconMaterial.gameObject.SetActive(false);
                }

            }
            else
            {
                IconMaterial.gameObject.SetActive(false);
            }

            //Checks if the jump key has been pressed.
            if (Input.GetKeyDown(KeyCode.Space) && Physics.Raycast(transform.position, -Vector3.up, GetComponent<Collider>().bounds.extents.y + 0.2f))
            {

                //Adds vertical force to the player.
                PlayerBody.AddForce(Vector3.up * PlayerJump, ForceMode.Impulse);

            }

        }

        //Checks if the player is opening inventory.
        if (Input.GetKeyDown(KeyCode.I) && (CanvasTutorial.gameObject.activeSelf == false))
        {

            //Enables the inventory object.
            if (CanvasInventory.gameObject.activeSelf != false)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                CanvasInventory.gameObject.SetActive(false);
                SelectIndex = -1;
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                CanvasInventory.gameObject.SetActive(true);
            }

        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            CanvasTutorial.gameObject.SetActive(true);
        }

    }

    void FixedUpdate()
    {

        GetComponent<Rigidbody>().velocity = new Vector3(0, GetComponent<Rigidbody>().velocity.y, 0);

        //Updates the player speed.
        var speed = PlayerSpeed - (Convert.ToInt32(PlayerThirst < 40) * (PlayerSpeed / 2));

        //Gets the movement direction for the keyboard.
        float MoveX = Convert.ToInt32(Input.GetKey(KeyCode.D)) - Convert.ToInt32(Input.GetKey(KeyCode.A));
        float MoveZ = Convert.ToInt32(Input.GetKey(KeyCode.W)) - Convert.ToInt32(Input.GetKey(KeyCode.S));

        //Checks if the player is currently moving.
        if ((MoveX != 0) || (MoveZ != 0))
        {

            //Updates the direction of the player.
            PlayerDirectionX = transform.right;
            PlayerDirectionZ = transform.forward;

        }

        //Approaches the maximum player speed for acceleration.
        PlayerMoveX = Mathf.MoveTowards(PlayerMoveX, MoveX, PlayerAccel * Time.deltaTime);
        PlayerMoveZ = Mathf.MoveTowards(PlayerMoveZ, MoveZ, PlayerAccel * Time.deltaTime);

        //Calculates the movement vector.
        Vector3 PlayerDirection = PlayerDirectionX * PlayerMoveX + PlayerDirectionZ * PlayerMoveZ;

        //Moves the player in the caulculated direction.
        PlayerBody.MovePosition(transform.position + PlayerDirection * speed * Time.deltaTime * Convert.ToInt32(!CanvasInventory.gameObject.activeSelf && !CanvasTutorial.gameObject.activeSelf));
        
    }

    protected void ClearInventory()
    {

        //Closes the inventory screen.
        CanvasInventory.gameObject.SetActive(false);

        //Clears all the slots in the inventory.
        foreach(var icon in IconInventory)
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
        var index = Array.FindIndex(PlayerInventory, item => (item.type == type));

        //Gets a counter variable.
        int counter = 0;

        //Checks if the inventory contains that item.
        if (index == -1)
        {

            //Searches for a free slot in the inventory.
            foreach(var item in PlayerInventory)
            {

                //Checks if the item is empty.
                if(item.type == -1)
                {

                    //Sets the item and amount of the slot.
                    PlayerInventory[counter].type = type;
                    PlayerInventory[counter].amount = amount;
                    break;

                }

                //Increments the counter.
                counter++;

            }

        }
        else
        {

            //Sets the item and amount of the slot.
            PlayerInventory[index].type = type;
            PlayerInventory[index].amount += amount;

        }

        //Updates the inventory.
        UpdateInventory();
       
    }

    public void UpdateInventory()
    {

        //Gets a counter variable.
        int counter = 0;

        //Updates the visuals in the inventory.
        foreach (var item in PlayerInventory)
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
        if ((Table != null) && (Table.SelectIndex != -1))
        {

            //Checks if the slot contains that item.
            if(PlayerInventory[slot].type == Table.TableInventory[Table.SelectIndex].type)
            {

                //Adds the item to the slot amount.
                PlayerInventory[slot].amount += Table.TableInventory[Table.SelectIndex].amount;
                Table.TableInventory[Table.SelectIndex].amount = 0;
                Table.TableInventory[Table.SelectIndex].type = 0;
                UpdateInventory();
                Table.UpdateInventory();

                //Resets the selected index.
                SelectIndex = -1;
                Table.SelectIndex = -1;

            }
            else
            {

                //Gets the item from the selected slot.
                Item temp = new Item();
                temp.type = Table.TableInventory[Table.SelectIndex].type;
                temp.amount = Table.TableInventory[Table.SelectIndex].amount;

                //Replaces the item values of the selected slot.
                Table.TableInventory[Table.SelectIndex].type = PlayerInventory[slot].type;
                Table.TableInventory[Table.SelectIndex].amount = PlayerInventory[slot].amount;

                //Replaces the item values of the selected slot.
                PlayerInventory[slot].type = temp.type;
                PlayerInventory[slot].amount = temp.amount;

                //Resets the selected index.
                SelectIndex = -1;
                Table.SelectIndex = -1;

                //Updates the inventory.
                UpdateInventory();
                Table.UpdateInventory();

            }

        }
        else if (SelectIndex == -1)
        {

            //Sets the index of the selected slot.
            SelectIndex = slot;

        }
        else
        {

            //Checks if the slot contains that item.
            if (PlayerInventory[slot].type == PlayerInventory[SelectIndex].type)
            {

                //Adds the item to the slot amount.
                PlayerInventory[slot].amount += PlayerInventory[SelectIndex].amount;
                PlayerInventory[SelectIndex].amount = 0;
                PlayerInventory[SelectIndex].type = 0;
                UpdateInventory();

                //Resets the selected index.
                SelectIndex = -1;

            }
            else
            {

                //Gets the item from the selected slot.
                Item temp = new Item();
                temp.type = PlayerInventory[SelectIndex].type;
                temp.amount = PlayerInventory[SelectIndex].amount;

                //Replaces the item values of the selected slot.
                PlayerInventory[SelectIndex].type = PlayerInventory[slot].type;
                PlayerInventory[SelectIndex].amount = PlayerInventory[slot].amount;

                //Replaces the item values of the selected slot.
                PlayerInventory[slot].type = temp.type;
                PlayerInventory[slot].amount = temp.amount;

                //Resets the selected index.
                SelectIndex = -1;

                //Updates the inventory.
                UpdateInventory();

            }

        }

    }

    public void ChangeWater(float amount)
    {
        PlayerThirst += amount;
        PlayerThirst = Mathf.Clamp(PlayerThirst, 0, 100);
        WaterSlider.value = PlayerThirst / 100;
    }

    public void SubtractWater()
    {
        ChangeWater(-1f * Convert.ToInt32(!CanvasInventory.gameObject.activeSelf && !CanvasTutorial.gameObject.activeSelf));
    }

    public void CraftItem(int itemtype)
    {

        //Gets the item recipe variables.
        var type1 = ItemRecipes1[itemtype].type;
        var type2 = ItemRecipes2[itemtype].type;
        var amount1 = ItemRecipes1[itemtype].amount;
        var amount2 = ItemRecipes2[itemtype].amount;

        //Gets the indexes of the item types in the inventory.
        var index1 = Array.FindIndex(PlayerInventory, item => (item.type == type1));
        var index2 = Array.FindIndex(PlayerInventory, item => (item.type == type2));

        //Checks if both indexes are valid.
        if((index1 != -1) && (index2 != -1))
        {

            //Checks if the amount of the items are enough.
            if((PlayerInventory[index1].amount >= amount1) && (PlayerInventory[index2].amount >= amount2))
            {

                //Removes the resource values.
                PlayerInventory[index1].amount -= amount1;
                PlayerInventory[index2].amount -= amount2;

                //Adds the new item to the player inventory.
                AddInventory(itemtype, 1);

            }
        }
    }

    public void AddInventory(int type)
    {

        //Gets the index of the item object.
        var index = Array.FindIndex(PlayerInventory, item => (item.type == type));

        //Gets a counter variable.
        int counter = 0;

        //Checks if the inventory contains that item.
        if (index == -1)
        {

            //Searches for a free slot in the inventory.
            foreach (var item in PlayerInventory)
            {

                //Checks if the item is empty.
                if (item.type == -1)
                {

                    //Sets the item and amount of the slot.
                    PlayerInventory[counter].type = type;
                    PlayerInventory[counter].amount = 1;
                    break;

                }

                //Increments the counter.
                counter++;

            }

        }
        else
        {

            //Sets the item and amount of the slot.
            PlayerInventory[index].type = type;
            PlayerInventory[index].amount += 1;

        }

        //Updates the inventory.
        UpdateInventory();

    }

}
