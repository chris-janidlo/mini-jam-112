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

        /// <summary>
        /// Has a value in [0, 1), starting at 0 and increasing over time until looping
        /// </summary>
        public float LoopTime => (float) FramesElapsedInLoop / FramesPerLoop;

        List<IClockFollower> followers;
        bool stopped;

        void Awake ()
        {
            SingletonOverwriteInstance(this);

            Time.fixedDeltaTime = DeltaTime;

            followers = new List<IClockFollower>();
        }
        
        void FixedUpdate ()
        {
            if (stopped) return;

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

        public void Stop ()
        {
            stopped = true;
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
