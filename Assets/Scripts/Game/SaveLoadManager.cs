using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SaveLoadManager : MonoBehaviour
{
    [SerializeField] private int seconds = 1;
    [SerializeField] private int finalTimeScale = 1;
    [SerializeField] private GameObject loadingScreen;

    public List<ItemInventory> inventory;
    public GameObject inventoryObject;
    public int sceneID = 0;
    private string _file_path;

    public void Start()
    {
        _file_path = Application.persistentDataPath + "/save.gamesave";
    }

    public void SaveGame()
    {
        BinaryFormatter bf = new();
        FileStream fileStream = new(_file_path, FileMode.Create);

        Save save = new()
        {
            sceneID = sceneID
        };

        save.SaveInventory(inventory);

        bf.Serialize(fileStream, save);

        fileStream.Close();
    }

    public void LoadGame()
    {
        if (!File.Exists(_file_path))
        {
            return;
        }
        BinaryFormatter bf = new();
        FileStream fileStream = new(_file_path, FileMode.Open);
        Save save = (Save)bf.Deserialize(fileStream);
        fileStream.Close();
        inventoryObject.GetComponent<Inventory>().LoadData(save.items);
    }

    public void LoadSceneWithLoadingScreen()
    {
        loadingScreen.SetActive(true);

        StartCoroutine(LoadAsync());
    }

    private IEnumerator LoadAsync()
    {
        yield return new WaitForSecondsRealtime(seconds);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneID);

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.allowSceneActivation)
            {
                Time.timeScale = finalTimeScale;
            }
            yield return null;
        }
    }
}

[System.Serializable]
public class Save
{
    public float sceneID;
    public List<SaveItemInventory> items;

    [System.Serializable]
    public struct SaveItemInventory
    {
        public int id;
        public int count;
        public int index;
        public SaveItemInventory(int id, int count, int index)
        {
            this.id = id;
            this.count = count;
            this.index = index;
        }
    }

    public void SaveInventory(List<ItemInventory> items)
    {
        for (int i = 0; i < items.Count; i++)
        {
            this.items.Add(new SaveItemInventory(items[i].id, items[i].count, i));
        }
    }
}