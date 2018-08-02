using Simulation;

namespace Model
{
    public class ItemConnection
    {
        public static readonly ItemConnection Void = new ItemConnection();
        
        public virtual bool Push(Item item) => false;
        public virtual Item Pull() => Item.None;
        public virtual Item Peek() => Item.None;
    }
}