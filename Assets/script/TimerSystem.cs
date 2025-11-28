using UnityEngine;
using System;
using System.Collections.Generic;


public class TimerSystem : MonoBehaviour
{
    // Singleton Instance so we can call it from anywhere
    public static TimerSystem Instance;

    [System.Serializable] public class TimerEvent
    {
        public string ID; //unique identifier for the timer event
        public float FinishTime; //time when the event should finish
        public Action Callback; //function to call when the timer finishes
    }

    [SerializeField] private List<TimerEvent> activeTimers = new List<TimerEvent>();
    
    void Awake()
    {
        // Singleton Setup
        if (Instance == null) Instance = this; // check if instance already exists. if not, set it to this
        else Destroy(gameObject); //if instance already exists, destroy this to enforce singleton
 
    }

    // Update is called once per frame
    void Update()
    {
        if (activeTimers.Count == 0) return; //no active timers, nothing to do

        // the head of the queue is always the timer that will finish soonest since we keep the list sorted

        while (activeTimers.Count > 0 && Time.time >= activeTimers[0].FinishTime) // loop since multiple timers could finish this frame
        {
            TimerEvent timerEvent = activeTimers[0];
            activeTimers.RemoveAt(0); //remove the timer from the list
            timerEvent.Callback?.Invoke(); //invoke the callback if it's not null
        }

    }

    // Add a new timer
    public void AddTimer(string id, float duration, Action callback)
    {
        TimerEvent newTimer = new TimerEvent
        {
            ID = id,
            FinishTime = Time.time + duration,
            Callback = callback
        };

        activeTimers.Add(newTimer);

        //sort the list by FinishTime to keep the soonest finishing timer at the front
        activeTimers.Sort((a, b) => a.FinishTime.CompareTo(b.FinishTime));
    }

    // Cancel a timer by ID
    public void CancelTimer(string id)
    {
        activeTimers.RemoveAll(timer => timer.ID == id);
    }

    public float GetProgress(string id, float duration)
    {
        TimerEvent timer = activeTimers.Find(t => t.ID == id);
        if (timer != null)
        {
            float timeLeft = timer.FinishTime - Time.time;
            return Mathf.Clamp01(1f - (timeLeft / duration));
        }
        return 0f; // if timer not found, assume it's complete
    }

}
