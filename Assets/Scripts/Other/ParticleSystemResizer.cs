using System.Collections.Generic;
using UnityEngine;
using Zenject;
using static UnityEngine.ParticleSystem;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleSystemResizer : MonoBehaviour
{
    [Inject(Id = "ReferenceCamSize")] private float referenceCamSize;
    private ParticleSystem particle;
    private MinMaxCurve initialSpeedCurve;
    private MinMaxCurve initialSizeCurve;
    private List<ParticleSystem> subEmiters = new List<ParticleSystem>();
    private List<MinMaxCurve> subParticleSpeedCurves = new List<MinMaxCurve>();
    private List<MinMaxCurve> subParticleSizeCurves = new List<MinMaxCurve>();

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
        initialSpeedCurve = particle.main.startSpeed;
        initialSizeCurve = particle.main.startSize;

        for (int i = 0; i < particle.subEmitters.subEmittersCount; i++)
        {
            ParticleSystem particleSystem = particle.subEmitters.GetSubEmitterSystem(i);
            subEmiters.Add(particleSystem);
            subParticleSizeCurves.Add(particleSystem.main.startSize);
            subParticleSpeedCurves.Add(particleSystem.main.startSpeed);
        }
    }

    private void OnEnable()
    {
        SetupParticles();
    }
    private void SetupParticles()
    {
        float currentCamSize = Camera.main.orthographicSize;

        float scaler = currentCamSize / referenceCamSize;

        MainModule mainModule = particle.main;
        MinMaxCurve speedCurve = particle.main.startSpeed;
        MinMaxCurve sizeCurve = particle.main.startSize;

        speedCurve.constantMin = scaler * initialSpeedCurve.constantMin;
        speedCurve.constantMax = scaler * initialSpeedCurve.constantMax;

        sizeCurve.constantMin = scaler * initialSizeCurve.constantMin;
        sizeCurve.constantMax = scaler * initialSizeCurve.constantMax;
        
        mainModule.startSpeed = speedCurve;
        mainModule.startSize = sizeCurve;

        for (int i = 0; i < subEmiters.Count; i++)
        {
            ParticleSystem particleSystem = subEmiters[i];
            MainModule subMainModule = particleSystem.main;
            MinMaxCurve subSpeedCurve = particleSystem.main.startSpeed;
            MinMaxCurve subSizeCurve = particleSystem.main.startSize;

            subSpeedCurve.constantMin = scaler * subParticleSpeedCurves[i].constantMin;
            subSpeedCurve.constantMax = scaler * subParticleSpeedCurves[i].constantMax;

            subSizeCurve.constantMin = scaler * subParticleSizeCurves[i].constantMin;
            subSizeCurve.constantMax = scaler * subParticleSizeCurves[i].constantMax;

            subMainModule.startSpeed = subSpeedCurve;
            subMainModule.startSize = subSizeCurve;
        }
    }
}
