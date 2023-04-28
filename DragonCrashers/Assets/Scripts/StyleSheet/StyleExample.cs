

// STYLE GUIDE - UI TOOLKIT DEMO

// NAMING/CASING:
// - Use Pascal case (e.g. ExamplePlayerController, MaxHealth, etc.) unless noted otherwise
// - Use camel case (e.g. examplePlayerController, maxHealth, etc.) for local/private variables, parameters.
// - kebab case for USS classes (e.g. char-data-panel, char-data-tabs, char-data-content, etc.)
// - Pascal case for UXML files (e.g CharScreen.uxml, HomeScreen.uxml, etc.)
// - Use m_ prefix for member variables (e.g. m_PlayerHealth, m_ShopScreen)
// - Use the UIToolkitDemo namespace for project-specific scripts

// FORMATTING:
// - Allman (opening curly braces on a new line) style braces.
// - Keep lines short (80-120 characters). 
// - Use a single space before flow control conditions, e.g. while (x == y)
// - Avoid spaces inside brackets, e.g. x = dataArray[index]
// - Use a single space after a comma between function arguments.
// - Don’t add a space after the parenthesis and function arguments, e.g. CollectItem(myObject, 0, 1);
// - Don’t use spaces between a function name and parenthesis, e.g. DropPowerUp(myPrefab, 0, 1);
// - Avoid Regions.

// COMMENTS:
// - Use the // comment to keep the explanation next to the logic.
// - Use a Tooltip instead of a comment for serialized fields. 


// USING LINES:
// - Keep using lines at the top of your file.
// - Remove unsed lines.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// NAMESPACES:
// - Pascal case, without special symbols or underscores.
// - Add using line at the top to avoid typing namespace repeatedly.
// - Create sub-namespaces with the dot (.) operator, e.g. MyApplication.GameFlow, MyApplication.AI, etc.
namespace StyleSheetExample
{

    // ENUMS:
    // - Use a singular type name.
    // - No prefix or suffix.
    // - Pascal case
    public enum Rarity
    {
        Rare,
        Special,
        Common
    }

    // FLAGS ENUMS:
    // - Use a plural type name 
    // - No prefix or suffix.
    // - Use column-alignment for binary values
    [Flags]
    public enum AttackModes
    {
        // Decimal                         // Binary
        None = 0,                          // 000000
        Melee = 1,                         // 000001
        Ranged = 2,                        // 000010
        Special = 4,                       // 000100

        MeleeAndSpecial = Melee | Special  // 000101
    }

    // INTERFACES:
    // - Name interfaces with adjective phrases.
    // - Use the 'I' prefix.
    public interface IDamageable
    {
        string damageTypeName { get; }
        float damageValue { get; }

        // METHODS:
        // - Start a methods name with a verbs or verb phrases to show an action.
        // - Parameter names are camelCase.
        bool ApplyDamage(string description, float damage, int numberOfHits);
    }

    public interface IDamageable<T>
    {
        void Damage(T damageTaken);
    }

    // CLASSES or STRUCTS:
    // - Name them with nouns or noun phrases.
    // - Avoid prefixes.
    // - One Monobehaviour per file. If you have a Monobehaviour in a file, the source file name must match. 
    public class StyleExample : MonoBehaviour
    {

        // FIELDS: 
        // - Avoid special characters (backslashes, symbols, Unicode characters)
        // - Use nouns for names, but prefix booleans with a verb.
        // - Use meaningful names. Make names searchable and pronounceable. Don’t abbreviate (unless it’s math).
        // - Use Pascal case for public fields. Use camel case for private variables.
        // - Add an underscore (m_) in front of private fields to differentiate from local variables (not using other prefixes)
        // - Omit the default private access modifier

        int m_ElapsedTimeInDays;

        // Use [SerializeField] attribute if you want to display a private field in Inspector.
        // Booleans ask a question that can be answered true or false.
        [SerializeField] bool m_IsPlayerDead;

        // This limits the values to a Range and creates a slider in the Inspector.
        [Range(0f, 1f)] [SerializeField] float m_RangedStat;

