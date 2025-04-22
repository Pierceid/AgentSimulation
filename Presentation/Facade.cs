using AgentSimulation.Observer;
using AgentSimulation.Structures;
using OxyPlot.Wpf;
using Simulation;
using System.Windows;
using System.Windows.Controls;

namespace AgentSimulation.Presentation {
    public class Facade {
        private Window? mainWindow;
        private MySimulation? mySimulation;
        private LineGraph? graph;
        private bool isRunning;

        public Facade(Window? window) {
            mainWindow = window;
            mySimulation = new();
            graph = null;
            isRunning = false;
        }

        public void StartSimulation() {
            if (mySimulation == null || graph == null || isRunning) return;

            isRunning = true;
            mySimulation.Simulate(mySimulation.ReplicationCount, Constants.SIMULATION_TIME);
            graph.RefreshGraph();
        }

        public bool PauseSimulation() {
            if (mySimulation == null) return false;

            if (mySimulation.IsPaused()) {
                mySimulation.ResumeSimulation();
            } else {
                mySimulation.PauseSimulation();
            }

            return mySimulation.IsPaused();
        }

        public void StopSimulation() {
            if (mySimulation == null || graph == null || !isRunning) return;

            isRunning = false;
            mySimulation.StopSimulation();
        }

        public void AnalyzeReplication() {
            if (mySimulation == null) return;


        }

        public void InitGraph(PlotView plotView) {
            if (isRunning) {
                StopSimulation();
            }

            graph = new(
                modelTitle: "Average Order Time",
                xAxisTitle: "Replications",
                yAxisTitle: "Order Time",
                seriesTitle: "Order Time",
                plotView: plotView
            );
        }

        public void InitCarpentry(int replications, double speed, int workersA, int workersB, int workersC, int workplaces) {
            if (mySimulation == null) return;

            if (isRunning) {
                StopSimulation();
            }

            mySimulation.Clear();
            mySimulation.InitWorkers(workersA, workersB, workersC);
            mySimulation.InitWorkplaces(workplaces);
            mySimulation.InitSpeed(speed);
        }

        public void UpdateCarpentry(double speed) {
            if (mySimulation == null) return;

            mySimulation.InitSpeed(speed);
        }

        public void InitObservers(TextBlock[] textBlocks, DataGrid[] dataGrids) {
            if (mySimulation == null || mainWindow == null || graph == null) return;

            LineGraphObserver lineGraphObserver = new(mainWindow, graph);
            TextBlockObserver textBlockObserver = new(textBlocks);
            DataGridObserver dataGridObserver = new(dataGrids);

            mySimulation.Attach(lineGraphObserver);
            mySimulation.Attach(textBlockObserver);
            mySimulation.Attach(dataGridObserver);
        }
    }
}
