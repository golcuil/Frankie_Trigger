using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutfitShopManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private OutfitButton outfitButtonPrefab;
    [SerializeField] private Transform outfitButtonsParent;

    [SerializeField] private SkinnedMeshRenderer playerMaterial;
    [SerializeField] private SkinnedMeshRenderer outfitPlayerMaterial;

    [SerializeField] private Button purchaseButton;

    private int clickedOutfitIndex;
    private int lastOutfitIndex;

    [Header("Data")]
    [SerializeField] private OutfitData[] outfitDatas;
    // Start is called before the first frame update
    void Start()
    {
        LoadLastOutfit();

        ApplyPlayerSkin(lastOutfitIndex);

        SaveOutfitState(0);

        CreateOutfitButtons();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateOutfitButtons()
    {
        for (int i = 0; i < outfitDatas.Length; i++)
        {
            CreateOutfitButton(i);
        }
    }

    private void CreateOutfitButton(int index)
    {
        bool isUnlocked = IsOutfitUnlocked(index);

        OutfitButton buttonInstance = Instantiate(outfitButtonPrefab, outfitButtonsParent);
        buttonInstance.Configure(outfitDatas[index], isUnlocked);

        buttonInstance.GetButton().onClick.AddListener(() => OutfitButtonCallback(index));
    }

    private void OutfitButtonCallback(int index)
    {
        

        int outfitMaterialsCount = outfitPlayerMaterial.materials.Length;

        Material[] newOutfitMaterials = new Material[outfitMaterialsCount];
        for (int i = 0; i < outfitMaterialsCount; i++)
        {
            newOutfitMaterials[i] = outfitDatas[index].mesh;
        }

        //Change the outfit of fake player
        outfitPlayerMaterial.materials = newOutfitMaterials;
        bool isUnlocked = IsOutfitUnlocked(index);
        //Manage the purchase button interactability
        purchaseButton.interactable = CashManager.Instance.CanPurchase(outfitDatas[index].price) && !IsOutfitUnlocked(index);

        clickedOutfitIndex = index;

        if(isUnlocked)
        {
            ApplyPlayerSkin(clickedOutfitIndex);

            //Save the last outfit index
            lastOutfitIndex = clickedOutfitIndex;
            SaveLastOutfit();
        }

    }

    private void ApplyPlayerSkin(int index)
    {
        int playerMaterialsCount = playerMaterial.materials.Length;

        Material[] newPlayerMaterials = new Material[playerMaterialsCount];
        for (int i = 0; i < playerMaterialsCount; i++)
        {
            newPlayerMaterials[i] = outfitDatas[index].mesh;
        }

        playerMaterial.materials = newPlayerMaterials;
    }

    public void Purchase()
    {
        // Remove the correct amount of coins
        CashManager.Instance.Purchase(outfitDatas[clickedOutfitIndex].price);

        // Unlock the Outfit Button
        outfitButtonsParent.GetChild(clickedOutfitIndex).GetComponent<OutfitButton>().Unlock();

        // Saving the outfit state
        SaveOutfitState(clickedOutfitIndex);

        purchaseButton.interactable = false;

        ApplyPlayerSkin(clickedOutfitIndex);

        // Save the last outfit index
        lastOutfitIndex = clickedOutfitIndex;
        SaveLastOutfit();
    }

    private bool IsOutfitUnlocked(int index)
    {
        return PlayerPrefs.GetInt("Outfit" + index) == 1;
    }

    private void SaveOutfitState(int index)
    {
        PlayerPrefs.SetInt("Outfit" + index, 1);
    }

    private void LoadLastOutfit()
    {
        lastOutfitIndex = PlayerPrefs.GetInt("LastOutfit");
    }

    private void SaveLastOutfit()
    {
        PlayerPrefs.SetInt("LastOutfit", lastOutfitIndex);
    }
}

[System.Serializable]
public struct OutfitData
{
    public Material mesh;
    public int price;
    public Sprite icon;
}
