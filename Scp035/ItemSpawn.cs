namespace Scp035
{
    /// <summary>
    /// Item Spawn chance and type.
    /// </summary>
    public struct ItemSpawn
    {
        /// <summary>
        /// The <see cref="ItemType"/> the item will spawn as.
        /// </summary>
        public ItemType Type { get; private set; }
        
        /// <summary>
        /// The chance of this item spawn being used.
        /// </summary>
        public int Chance { get; private set; }

        /// <summary>
        /// Constructs a new instance of this struct.
        /// </summary>
        /// <param name="type"><inheritdoc cref="Type"/></param>
        /// <param name="chance"><inheritdoc cref="Chance"/></param>
        public ItemSpawn(ItemType type, int chance)
        {
            Type = type;
            Chance = chance;
        }

        /// <summary>
        /// Deconstructs the object into it's various parts.
        /// </summary>
        /// <param name="type"><inheritdoc cref="Type"/></param>
        /// <param name="chance"><inheritdoc cref="Chance"/></param>
        public void Deconstruct(out ItemType type, out int chance)
        {
            type = Type;
            chance = Chance;
        }
    }
}