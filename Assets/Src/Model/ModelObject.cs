namespace Model
{
    public abstract class ModelObject
    {
        public ModelWorld modelWorld { get; private set; }

        public virtual bool PreTick() => true;
        public virtual void Tick() {}
        
        public virtual void Spawn(ModelWorld modelWorld)
        {
            this.modelWorld = modelWorld;
        }
    }
}