using System.Collections.Generic;
using TYU.Script.Virtual_OS.Application;
using UnityEngine;

namespace TYU.Script.Virtual_OS.TaskBar
{
    public class TaskBarManager : MonoBehaviour
    {
        [SerializeField] private Canvas taskBarCanvas;
        [SerializeField] private GameObject appBase;
        [field: SerializeField] public TaskBarAppMover AppMover;
        private List<TaskBarApplication> _tBApps = new();
        private Dictionary<ApplicationSO, TaskBarApplication> _appDict = new();
        private float _baseWidth;
        public void Initialize()
        {
            _baseWidth = (appBase.transform as RectTransform)!.sizeDelta.x;
            AppMover.Initialize(taskBarCanvas, this);
        }
        
        public void MakeNewApp(ApplicationSO data)
        {
            var go = Instantiate(appBase, transform);
            var tb = go.GetComponent<TaskBarApplication>();
            tb.Initialize(data, this, new Vector2(_tBApps.Count * _baseWidth, 0f));
            _appDict.TryAdd(data, tb);
            _tBApps.Add(tb);
        }

        private void SortApps()
        {
            for (var i = 0; i < _tBApps.Count; ++i)
            {
                _tBApps[i].MoveTo(i * _baseWidth);
            }
        }

        public void RemoveApp(ApplicationSO data)
        {
            if (!_appDict.TryGetValue(data, out var key)) return;
            _tBApps.Remove(key);
            _appDict[data].RemoveApp();
            _appDict.Remove(data);
            SortApps();
        }

        public void ChangePos(TaskBarApplication user, TaskBarApplication target)
        {
            if (!_tBApps.Contains(user) || !_tBApps.Contains(target))
            {
                Debug.LogError("이거 개병신 됌 ㅈ땜 그냥 코드 다시 써라");
                return;
            }

            int targetIdx = _tBApps.IndexOf(target);
            _tBApps.Remove(user);
            _tBApps.Insert(targetIdx, user);
            SortApps();
        }

    }
}