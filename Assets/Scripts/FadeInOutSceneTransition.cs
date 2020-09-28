using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class FadeInOutSceneTransition : MonoBehaviour
{
    public static FadeInOutSceneTransition Instance;

    [SerializeField] private float _fadeInOutTime = 1.0f;
    [SerializeField] private Ease _ease;
    [SerializeField] private PostProcessVolume _volume;

    private Tween FadeAnimation;
    public bool FadeInCompleted { get; private set; }
    public bool FadeOutCompleted { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        FadeInCompleted = false;

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        FadeAnimation = DOTween.To(() => _volume.weight, value => _volume.weight = value, 1f, _fadeInOutTime)
            .SetAutoKill(false).SetEase(_ease).From(0f);
        FadeAnimation.Rewind();
        FadeAnimation.onComplete += () => FadeInCompleted = true;
        FadeAnimation.onRewind += () => FadeOutCompleted = true;
    }

    public static void LoadFadeScene()
    {
        SceneManager.LoadSceneAsync("FadeInOut", LoadSceneMode.Additive);
    }

    public void FadeIn()
    {
        FadeAnimation.Restart();
        FadeInCompleted = false;
        FadeOutCompleted = false;
    }

    public void FadeOut()
    {
        FadeOutCompleted = false;
        FadeAnimation.SmoothRewind();
    }

    private void OnValidate()
    {
        if (_volume == null)
        {
            _volume = this.GetComponent<PostProcessVolume>();
        }
    }
}