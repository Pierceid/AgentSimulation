using AgentSimulation.Presentation;
using AgentSimulation.Utilities;
using System.Windows;
using System.Windows.Controls;

namespace AgentSimulation.Windows;

public partial class MainWindow : Window {
    private Facade facade;

    public MainWindow() {
        InitializeComponent();

        facade = new(this);
        facade.InitGraph(plotView);

        InitUI();
    }

    private void ButtonClick(object sender, RoutedEventArgs e) {
        if (sender is Button button) {
            if (button == btnStart) {
                InitCarpentry();
                facade?.StartSimulation();
                btnStart.IsEnabled = false;
            } else if (button == btnPause) {
                bool isPaused = facade?.PauseSimulation() ?? false;
                btnStart.IsEnabled = !isPaused;
                btnStop.IsEnabled = !isPaused;
            } else if (button == btnStop) {
                facade?.StopSimulation();
                btnStart.IsEnabled = true;
            } else if (button == btnAnalyze) {
                facade?.AnalyzeReplication();
            }
        }
    }

    private void CheckBoxClick(object sender, RoutedEventArgs e) {
        if (sender is CheckBox checkBox) {
            chkConfig1.IsChecked = checkBox == chkConfig1;
            chkConfig2.IsChecked = checkBox == chkConfig2;
            chkConfig3.IsChecked = checkBox == chkConfig3;
            chkConfig4.IsChecked = checkBox == chkConfig4;

            if (checkBox == chkConfig1) {
                UpdateUI(new Config1());
            } else if (checkBox == chkConfig2) {
                UpdateUI(new Config2());
            } else if (checkBox == chkConfig3) {
                UpdateUI(new Config3());
            } else if (checkBox == chkConfig4) {
                UpdateUI(new Config4());
            } else if (checkBox == chkAnimation) {
                bool isAnimation = chkAnimation.IsChecked!.Value;
                facade?.SetAnimator(isAnimation);
            }
        }
    }

    private void SliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
        if (sender is Slider slider) {
            if (slider == sldSpeed) {
                UpdateCarpentry();
            }
        }
    }

    private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e) {
        var result = MessageBox.Show("A simulation is running. Do you really want to exit?", "Confirm Exit", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        if (result == MessageBoxResult.No) {
            e.Cancel = true;
            return;
        }

        facade?.StopSimulation();
    }

    private void InitCarpentry() {
        if (!int.TryParse(txtReplications.Text, out int replications)) replications = 0;
        if (!int.TryParse(txtWorkersA.Text, out int workersA)) workersA = 0;
        if (!int.TryParse(txtWorkersB.Text, out int workersB)) workersB = 0;
        if (!int.TryParse(txtWorkersC.Text, out int workersC)) workersC = 0;
        if (!int.TryParse(txtWorkplaces.Text, out int workplaces)) workplaces = 0;

        facade?.InitCarpentry(replications, sldSpeed.Value, workersA, workersB, workersC, workplaces);

        TextBlock[] textBlocks = [txtTime, txtQueueA, txtQueueB, txtQueueC, txtQueueD, txtUtilityA, txtUtilityB, txtUtilityC, txtFinishedOrders, txtPendingOrders, txtConfidenceInterval];
        DataGrid[] dataGrids = [dgOrders, dgProducts, dgWorkers];

        facade?.InitObservers(textBlocks, dataGrids);

        UpdateCarpentry();
    }

    private void UpdateCarpentry() {
        double[] snapValues = [1, 60, 3600, 36000, double.MaxValue];
        int index = (int)(sldSpeed.Value - 1);
        double speed = snapValues[index];

        facade?.UpdateCarpentry(speed);
        lblSpeed.Content = $"Speed: {(index == snapValues.Length - 1 ? "VIRTUAL" : speed):0x}";
    }

    private void UpdateUI(Config config) {
        txtReplications.Text = config.Replications.ToString();
        txtWorkersA.Text = config.WorkersA.ToString();
        txtWorkersB.Text = config.WorkersB.ToString();
        txtWorkersC.Text = config.WorkersC.ToString();
        txtWorkplaces.Text = config.Workplaces.ToString();
    }

    private void InitUI() {
        sldSpeed.Value = 3;
        txtTime.Text = "00d 00h 00m 00s";
        txtFinishedOrders.Text = "0.00";
        txtPendingOrders.Text = "0.00";
        txtUtilityA.Text = "0.00";
        txtUtilityB.Text = "0.00";
        txtUtilityC.Text = "0.00";
        txtQueueA.Text = "0";
        txtQueueB.Text = "0";
        txtQueueC.Text = "0";
        txtQueueD.Text = "0";
        txtConfidenceInterval.Text = "< - ; - >";
        chkConfig1.IsChecked = true;

        UpdateUI(new Config1());
        InitCarpentry();
    }
}