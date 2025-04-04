using AgentSimulation.Randoms.Continuous;
using AgentSimulation.Randoms.Other;
using AgentSimulation.Structures;

namespace AgentSimulation.Generators {
    public class RandomGenerators {
        public Triangular WorkerMoveToStorageTime = new(60, 480, 120);
        public Triangular MaterialPreparationTime = new(300, 900, 500);
        public Triangular WorkerMoveBetweenStationsTime = new(120, 500, 150);
        public Exponential OrderArrivalTime = new(1800);
        public UniformC RNG = new(0, 1);

        public UniformC WardrobeMountingTime = new(900, 1500);
        public UniformC WardrobeAssemblyTime = new(2100, 4500);
        public UniformC WardrobePaintingTime = new(36000, 42000);
        public UniformC WardrobeCuttingTime = new(900, 4800);

        public UniformC ChairAssemblyTime = new(840, 1440);
        public UniformC ChairPaintingTime = new(12600, 32400);
        public UniformC ChairCuttingTime = new(720, 960);

        public UniformC TableAssemblyTime = new(1800, 3600);
        public UniformC TablePaintingTime = new(12000, 36600);
        public EmpiricC TableCuttingTime = new(tableCuttingData);
        private static EmpiricData<double>[] tableCuttingData = [
            new(600, 1500, 0.6),
            new(1500, 3000, 0.4)
        ];
    }
}
