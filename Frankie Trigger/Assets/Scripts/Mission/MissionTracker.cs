using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MissionManager))]
public class MissionTracker : MonoBehaviour
{
    private MissionManager missionManager;

    private void Awake()
    {
        missionManager = GetComponent<MissionManager>();

        Enemy.onDead += EnemyDiedCallback;
        OutfitShopManager.onOutfitUnlocked += OutfitUnlockedCallback;
    }

    private void OnDestroy()
    {
        Enemy.onDead -= EnemyDiedCallback;
        OutfitShopManager.onOutfitUnlocked -= OutfitUnlockedCallback;
    }

    private void EnemyDiedCallback()
    {
        // Check if we have a mission that has the Kill type

        Dictionary<int, Mission> missions = missionManager.GetCurrentMissions();

        foreach (KeyValuePair<int, Mission> missionData in missions)
        {
            Mission mission = missionData.Value;

            if (mission.missionType == MissionType.Kill)
            {

                // Update the progress of that mission
                int currentEnemiesKilled = (int)(mission.progress * mission.target);
                currentEnemiesKilled++;

                float newProgress = (float)currentEnemiesKilled / mission.target;

                missionManager.UpdateMissionProgress(missionData.Key, newProgress);
            }
        }
    }

    private void OutfitUnlockedCallback()
    {
        // Check if we have a mission that has the Kill type

        Dictionary<int, Mission> missions = new Dictionary<int, Mission>(missionManager.GetCurrentMissions());

        foreach (KeyValuePair<int, Mission> missionData in missions)
        {
            Mission mission = missionData.Value;

            if (mission.missionType == MissionType.Outfit)
            {

                // Update the progress of that mission
                int currentOutfitsUnlocked = (int)(mission.progress * mission.target);
                currentOutfitsUnlocked++;

                float newProgress = (float)currentOutfitsUnlocked / mission.target;

                missionManager.UpdateMissionProgress(missionData.Key, newProgress);
            }
        }
    }
}
