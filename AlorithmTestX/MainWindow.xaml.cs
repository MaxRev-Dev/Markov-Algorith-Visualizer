using MarcovAlgorithmVisualizer.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace MarcovAlgorithmVisualizer
{
    public class App : Application
    {
        [STAThread]
        static void Main()
        {
            new Application();
            Application.Current.Run(new MainWindow());
        }
    }
    [Serializable]
    public class Rule
    {
        public Rule() { NullW = RuleControl.Null; }
        public string A { get; set; }
        public string B { get; set; }
        private string NullW;
        public void ReInit()
        {
            A = A.Replace(NullW, RuleControl.Null);
            B = B.Replace(NullW, RuleControl.Null);
            NullW = RuleControl.Null;
        }
        public override string ToString()
        {
            return $"{A} => {B}";
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public static bool operator ==(Rule l, Rule r)
        {
            return l != r;
        }
        public static bool operator !=(Rule l, Rule r)
        {
            if (l is null) return true;
            return l.Equals(r);
        }
        public override bool Equals(object obj)
        {
            Rule r = obj as Rule;
            if (r is null) return false;
            return this.A == (r.A) && this.B == r.B;
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            nullWord.Text = RuleControl.Null = "_";
            rlist = new ObservableCollection<RuleControl>();

            if (!string.IsNullOrEmpty(Settings.Default.list))
            {
                var rules = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Rule>>(Settings.Default.list);
                if (rules.Count == 1 && rules.First() == null)
                {
                    Settings.Default.Reset();
                    rlist = new ObservableCollection<RuleControl>();
                }
                else
                {
                    rlist = new ObservableCollection<RuleControl>(rules.Select(x => CreateRuleControl(x)));
                }
            }

            list.ItemsSource = rlist;
            wordEx.TextChanged += (o, e) => ReCalculate();
            list.MouseLeave += List_LostFocus;
            rlist.CollectionChanged += (o, e) =>
            {
                lock (obj) {
                    if (!worker.IsBusy)
                    ReCalculate();
                }
            };
        }
        RuleControl CreateRuleControl(object datacontext)
        {
            RuleControl control = new RuleControl()
            {
                DataContext = datacontext
            };
            control.MoveUpEvent += Rule_MoveUpEvent;
            control.MoveDownEvent += Rule_MoveDownEvent;
            control.RuleValueChangedEvent += Rule_RuleValueChangedEvent;
            control.DelClick += (s, e) => rlist.Remove(control);
            return control;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Settings.Default.list = Newtonsoft.Json.JsonConvert.SerializeObject(rlist.Select(x => x.DataContext as Rule).ToArray());
            Settings.Default.Save();
            base.OnClosing(e);
        }
        object obj = new object();
        private void List_LostFocus(object sender, RoutedEventArgs e) => list.SelectedIndex = -1;

        // CancellationTokenSource tokenSource = new CancellationTokenSource();
        BackgroundWorker worker;
        private void ReCalculate()
        {
            if (worker == null) worker = new BackgroundWorker();
            while (worker.IsBusy) worker.CancelAsync();
            worker.WorkerSupportsCancellation = true;
            worker.DoWork -= Worker_DoWork;
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerAsync();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            InitWork(e);
        }

        private void InitWork(DoWorkEventArgs dwe)
        {
            var word = Dispatcher.Invoke(() => wordEx.Text);
            if (rlist.Count == 0 || !canUpdate || string.IsNullOrWhiteSpace(word))
                return;
            var rules =
            Dispatcher.Invoke(() =>
            {
                resBox.Document.Blocks.Clear();
                bar.Visibility = Visibility.Visible;
                resBox.AppendText(wordEx.Text);
                return list.Items.Cast<RuleControl>().Select(x => x.DataContext as Rule).ToArray();
            });
            if (list.Items.Count == 0 || rules.Count() == 0) return;

            //run
            Dispatcher.Invoke(() =>
            {
                resBox.Document.Blocks.Clear();
                bar.Visibility = Visibility.Visible;
                resBox.AppendText(wordEx.Text);
            });
            var curRule = rules.First();
            Rule GetNext(Rule current)
            {
                var ind = rules.ToList().IndexOf(current) + 1;
                if (ind < rules.Length) return rules[ind];
                return null;
            }
            string nullW = Dispatcher.Invoke(() => nullWord.Text);
            string result = "";

            System.Timers.Timer timer = new System.Timers.Timer();

            timer.Elapsed += (s, e) => Dispatcher.Invoke(() =>
                {
                    Dispatcher.Invoke(() =>
                    btnCancel.Visibility = Visibility.Visible);
                });
            timer.AutoReset = false;
            timer.Interval = 5000;
            timer.Start();
            btnCancel.Click += (s, e) =>
            {
                worker.CancelAsync();
                Dispatcher.Invoke(() =>
                {
                    bar.Visibility =
                    btnCancel.Visibility = Visibility.Hidden;
                });
            };

            int len = word.Length, replacer = 0, lx = 0;

            bool infiniteFlag = false;
            List<string> listerInf = new List<string>();
            bool Pass()
            {
                ++replacer;
                result += " -> " + word.Replace(nullW, "");
                if (curRule.B.Contains("."))
                    return true;
                curRule = rules.First();
                return false;
            }
            bool InfiniteHelper()
            {
                listerInf.Add(word);
                if (listerInf.Count <= len * 2) return false;
                try
                {
                   //var unq = listerInf.Skip(listerInf.Count - 5).Distinct().Count();
                    if (  result.Length / word.Length > rules.Count() * word.Length )
                    {
                        infiniteFlag = true;
                        return true;
                    }
                }
                catch { return false; }
                return false;
            }
            int counter = 0;
            System.Diagnostics.Stopwatch sp = new System.Diagnostics.Stopwatch();
            sp.Start();
            try
            {

                Dispatcher.Invoke(() =>
                {
                    btnCancel.IsEnabled = true;
                    word = nullWord.Text + word + nullWord.Text;
                });
                void Update()
                {
                    var u = (!result.Contains("->") ? result : result.Substring(result.LastIndexOf("->") + 2)).Trim();
                    f.Text =
                        string.Join(" ", new string[]{
                        $"Wl:{wordEx.Text.Length}",
                        $"Iter: {(counter).ToString()}",
                        $"R: {replacer}",
                        $"Ck: {lx}",
                        $"T: {sp.Elapsed.Minutes}m {sp.Elapsed.Seconds}s {sp.Elapsed.Milliseconds}ms",
                        $"ResLen: {u.Length}",
                        $"\nRes: {u}"
                        });

                }
                while (true)
                {
                    //if (((CancellationToken)token).IsCancellationRequested) return;
                    if (worker.CancellationPending)
                        break;
                    ++counter;
                    if (!word.EndsWith(nullW)) word += nullW;
                    ++lx;
                    if (!word.StartsWith(nullW)) word = nullW + word;
                    ++lx;
                    if (curRule == null || InfiniteHelper()) break;
                    ++lx;
                    if (curRule.A == null || curRule.A == nullW)
                    {
                        word = nullW + curRule.B + word.Substring(1);
                        Pass();
                    }
                    ++lx;
                    if (curRule.A != null && curRule.B == null)
                    {
                        word = word.Replace(curRule.A, "");
                        Pass();
                        break;
                    }
                    ++lx;
                    if (curRule.A != null && word.Contains(curRule.A))
                    {
                        void Replace()
                        {
                            ++replacer;
                            var beg = word.Substring(0, word.IndexOf(curRule.A));
                            var mid = curRule.B.Replace(".", "");
                            var end = word.Substring(word.IndexOf(curRule.A) + curRule.A.Length);
                            word = beg + mid + end;
                        }
                        Replace();
                        if (Pass()) break;
                    }
                    else
                        curRule = GetNext(curRule);
                    if (lx % 1000 == 0)
                    {
                        if (!dwe.Cancel)
                            Dispatcher.Invoke(new Action(() =>
                            {
                                Update();
                            }));
                    }
                }
                Dispatcher.Invoke(new Action(() =>
                {
                    bar.Visibility = Visibility.Collapsed;
                    timer.Close();
                    btnCancel.IsEnabled = false;
                    Update();
                    sp.Stop();
                    var u = result.Length < 10000 ? result : (result.Substring(0, result.IndexOf("->")) +
                    $" >>> ... to long result length {result.Length} ... <<<" +
                    result.Substring(result.LastIndexOf("->") + 2));
                    resBox.AppendText(u);
                    if (infiniteFlag) InfiniteMessage();
                }));

            }
            catch (OutOfMemoryException) { }

        }
        void InfiniteMessage()
        {
            string messageBoxText = "Алгоритм зациклився. Вимкніть Live-режим поки редагуєте правила";
            string caption = "Завдання не має розв'язку";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBox.Show(messageBoxText, caption, button, icon);
        }
        void CantAddMessage()
        {
            string messageBoxText = "Таке правило вже існує";
            string caption = "Помилка";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBox.Show(messageBoxText, caption, button, icon);
        }
        ObservableCollection<RuleControl> rlist;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool TryAdd(Rule adorner)
            {
                var f = rlist.Select(x => (x as RuleControl).DataContext as Rule);
                var g = f
                    .Where(x => x.A == adorner.A && x.B == adorner.B);
                if (g.Count() == 0)
                {
                    rlist.Add(CreateRuleControl(adorner)); return true;
                }
                CantAddMessage();
                return false;
            }

            if (!string.IsNullOrEmpty(val1.Text) && !string.IsNullOrEmpty(val2.Text))
            {
                TryAdd(new Rule() { A = val1.Text, B = val2.Text });
            }
            else if (!string.IsNullOrEmpty(val2.Text))
            {
                TryAdd(new Rule() { A = null, B = val2.Text });
            }
            else if (!string.IsNullOrEmpty(val1.Text))
            {
                TryAdd(new Rule() { A = val1.Text, B = null });
            }
        }

        private void Rule_RuleValueChangedEvent(object sender, EventArgs e) => ReCalculate();

        private void Rule_MoveDownEvent(object sender, EventArgs e)
        {
            if (rlist.Count < 2) return;
            var item = sender as RuleControl;
            var ind = rlist.IndexOf(item);
            rlist.RemoveAt(ind);
            if (ind == rlist.Count)
                rlist.Insert(0, item);
            else
                rlist.Insert(ind + 1, item);
        }

        private void Rule_MoveUpEvent(object sender, EventArgs e)
        {
            if (rlist.Count < 2) return;
            var item = sender as RuleControl;
            var ind = rlist.IndexOf(item);
            rlist.RemoveAt(ind);
            if (ind == 0)
                rlist.Insert(rlist.Count, item);
            else
                rlist.Insert(ind - 1, item);
        }

        private void Alf_KeyUp(object sender, KeyEventArgs e)
        {
            var t = sender as TextBox;
            t.Text = string.Join(",",
                string.Join("", t.Text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                .Distinct().Select(x => x.ToString()).ToArray());
            t.CaretIndex = t.Text.Length;
        }

        private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var i in rlist)
                i.SetValue(RuleControl.EditModeProperty, true);
            var lv = sender as ListView;
            if (lv.SelectedIndex == -1) return;
            (lv.SelectedItem as RuleControl).SetValue(RuleControl.EditModeProperty, false);
            var st = lv.SelectedValue as Rule;
            var l = lv.ItemContainerGenerator.ContainerFromIndex(lv.SelectedIndex) as ListViewItem;
            l.MouseDoubleClick += (c, s) => rlist.Remove(l.Content as RuleControl);
        }
        bool canUpdate = false;
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            canUpdate = true;
            ReCalculate();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e) => canUpdate = false;


        private void NullWord_TextChanged(object sender, TextChangedEventArgs e)
        {
            var t = (sender as TextBox).Text;
            if (string.IsNullOrWhiteSpace(t)) return;
            RuleControl.Null = t;
            if (rlist != null)
                foreach (var i in rlist)
                    i.ReInit();
        }
    }

    public sealed class NotEx : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value == false ? Visibility.Visible : Visibility.Hidden);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
