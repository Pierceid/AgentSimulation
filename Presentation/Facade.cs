using AgentSimulation.Delegates;
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

        public Facade(Window? window) {
            mainWindow = window;
            mySimulation = new();
            graph = null;
        }

        public void StartSimulation() {
            if (mySimulation == null || graph == null || mySimulation.IsRunning()) return;

            mySimulation.Simulate(Constants.REPLICATION_COUNT, Constants.SIMULATION_TIME);
            graph.RefreshGraph();
        }

        public bool PauseSimulation() {
            if (mySimulation == null) return false;

            if (mySimulation.IsRunning()) {
                mySimulation.PauseSimulation();
            } else {
                mySimulation.ResumeSimulation();
            }

            return mySimulation.IsPaused();
        }

        public void StopSimulation() {
            if (mySimulation == null || graph == null || !mySimulation.IsRunning()) return;

            mySimulation.StopSimulation();
        }

        public void AnalyzeReplication() {
            if (mySimulation == null) return;


        }

        public void InitGraph(PlotView plotView) {
            if (mySimulation != null && mySimulation.IsRunning()) {
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

            if (mySimulation.IsRunning()) {
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

            LineGraphDelegate lineGraphObserver = new(mainWindow, graph);
            TextBlockDelegate textBlockObserver = new(textBlocks);
            DataGridDelegate dataGridObserver = new(dataGrids);

            mySimulation.RegisterDelegate(lineGraphObserver);
            mySimulation.RegisterDelegate(textBlockObserver);
            mySimulation.RegisterDelegate(dataGridObserver);
        }
    }
}
