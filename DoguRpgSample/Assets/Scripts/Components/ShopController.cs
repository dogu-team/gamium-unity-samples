using Data.Static;
using UI;

public class ShopController : RadiusInteractable
{
    public ShopInfo shopInfo;
    private ShopView shopView;
    private GameSceneView gameSceneView;


    protected override void Start()
    {
        base.Start();
        gameSceneView = FindObjectOfType<GameSceneView>(true);
        shopView = FindObjectOfType<ShopView>(true);
    }

    protected override void OnInteractStart()
    {
        gameSceneView.HideGameUI();
        shopView.onClosed = () => { gameSceneView.ShowGameUI(); };
        shopView.productStacks = shopInfo.productStacks;
        shopView.gameObject.SetActive(true);
    }

    protected override void OnInteractEnd()
    {
        gameSceneView.ShowGameUI();
        shopView.gameObject.SetActive(false);
    }
}