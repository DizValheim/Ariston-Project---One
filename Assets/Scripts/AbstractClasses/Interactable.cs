using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ariston
{
    /// <summary>
    /// Abstract class for handling interactable objects
    /// </summary>
    public abstract class Interactable : MonoBehaviour
    {
        //Interaction method, to be implemented as per specifics
        protected abstract void Interact();
        
        //Method to define what happens on hovering mouse cursor over object
        protected void OnMouseHover()
        {

        }

        //Method to define what happens on exiting hovering state
        protected void OnMouseExit()
        {

        }
    }
}
