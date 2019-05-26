using OxyPlot;
using OxyPlot.Axes;
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
        private const string secondFunctionName = "cos(x)";
        public IList<DataPoint> Points { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            this.stackPanel.Visibility = Visibility.Hidden;
            this.generateModel.Visibility = Visibility.Hidden;
            this.degreeOfPolynomial.Visibility = Visibility.Hidden;
            this.degreeOfPolynomialLbl.Visibility = Visibility.Hidden;
            this.scrollViewer.Visibility = Visibility.Hidden;
            this.Points = new List<DataPoint>();
        }

        public double secondFunction(double x)
        {
            return /*(Math.Pow(x, 3) - (2 * x));*/Math.Cos(x);
        }

        public double derivedSecondFunction(double x)
        {
            return /*((3 * Math.Pow(x,2)) - 2);*/-1 * Math.Sin(x);
        }

        public double secondaryDerivateSecondFunction(double x)
        {
            return /*(6 * x);*/-1 * Math.Cos(x);
        }

        private void firstFunctionBtn_Click(object sender, RoutedEventArgs e)
        {
            this.stackPanel.Visibility = Visibility.Visible;
            this.generateModel.Visibility = Visibility.Visible;
            this.degreeOfPolynomial.Visibility = Visibility.Visible;
            this.degreeOfPolynomialLbl.Visibility = Visibility.Visible;
            this.scrollViewer.Visibility = Visibility.Visible;
        }

        private void Test2_Click(object sender, RoutedEventArgs e)
        {
            this.stackPanel.Visibility = Visibility.Hidden;
            this.generateModel.Visibility = Visibility.Hidden;
            this.degreeOfPolynomial.Visibility = Visibility.Hidden;
            this.degreeOfPolynomialLbl.Visibility = Visibility.Hidden;
            this.scrollViewer.Visibility = Visibility.Hidden;

            this.Model = new PlotModel { Title = secondFunctionName, PlotType = PlotType.XY };
            this.Model.Series.Add(new FunctionSeries((x) => Math.Cos(x)/*(Math.Pow(x, 3) - (2 * x))*/, -5, 5, 0.0001, secondFunctionName));
            this.Model.Axes.Add(new OxyPlot.Axes.LinearAxis { Position = OxyPlot.Axes.AxisPosition.Bottom, ExtraGridlines = new double[] { 0 }, ExtraGridlineThickness = 1, ExtraGridlineColor = OxyColors.Black, });
            this.Model.Axes.Add(new OxyPlot.Axes.LinearAxis { Position = OxyPlot.Axes.AxisPosition.Left, ExtraGridlines = new double[] { 0 }, ExtraGridlineThickness = 1, ExtraGridlineColor = OxyColors.Black, });
            this.Model.InvalidatePlot(true);
            this.FunctionView.Model = this.Model;
        }

        private void calculateBtn_Click(object sender, RoutedEventArgs e)
        {
            if(this.Model == null)
            {
                MessageBox.Show("Wybierz funkcje");
            }
            else if (this.Model.Title != secondFunctionName && this.Model != null)
            {
                double beginningVal;
                double endVal;
                double precision;
                string beginningTxt = this.beginningOfTheCompartment.Text;
                string endTxt = this.endOfTheCompartment.Text;
                string precisionTxt = this.approximation.Text;

                if (beginningTxt.Contains('.'))
                    beginningTxt = beginningTxt.Replace('.', ',');

                if (endTxt.Contains('.'))
                    endTxt = endTxt.Replace('.', ',');

                if (precisionTxt.Contains('.'))
                    precisionTxt = precisionTxt.Replace('.', ',');

                if (Double.TryParse(beginningTxt, out beginningVal) && Double.TryParse(endTxt, out endVal) && Double.TryParse(precisionTxt, out precision))
                {
                    var functionValBeginning = firstFunction(beginningVal);
                    var functionValEnd = firstFunction(endVal);
                    var secDerFunctionValBeginning = secondaryDerivatePolynomial(beginningVal);
                    var secDerFunctionValEnd = secondaryDerivatePolynomial(endVal);
                    List<DataGridObjects> approximations = new List<DataGridObjects>();
                    List<DataGridObjects> approximationsRegulaFalsi = new List<DataGridObjects>();
                    int i = 0;
                    if ((functionValBeginning * functionValEnd) < 0)
                    {
                        if ((functionValBeginning * secDerFunctionValBeginning) > 0)
                        {
                            approximations.Add(new DataGridObjects { StepNumber = 0, Value = beginningVal });
                            approximations.Add(new DataGridObjects { StepNumber = 1, Value = (approximations[0].Value - (firstFunction(approximations[0].Value) / derivedPolynomial(approximations[0].Value))) });
                            i = 2;
                            while ((Math.Abs((approximations[i - 2].Value - approximations[i - 1].Value)) >= precision) && (i < 100))
                            {
                                approximations.Add(new DataGridObjects { StepNumber = i, Value = (approximations[i - 1].Value - (firstFunction(approximations[i - 1].Value) / derivedPolynomial(approximations[i - 1].Value))) });
                                i++;
                            }
                        }
                        else
                        {
                            approximations.Add(new DataGridObjects { StepNumber = 0, Value = endVal });
                            approximations.Add(new DataGridObjects { StepNumber = 1, Value = (approximations[0].Value - (firstFunction(approximations[0].Value) / derivedPolynomial(approximations[0].Value))) });
                            i = 2;
                            while ((Math.Abs((approximations[i - 2].Value - approximations[i - 1].Value)) >= precision) && (i < 100))
                            {
                                approximations.Add(new DataGridObjects { StepNumber = i, Value = (approximations[i - 1].Value - (firstFunction(approximations[i - 1].Value) / derivedPolynomial(approximations[i - 1].Value))) });
                                i++;
                            }                                
                        }
                        this.Model.InvalidatePlot(true);

                        double a = beginningVal;
                        double b = endVal;
                        double x1 = a;
                        double x0 = b;
                        double fa = firstFunction(beginningVal);
                        double fb = firstFunction(endVal);
                        double f0;
                        approximationsRegulaFalsi.Add(new DataGridObjects { StepNumber = 0, Value = b });
                        approximationsRegulaFalsi.Add(new DataGridObjects { StepNumber = 1, Value = a });
                        i = 2;
                        while (Math.Abs(approximationsRegulaFalsi[i - 2].Value - approximationsRegulaFalsi[i - 1].Value) >= precision && (i < 100))
                        {
                            x1 = x0;
                            x0 = a - fa * (b - a) / (fb - fa);
                            f0 = firstFunction(x0);
                            if (fa * f0 < 0)
                            {
                                b = x0;
                                fb = f0;
                            }
                            else
                            {
                                a = x0;
                                fa = f0;
                            }
                            approximationsRegulaFalsi.Add(new DataGridObjects { StepNumber = i, Value = x0 });
                            i++;
                        }

                        int k = 0;
                        foreach (var p in approximations)
                        {
                            var s1 = new LineSeries();

                            s1.Color = OxyColors.LightBlue;
                            s1.MarkerFill = OxyColors.Blue;
                            s1.MarkerType = OxyPlot.MarkerType.Circle;
                            s1.IsVisible = true;
                            s1.Points.Add(new DataPoint(p.Value, 0));
                            s1.ToolTip = "x" + p.StepNumber;
                            s1.LabelFormatString = "x" + p.StepNumber;
                            this.Model.Series.Add(s1);
                            k++;
                        }
                        this.Model.InvalidatePlot(true);

                        k = 0;
                        foreach (var p in approximationsRegulaFalsi)
                        {
                            var s1 = new LineSeries();

                            s1.Color = OxyColors.LightBlue;
                            s1.MarkerFill = OxyColors.Red;
                            s1.MarkerType = OxyPlot.MarkerType.Circle;
                            s1.IsVisible = true;
                            s1.Points.Add(new DataPoint(p.Value, 0));
                            s1.ToolTip = "x" + p.StepNumber;
                            s1.LabelFormatString = "x" + p.StepNumber;
                            this.Model.Series.Add(s1);
                            k++;
                        }
                        this.Model.InvalidatePlot(true);
                    }
                    else
                    {
                        MessageBox.Show("Niepoprawny przedział");
                    }
                    this.results.ItemsSource = approximations;
                    this.resultsRegulaFalsi.ItemsSource = approximationsRegulaFalsi;
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
                string beginningTxt = this.beginningOfTheCompartment.Text;
                string endTxt = this.endOfTheCompartment.Text;
                string precisionTxt = this.approximation.Text;

                if (beginningTxt.Contains('.'))
                    beginningTxt = beginningTxt.Replace('.', ',');

                if (endTxt.Contains('.'))
                    endTxt = endTxt.Replace('.', ',');

                if (precisionTxt.Contains('.'))
                    precisionTxt = precisionTxt.Replace('.', ',');

                if (Double.TryParse(beginningTxt, out beginningVal) && Double.TryParse(endTxt, out endVal) && Double.TryParse(precisionTxt, out precision))
                {
                    var functionValBeginning = secondFunction(beginningVal);
                    var functionValEnd = secondFunction(endVal);
                    var secDerFunctionVal = secondaryDerivateSecondFunction(beginningVal);
                    List<DataGridObjects> approximations = new List<DataGridObjects>();
                    List<DataGridObjects> approximationsRegulaFalsi = new List<DataGridObjects>();
                    int i = 0;
                    if ((functionValBeginning * functionValEnd) < 0)
                    {
                        if ((functionValBeginning * secDerFunctionVal) > 0)
                        {
                            approximations.Add(new DataGridObjects { StepNumber = 0, Value = beginningVal });
                            approximations.Add(new DataGridObjects { StepNumber = 1, Value = (approximations[0].Value - (secondFunction(approximations[0].Value) / derivedSecondFunction(approximations[0].Value))) });
                            i = 2;
                            while ((Math.Abs((approximations[i - 2].Value - approximations[i - 1].Value)) >= precision) && (i < 100))
                            {
                                approximations.Add(new DataGridObjects { StepNumber = i, Value = (approximations[i - 1].Value - (secondFunction(approximations[i - 1].Value) / derivedSecondFunction(approximations[i - 1].Value))) });
                                i++;
                            }
                        }
                        else
                        {
                            approximations.Add(new DataGridObjects { StepNumber = 0, Value = endVal });
                            approximations.Add(new DataGridObjects { StepNumber = 1, Value = (approximations[0].Value - (secondFunction(approximations[0].Value) / derivedSecondFunction(approximations[0].Value))) });
                            i = 2;
                            while ((Math.Abs((approximations[i - 2].Value - approximations[i - 1].Value))) >= precision && (i < 100))
                            {
                                approximations.Add(new DataGridObjects { StepNumber = i, Value = (approximations[i - 1].Value - (secondFunction(approximations[i - 1].Value) / derivedSecondFunction(approximations[i - 1].Value))) });
                                i++;
                            }
                        }
                        double a = beginningVal;
                        double b = endVal;
                        double x1 = a;
                        double x0 = b;
                        double fa = secondFunction(beginningVal);
                        double fb = secondFunction(endVal);
                        double f0;
                        approximationsRegulaFalsi.Add(new DataGridObjects { StepNumber = 0, Value = b });
                        approximationsRegulaFalsi.Add(new DataGridObjects { StepNumber = 1, Value = a });
                        i = 2;
                        while(Math.Abs(approximationsRegulaFalsi[i-2].Value - approximationsRegulaFalsi[i - 1].Value) >= precision  && (i < 100))
                        {
                            x1 = x0;
                            x0 = a - fa * (b - a) / (fb - fa);
                            f0 = secondFunction(x0);
                            if(fa * f0 < 0)
                            {
                                b = x0;
                                fb = f0;
                            }
                            else
                            {
                                a = x0;
                                fa = f0;
                            }

                            approximationsRegulaFalsi.Add(new DataGridObjects { StepNumber = i, Value = x0 });
                            i++;
                        }

                        int k = 0;
                        foreach (var p in approximations)
                        {
                            var s1 = new LineSeries();

                            s1.Color = OxyColors.LightBlue;
                            s1.MarkerFill = OxyColors.Blue;
                            s1.MarkerType = OxyPlot.MarkerType.Circle;
                            s1.IsVisible = true;
                            s1.Points.Add(new DataPoint(p.Value, 0));
                            s1.ToolTip = "x" + p.StepNumber;
                            s1.LabelFormatString = "x" + p.StepNumber;
                            this.Model.Series.Add(s1);
                            k++;
                        }
                        this.Model.InvalidatePlot(true);

                        k = 0;
                        foreach (var p in approximationsRegulaFalsi)
                        {
                            var s1 = new LineSeries();

                            s1.Color = OxyColors.LightBlue;
                            s1.MarkerFill = OxyColors.Red;
                            s1.MarkerType = OxyPlot.MarkerType.Circle;
                            s1.IsVisible = true;
                            s1.Points.Add(new DataPoint(p.Value, 0));
                            s1.ToolTip = "x" + p.StepNumber;
                            s1.LabelFormatString = "x" + p.StepNumber;
                            this.Model.Series.Add(s1);
                            k++;
                        }
                        this.Model.InvalidatePlot(true);
                    }
                    else
                    {
                        MessageBox.Show("Niepoprawny przedział");
                    }
                    this.results.ItemsSource = approximations;
                    this.resultsRegulaFalsi.ItemsSource = approximationsRegulaFalsi;
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

        private void degreeOfPolynomial_TextChanged(object sender, TextChangedEventArgs e)
        {
            int degree;
            for (int i = 0; i < this.stackPanel.Children.Count / 2; i++)
                this.stackPanel.UnregisterName("a" + i);
            this.stackPanel.Children.Clear();
            this.stackPanel.Width = 240;
            if (int.TryParse(this.degreeOfPolynomial.Text, out degree))
            {
                for(int i = degree; i >=0 ; i--)
                {
                    TextBox textBox = new TextBox();
                    textBox.Text = "";
                    textBox.Width = 30;
                    textBox.Height = 20;
                    textBox.Margin = new Thickness(0,0, 0, 5);
                    textBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    textBox.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    textBox.Name = "a" + i;
                    this.stackPanel.RegisterName(textBox.Name, textBox);
                    Label label = new Label();
                    label.Content = "x^" + i;
                    label.Height = 30;
                    label.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    label.VerticalAlignment = System.Windows.VerticalAlignment.Top;

                    this.stackPanel.Children.Add(textBox);
                    this.stackPanel.Children.Add(label);
                    if (i >= 4)
                    {
                        this.stackPanel.Width = this.stackPanel.Width + 58;
                    }

                }
            }
        }

        private void generateModel_Click(object sender, RoutedEventArgs e)
        {
            double[] coeff = new double[this.stackPanel.Children.Count/2];
            int j = 0;
            bool isValid = true;

            for(int i = 0; i < coeff.Length; i++)
            {
                double tmp;
                TextBox tmpTxt = (TextBox)this.stackPanel.FindName("a" + i);
                if(!Double.TryParse(tmpTxt.Text, out tmp))
                {
                    isValid = false;
                    break;
                }
            }
            if(isValid)
            {
                for (int i = 0; i < coeff.Length; i++)
                {
                    TextBox tmpTxt = (TextBox)this.stackPanel.FindName("a" + i);
                    coeff[i] = Double.Parse(tmpTxt.Text);
                    j++;
                }
                //coeff = coeff.Reverse().ToArray();
                string name = string.Empty;
                for (int i = coeff.Length-1; i >= 0; i--)
                {
                    if(name != "" && name[name.Length - 1] != '+' && coeff[i] > 0)
                        name = String.Concat(name, '+');
                    if (i != 0 && i != 1)
                    {
                        if (coeff[i] != 1 && coeff[i] != 0 && coeff[i] != -1)
                            name = String.Concat(name, String.Format("{0}x^{1}", coeff[i], i));
                        else if (coeff[i] == 1)
                            name = String.Concat(name, String.Format("x^{1}", coeff[i], i));
                        else if (coeff[i] == -1)
                            name = String.Concat(name, String.Format("-x^{1}", coeff[i], i));
                    }
                    else if(i == 1)
                    {
                        if (coeff[i] != 1 && coeff[i] != 0)
                            name = String.Concat(name, String.Format("{0}x", coeff[i]));
                        else if (coeff[i] == 1)
                            name = String.Concat(name, "x");
                        else if (coeff[i] == -1)
                            name = String.Concat(name,"-x");
                    }
                    else
                    {
                        if (coeff[i] != 0)
                            name = String.Concat(name, String.Format("{0}", coeff[i]));
                    }

                    if (coeff[i] != 0 && i != 0 && coeff[i-1] > 0)
                    {
                        name = String.Concat(name, '+');
                    }

                }

                Func<double, double> func = CreatePolynomialFunction(coeff);
                this.Model = new PlotModel { Title = name, PlotType = PlotType.XY };
                this.Model.Series.Add(new FunctionSeries(func, -5, 5, 0.0001, name));
                this.Model.Axes.Add(new OxyPlot.Axes.LinearAxis { Position = OxyPlot.Axes.AxisPosition.Bottom, ExtraGridlines = new double[] { 0 }, ExtraGridlineThickness = 1, ExtraGridlineColor = OxyColors.Black, });
                this.Model.Axes.Add(new OxyPlot.Axes.LinearAxis { Position = OxyPlot.Axes.AxisPosition.Left, ExtraGridlines = new double[] { 0 }, ExtraGridlineThickness = 1, ExtraGridlineColor = OxyColors.Black, });
                this.FunctionView.Model = this.Model;
            }
            else
            {
                MessageBox.Show("Niepoprawne współczynniki");
            }

        }

        public double firstFunction(double x)
        {
            double[] coeff = new double[this.stackPanel.Children.Count / 2];
            int j = 0;
            bool isValid = true;

            for (int i = 0; i < coeff.Length; i++)
            {
                double tmp;
                TextBox tmpTxt = (TextBox)this.stackPanel.FindName("a" + i);
                if (!Double.TryParse(tmpTxt.Text, out tmp))
                {
                    isValid = false;
                    break;
                }
            }
            if (isValid)
            {
                for (int i = 0; i < coeff.Length; i++)
                {
                    TextBox tmpTxt = (TextBox)this.stackPanel.FindName("a" + i);
                    coeff[i] = Double.Parse(tmpTxt.Text);
                    j++;
                }
                //Func<double, double> func = CreatePolynomialFunction(coeff);
                //return func.Invoke(x);
                var coeffTmp = coeff.Reverse().ToList() ;
                double result = coeffTmp[0];

                for (int i = 1; i < coeffTmp.Count; i++)
                    result = result * x + coeffTmp[i];

                return result;
            }
            else
            {
                MessageBox.Show("Niepoprawne współczynniki");
                return 0.0;
            }
        }

        public double derivedPolynomial(double x)
        {

            double[] coeff = new double[(this.stackPanel.Children.Count / 2) - 1];
            int j = 0;
            bool isValid = true;

            for (int i = 0; i < coeff.Length; i++)
            {
                double tmp;
                TextBox tmpTxt = (TextBox)this.stackPanel.FindName("a" + i);
                if (!Double.TryParse(tmpTxt.Text, out tmp))
                {
                    isValid = false;
                    break;
                }
            }
            if (isValid)
            {
                if (coeff.Length > 1)
                {
                    for (int i = 1; i <= coeff.Length; i++)
                    {
                        TextBox tmpTxt = (TextBox)this.stackPanel.FindName("a" + i);
                        coeff[j] = Double.Parse(tmpTxt.Text) * i;
                        j++;
                    }
                    //Func<double, double> func = CreatePolynomialFunction(coeff);
                    //return func.Invoke(x);
                    var coeffTmp = coeff.Reverse().ToList();
                    double result = coeffTmp[0];

                    for (int i = 1; i < coeffTmp.Count; i++)
                        result = result * x + coeffTmp[i];

                    return result;
                }
                else
                    return 0.0;
            }
            else
            {
                MessageBox.Show("Niepoprawne współczynniki");
                return 0.0;
            }
        }

        public double secondaryDerivatePolynomial(double x)
        {
            double[] coeff = new double[(this.stackPanel.Children.Count / 2) - 1];
            int j = 0;
            bool isValid = true;

            for (int i = 0; i < coeff.Length; i++)
            {
                double tmp;
                TextBox tmpTxt = (TextBox)this.stackPanel.FindName("a" + i);
                if (!Double.TryParse(tmpTxt.Text, out tmp))
                {
                    isValid = false;
                    break;
                }
            }
            if (isValid)
            {
                if (coeff.Length > 2)
                {
                    for (int i = 1; i <= coeff.Length; i++)
                    {
                        TextBox tmpTxt = (TextBox)this.stackPanel.FindName("a" + i);
                        coeff[j] = Double.Parse(tmpTxt.Text) * i;
                        j++;
                    }
                    j = 0;
                    double[] coeffSecDer = new double[coeff.Length - 1];
                    for (int i = 1; i <= coeffSecDer.Length; i++)
                    {
                        coeffSecDer[j] = coeff[i] * i;
                        j++;
                    }
                    //Func<double, double> func = CreatePolynomialFunction(coeffSecDer);
                    //return func.Invoke(x);
                    var coeffTmp = coeffSecDer.Reverse().ToList();
                    double result = coeffTmp[0];

                    for (int i = 1; i < coeffTmp.Count; i++)
                        result = result * x + coeffTmp[i];

                    return result;
                }
                else
                {
                    return 0.0;
                }
            }
            else
            {
                MessageBox.Show("Niepoprawne współczynniki");
                return 0.0;
            }
        }

        public static Func<double, double> CreatePolynomialFunction(params double[] coeff)
        {
            if (coeff == null) throw new ArgumentNullException("coeff");
            return x =>
            {
                double sum = 0.0;
                double xPower = 1;
                for (int power = 0; power < coeff.Length; power += 1)
                {
                    sum += xPower * coeff[power];
                    xPower *= x;
                }
                return sum;
            };
        }
    }
}
