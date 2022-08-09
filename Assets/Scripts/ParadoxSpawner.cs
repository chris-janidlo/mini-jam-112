using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using crass;

namespace mj112
{
    public class ParadoxSpawner : MonoBehaviour, IClockFollower
    {
        [MinMaxSlider(0, 600)]
        public Vector2Int FramesBetweenSpawnRange;

        public RectTransform SpawnZone;

        public Paradox ParadoxPrefab;

        enum State
        {
            ParadoxSpawned, WaitingToSpawnParadox
        }

        State currentState;
        int frameTimer;

        void Start ()
        {
            Clock.Instance.Register(this);
            spawnParadox();
        }

        public void TimedUpdate ()
        {
            switch (currentState)
            {
                case State.ParadoxSpawned:
                    break;

                case State.WaitingToSpawnParadox:
                    frameTimer--;
                    if (frameTimer <= 0) spawnParadox();
                    break;
            }
        }

        public void OnAteParadox ()
        {
            enterWaitState();
        }

        void enterWaitState ()
        {
            currentState = State.WaitingToSpawnParadox;
            frameTimer = RandomExtra.Range(FramesBetweenSpawnRange);
        }

        void spawnParadox ()
        {
            Instantiate(ParadoxPrefab, getSpawnPosition(), Quaternion.identity);
            currentState = State.ParadoxSpawned;
        }

        // TODO: try to avoid holeworms
        Vector3 getSpawnPosition ()
        {
            var bounds = SpawnZone.rect;
            return new Vector3
            (
                Random.Range(bounds.xMin, bounds.xMax),
                Random.Range(bounds.yMin, bounds.yMax),
                SpawnZone.position.z
            );
        }
    }
}
