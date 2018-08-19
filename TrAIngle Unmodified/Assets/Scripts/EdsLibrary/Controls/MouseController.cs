using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace Controls
{
    public class MouseController : MonoBehaviour
    {
        [HideInInspector]
        public Button Lmb;

        [SerializeField]
        protected UnityEvent OnLmbDown;
        [SerializeField]
        protected UnityEvent OnLmbUp;

        private void Start()
        {
            Lmb = new Button();
        }

        private void FixedUpdate()
        {
            if (Lmb.IsDown == false && Input.GetMouseButton(0) == true)
            {
                Lmb.Down();
                if (OnLmbDown != null) OnLmbDown.Invoke();
            }
            else if (Lmb.IsDown == true && Input.GetMouseButton(0) == false)
            {
                Lmb.Up();
                if (OnLmbUp != null) OnLmbUp.Invoke();
            }
        }
    }
}