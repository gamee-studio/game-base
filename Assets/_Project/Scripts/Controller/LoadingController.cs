using DG.Tweening;
using Pancake.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LoadingController : MonoBehaviour
{
    [FormerlySerializedAs("TimeLoading")]
    [Header("Attributes")] 
    [Range(0.1f, 10f)] public float timeLoading = 5f;
    
    [Header("Components")]
    public Image progressBar;
    public TextMeshProUGUI loadingText;

    

    private bool _flagDoneProgress;
    private AsyncOperation _operation;

    void Start()
    {
        _operation = SceneManager.LoadSceneAsync(Constant.GameplayScene);
        _operation.allowSceneActivation = false;
        
        progressBar.fillAmount = 0;
        progressBar.DOFillAmount(1, timeLoading).OnUpdate(()=>loadingText.text = $"Loading {(int) (progressBar.fillAmount * 100)}%").OnComplete(()=> _flagDoneProgress = true);
        WaitProcess();
    }
    
    private async void WaitProcess()
    {
        await UniTask.WaitUntil(() => AdsController.Instance.IsInitialized && FirebaseController.Instance.isInitialized && _flagDoneProgress);
        ConfigController.Instance.Initialize();
        _operation.allowSceneActivation = true;
    }
}
