using NUnit.Framework;
using UnityEngine;

public class FishTests
{
    [Test]
    // Test to verify that TakeHit increments the hit counter
    public void TakeHit_Increments_Hit_Counter()
    {
        GameObject go = new GameObject();
        // We use ChaserJellyfish because HostileFish is abstract and can't be added directly
        ChaserJellyfish fish = go.AddComponent<ChaserJellyfish>();
        
        fish.maxHits = 5;
        fish.currentHits = 0;

        // Note: We need a dummy TimerSystem because TakeHit tries to call it
        GameObject timerGO = new GameObject();
        TimerSystem timerSys = timerGO.AddComponent<TimerSystem>();
        // Manually set the singleton for the test
        TimerSystem.Instance = timerSys;


        fish.TakeHit();

        // After one hit, currentHits should be 1
        Assert.AreEqual(1, fish.currentHits);

        // Cleanup
        Object.DestroyImmediate(go);
        Object.DestroyImmediate(timerGO);
    }

/*
    [Test]
    // Test to verify that TakeHit triggers the stun timer
    public void TakeHit_Triggers_Stun_Timer()
    {
        
    }
    */
}