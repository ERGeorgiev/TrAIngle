using UnityEngine;
//using TMPro;

//namespace EdsLibrary
//{
//    namespace Units
//    {
//        public class StatusIndicatorController : MonoBehaviour
//        {
//            [SerializeField]
//            private Unit unit;
//            [SerializeField]
//            private RectTransform healthBarRect;
//            [SerializeField]
//            private TextMeshProUGUI healthText;

//            void Start()
//            {
//                if (healthBarRect == null)
//                {
//                    Debug.LogError("No HealthBar Referenced!");
//                }
//                if (healthText == null)
//                {
//                    Debug.LogError("No HealthText Referenced!");
//                }
//            }

//            public void SetHealth(float _cur, float _max)
//            {
//                float _value = (_cur / _max);
//                healthBarRect.localScale = new Vector3(_value, healthBarRect.localScale.y, healthBarRect.localScale.z);
//                healthText.text = _cur + "/" + _max;
//            }

//            public void Refresh()
//            {
//                SetHealth(unit.HealthCurrent, unit.healthMax);
                
//            }
//        }
//    }
//}