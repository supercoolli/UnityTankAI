using SPINACH.AI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace AITank
{
    public class WorldController : MonoBehaviour
    {
        //public int setpsPerSecond = 1;
        //public float physicsStepLength = 0.02f;

        public int totalStepsPerEpoch = 1000;

        public int tankCount;
        public Transform generatePoint;
        public float generateRadius;
        public GameObject tankPrefab;

        private List<Vector3> _initPosition = new List<Vector3>();
        private List<TankDriver> _drivers = new List<TankDriver>();

        private GeneticOptimisation _evolver;

        public int currentStepsInEpoch { get; private set; }
        public int epoch { get; private set; }

        public void GenerateInitial()
        {
            _evolver = new GeneticOptimisation(tankCount, 0.15, SelectionMethod.Natural);

            for (int i = 0; i < tankCount; i++)
            {
                var rp = Random.insideUnitSphere;
                rp.y = 0;
                rp = generatePoint.position + rp.normalized * Random.Range(1, generateRadius);
                _initPosition.Add(rp);

                _drivers.Add(Instantiate(tankPrefab, rp, Quaternion.identity).GetComponent<TankDriver>());
                _evolver.population.Add(_drivers[i].network);
            }

            _evolver.RandomizePopulation(-1d, 1d);
        }

        public void Evolve()
        {
            var fitnesses = new double[tankCount];
            for(int i = 0; i < _drivers.Count; i++)
            {
                fitnesses[i] = _drivers[i].CalculateFintness();
            }
            var max = new List<double>(fitnesses).OrderByDescending(x => x).FirstOrDefault();
            Debug.Log($"<color=#E91E63>Epoch {epoch} finished with highest fitnesses {max}.</color>");

            _evolver.Evolve(fitnesses);
            for (var i = 0; i < _drivers.Count; i++)
            {
                _drivers[i].network = _evolver.population[i] as GeneticOptimizeableNerualNetwork;
            }

            epoch++;
            RestoreInitial();
        }

        public void RestoreInitial()
        {
            currentStepsInEpoch = 0;
            for (var i = 0; i < tankCount; i++)
            {
                _drivers[i].transform.position = _initPosition[i];
                _drivers[i].GetComponent<Tank>().Setup();
            }
        }

        private void Start()
        {
            GenerateInitial();
        }

        private void Update()
        {
            if (currentStepsInEpoch > totalStepsPerEpoch) Evolve();
            //for (var i = 0; i < setpsPerSecond; i++)
            {
                TrainingUpdate();
            }
        }

        public void TrainingUpdate()
        {
            //Physics.Simulate(physicsStepLength);
            currentStepsInEpoch++;
            foreach (var tankDriver in _drivers)
            {
                tankDriver.DoSomethingUseful();
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(generatePoint.position, generateRadius);
        }

    }
}