using UnityEngine;

namespace Yajima
{
    public partial class RuleManager : MonoBehaviour
    {
        static RuleManager _instance;
        public static RuleManager Instance => _instance;

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }
}