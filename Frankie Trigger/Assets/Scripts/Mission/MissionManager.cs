using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MissionType { Kill, Headshot, Outfit}

[System.Serializable]
public struct Mission
{
    public MissionType missionType;
    public int target;
    public int reward;
    public float progress;
}

public class MissionManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private Mission[] missions;
    private Dictionary<int, Mission> uncompletedMissionsDictionary = new Dictionary<int, Mission>();

    [Header("Elements")]
    [SerializeField] private MissionContainer missionContainerPrefab;
    [SerializeField] private Transform missionContainersParent;

    private void Awake()
    {
        MissionContainer.onRewardClaimed += MissionRewardClaimedCallback;
    }

    private void OnDestroy()
    {
        MissionContainer.onRewardClaimed -= MissionRewardClaimedCallback;
    }
    // Start is called before the first frame update
    void Start()
    {
        CreateMissionContainers();
    }

    private void MissionRewardClaimedCallback(int missionIndex)
    {
        SetMissionComplete(missionIndex);

        //Reward the player
        int reward = missions[missionIndex].reward;
        CashManager.Instance.AddCoins(reward);

        UpdateMissions();
    }

    private void UpdateMissions()
    {
        //Destroy all of the previous mission containers
        while(missionContainersParent.childCount > 0)
        {
            Transform t = missionContainersParent.GetChild(0);
            t.SetParent(null);
            Destroy(t.gameObject);
        }

        CreateMissionContainers();
    }

    private void CreateMissionContainers()
    {
        //Store Missions
        StoreUncompletedMissions();

        foreach (KeyValuePair<int, Mission> missionData in uncompletedMissionsDictionary)
        {
            CreateMissionContainer(missionData);
        }
    }

    private void StoreUncompletedMissions()
    {
        uncompletedMissionsDictionary.Clear();

        for (int i = 0; i < missions.Length; i++)
        {
            if (IsMissionComplete(i))
                continue;

            Mission mission = missions[i];
            mission.progress = GetMissionProgress(new KeyValuePair<int, Mission>(i,mission));


            //Add the mission to the uncompleted missions list
            //uncompletedMissionsDictionary.Add(i, missions[i]);
            uncompletedMissionsDictionary.Add(i, mission);

            if (uncompletedMissionsDictionary.Count >= 3)
                break;

        }
    }

    private void CreateMissionContainer(KeyValuePair<int, Mission> missionData)
    {
        MissionContainer missionContainerInstance = Instantiate(missionContainerPrefab, missionContainersParent);

        string title = GetMissionTitle(missionData.Value);
        string rewardString = missionData.Value.reward.ToString();
        float progress = GetMissionProgress(missionData);

        missionContainerInstance.Configure(title, rewardString, progress, missionData.Key);
    }

    public void UpdateMissionProgress(int missionIndex, float newProgress)
    {
        //Save the mission Progress
        SaveMissionProgress(missionIndex, newProgress);

        Mission mission = missions[missionIndex];
        mission.progress = newProgress;
        missions[missionIndex] = mission;

        uncompletedMissionsDictionary[missionIndex] = mission;

        //Loop through all the mission containers and update the one that has the key missionIndex
        for (int i = 0; i < missionContainersParent.childCount; i++)
        {
            MissionContainer missionContainer = missionContainersParent.GetChild(i).GetComponent<MissionContainer>();
            if (missionContainer.GetKey() != missionIndex)
                continue;

            //Update Mission Container
            missionContainer.UpdateProgress(newProgress);
        }
    }

    private string GetMissionTitle(Mission mission)
    {
        switch(mission.missionType)
        {
            case MissionType.Kill:
                return "Kill " + mission.target.ToString() + " enemies";

            case MissionType.Headshot:
                return "Do " + mission.target.ToString() + " headshot";

            case MissionType.Outfit:
                return "Unlock " + mission.target.ToString() + " outfits";

            default:
                return "Blank";
        }
    }

    public Dictionary<int,Mission> GetCurrentMissions()
    {
        return uncompletedMissionsDictionary;
    }

    private float GetMissionProgress(KeyValuePair<int, Mission> missionData)
    {
        return PlayerPrefs.GetFloat("MissionProgress" + missionData.Key);
    }

    private void SaveMissionProgress(int key, float progress)
    {
        PlayerPrefs.SetFloat("MissionProgress" + key, progress);
    }

    private void SetMissionComplete(int missionIndex)
    {
        PlayerPrefs.SetInt("Mission" + missionIndex, 1);
    }

    private bool IsMissionComplete(int missionIndex)
    {
        return PlayerPrefs.GetInt("Mission" + missionIndex) == 1;
    }
}
