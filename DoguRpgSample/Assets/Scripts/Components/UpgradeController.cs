using UI;

public class UpgradeController : RadiusInteractable
{
    private UpgradeView upgradeView;
    private GameSceneView gameSceneView;


    protected override void Start()
    {
        base.Start();
        gameSceneView = FindObjectOfType<GameSceneView>(true);
        upgradeView = FindObjectOfType<UpgradeView>(true);
    }

    protected override void OnInteractStart()
    {
        gameSceneView.HideGameUI();
        upgradeView.onClosed = () => { gameSceneView.ShowGameUI(); };
        upgradeView.gameObject.SetActive(true);
    }

    protected override void OnInteractEnd()
    {
        upgradeView.gameObject.SetActive(false);
        gameSceneView.ShowGameUI();
    }
}