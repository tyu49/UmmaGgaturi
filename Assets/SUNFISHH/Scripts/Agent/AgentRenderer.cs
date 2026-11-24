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
        
        public void Flip(float dirX)
        {
            if (Mathf.Abs(dirX) < 0.01f) return;    
            Vector3 s = transform.localScale;
            s.x = Mathf.Abs(s.x) * (dirX < 0f ? -1f : 1f);
            transform.localScale = s;
        }
    }
}