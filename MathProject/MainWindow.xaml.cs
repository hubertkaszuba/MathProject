using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace MathProject
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public PlotModel Model;
        private const string firstFunctionName = "x^2-2x-1";
        private const string secondFunctionName = "x^3-2x";

        public MainWindow()
        {
            InitializeComponent();
        }

        public double firstFunction(double x)
        {
            return (Math.Pow(x, 2) - (2 * x) - 1);
        }

        public double derivedFirstFunction(double x)
        {
            return (2 * x - 2);
        }

        public double secondaryDerivateFirstFunction()
        {
            return 2;
        }

        public double secondFunction(double x)
        {
            return (Math.Pow(x, 3) - (2 * x));
        }

        public double derivedSecondFunction(double x)
        {
            return ((3 * Math.Pow(x,2)) - 2);
        }

        public double secondaryDerivateSecondFunction(double x)
        {
            return (6 * x);
        }

        private void firstFunctionBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Model = new PlotModel { Title = firstFunctionName, PlotType = PlotType.XY };
            this.Model.Series.Add(new FunctionSeries((x) => (Math.Pow(x, 2) - (2 * x) - 1), -5, 5, 0.1, firstFunctionName));
            this.FunctionView.Model = this.Model;
        }

        private void Test2_Click(object sender, RoutedEventArgs e)
        {
            this.Model = new PlotModel { Title = secondFunctionName, PlotType = PlotType.XY };
            this.Model.Series.Add(new FunctionSeries((x) => (Math.Pow(x, 3) - (2 * x)), -5, 5, 0.1, secondFunctionName));
            this.FunctionView.Model = this.Model;
        }

        private void calculateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.Model.Title == firstFunctionName)
            {
                double beginningVal;
                double endVal;
                double precision;

                if(Double.TryParse(this.beginningOfTheCompartment.Text, out beginningVal) && Double.TryParse(this.endOfTheCompartment.Text, out endVal) && Double.TryParse(this.approximation.Text, out precision))
                {
                    var functionValBeginning = firstFunction(beginningVal);
                    var secDerFunctionVal = secondaryDerivateFirstFunction();
                    List<DataGridObjects> approximations = new List<DataGridObjects>();
                    if ((functionValBeginning * secDerFunctionVal) > 0)
                    {
                        approximations.Add(new DataGridObjects { StepNumber = 1, Value = beginningVal });
                        approximations.Add(new DataGridObjects { StepNumber = 2, Value = (approximations[0].Value - (firstFunction(approximations[0].Value) / derivedFirstFunction(approximations[0].Value))) });
                        int i = 2;
                        while(Math.Abs((approximations[i-2].Value - approximations[i-1].Value)) >= precision)
                        {
                            approximations.Add(new DataGridObjects { StepNumber = i + 1, Value = (approximations[i - 1].Value - (firstFunction(approximations[i - 1].Value) / derivedFirstFunction(approximations[i - 1].Value))) });
                            i++;
                        }
                    }
                    functionValBeginning = firstFunction(endVal);
                    if((functionValBeginning * secDerFunctionVal) > 0)
                    {
                        approximations.Add(new DataGridObjects { StepNumber = 1, Value = endVal });
                        approximations.Add(new DataGridObjects { StepNumber = 2, Value = (approximations[0].Value - (firstFunction(approximations[0].Value) / derivedFirstFunction(approximations[0].Value))) });
                        int i = 2;
                        while (Math.Abs((approximations[i - 2].Value - approximations[i - 1].Value)) >= precision)
                        {
                            approximations.Add(new DataGridObjects { StepNumber = i + 1, Value = (approximations[i - 1].Value - (firstFunction(approximations[i - 1].Value) / derivedFirstFunction(approximations[i - 1].Value))) });
                            i++;
                        }

                    }
                    else
                    {
                        MessageBox.Show("Niepoprawny przedział");
                    }
                    this.results.ItemsSource = approximations;
                }
                else
                {
                    MessageBox.Show("Niepoprawne dane");
                }

            }
            else if(this.Model.Title == secondFunctionName)
            {
                double beginningVal;
                double endVal;
                double precision;

                if (Double.TryParse(this.beginningOfTheCompartment.Text, out beginningVal) && Double.TryParse(this.endOfTheCompartment.Text, out endVal) && Double.TryParse(this.approximation.Text, out precision))
                {
                    var functionValBeginning = secondFunction(beginningVal);
                    var secDerFunctionVal = secondaryDerivateSecondFunction(beginningVal);
                    List<DataGridObjects> approximations = new List<DataGridObjects>();
                    if ((functionValBeginning * secDerFunctionVal) > 0)
                    {
                        approximations.Add(new DataGridObjects { StepNumber = 1, Value = beginningVal });
                        approximations.Add(new DataGridObjects { StepNumber = 2, Value = (approximations[0].Value - (secondFunction(approximations[0].Value) / derivedSecondFunction(approximations[0].Value))) });
                        int i = 2;
                        while (Math.Abs((approximations[i - 2].Value - approximations[i - 1].Value)) >= precision)
                        {
                            approximations.Add(new DataGridObjects { StepNumber = i + 1, Value = (approximations[i - 1].Value - (secondFunction(approximations[i - 1].Value) / derivedSecondFunction(approximations[i - 1].Value))) });
                            i++;
                        }
                    }
                    functionValBeginning = secondFunction(endVal);
                    if ((functionValBeginning * secDerFunctionVal) > 0)
                    {
                        approximations.Add(new DataGridObjects { StepNumber = 1, Value = endVal });
                        approximations.Add(new DataGridObjects { StepNumber = 2, Value = (approximations[0].Value - (secondFunction(approximations[0].Value) / derivedSecondFunction(approximations[0].Value))) });
                        int i = 2;
                        while (Math.Abs((approximations[i - 2].Value - approximations[i - 1].Value)) >= precision)
                        {
                            approximations.Add(new DataGridObjects { StepNumber = i + 1, Value = (approximations[i - 1].Value - (secondFunction(approximations[i - 1].Value) / derivedSecondFunction(approximations[i - 1].Value))) });
                            i++;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Niepoprawny przedział");
                    }
                    this.results.ItemsSource = approximations;
                }
                else
                {
                    MessageBox.Show("Niepoprawne dane");
                }
            }
            else
            {
                MessageBox.Show("Wybierz funkcje");
            }

        }
    }
}
