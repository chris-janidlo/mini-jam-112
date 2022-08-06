using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mj112
{
    public class RandomMover : PrefabEchoable, IClockFollower
    {
        Vector2 direction;
        float timer;

        void Start ()
        {
            Clock.Instance.Register(this);
        }

        void OnDestroy ()
        {
            Clock.Instance.Deregister(this);
        }

        public override object GetState ()
        {
            return (transform.position, transform.rotation, transform.localScale);
        }

        public void TimedUpdate ()
        {
            timer -= Clock.Instance.DeltaTime;
            if (timer <= 0)
            {
                timer += Random.Range(1f, 3f);
                direction = Random.insideUnitCircle.normalized;
            }

            transform.position += (Vector3) direction * Clock.Instance.DeltaTime;
        }
    }
}
