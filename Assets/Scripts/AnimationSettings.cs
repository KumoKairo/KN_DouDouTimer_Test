using DG.Tweening;
using UnityEngine;

[CreateAssetMenu]
public class AnimationSettings : ScriptableObject
{
   public float timerButtonPositionOffset = -80f;
   public float genericTweenDuration = 1f;
   public float genericDelay = 0.3f;
   public float interScreenDelay = 0.5f;
   public Ease genericEasing = Ease.OutCubic;
   
   public float timerWobbleDuration = 3f;
   public float timerWobbleStrength = 2f;
   public int timerWobbleVibrato = 10;
   [Range(0f, 1f)]
   public float timerWobbleElasticity = 1f;
}
