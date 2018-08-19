namespace EdsLibrary
{
    namespace Units
    {
        public enum HeroNames
        {
            Unnamed,
            Ed,
            Muradin
        }

        public enum DeathType
        {
            Destroy,
            Remove,
            NoRevive,
            NoEffects,
            NoSounds
        }

        public class DeathFlags
        {
            public bool destroy = false;
            public bool remove = false;
            public bool noRevive = false;
            public bool noEffects = false;
            public bool noSounds = false;

            public DeathFlags(DeathType[] deathTypes)
            {
                Set(deathTypes);
            }

            public void Set(DeathType[] deathTypes)
            {
                foreach (DeathType deathType in deathTypes)
                {
                    switch (deathType)
                    {
                        case DeathType.Destroy:
                            destroy = true;
                            return;
                        case DeathType.Remove:
                            remove = true;
                            return;
                        case DeathType.NoRevive:
                            noRevive = true;
                            break;
                        case DeathType.NoEffects:
                            noEffects = true;
                            break;
                        case DeathType.NoSounds:
                            noSounds = true;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public enum AnimationNames
        {
            Idle,
            Jump,
            Attack,
            Death,
            Remove
        }
    }
}
