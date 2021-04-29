using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireModifier : MonoBehaviour {

    public FireParams atkParams;
    public float recoverTime = 1f;

    private ParticleSystem fireParticles;
    private ParticleSystem.MainModule mainModule;
    private ParticleSystem.InheritVelocityModule velocityModule;
    private ParticleSystem.EmissionModule emissionModule;

    private FireParams baseParams;

    private void Start() {
        fireParticles = GetComponent<ParticleSystem>();
        mainModule = fireParticles.main;
        velocityModule = fireParticles.inheritVelocity;
        emissionModule = fireParticles.emission;

        baseParams.lifeTime = mainModule.startLifetimeMultiplier;
        baseParams.inheritVelocity = velocityModule.curveMultiplier;
        baseParams.emission = emissionModule.rateOverTimeMultiplier;
    }

    public void EnableAttackParams() {
        SetParams(atkParams);
        Invoke(nameof(ResetFire), recoverTime);
    }

    public void SetParams(FireParams fireParams) {
        mainModule.startLifetimeMultiplier = fireParams.lifeTime;
        velocityModule.curveMultiplier = fireParams.inheritVelocity;
        emissionModule.rateOverTimeMultiplier = fireParams.emission;
    }

    private void ResetFire() {
        SetParams(baseParams);
    }

    [System.Serializable]
    public struct FireParams {
        public float lifeTime;
        public float inheritVelocity;
        public float emission;
    }
}
