namespace Ariston
{
    /// <summary>
    /// Interface to deal with holdable objects
    /// </summary>
    public interface IHoldableItem
    {
        //Drop method
        public abstract void Drop();
        //Use method
        public abstract void Use();
    }
}