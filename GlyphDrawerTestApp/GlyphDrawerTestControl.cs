using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows;
using CustomControlBaseLib;
using System.Diagnostics;
using System.Globalization;

namespace GlyphDrawerTestApp {


  public class GlyphDrawerTestControl: CustomControlBase {

    #region Constructor
    //      -----------

    readonly MainWindow mainWindow;
    readonly GlyphDrawer glyphDrawer;



    /// <summary>
    /// Default constructor
    /// </summary>
    public GlyphDrawerTestControl(MainWindow mainWindow) {
      this.mainWindow = mainWindow;
      glyphDrawer = new GlyphDrawer(FontFamily, FontStyle, FontWeight, FontStretch, VisualTreeHelper.GetDpi(this).PixelsPerDip);

      Background = Brushes.WhiteSmoke;
      BorderBrush = Brushes.Gray;
    }
    #endregion


    #region Overrides
    //      ---------

    protected override Size MeasureContentOverride(Size constraint) {
      //cannot return constraint, which might be ∞, which causes an exception.
      return new Size(0, 0);
    }


    protected override Size ArrangeContentOverride(Rect arrangeRect) {
      //use all the space offered
      return arrangeRect.Size;
    }

    
    protected override void OnRenderContent(System.Windows.Media.DrawingContext drawingContext, Size renderContentSize) {
      ///////////////////////////////////////////////////////////////////////////////////////////////////////////
      ////speed tests
      //var stopwatch = new Stopwatch();
      //stopwatch.Restart();
      //stopwatch.Stop();
      //var empty1Time = stopwatch.Elapsed;

      //stopwatch.Restart();
      //stopwatch.Stop();
      //var empty2Time = stopwatch.Elapsed;

      //var dpi = VisualTreeHelper.GetDpi(this).PixelsPerDip;
      //stopwatch.Restart();
      //var glyphDrawerNormal = new GlyphDrawer(FontFamily, FontStyle, FontWeight, FontStretch, dpi);
      //stopwatch.Stop();
      //var createGlyphDrawerNormalTime = stopwatch.Elapsed;

      //stopwatch.Restart();
      //var glyphDrawerBold = new GlyphDrawer(FontFamily, FontStyle, FontWeights.Bold, FontStretch, VisualTreeHelper.GetDpi(this).PixelsPerDip);
      //stopwatch.Stop();
      //var createGlyphDrawerBoldTime = stopwatch.Elapsed;

      //var point = new Point(0, 0);
      //stopwatch.Restart();
      //point = glyphDrawerNormal.Write(drawingContext, point, "some text", 12, Brushes.Black, false);
      //stopwatch.Stop();
      //var write1Time = stopwatch.Elapsed;

      //stopwatch.Restart();
      //point = glyphDrawerNormal.Write(drawingContext, point, "some text", 12, Brushes.Black, false);
      //stopwatch.Stop();
      //var write2Time = stopwatch.Elapsed;

      //stopwatch.Restart();
      //point = glyphDrawerNormal.Write(drawingContext, point, "some text", 12, Brushes.Black, false);
      //point = glyphDrawerNormal.Write(drawingContext, point, "other text", 12, Brushes.Black, false);
      //stopwatch.Stop();
      //var write3Time = stopwatch.Elapsed;

      //stopwatch.Restart();
      //point = glyphDrawerNormal.Write(drawingContext, point, "some text", 12, Brushes.Black, false);
      //point = glyphDrawerNormal.Write(drawingContext, point, "other text", 12, Brushes.Black, false);
      //point = glyphDrawerNormal.Write(drawingContext, point, "s12345678", 12, Brushes.Black, false);
      //point = glyphDrawerNormal.Write(drawingContext, point, "asdfghjkl", 12, Brushes.Black, false);
      //stopwatch.Stop();
      //var write4Time = stopwatch.Elapsed;

      //stopwatch.Restart();
      //var formattedText = new FormattedText("some text", CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight,
      //  new Typeface("Verdana"), 12, Brushes.Black, dpi);
      //drawingContext.DrawText(formattedText, point);
      //stopwatch.Stop();
      //var drawText1Time = stopwatch.Elapsed;

      //stopwatch.Restart();
      //formattedText = new FormattedText("some text", CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight,
      //  new Typeface("Verdana"), 12, Brushes.Black, dpi);
      //drawingContext.DrawText(formattedText, point);
      //stopwatch.Stop();
      //var drawText2Time = stopwatch.Elapsed;

      ///////////////////////////////////////////////////////////////////////////////////////////////////////////

      double offsetX = renderContentSize.Width/2;
      double offsetY = renderContentSize.Height/2;

      var coordinatePen1 = new Pen(Brushes.Blue, 1);
      drawingContext.DrawLine(coordinatePen1, new Point(offsetX, 0), new Point(offsetX, renderContentSize.Height));
      drawingContext.DrawLine(coordinatePen1, new Point(0, offsetY), new Point(renderContentSize.Width, offsetY));
      var nextPoint =  glyphDrawer.Write(drawingContext, new Point(offsetX, offsetY), mainWindow.Text1, 
        mainWindow.TestFontSize1, Foreground, mainWindow.IsRightAligned1, mainWindow.IsSideways1, mainWindow.Angle1);
      var nextPen1 = new Pen(Brushes.DarkBlue, 1);
      drawingContext.DrawLine(nextPen1, new Point(nextPoint.X-FontSize, nextPoint.Y), new Point(nextPoint.X+FontSize, nextPoint.Y));
      drawingContext.DrawLine(nextPen1, new Point(nextPoint.X, nextPoint.Y-FontSize), new Point(nextPoint.X, nextPoint.Y+FontSize));
      mainWindow.NextPoint1 = new Point(nextPoint.X - offsetX, nextPoint.Y - offsetY);

      if (!mainWindow.IsShowSecondString) return;

      offsetX = nextPoint.X;
      offsetY = nextPoint.Y;

      var coordinatePen2 = new Pen(Brushes.Green, 1);
      drawingContext.DrawLine(coordinatePen2, new Point(offsetX, 0), new Point(offsetX, renderContentSize.Height));
      drawingContext.DrawLine(coordinatePen2, new Point(0, offsetY), new Point(renderContentSize.Width, offsetY));
      nextPoint = glyphDrawer.Write(drawingContext, nextPoint, mainWindow.Text2, mainWindow.TestFontSize2, Foreground, 
        mainWindow.IsRightAligned2, mainWindow.IsSideways2, mainWindow.Angle2);
      var nextPen2 = new Pen(Brushes.DarkGreen, 1);
      drawingContext.DrawLine(nextPen2, new Point(nextPoint.X-FontSize, nextPoint.Y), new Point(nextPoint.X+FontSize, nextPoint.Y));
      drawingContext.DrawLine(nextPen2, new Point(nextPoint.X, nextPoint.Y-FontSize), new Point(nextPoint.X, nextPoint.Y+FontSize));
      mainWindow.NextPoint2 = new Point(nextPoint.X - offsetX, nextPoint.Y - offsetY);
    }
    #endregion
  }
}
