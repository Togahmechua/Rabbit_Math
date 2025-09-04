using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LooseCanvas : UICanvas
{
    [Header("---Other Button---")]
    [SerializeField] private Button retryBtn;
    [SerializeField] private Button menuBtn;

    private bool isClick;

    private void OnEnable()
    {
        AudioManager.Ins.PlaySFX(AudioManager.Ins.loose);
    }

    private void Start()
    {
        retryBtn.onClick.AddListener(() =>
        {
            AudioManager.Ins.PlaySFX(AudioManager.Ins.click);

            UIManager.Ins.TransitionUI<ChangeUICanvas, LooseCanvas>(0.6f,
               () =>
               {
                   LevelManager.Ins.DespawnLevel();
                   UIManager.Ins.OpenUI<MainCanvas>();
                   LevelManager.Ins.SpawnLevel();
               });
        });

        menuBtn.onClick.AddListener(() =>
        {
            AudioManager.Ins.PlaySFX(AudioManager.Ins.click);

            UIManager.Ins.TransitionUI<ChangeUICanvas, LooseCanvas>(0.6f,
               () =>
               {
                   LevelManager.Ins.DespawnLevel();
                   UIManager.Ins.OpenUI<StartCanvas>();
               });
        });
    }
}
