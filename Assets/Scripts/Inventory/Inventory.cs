using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private GameObject _panelInventory;
    [SerializeField] private GameObject _player;

    public int currentID;
    public ItemInventory currentItem;

    public RectTransform movingObject;
    public Vector3 offset;

    public DataBase data;
    
    public List<ItemInventory> items = new();
    public GameObject gameObjectShow;
    public GameObject InventoryMainObject;
    public GameObject background;

    public int maxCount = 64;
    
    public void Start()
    {
        background.SetActive(false);
        if (items.Count == 0)
        {
            AddGraphics();
            for (int i = 0; i < 5; i++)
            {
                GameObject newItem = Instantiate(gameObjectShow, _panelInventory.transform) as GameObject;
                newItem.name = items[i].id.ToString();
                newItem.GetComponent<Image>().sprite = items[i].image;
                newItem.GetComponentInChildren<Text>().text = items[i].count.ToString();
            }
        }

        for (int i = 0; i < maxCount; i++)
        {
            AddItem(i, data.items[UnityEngine.Random.Range(0, data.items.Count)], UnityEngine.Random.Range(0, maxCount));
        }
        UpdateInventory();
    }

    public void Update()
    {
        if (currentID != -1 && currentID != 0)
        {
            MoveObject();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!background.activeSelf && _player.GetComponent<PlayerMovement>().GetMovementStatus())
            {
                _player.GetComponent<PlayerMovement>().UnlockMovement();
                Cursor.lockState = CursorLockMode.Confined;
                //Cursor.visible = true;

                background.SetActive(true);
            }
            else if (background.activeSelf && !_player.GetComponent<PlayerMovement>().GetMovementStatus() && currentID == -1)
            {
                _player.GetComponent<PlayerMovement>().LockMovement();

                Cursor.lockState = CursorLockMode.Locked;
                //Cursor.visible = false;

                background.SetActive(false);
            }
        }

    }

    public void LateUpdate()
    {
        UpdateInventory();        
    }

    public void SearchForSameItem(Item item, int count)
    {
        for (int i = 0; i < maxCount; i++) 
        {
            if (items[i].id == item.id && items[0].count < 64)
            {
                items[i].count += count;

                if (items[i].count > 64)
                {
                    count = items[i].count - 64;
                    items[i].count = 64;
                }
                else
                {
                    count = 0;
                    i = maxCount;
                }
            }
        }
        if (count > 0)
        {
            for (int i = 0; i < maxCount; i++)
            {
                if (items[i].id == 0)
                {
                    AddItem(i, item, count);
                    i = maxCount;
                }
            }
        }
    }

    public void AddItem(int id, Item item, int count)
    {
        items[id].id = item.id;
        items[id].count = count;
        items[id].image = data.items[item.id].image;
        items[id].ItemGameObject.GetComponent<Image>().sprite = item.image;
        
        if (count > 1 && item.id != 0)
        {
            items[id].ItemGameObject.GetComponentInChildren<Text>().text = count.ToString();
        }
        else
        {
            items[id].ItemGameObject.GetComponentInChildren<Text>().text = "";
        }
    }

    public void AddInventoryItem(int id, ItemInventory invItem)
    {
        items[id].id = invItem.id;
        items[id].count = invItem.count;
        items[id].image = data.items[invItem.id].image;
        items[id].ItemGameObject.GetComponent<Image>().sprite = data.items[invItem.id].image;

        if (invItem.count > 1 && invItem.id != 0)
        {
            items[id].ItemGameObject.GetComponentInChildren<Text>().text = invItem.count.ToString();
        }
        else
        {
            items[id].ItemGameObject.GetComponentInChildren<Text>().text = "";
        }
    }
    public void AddGraphics()
    {
        for (int i = 0; i < maxCount; i++)
        {
            GameObject newItem = Instantiate(gameObjectShow, InventoryMainObject.transform);
            
            newItem.name = i.ToString();

            ItemInventory itemInvontory = new()
            {
                ItemGameObject = newItem
            };

            RectTransform rectTransform = newItem.GetComponent<RectTransform>();
            rectTransform.localPosition = new Vector3(0, 0, 0);
            rectTransform.localScale = new Vector3(1, 1, 1);

            newItem.GetComponentInChildren<RectTransform>().localScale = new Vector3(1, 1, 1);

            Button tempButton = newItem.GetComponent<Button>();

            tempButton.onClick.AddListener(delegate { SelectObject(); });

            items.Add(itemInvontory);
        }
    }

    public void UpdateInventory()
    {
        for (int i = 0; i < maxCount; i++)
        {
            if (items[i].id != 0 && items[i].count > 1)
            {
                items[i].ItemGameObject.GetComponentInChildren<Text>().text = items[i].count.ToString();
            }
            else
            {
                items[i].ItemGameObject.GetComponentInChildren<Text>().text = "";
            }

            items[i].ItemGameObject.GetComponent<Image>().sprite = data.items[items[i].id].image;
        }
        Image[] images = _panelInventory.GetComponentsInChildren<Image>();
        Text[] texts = _panelInventory.GetComponentsInChildren<Text>();
        for (int i = 0; i < 5; i++)
        {
            images[i + 1].sprite = data.items[items[i].id].image;

            if (items[i].id != 0 && items[i].count > 1)
            {
                texts[i].text = items[i].count.ToString();
            }
            else
            {
                texts[i].text = "";
            }
        }
    }

    public void SelectObject()
    {
        if (currentID == -1)
        {
            if (items[int.Parse(eventSystem.currentSelectedGameObject.name)].id == 0)
            {
                // maybe want fix
                return;
            }
            currentID = int.Parse(eventSystem.currentSelectedGameObject.name);
            PlayerPrefs.SetInt("CurrentID", currentID);
            currentItem = CopyInventoryItem(items[currentID]);

            movingObject.gameObject.SetActive(true);
            movingObject.GetComponent<Image>().sprite = data.items[currentItem.id].image;

            AddItem(currentID, data.items[0], 0);
        }
        else
        {
            ItemInventory itemInvontory = items[int.Parse(eventSystem.currentSelectedGameObject.name)];

            if (currentItem.id != itemInvontory.id)
            {
                AddInventoryItem(currentID, itemInvontory);

                AddInventoryItem(int.Parse(eventSystem.currentSelectedGameObject.name), currentItem);
            }
            else
            {
                if (itemInvontory.count + currentItem.count <= 64)
                {
                    itemInvontory.count += currentItem.count;
                }
                else
                {
                    AddItem(currentID, data.items[itemInvontory.id], itemInvontory.count + currentItem.count - 64);
                    itemInvontory.count = 64;
                }
            }
            
            currentID = -1;
            PlayerPrefs.SetInt("CurrentID", currentID);
            movingObject.gameObject.SetActive(false);
        }
        UpdateInventory();
    }

    private ItemInventory CopyInventoryItem(ItemInventory oldItem)
    {
        ItemInventory newItem = new()
        {
            id = oldItem.id,
            ItemGameObject = oldItem.ItemGameObject,
            count = oldItem.count
        };

        return newItem;
    }

    public void MoveObject()
    {

        Vector3 position = Input.mousePosition + offset;

        movingObject.position = position;
    }

    public void LoadData(List<Save.SaveItemInventory> inventoris)
    {
        for (int i = 0; i < inventoris.Count; i++)
        {
            items[inventoris[i].index].id = inventoris[i].id;
            items[inventoris[i].index].count = inventoris[i].count;
            for (int j = 0; j < data.items.Count; j++)
            {
                if (data.items[j].id == inventoris[i].id)
                {
                    items[inventoris[i].index].image = data.items[j].image;
                    break;
                }
            }
        }
    }
}

[System.Serializable]
public class ItemInventory
{
    public int id;
    public GameObject ItemGameObject;

    public int count;
    public Sprite image;
}