using Data;
using Data.Static;
using MobileInput;
using UnityEngine;
using UnityEngine.AI;
using Util;
#if USE_GAMIUM
using Input = Gamium.Input;
#endif

public class PlayerCharacterController : MonoBehaviour
{
    public MobileInputController mobileInputController;
    public CharacterActionController actionController { get; private set; }
    public EnemyController target;
    private CameraController cam;
    private NavMeshAgent navMeshAgent;
    private float moveSpeed = 7.00f;
    private float rotationSpeed = 1500.0f;
    private Quaternion lookRotation = Quaternion.identity;
    private Vector3 previousPosition = Vector3.zero;


    void Awake()
    {
        actionController = GetComponent<CharacterActionController>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = moveSpeed;
        cam = Camera.main.GetComponent<CameraController>();
        cam.target = navMeshAgent;

        previousPosition = transform.position;
        lookRotation = transform.rotation;

        var playerData = PlayerDataController.Instance.GetCurrentPlayerCharacter();
        var characterInfo = CharacterInfoList.Instance.GetEntry(playerData.characterId);
        var stat = actionController.stat;
        var currentCharacter = PlayerDataController.Instance.GetCurrentPlayerCharacter();
        currentCharacter.GetLevelInfo(out var levelInfo, out var nextLevelInfo);

        stat.ApplyCharacterInfo(characterInfo, levelInfo.level);
        currentCharacter.OnLevelChanged = (level) =>
        {
            stat.stats.Get(Stat.StatType.Health).ResetValue();
            stat.ApplyCharacterInfo(characterInfo, level.level);
        };
        InventoryData.Instance.onEquip = (itemStack) => { actionController.stat.ApplyEquip(itemStack); };
        InventoryData.Instance.onUnequip = (itemStack) => { actionController.stat.ApplyUnequip(itemStack); };
        foreach (var itemStack in InventoryData.Instance.GetEquipedItems())
        {
            InventoryData.Instance.Equip(itemStack);
        }

        actionController.stat.Get(Stat.StatType.Experience).Increment(playerData.Experience);
    }

    void Update()
    {
        UpdateAttackInput();
        UpdateMoveInput();
        UpdateRotation();
        UpdateTarget();
    }


    private void UpdateMoveInput()
    {
        if (actionController.isGettingHit || actionController.isAttacking)
        {
            return;
        }

        MoveIfHasInput();

        Vector3 currentMove = transform.position.XZPlane() - previousPosition.XZPlane();
        var currentSpeed = currentMove.magnitude / Time.deltaTime;
        previousPosition = transform.position.XZPlane();
        UpdateLookRotation(currentSpeed, currentMove);

        if (currentSpeed < 0.2f)
        {
            actionController.Idle();
        }
        else if (currentSpeed < 1.3f)
        {
            actionController.Walk();
        }
        else
        {
            actionController.Run();
        }
    }

    private void UpdateLookRotation(float currentSpeed, Vector3 currentMove)
    {
        if (target != null)
        {
            lookRotation = Quaternion.LookRotation(target.transform.position - transform.position);
            return;
        }

        if (!Mathf.Approximately(currentSpeed, 0.0f))
        {
            lookRotation = Quaternion.LookRotation(currentMove);
            lookRotation.x = 0;
            lookRotation.z = 0;
            return;
        }

        lookRotation = transform.rotation;
    }

    private void MoveIfHasInput()
    {
        var horizontal = Input.GetAxis("Horizontal");
        if (Mathf.Approximately(horizontal, 0.0f))
        {
            horizontal = mobileInputController.Horizontal;
        }

        var vertical = Input.GetAxis("Vertical");
        if (Mathf.Approximately(vertical, 0.0f))
        {
            vertical = mobileInputController.Vertical;
        }

        var direction = new Vector3(horizontal, 0, vertical);
        var transformDirection = cam.transform.TransformDirection(direction);
        navMeshAgent.Move(transformDirection * moveSpeed * Time.deltaTime);
    }

    private void UpdateAttackInput()
    {
        if (actionController.isGettingHit || actionController.isAttacking)
        {
            return;
        }

        if (!mobileInputController.GetButtonDown("Fire1"))
        {
            return;
        }

        actionController.Attack();
    }

    private void UpdateRotation()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }

    private void UpdateTarget()
    {
        if (null == target)
        {
            return;
        }

        if (target.actionController.isDead)
        {
            target = null;
        }
    }
}