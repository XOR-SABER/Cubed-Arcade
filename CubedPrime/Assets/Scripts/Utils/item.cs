using UnityEngine;

namespace Scripts
{
    [System.Serializable]
    public class item
    {
         public enum MyEnum
        {
            Hat,
            Skin,
            Weapon
        }
        
        
        public int price;
        public GameObject go;
        public MyEnum type;
        public bool owned;
    }
}