        // A tooltip can replace a comment on a serialized field and do double duty.
        [Tooltip("This is another statistic for the player.")]
        [SerializeField] float m_AnotherStat;


        // PROPERTIES:
        // - Preferable to a public field.
        // - Pascal case, without special characters.
        // - Use the expression-bodied properties to shorten
        // - e.g. use expression-bodied for read-only properties but { get; set; } for everything else.
        // - Use the Auto-Implementated Property for a public property without a backing field.

        // the private backing field
        int m_MaxHealth;

        // read-only, returns backing field (most common)
        public int MaxHealthReadOnly => m_MaxHealth;

        // explicitly implementing getter and setter
        public int MaxHealth
        {
            get => m_MaxHealth;
            set => m_MaxHealth = value;
        }

        // write-only (not using backing field)
        public int Health { private get; set; }

        // write-only, without an explicit setter
        public void SetMaxHealth(int newMaxValue) => m_MaxHealth = newMaxValue;

        // auto-implemented property without backing field
        public string DescriptionName { get; set; } = "Fireball";

#pragma warning disable 0067

        // EVENTS:
        // - Name with a verb phrase.
        // - Present participle means "before" and past participle mean "after."
        // - Use System.Action delegate for most events (can take 0 to 16 parameters).
        // - Define a custom EventArg only if necessary (either System.EventArgs or a custom struct).

        // event before
        public event Action OpeningDoor;

        // event after
        public event Action DoorOpened;

        public event Action<int> PointsScored;
        public event Action<CustomEventArgs> ThingHappened;

#pragma warning restore 0067

        // These are event raising methods, e.g. OnDoorOpened, OnPointsScored, etc.
        public void OnDoorOpened()
        {
            DoorOpened?.Invoke();
        }

        public void OnPointsScored(int points)
        {
            PointsScored?.Invoke(points);
        }

        // This is a custom EventArg made from a struct.
        public struct CustomEventArgs
        {
            public int ObjectID { get; }
            public Color Color { get; }

            public CustomEventArgs(int objectId, Color color)
            {
                this.ObjectID = objectId;
                this.Color = color;
            }
        }

        // METHODS:
        // - Start a methods name with a verbs or verb phrases to show an action.
        // - Parameter names are camel case.

        // Methods start with a verb.
        public void SetInitialPosition(float x, float y, float z)
        {
            transform.position = new Vector3(x, y, z);
        }

        // Methods ask a question when they return bool.
        public bool IsNewPosition(Vector3 newPosition)
        {
            return (transform.position == newPosition);
        }

        void FormatExamples(int someExpression)
        {
            // VAR:
            // - Use var if it helps readability, especially with long type names.
            // - Avoid var if it makes the type ambiguous.
            var powerUps = new List<GameObject>();
            var dict = new Dictionary<string, List<GameObject>>();


            // SWITCH STATEMENTS:
            switch (someExpression)
            {
                case 0:
                    // ..
                    break;
                case 1:
                    // ..
                    break;
                case 2:
                    // ..
                    break;
            }

            // BRACES: 
            // - Keep braces for clarity when using single-line statements.
            // - Or avoid single-line statement entirely for debuggability.
            // - Keep braces in nested multi-line statements.

            // This single-line statement keeps the braces...
            for (int i = 0; i < 100; i++) { DoSomething(i); }

            // ... but this is more debuggable. You can set a breakpoint on the clause.
            for (int i = 0; i < 100; i++)
            {
                DoSomething(i);
            }

            // Don't remove the braces here.
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    DoSomething(j);
                }
            }
        }

        private void DoSomething(int x)
        {
            // .. 
        }
    }

    // STRUCTS and SCRIPTABLE OBJECTS:
    // - camelcase for fields
    // - Pascal case for ScriptableObject methods
    [System.Serializable]
    public struct RarityIcon
    {
        public Sprite icon;
        public Rarity rarity;
    }

    public class GameIconsSO : ScriptableObject
    {
        public List<RarityIcon> rarityIcons;
        public Sprite GetRarityIcon(Rarity rarity)
        {
            RarityIcon match = rarityIcons.Find(x => x.rarity == rarity);
            return match.icon;
        }
    }
}
