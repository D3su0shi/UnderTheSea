using NUnit.Framework;
using UnityEngine;

public class TimerTests
{
    [Test]
    // Test to verify that timers are auto-sorted by earliest finish time
    public void Timers_Auto_Sort_By_Earliest_Finish_Time()
    {
        GameObject go = new GameObject();
        TimerSystem sys = go.AddComponent<TimerSystem>();

        // Add a "Slow" timer (10 seconds from now)
        sys.AddTimer("SlowTimer", 10f, () => { });
        
        // Add a "Fast" timer (1 second from now)
        // added second, but it should be processed first
        sys.AddTimer("FastTimer", 1f, () => { });


        // Check Index 0 (The Head of the Queue). It should be the Fast Timer.
        Assert.AreEqual("FastTimer", sys.activeTimers[0].ID);
        
        // Check Index 1. It should be the Slow Timer.
        Assert.AreEqual("SlowTimer", sys.activeTimers[1].ID);

        // Cleanup
        Object.DestroyImmediate(go);
    }

    [Test]
    // Test to verify that CancelTimer removes the correct timer
    public void Cancel_Timer_Removes_Item_From_Queue()
    {
        GameObject go = new GameObject();
        TimerSystem sys = go.AddComponent<TimerSystem>();
        
        // Add a timer
        sys.AddTimer("ToCancel", 5f, () => { });
        
        // Confirm it's there
        Assert.AreEqual(1, sys.activeTimers.Count);

        // Cancel it
        sys.CancelTimer("ToCancel");

        // List should be empty
        Assert.AreEqual(0, sys.activeTimers.Count);

        // Cleanup
        Object.DestroyImmediate(go);
    }
}