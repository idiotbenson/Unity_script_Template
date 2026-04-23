using HighlightPlus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class CameraCaptureController : MonoBehaviour
{
    Animator anim;
    public GameObject inventoryPanel;
    [SerializeField] private FollowHead inventoryFollowHead;
    [SerializeField] private Animator inventoryAnimator;
    bool isOpened;
    [SerializeField] private bool useSharedInputConfig = true;
    [SerializeField] private KeyCode inventoryToggleKey = KeyCode.I;
    [SerializeField] private KeyCode captureKey = KeyCode.E;

    public float focusDistance = 5.0f;    // 相机对焦距离
    public GameObject reporter;
    public GameObject holder;

    public LayerMask captureLayer;

    //public GameObject[] collectibles;    // 可收集物品的数组
    public List<InventoryItemIcon> itemSlots = new List<InventoryItemIcon>();           // 背包里所有物品槽
    public Sprite emptySlot;             // 空物品槽 

    private PhotoCollectibleItem currentCollectible;    // 当前正在拍照的可收集物品
    public int collectedCount = 0;           // 已收集的物品数量

    public List<string> itemDataString = new List<string>();
    public List<string> loadedDataString = new List<string>();
    public List<PhotoCollectibleItem> AllCollectibleItems = new List<PhotoCollectibleItem>();

    public Book CollectionBook;

    //public List<string> collectedItemName = new List<string>();

    private void Start()
    {
        anim = GetComponent<Animator>();
        if (inventoryPanel != null)
        {
            if (inventoryFollowHead == null)
            {
                inventoryFollowHead = inventoryPanel.GetComponent<FollowHead>();
            }

            if (inventoryAnimator == null)
            {
                inventoryAnimator = inventoryPanel.GetComponent<Animator>();
            }
        }

        reporter = GameObject.Find("Reporter");
        if (reporter != null)
        {
            var holderTransform = reporter.transform.Find("Holder");
            if (holderTransform != null)
            {
                holder = holderTransform.gameObject;
            }
        }

        if (!holder)
        {
            this.enabled = false;
            return;
        }
        AllCollectibleItems = GameObject.FindObjectsOfType<PhotoCollectibleItem>().ToList();

        //Init
        LoadGame();

        for (int i = 0; i < loadedDataString.Count; i++)
        {
            itemSlots[i].LoadData(loadedDataString[i]);

            //Update book
            CollectionBook.AllBookPages[i].SetUpData(Resources.Load<CaptureInventoryItemData>("InventoryItem/" + loadedDataString[i]));
        }

        itemDataString = loadedDataString;
        collectedCount = itemDataString.Count;

        for (int i = 0; i < AllCollectibleItems.Count; i++)
        {
            if (itemDataString.Count == 0)
            {
                break;
            }

            for (int j = 0; j < itemDataString.Count; j++)
            {
                if (AllCollectibleItems[i].itemData.iconName == itemDataString[j])
                {
                    AllCollectibleItems[i].RemoveCollectibleItem();
                    AllCollectibleItems[i] = null;
                    break;
                }
            }

        }

        //remove all null elements
        AllCollectibleItems.RemoveAll(item => item == null);



    }


    private void Update()
    {
        KeyCode activeInventoryKey = inventoryToggleKey;
        KeyCode activeCaptureKey = captureKey;
        if (useSharedInputConfig && InputConfig.TryGet(out InputConfig inputConfig))
        {
            activeInventoryKey = inputConfig.inventoryKey;
            activeCaptureKey = inputConfig.captureKey;
        }

        if (holder)
        {
            if (holder.activeSelf)
            {
                TakePhoto(activeCaptureKey);
                Debug.Log("holder" + holder.activeSelf);
            }
            else
            {
                CancelHighlight();
            }
        }
        if (Input.GetKeyDown(activeInventoryKey))
        {
            if (inventoryPanel == null || inventoryAnimator == null)
            {
                return;
            }

            if (!isOpened)
            {
                inventoryFollowHead?.SetInFrontOfHead();
                inventoryAnimator.SetTrigger("Open");
                isOpened = true;
            }
            else
            {
                inventoryAnimator.SetTrigger("Close");
                isOpened = false;
            }


        }

    }






    // 拍照功能
    public void TakePhoto()
    {
        TakePhoto(captureKey);
    }

    private void TakePhoto(KeyCode activeCaptureKey)
    {
        RaycastHit hit;
        Ray ray = new Ray(holder.transform.position, holder.transform.forward * focusDistance);
        if (Physics.SphereCast(ray, 0.1f, out hit, 10f, captureLayer))
        {
            Debug.Log("Hitted capture layer");
            Debug.Log("Hitted" + hit.collider.name);


            GameObject obj = hit.collider.gameObject;
            HighlightObject(obj);  // 高亮显示
            currentCollectible = obj.GetComponent<PhotoCollectibleItem>();

            if (Input.GetKeyDown(activeCaptureKey))
            {
                Debug.Log("Taken pict");
                UpdateInventory();
            }

        }
        else
        {
            CancelHighlight();  // 高亮显示
        }
    }

    // 高亮显示
    private void HighlightObject(GameObject obj)
    {
        if (obj.TryGetComponent<HighlightEffect>(out HighlightEffect outlineableObj))
        {
            outlineableObj.highlighted = true;
        }
    }

    // 取消高亮显示
    private void CancelHighlight()
    {
        if (currentCollectible != null)
        {
            if (currentCollectible.TryGetComponent<HighlightEffect>(out HighlightEffect outlineableObj))
            {
                outlineableObj.highlighted = false;
            }
            currentCollectible = null;
        }
    }

    // 是否为可收集物品
    //private bool IsCollectible(GameObject obj)
    //{
    //    foreach (GameObject collectible in collectibles)
    //    {
    //        if (collectible == obj)
    //        {
    //            return true;
    //        }
    //    }
    //    return false;
    //}





    // 更新背包
    private void UpdateInventory()
    {
        Debug.Log("update inventory");

        if (currentCollectible != null)
        {
            Debug.Log("itemSlots" + itemSlots.Count);
            for (int i = 0; i < itemSlots.Count; i++)
            {
                Debug.Log("Scan all itemlots");
                if (itemSlots[i].IconImage.sprite == emptySlot)   // 找到第一个空物品槽
                {
                    Debug.Log("Found empty space in inventory");

                    int ContinueChecking = 1;

                    if (itemSlots[i].itemData == null)
                    {
                        for (int n = 0; n < itemDataString.Count; n++)
                        {
                            if (itemDataString[n] == currentCollectible.itemData.iconName)
                            {
                                Debug.Log("Same object stored in inventory");
                                ContinueChecking = 0;
                                break;
                            }
                        }

                    }

                    if (ContinueChecking != 1)
                    {
                        break;
                    }

                    itemSlots[i].SetUpData(currentCollectible.itemData);
                    itemDataString.Add(currentCollectible.itemData.iconName);

                    //UpdateBook
                    UpdateBook(i, currentCollectible.itemData);

                    SaveGame(itemDataString);

                    currentCollectible.RemoveCollectibleItem();   // 收集物品脱离场景
                    currentCollectible.SetCollected(true); //设置为已收集
                    currentCollectible = null;
                    collectedCount++;   // 更新已收集数量
                    //if (collectedCount == collectibles.Length)   // 全部收集完成
                    //{
                    //    Debug.Log("You have collected all the items!");
                    //}


                    Debug.Log("Updated Icon");
                    break;
                }
            }
        }
        else
        {
            Debug.Log(currentCollectible + " is");
        }


    }

    void UpdateBook(int i, CaptureInventoryItemData data)
    {
        CollectionBook.AllBookPages[i].SetUpData(data);
    }




    public void SaveGame(List<string> data)
    {
        FileStream file = null;

        try
        {
            BinaryFormatter bf = new BinaryFormatter();

            if (File.Exists(Application.persistentDataPath + "/GameData.dat"))
            {
                file = File.Open(Application.persistentDataPath + "/GameData.dat", FileMode.Open);
            }
            else
            {
                file = File.Create(Application.persistentDataPath + "/GameData.dat");

            }

            //file = File.Create(Application.persistentDataPath + "/GameData.dat");

            if (data != null)
            {

                Debug.Log("SaveGame");
                bf.Serialize(file, data);

            }


        }
        catch (Exception e)
        {

        }
        finally
        {
            if (file != null)
            {
                file.Close();
            }

        }
    }

    public void LoadGame()
    {
        FileStream file = null;

        try
        {
            BinaryFormatter bf = new BinaryFormatter();

            file = File.Open(Application.persistentDataPath + "/GameData.dat", FileMode.Open);

            loadedDataString = (List<string>)bf.Deserialize(file);

        }
        catch (Exception e)
        {

        }
        finally
        {
            if (file != null)
            {
                file.Close();
            }

        }
    }


    //// 检查是否收集完成
    //private bool CheckCollectedAll()
    //{
    //    foreach (GameObject collectible in collectibles)
    //    {
    //        if (!collectible.GetComponent<CollectibleItem>().IsCollected())
    //        {
    //            return false;
    //        }
    //    }
    //    return true;
    //}

    //// 重置收集状态
    //private void ResetCollected()
    //{
    //    foreach (GameObject collectible in collectibles)
    //    {
    //        collectible.GetComponent<CollectibleItem>().SetCollected(false);
    //    }
    //    collectedCount = 0;
    //}

    //public void RestartGame()
    //{
    //    ResetCollected();
    //    foreach (GameObject collectible in collectibles)
    //    {
    //        collectible.SetActive(true);
    //    }
    //    foreach (Image itemSlot in itemSlots)
    //    {
    //        itemSlot.sprite = emptySlot;
    //    }
    //}
}
