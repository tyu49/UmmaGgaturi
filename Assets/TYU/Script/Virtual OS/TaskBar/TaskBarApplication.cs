using TYU.Script.Virtual_OS.Application;
using UnityEngine;
using UnityEngine.UI;

namespace TYU.Script.Virtual_OS.TaskBar
{
    public class TaskBarApplication : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Image backGround;

        private TaskBarManager _manager;
        public void Initialize(ApplicationSO data, TaskBarManager manager)
        {
            _manager = manager;
            icon.sprite = data.Icon;
        }
    }
}