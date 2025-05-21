using AgentSimulation.Presentation;
using AgentSimulation.Utilities;
using System.Windows;
using System.Windows.Controls;
using Slider = System.Windows.Controls.Slider;

namespace AgentSimulation.Windows;

public partial class MainWindow : Window {
    private Facade facade;
    private bool isAnimation;
    public MainWindow() {
        InitializeComponent();

        facade = new(this);
        facade.InitGraph(plotView);
        isAnimation = false;

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
            if (checkBox == chkAnimation) {
                isAnimation = !isAnimation;
                if (isAnimation && sldSpeed.Value == 5) {
                    sldSpeed.Value = 4;
                }

                UpdateCarpentry();
                facade?.SetAnimator(isAnimation);
                return;
            }

            chkConfig1.IsChecked = checkBox == chkConfig1;
            chkConfig2.IsChecked = checkBox == chkConfig2;
            chkConfig3.IsChecked = checkBox == chkConfig3;
            chkConfig4.IsChecked = checkBox == chkConfig4;

            Config? config = null;

            if (checkBox == chkConfig1) {
                config = Util.LoadConfig("config.xlsx", 1);
            } else if (checkBox == chkConfig2) {
                config = Util.LoadConfig("config.xlsx", 2);
            } else if (checkBox == chkConfig3) {
                config = Util.LoadConfig("config.xlsx", 3);
            } else if (checkBox == chkConfig4) {
                config = Util.LoadConfig("config.xlsx", 4);
            }

            if (config != null) UpdateUI(config);
        }
    }

    private void SliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
        if (sender is Slider slider) {
            if (slider == sldSpeed) {
                if (isAnimation && slider.Value == 5) {
                    sldSpeed.Value = 4;
                }
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
        txtUtilityA.Text = "( - ; - ) %";
        txtUtilityB.Text = "( - ; - ) %";
        txtUtilityC.Text = "( - ; - ) %";
        txtQueueA.Text = "0";
        txtQueueB.Text = "0";
        txtQueueC.Text = "0";
        txtQueueD.Text = "0";
        txtConfidenceInterval.Text = "( - ; - ) h";
        chkConfig1.IsChecked = true;

        UpdateUI(new Config1());
        InitCarpentry();
    }
}