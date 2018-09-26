//Jordan Black 2018

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using DG.Tweening;

public class Tutorial : MonoBehaviour
{

    public static Tutorial Instance;

    public GameObject tutorialPanel;

    //public Text textTemplate;
    //public Button confirmButton;

    public Text readyText;

    public TutorialItem[] tutorialItems;

    public bool showTutorial;

    public Image fillBar;

    private AudioSource audioSource;

    //public TutorialData tutorialData;

    private int tutorialIndex;

    private Tween tutorialTween;

    private Image tutorialBackground;
    private CanvasGroup tutorialCanvasGroup;

    //private float readTimer;

    public delegate void TutorialFinishedEventDelegate();
    public static event TutorialFinishedEventDelegate TutorialFinishedEvent;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    void Start()
    {
        tutorialCanvasGroup = tutorialPanel.GetComponent<CanvasGroup>();
        tutorialBackground = tutorialPanel.GetComponent<Image>();

        tutorialPanel.SetActive(false);

        foreach (TutorialItem t in tutorialItems)
        {
            t.itemGameObject.SetActive(false);
        }

        audioSource = this.GetComponent<AudioSource>();

        if (showTutorial == true)
        {
            PlayerPrefs.SetInt("ShowTutorial", 1);
        }

        readyText.text = "Skip";

    }

    private void Update()
    {
        if (tutorialTween != null && tutorialTween.IsActive() && tutorialIndex > 0)
        {
            fillBar.fillAmount = Mathf.Clamp01(tutorialTween.Elapsed() / tutorialItems[tutorialIndex - 1].timeToRead);
        }

    }

    public void ResetTutorial()
    {
        tutorialTween.Kill();

        tutorialIndex = 0;
        tutorialPanel.SetActive(true);

        InitializeTutorialSequence();
    }


    public void InitializeTutorialSequence()
    {
        DOVirtual.DelayedCall(1f, () =>
        {
            IncrementTutorialText();
        });
    }

    private void IncrementTutorialText()
    {
        //play the sound, animation, set the current timer, and reset the button
        PlaySoundAndAnimate();

        //make sure the panel is active
        if (tutorialPanel.activeInHierarchy == false)
        {
            tutorialPanel.SetActive(true);
            tutorialCanvasGroup.interactable = true;
            tutorialCanvasGroup.blocksRaycasts = true;
            tutorialBackground.DOFade(0f, 1f).From();
        }

        if (tutorialIndex < tutorialItems.Length - 1)
        {
            tutorialTween = DOVirtual.DelayedCall(tutorialItems[tutorialIndex].timeToRead, () => { IncrementTutorialText(); });
            tutorialIndex++;
        }
        else
        {
            tutorialTween = DOVirtual.DelayedCall(tutorialItems[tutorialIndex].timeToRead, () =>
            {
                readyText.DOFade(0f, 0.5f).OnComplete(() =>
                {
                    readyText.DOFade(1f, 0.5f);
                    readyText.text = "Ready!";
                });

            //EndTutorialSequence();
        });
        }

    }

    private void PlaySoundAndAnimate()
    {
        audioSource.PlayOneShot(tutorialItems[tutorialIndex].itemSound);
        tutorialItems[tutorialIndex].itemGameObject.SetActive(true);
        tutorialItems[tutorialIndex].itemGameObject.transform.DOLocalMoveY(-Screen.height, 1f).From().SetEase(Ease.InOutQuad);
    }

    //used by ready button
    public void EndTutorialSequence()
    {
        tutorialTween.Kill();

        tutorialIndex = 0;

        if (TutorialFinishedEvent != null) TutorialFinishedEvent();

        tutorialCanvasGroup.DOFade(0f, 1f).OnComplete(() =>
        {
            tutorialCanvasGroup.interactable = false;
            tutorialCanvasGroup.blocksRaycasts = false;
            tutorialPanel.SetActive(false);
        });

        tutorialBackground.DOFade(0f, 1f).OnComplete(() =>
        {
            tutorialPanel.SetActive(false);
        });


        PlayerPrefs.SetInt("ShowTutorial", 0);
        PlayerPrefs.Save();
    }

    public void ConfirmPress()
    {
        if (tutorialIndex < tutorialItems.Length - 1)
        {
            tutorialTween.Kill();
            IncrementTutorialText();
        }
        else
        {
            EndTutorialSequence();
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    [System.Serializable]
    public class TutorialItem
    {
        public GameObject itemGameObject;
        public float timeToRead;
        public AudioClip itemSound;
    }

} 
