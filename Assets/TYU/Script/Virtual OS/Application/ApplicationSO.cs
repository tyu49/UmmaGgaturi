using UnityEngine;

namespace TYU.Script.Virtual_OS.Application
{
    [CreateAssetMenu(fileName = "ApplicationData", menuName = "Virtual/Application", order = 0)]
    public class ApplicationSO : ScriptableObject
    {
        [field: SerializeField] public Sprite icon { get; private set; }
        [field:SerializeField] public string name { get; private set; }
        //대충 여기다가 나중에 추상화 클래스로 창을 프로퍼티로 넣으셈
    }
}