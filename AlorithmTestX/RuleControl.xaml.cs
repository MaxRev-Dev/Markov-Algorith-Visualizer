using System;
using System.Windows;
using System.Windows.Controls;

namespace MarcovAlgorithmVisualizer
{
    /// <summary>
    /// Interaction logic for RuleControl.xaml
    /// </summary>
    public partial class RuleControl : UserControl
    {
        public RuleControl()
        {
            InitializeComponent();
            this.Loaded += (c, e) => SetEx();
        }

        public event EventHandler 
            MoveUpEvent,
            MoveDownEvent,
            RuleValueChangedEvent,
            DelClick;
        protected virtual void OnDelClick(EventArgs e) => DelClick?.Invoke(this, e);
        protected virtual void OnMoveUp(EventArgs args) => MoveUpEvent?.Invoke(this, args);
        protected virtual void OnMoveDown(EventArgs e) => MoveDownEvent?.Invoke(this, e);
        protected virtual void OnRuleChange(EventArgs e)
        {
            if (e != null)
                RuleValueChangedEvent(this, e);
            SetEx();
        }
        private void MoveUp_Click(object sender, RoutedEventArgs e) => OnMoveUp(e);


        public string ContentX
        {
            get { return (string)GetValue(ContentXProperty); }
            set { SetValue(ContentXProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Content.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentXProperty =
            DependencyProperty.Register("ContentX", typeof(string), typeof(RuleControl), new PropertyMetadata(""));



        public void ReInit()
        {
            (DataContext as Rule).ReInit();
            SetEx();
        }
        public bool EditMode
        {
            get { return (bool)GetValue(EditModeProperty); }
            set { SetValue(EditModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EditMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EditModeProperty =
            DependencyProperty.Register("EditMode", typeof(bool), typeof(RuleControl), new PropertyMetadata(true));
        public static string Null { get; set; }
        private void SetEx()
        {
            
            Rule r = DataContext as Rule;
            if (r == null) return;
            var a =r.A ?? RuleControl.Null;
            var b =r.B?? RuleControl.Null;
            SetValue(ContentXProperty, $" {(a ?? "[[start]]")} -> {(b ?? "[[end]]")} ");
            UpdateLayout();
        }


        private void B_LostFocus(object sender, RoutedEventArgs e)
        {
            Rule r = DataContext as Rule;
            r.B = BV.Text;
            r.ReInit();
            OnRuleChange(e);
        }

        private void Del_Click(object sender, RoutedEventArgs e) => OnDelClick(e);

        private void A_LostFocus(object sender, RoutedEventArgs e)
        {
            Rule r = DataContext as Rule;
            r.A = AV.Text;
            r.ReInit();
            OnRuleChange(e);
        }

        private void MoveDown_Click(object sender, RoutedEventArgs e) => OnMoveDown(e);
    }
}
