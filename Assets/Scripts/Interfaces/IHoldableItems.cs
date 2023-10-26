namespace Ariston
{
    /// <summary>
    /// Interface to deal with holdable objects
    /// </summary>
    public interface IHoldableItems
    {
        //Pick Up method
        public abstract void PickUp();
        //Drop method
        public abstract void Drop();
        //Use method
        public abstract void Use();
    }
}