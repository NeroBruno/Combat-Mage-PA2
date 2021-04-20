using UnityEngine;

public class EntityVitals : GenericVitals
{
    [SerializeField]
    [Range(1f, 15f)]
    [Tooltip("At which landing speed the entity will start taking damage")]
    private float _MinFallSpeed = 4f;

    [SerializeField]
    [Range(10f, 50f)]
    [Tooltip("At which landing speed the entity will die if it has no defense")]
    private float _MaxFallSpeed = 15f;

    [SerializeField]
    [Tooltip("The sounds that will be played when this entity receives damaage")]
    private SoundPlayer _HurtAudio = null;

    [SerializeField]
    private float _TimeBetweenScreams = 1f;

    [SerializeField]
    private SoundPlayer _FallDamageAudio = null;

    [SerializeField]
    private Animator _Animator = null;

    [SerializeField]
    private float _GetHitMax = 30f;

    private float _NextTimeCanScream;

    private void Awake()
    {
        Entity.ChangeHealth.SetTryer(Try_ChangeHealth);
        Entity.FallImpact.AddListener(On_FallImpact);
        Entity.Health.AddChangeListener(OnChanged_Health);
    }

    private void OnChanged_Health(float health)
    {
        float delta = health - Entity.Health.GetPreviousValue();

        if (delta < 0f)
        {
            if(_Animator != null)
            {
                _Animator.SetFloat("Get Hit Amount", Mathf.Abs(delta / _GetHitMax));
                _Animator.SetTrigger("Get Hit");
            }

            if (Time.time > _NextTimeCanScream)
            {
                //_HurtAudio.Play(ItemSelection.Method.RandomExcludeLast, _AudioSource);
                _NextTimeCanScream = Time.time + _TimeBetweenScreams;
            }
        }
    }

    private void On_FallImpact(float impactSpeed)
    {
        if (impactSpeed >= _MinFallSpeed)
        {
            Entity.ChangeHealth.Try(new HealthEventData(-100f * (impactSpeed / _MaxFallSpeed)));
            _FallDamageAudio.Play(ItemSelection.Method.Random, _AudioSource);
        }
    }
}
