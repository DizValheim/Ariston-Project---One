using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ariston
{
    /// <summary>
    /// Base item class
    /// </summary>
    public class Item : Interactable , IHoldableItems
    {
        protected override void Interact()
        {
            throw new System.NotImplementedException();
        }

        public virtual void PickUp() {}
       
        public virtual void Drop() {}
        
        public virtual void Use() {}
       
    }
}