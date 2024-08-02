using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using CustomControlBaseLib;
using static System.Net.Mime.MediaTypeNames;


namespace GlyphDrawerTestApp {


  /// <summary>
  /// WPF app to test GlyphDrawer visually
  /// </summary>
  public partial class MainWindow: Window {

    public string Text1 { get; private set; }
    public double TestFontSize1 { get; private set; }
    public bool IsRightAligned1 { get; private set; }
    public bool IsSideways1 { get; private set; }
    public double Angle1 { get; private set; }
    public Point NextPoint1 {
      get { return nextPoint1; }
      set { 
        nextPoint1 = value; 
        NextPointTextBox1.Text = $"X:{value.X:F2}, Y:{value.Y:F2}";
      }
    }
    private Point nextPoint1;

    public bool IsShowSecondString { get; private set; }
    public string Text2 { get; private set; }
    public double TestFontSize2 { get; private set; }
    public bool IsRightAligned2 { get; private set; }
    public bool IsSideways2 { get; private set; }
    public double Angle2 { get; private set; }
    public Point NextPoint2 {
      get { return nextPoint2; }
      set {
        nextPoint2 = value;
        NextPointTextBox2.Text = $"X:{value.X:F2}, Y:{value.Y:F2}";
      }
    }
    private Point nextPoint2;

    GlyphDrawerTestControl glyphDrawerTestControl;


    public MainWindow() {
      InitializeComponent();

      glyphDrawerTestControl = new GlyphDrawerTestControl(this);
      Grid.SetRow(glyphDrawerTestControl, 0);
      Grid.SetColumn(glyphDrawerTestControl, 3);
      Grid.SetRowSpan(glyphDrawerTestControl, 15);
      MainGrid.Children.Add(glyphDrawerTestControl);

      TextTextBox1.TextChanged += TextTextBox1_TextChanged;
      FontSizeTextBox1.TextChanged += FontSizeTextBox1_TextChanged;
      IsRightAlignedCheckBox1.Click += IsRightAlignedCheckBox1_Click;
      IsSidewaysCheckBox1.Click += IsSidewaysCheckBox1_Click;
      AngleTextBox1.TextChanged += AngleTextBox1_TextChanged;

      IsShowsecondStringCheckBox.Click += IsShowSecondStringCheckBox_Click;
      TextTextBox2.TextChanged += TextTextBox2_TextChanged;
      FontSizeTextBox2.TextChanged += FontSizeTextBox2_TextChanged;
      IsRightAlignedCheckBox2.Click += IsRightAlignedCheckBox2_Click;
      IsSidewaysCheckBox2.Click += IsSidewaysCheckBox2_Click;
      AngleTextBox2.TextChanged += AngleTextBox2_TextChanged;

      TextTextBox1.Text = "Test String";
      FontSizeTextBox1.Text = "12";
      AngleTextBox1.Text = "0";
      TextTextBox2.Text = " Another String";
      FontSizeTextBox2.Text = "15";
      AngleTextBox2.Text = "15";
    }


    private void TextTextBox1_TextChanged(object sender, TextChangedEventArgs e) {
      Text1 = TextTextBox1.Text;
      glyphDrawerTestControl.InvalidateVisual();
    }


    private void TextTextBox2_TextChanged(object sender, TextChangedEventArgs e) {
      Text2 = TextTextBox2.Text;
      glyphDrawerTestControl.InvalidateVisual();
    }


    private void FontSizeTextBox1_TextChanged(object sender, TextChangedEventArgs e) {
      if (!double.TryParse(FontSizeTextBox1.Text, out double fontSize)) {
        fontSize = FontSize;
      }
      TestFontSize1 = fontSize;
      glyphDrawerTestControl.InvalidateVisual();
    }


    private void FontSizeTextBox2_TextChanged(object sender, TextChangedEventArgs e) {
      if (!double.TryParse(FontSizeTextBox2.Text, out double fontSize)) {
        fontSize = FontSize;
      }
      TestFontSize2 = fontSize;
      glyphDrawerTestControl.InvalidateVisual();
    }


    private void IsRightAlignedCheckBox1_Click(object sender, RoutedEventArgs e) {
      IsRightAligned1 = IsRightAlignedCheckBox1.IsChecked!.Value;
      glyphDrawerTestControl.InvalidateVisual();
    }


    private void IsRightAlignedCheckBox2_Click(object sender, RoutedEventArgs e) {
      IsRightAligned2 = IsRightAlignedCheckBox2.IsChecked!.Value;
      glyphDrawerTestControl.InvalidateVisual();
    }


    private void IsSidewaysCheckBox1_Click(object sender, RoutedEventArgs e) {
      IsSideways1 = IsSidewaysCheckBox1.IsChecked!.Value;
      glyphDrawerTestControl.InvalidateVisual();
    }


    private void IsSidewaysCheckBox2_Click(object sender, RoutedEventArgs e) {
      IsSideways2 = IsSidewaysCheckBox2.IsChecked!.Value;
      glyphDrawerTestControl.InvalidateVisual();
    }


    private void AngleTextBox1_TextChanged(object sender, TextChangedEventArgs e) {
      if (!double.TryParse(AngleTextBox1.Text, out double angle)) {
        angle = 0;
      }
      Angle1 = angle;
      glyphDrawerTestControl.InvalidateVisual();
    }


    private void AngleTextBox2_TextChanged(object sender, TextChangedEventArgs e) {
      if (!double.TryParse(AngleTextBox2.Text, out double angle)) {
        angle = 0;
      }
      Angle2 = angle;
      glyphDrawerTestControl.InvalidateVisual();
    }


    private void IsShowSecondStringCheckBox_Click(object sender, RoutedEventArgs e) {
      IsShowSecondString = IsShowsecondStringCheckBox.IsChecked!.Value;
      glyphDrawerTestControl.InvalidateVisual();
    }
  }
}