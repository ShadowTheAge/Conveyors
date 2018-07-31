using System.Collections;

namespace Simulation
{
    public enum ObjectId {None}
    
    public class SerializableObject : SimObject
    {
        public ObjectId id;
        
        public virtual void Setup() {}
    }
}