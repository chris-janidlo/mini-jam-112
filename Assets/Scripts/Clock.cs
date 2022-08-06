using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

namespace mj112
{
    public class Clock : Singleton<Clock>
    {
        public int FPS, FramesPerLoop;

        public int FramesElapsedInLoop { get; private set; }

        public float DeltaTime => 1f / FPS;

        public float TotalLoopTime => (float) FramesPerLoop / FPS;
        public float ElapsedLoopTime => (float) FramesElapsedInLoop / FPS;

        List<IClockFollower> followers;

        void Awake ()
        {
            SingletonSetPersistantInstance(this);

            Application.targetFrameRate = FPS;
            Time.fixedDeltaTime = DeltaTime;

            followers = new List<IClockFollower>();
        }

        void FixedUpdate ()
        {
            FramesElapsedInLoop = (FramesElapsedInLoop + 1) % FramesPerLoop;

            foreach (var follower in followers)
            {
                follower.TimedUpdate();
            }
        }

        public void Register (IClockFollower follower)
        {
            followers.Add(follower);
        }

        public void Deregister (IClockFollower follower)
        {
            followers.Remove(follower);
        }
    }
}
