using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;

public class PopupInGame : Popup
{
   public TextMeshProUGUI levelText;
   public TextMeshProUGUI levelTypeText;

   private List<UIEffect> UIEffects => GetComponentsInChildren<UIEffect>().ToList();

   public void Start()
   {
      Observer.WinLevel += HideUI;
      Observer.LoseLevel += HideUI;
   }

   public void OnDestroy()
   {
      Observer.WinLevel -= HideUI;
      Observer.LoseLevel -= HideUI;
   }

   protected override void BeforeShow()
   {
      base.BeforeShow();
      
      if (!Data.IsTesting) AdsManager.ShowBanner();
      Setup();
   }

   protected override void BeforeHide()
   {
      base.BeforeHide();
      AdsManager.HideBanner();
   }

   private void Setup()
   {
      levelText.text = $"Level {Data.CurrentLevel}";
      levelTypeText.text = $"Level {(Data.UseLevelABTesting == 0 ? "A" : "B")}";
   }

   public void OnClickHome()
   {
      GameManager.Instance.ReturnHome();
   }

   public void OnClickReplay()
   {
      if (Data.IsTesting)
      {
         GameManager.Instance.ReplayGame();
      }
      else
      {
         AdsManager.ShowInterstitial(() =>
         {
            GameManager.Instance.ReplayGame();
         });
      }
   }

   public void OnClickPrevious()
   {
      GameManager.Instance.BackLevel();
   }

   public void OnClickSkip()
   {
      if (Data.IsTesting)
      {
         GameManager.Instance.NextLevel();
      }
      else
      {
         AdsManager.ShowRewardAds(() =>
         {
            GameManager.Instance.NextLevel();
         });
      }
   }

   public void OnClickLevelA()
   {
      Data.UseLevelABTesting = 0;
      GameManager.Instance.ReplayGame();
   }

   public void OnClickLevelB()
   {
      Data.UseLevelABTesting = 1;
      GameManager.Instance.ReplayGame();
   }

   public void OnClickLose()
   {
      GameManager.Instance.OnLoseGame(1f);
   }

   public void OnClickWin()
   {
      GameManager.Instance.OnWinGame(1f);
   }

   private void HideUI(Level level = null)
   {
      foreach (UIEffect item in UIEffects)
      {
         item.PlayAnim();
      }
   }
}
