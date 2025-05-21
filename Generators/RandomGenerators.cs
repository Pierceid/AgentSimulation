using AgentSimulation.Randoms.Continuous;
using AgentSimulation.Randoms.Discrete;
using AgentSimulation.Randoms.Other;
using AgentSimulation.Structures;

namespace AgentSimulation.Generators {
    public class RandomGenerators {
        public Triangular WorkerMoveToStorageTime = new(60, 480, 120);
        public Triangular MaterialPreparationTime = new(300, 900, 500);
        public Triangular WorkerMoveBetweenStationsTime = new(120, 500, 150);
        public Exponential OrderArrivalTime = new(1800);
        public UniformC RNG = new(0, 1);
        public UniformD ProductCount = new(1, 6);
        public Triangular DryingTime = new(300, 900, 500);

        public UniformC WardrobeCuttingTime = new(900, 4800);
        public UniformC WardrobePaintingTime = new(18000, 36000);
        public UniformC WardrobePicklingTime = new(15000, 33600);
        public UniformC WardrobeAssemblyTime = new(2100, 4500);
        public UniformC WardrobeMountingTime = new(900, 1500);

        public UniformC ChairCuttingTime = new(720, 960);
        public UniformC ChairPaintingTime = new(5400, 24000);
        public UniformC ChairPicklingTime = new(2400, 12000);
        public UniformC ChairAssemblyTime = new(840, 1440);

        public EmpiricC TableCuttingTime = new(tableCuttingData);
        public UniformC TablePaintingTime = new(6000, 28800);
        public EmpiricC TablePicklingTime = new(tablePicklingData);
        public UniformC TableAssemblyTime = new(1800, 3600);

        private static EmpiricData<double>[] tableCuttingData = [
            new(600, 1500, 0.6),
            new(1500, 3000, 0.4)
        ];

        private static EmpiricData<double>[] tablePicklingData = [
            new(3000, 4200, 0.1),
            new(4200, 9000, 0.6),
            new(9000, 12000, 0.3)
        ];
    }
}
