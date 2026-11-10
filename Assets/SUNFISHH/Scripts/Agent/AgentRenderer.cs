using System;
using UnityEngine;
using UnityEngine.UI;

namespace SUNFISHH.Scripts.Player
{
    public class AgentRenderer : MonoBehaviour
    {
        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }
    }
}