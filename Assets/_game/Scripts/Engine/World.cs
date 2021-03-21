using System.Collections.Generic;
using UnityEngine;
using System;

namespace RomenoCompany
{
    public class World : MonoBehaviour
    {
        [Serializable]
        public class VerticalDomain
        {
            public bool IsGround;
            public float leftBound;
            public float height;

            public VerticalDomain Clone(float offset)
            {
                var clone = (VerticalDomain)MemberwiseClone();
                clone.leftBound += offset;
                return clone;
            }
        }

        [SerializeField] private GameObject[] _enviromentPrefabs = null;
        [SerializeField] private float _width = 0;
        public bool increaseChunkIndexByLevel = false;
        [SerializeField] float zPos = -1.9f;
        private int currentLevel;

        private Camera _camera;

        public VerticalDomain GetVerticalDomain(float xPos)
        {
            for(int i = 0; i < _verticalDomains.Count; i++)
            {
                if(_verticalDomains[i].leftBound < xPos && (i + 1 >= _verticalDomains.Count  || _verticalDomains[i + 1].leftBound > xPos))
                {
                    return _verticalDomains[i];
                }
            }
            return null;
        }
        public List<VerticalDomain> _verticalDomains = new List<VerticalDomain>();
        private System.Comparison<VerticalDomain> domenComparsion;
        [NonSerialized] public bool spawnVerticalization;

        // Runtime
        private List<int> updateIndicies = new List<int>(3);
        private List<VerticalDomain> newDomens = new List<VerticalDomain>(3);

        void Start()
        {
            // currentLevel = Inventory.Instance.completion.Value.level;
            domenComparsion = new Comparison<VerticalDomain>((a, b) => a.leftBound.CompareTo(b.leftBound));
            Vector3 pos = transform.position;
            transform.position = new Vector3(pos.x, pos.y, zPos);
            StartCoroutine(VerticalCutoffScync());
        }

    #if UNITY_EDITOR

        void OnDrawGizmos()
        {
            if(_verticalDomains != null && _verticalDomains.Count > 0)
            {
                for(int i = 0; i < _verticalDomains.Count - 1; i++)
                {
                    Gizmos.color = _verticalDomains[i].IsGround ? Color.yellow : Color.cyan;
                    Gizmos.DrawWireCube(new Vector3((_verticalDomains[i].leftBound + _verticalDomains[i + 1].leftBound) / 2f, _verticalDomains[i].height + 2.5f, 1), new Vector3(_verticalDomains[i + 1].leftBound - _verticalDomains[i].leftBound, 5, 5));
                }
            }
        }

    #endif

        private int _positionIndex = -100;

        private void Update()
        {
            if (_camera == null) _camera = Camera.main;
            if (_camera == null) return;

            int positionIndex = (int)Mathf.Round(_camera.transform.position.x / _width);
            
            if (_positionIndex != positionIndex)
            {
                if (increaseChunkIndexByLevel)
                {
                    positionIndex += currentLevel;
                }
                // spawnVerticalization = DB.Instance.sharedGameData.startVerticalizationAtLevel <= currentLevel;
                _positionIndex = positionIndex;
                updateIndicies.Clear();
                bool updateDomens = false;
                for (int i = -1; i < 2; i++)
                {
                    positionIndex = _positionIndex + i;
                    Vector3 position = new Vector3(Mathf.Round((_camera.transform.position.x + (_width * i)) / _width) * _width, 0, 0);
                    updateIndicies.Add(positionIndex);
                }

                if (updateDomens)
                {
                    newDomens.Clear();
                    if (newDomens.Count == 0)
                    {
                        newDomens.Add(new VerticalDomain() { height = 0, IsGround = true, leftBound = float.MinValue });
                    }

                    for(int i = 0; i < _verticalDomains.Count; i++)
                    {
                        if(newDomens.Exists(x => Mathf.Approximately(x.leftBound, _verticalDomains[i].leftBound)) == false)
                        {
                            _verticalDomains.RemoveRange(i--, 1);
                        }
                    }
                    for(int i = 0; i < newDomens.Count; i++)
                    {
                        if (_verticalDomains.Exists(x => Mathf.Approximately(x.leftBound, newDomens[i].leftBound)) == false)
                        {
                            _verticalDomains.Add(newDomens[i]);
                        }
                    }
                    _verticalDomains.Sort(domenComparsion);

                    // if (Inventory.Instance.completion.Value.level >= DB.Instance.sharedGameData.startTossablesAtLevel)
                    // {
                    // }
                }
            }
        }

        private System.Collections.IEnumerator VerticalCutoffScync()
        {
            while (Application.isPlaying)
            {
                int index = -1;
                float distance = 1000;
                for (int i = 0; i < _verticalDomains.Count - 1; i++)
                {
                    if (_verticalDomains[i].IsGround == false)
                    {
                        float left = _verticalDomains[i].leftBound;
                        float right = _verticalDomains[i + 1].leftBound;
                    }
                }
                yield return new WaitForSeconds(3);
            }
        }
    }
}
