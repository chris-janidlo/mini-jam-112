using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using crass;

namespace mj112
{
    public class Clock : Singleton<Clock>
    {
        public int FPS, FramesPerLoop;
        public UnityEvent OnTimeJumped;

        public int FramesElapsedInLoop { get; private set; }
        public bool JumpedThisFrame { get; private set; }

        public float DeltaTime => 1f / FPS;

        public float TotalLoopTime => (float) FramesPerLoop / FPS;
        public float ElapsedLoopTime => (float) FramesElapsedInLoop / FPS;

        List<IClockFollower> followers;

        void Awake ()
        {
            SingletonSetPersistantInstance(this);

            Time.fixedDeltaTime = DeltaTime;

            followers = new List<IClockFollower>();
        }
        
        void FixedUpdate ()
        {
            foreach (var follower in followers)
            {
                follower.TimedUpdate();
            }

            JumpedThisFrame = false;

            FramesElapsedInLoop++;
            if (FramesElapsedInLoop >= FramesPerLoop)
            {
                FramesElapsedInLoop = 0;
                JumpedThisFrame = true;
                OnTimeJumped.Invoke();
            }
        }

        public void Register (IClockFollower follower, UnityAction onTimeJumped = null)
        {
            followers.Add(follower);

            if (onTimeJumped != null) OnTimeJumped.AddListener(onTimeJumped);
        }

        public void Deregister (IClockFollower follower, UnityAction onTimeJumped = null)
        {
            followers.Remove(follower);

            if (onTimeJumped != null) OnTimeJumped.RemoveListener(onTimeJumped);
        }
    }
}
