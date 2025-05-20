using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Wpf;
using Simulation;

namespace AgentSimulation.Presentation {
    public class LineGraph {
        private PlotModel model;
        private LinearAxis xAxis;
        private LinearAxis yAxis;
        private LineSeries mainSeries;
        private LineSeries upperSeries;
        private LineSeries lowerSeries;
        private TextAnnotation valueAnnotation;
        private PlotView plotView;

        public LineGraph(string modelTitle, string xAxisTitle, string yAxisTitle, string seriesTitle, PlotView plotView) {
            model = new PlotModel { Title = modelTitle };

            xAxis = new LinearAxis { Position = AxisPosition.Bottom, Title = xAxisTitle, Minimum = 0, Maximum = 1000 };
            model.Axes.Add(xAxis);

            yAxis = new LinearAxis { Position = AxisPosition.Left, Title = yAxisTitle, Minimum = 0, Maximum = 1000 };
            model.Axes.Add(yAxis);

            mainSeries = new LineSeries { Title = seriesTitle, Color = OxyColors.Green };
            model.Series.Add(mainSeries);

            upperSeries = new LineSeries { Title = "Upper Bound (95%)", Color = OxyColors.Blue, LineStyle = LineStyle.Dash };
            model.Series.Add(upperSeries);

            lowerSeries = new LineSeries { Title = "Lower Bound (95%)", Color = OxyColors.Red, LineStyle = LineStyle.Dash };
            model.Series.Add(lowerSeries);

            valueAnnotation = new TextAnnotation {
                Text = "0",
                StrokeThickness = 0,
                TextColor = OxyColors.Green,
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                TextHorizontalAlignment = HorizontalAlignment.Right,
                TextVerticalAlignment = VerticalAlignment.Top,
                TextPosition = new DataPoint(0, 0)
            };
            model.Annotations.Add(valueAnnotation);

            this.plotView = plotView;
            this.plotView.Model = model;
        }

        public void UpdatePlot(OSPABA.Simulation simulation) {
            if (simulation is MySimulation ms) {
                double rep = ms.CurrentReplication;
                double mean = ms.AverageOrderTime.Mean();
                double[] interval = (ms.AverageOrderTime.SampleSize < 2) ? [double.NaN, double.NaN] : ms.AverageOrderTime.ConfidenceInterval95;

                if (rep > 0) mainSeries.Points.Add(new DataPoint(rep, mean));

                if (rep > 29 && !double.IsNaN(interval[0]) && !double.IsNaN(interval[1])) {
                    upperSeries.Points.Add(new DataPoint(rep, interval[1]));
                    lowerSeries.Points.Add(new DataPoint(rep, interval[0]));
                }

                xAxis.Maximum = rep;
                yAxis.Minimum = lowerSeries.Points.Count > 0 ? Math.Min(mainSeries.MinY, lowerSeries.MinY) : mainSeries.MinY;
                yAxis.Maximum = upperSeries.Points.Count > 0 ? Math.Max(mainSeries.MaxX, upperSeries.MaxY) : mainSeries.MaxY;

                valueAnnotation.Text = $"{mean:F0}";
                valueAnnotation.TextPosition = new DataPoint(xAxis.Maximum * 0.99, yAxis.Maximum);
                model.InvalidatePlot(true);
            }
        }

        public void RefreshGraph() {
            mainSeries.Points.Clear();
            upperSeries.Points.Clear();
            lowerSeries.Points.Clear();

            xAxis.Minimum = 0;
            xAxis.Maximum = 1000;
            yAxis.Minimum = 0;
            yAxis.Maximum = 1000;
            model.InvalidatePlot(true);
        }

        public void InvalidatePlot() {
            model.InvalidatePlot(true);
        }
    }
}
