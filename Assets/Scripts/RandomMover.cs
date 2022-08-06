using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mj112
{
    public class RandomMover : MonoBehaviour, IClockFollower
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
