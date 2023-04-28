namespace Data
{
    public enum CharacterType
    {
        Player,
        Enemy
    }

    public static class CharacterTypeExtensions
    {
        public static string ToTag(this CharacterType characterType)
        {
            switch (characterType)
            {
                case CharacterType.Player:
                    return "Player";
                case CharacterType.Enemy:
                    return "Enemy";
            }

            return "Unknown";
        }
    }
}