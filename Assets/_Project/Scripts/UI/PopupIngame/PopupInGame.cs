using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PopupInGame : Popup
{
   [Header("Components")]
   public TextMeshProUGUI LevelText;
   public TextMeshProUGUI LevelTypeText;

   private List<UIEffect> UIEffects => GetComponentsInChildren<UIEffect>().ToList();

   public void Start()
   {
      EventController.OnWinLevel += HideUI;
      EventController.OnLoseLevel += HideUI;
   }

   public void OnDestroy()
   {
      EventController.OnWinLevel -= HideUI;
      EventController.OnLoseLevel -= HideUI;
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

   public void Setup()
   {
      LevelText.text = $"Level {Data.CurrentLevel}";
      LevelTypeText.text = $"Level {(Data.UseLevelABTesting == 0 ? "A" : "B")}";
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
            FirebaseManager.OnClickButtonReplay();
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
            FirebaseManager.OnClickButtonSkipLevel();
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

   private void HideUI()
   {
      foreach (UIEffect item in UIEffects)
      {
         item.PlayAnim();
      }
   }
}
