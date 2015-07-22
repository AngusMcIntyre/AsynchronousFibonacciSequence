using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;

namespace ThreadingExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FibonacciSequence sequence = new FibonacciSequence();

        System.Threading.CancellationTokenSource cancelSource = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Button_Cancel.IsEnabled = true;
            this.Button_Calculate.IsEnabled = false;

            int count = 10;

            TextBox_Results.Text = string.Empty;
            ProgressBar_Progress.Value = 0;
            ProgressBar_Progress.Maximum = count;
            this.cancelSource = new CancellationTokenSource();

            IProgress<int> progress = new Progress<int>(elementValue =>
            {
                this.TextBox_Results.Text += elementValue + " ";
                this.ProgressBar_Progress.Value += 1;
            });

            try
            {
                await sequence.CalculateAsync(count, cancelSource.Token, progress);

                this.TextBox_Results.Text += Environment.NewLine + "Sequence Complete!";
            }
            catch (OperationCanceledException)
            {
                this.TextBox_Results.Text += Environment.NewLine + "Sequence Cancelled!";
            }
            catch
            {
                this.TextBox_Results.Text += Environment.NewLine + "Error generating sequence.";
            }
            finally
            {
                this.ProgressBar_Progress.Value = ProgressBar_Progress.Maximum;
                this.Button_Cancel.IsEnabled = false;
                this.Button_Calculate.IsEnabled = true;
            }
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (this.cancelSource != null)
            {
                this.cancelSource.Cancel();
            }
        }
    }
}
