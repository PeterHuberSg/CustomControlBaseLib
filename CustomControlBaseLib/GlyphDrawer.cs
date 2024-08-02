/**************************************************************************************

CustomControlBaseLib.GlyphDrawer
================================

Writes text to a DrawingContext. Can also be used to calculate the length of text.

Written 2014 - 2024 by Jürgpeter Huber by Jürgpeter Huber 
Contact: PeterCode at Peterbox dot com

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see COPYING.txt file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Media;


namespace CustomControlBaseLib {


  /// <summary>
  /// Draws glyphs to a DrawingContext. From the font information in the constructor, GlyphDrawer creates and stores 
  /// the GlyphTypeface, which is used every time for the drawing of the string. Can also be used to calculate the
  /// length of text.
  /// </summary>
  public class GlyphDrawer {

    readonly Typeface typeface;


    /// <summary>
    /// Contains the measurement information of one particular font and the specified font properties
    /// </summary>
    public GlyphTypeface GlyphTypeface {
      get { return glyphTypeface; }
    }
    readonly GlyphTypeface glyphTypeface;


    /// <summary>
    /// Screen resolution. 
    /// </summary>
    public float PixelsPerDip { get; }


    /// <summary>
    /// Construct a GlyphTypeface with the specified font properties
    /// </summary>
    public GlyphDrawer(FontFamily fontFamily, FontStyle fontStyle, FontWeight fontWeight,
      FontStretch fontStretch, double pixelsPerDip) 
    {
      typeface = new Typeface(fontFamily, fontStyle, fontWeight, fontStretch);
      if (!typeface.TryGetGlyphTypeface(out glyphTypeface))
        throw new InvalidOperationException("No GlyphTypeface found");
      PixelsPerDip = (float)pixelsPerDip;
    }


    /// <summary>
    /// Writes a string to a DrawingContext, using the GlyphTypeface stored in the GlyphDrawer. Returns the coordinate 
    /// where next character should be placed.
    /// </summary>
    /// <param name="drawingContext"></param>
    /// <param name="origin">if isRightAligned then left bottom pixel else right bottom pixel</param>
    /// <param name="text"></param>
    /// <param name="size">same unit like FontSize: (em)</param>
    /// <param name="brush"></param>
    /// <param name="isRightAligned"></param>
    /// <param name="isSideways">Turns each character by 90 degrees</param>
    /// <param name="angle">Rotates clockwise in degrees</param>
    /// <returns>position of top left pixel immediately after last character drawn</returns>
    public Point Write(DrawingContext drawingContext, Point origin, string text, double size, Brush brush, 
      bool isRightAligned = false, bool isSideways = false, double angle = 0) 
    {
      if (string.IsNullOrEmpty(text)) return origin;

      //translate string to glyphs
      double totalWidth = 0;
      ushort[] glyphIndexes = new ushort[text.Length];
      double[] advanceWidths = new double[text.Length];

      var glyphIndexesIndex = 0;
      for (int charIndex = 0; charIndex<text.Length; charIndex++) {
        var codePoint = (int)text[charIndex];
        //https://docs.microsoft.com/en-us/dotnet/standard/base-types/character-encoding-introduction#surrogate-pairs
        if (codePoint<0xd800) {
          // codePoint consists of only 1 integer, nothing to do
        } else if (codePoint<0xdc00) {
          //high surrogate code point
          if (charIndex>=text.Length) {
            //low surrogate code point missing
            System.Diagnostics.Debugger.Break();
            codePoint = (int)'?';
          } else {
            var lowCodPoint = (int)text[++charIndex];
            if (lowCodPoint<0xdc00 || lowCodPoint>=0xe000) {
              //illegal second surrogate code point
              System.Diagnostics.Debugger.Break();
              codePoint = (int)'?';
            } else {
              codePoint = 0x10000 + ((codePoint - 0xD800) *0x0400) + (lowCodPoint - 0xDC00);
            }
          }
        } else if (codePoint<0xe000) {
          //illegal low surrogate code point, high should come first
          System.Diagnostics.Debugger.Break();
          codePoint = (int)'?';
        } else {
          // codePoint consists of only 1 integer, nothing to do
        }

        if (!glyphTypeface.CharacterToGlyphMap.TryGetValue(codePoint, out var glyphIndex)) {
          glyphIndex = glyphTypeface.CharacterToGlyphMap[(int)'?'];
        };
        glyphIndexes[glyphIndexesIndex] = glyphIndex;
        double width;
        if (isSideways) {
          width = glyphTypeface.AdvanceHeights[glyphIndex] * size;
        } else {
          width = glyphTypeface.AdvanceWidths[glyphIndex] * size;
        }
        advanceWidths[glyphIndexesIndex++] = width;
        totalWidth += width;
      }

      //shorten glyphIndexes if there were 2 characters resulting in just 1 glyph
      if (glyphIndexes.Length!=glyphIndexesIndex) {
        glyphIndexes = glyphIndexes[0..glyphIndexesIndex];
        advanceWidths = advanceWidths[0..glyphIndexesIndex];
      }

      //paint glyphs
      var originAligned = isRightAligned ? new Point(origin.X - totalWidth, origin.Y) : origin;
      var glyphRun = new GlyphRun(glyphTypeface, 0, isSideways: isSideways, size, PixelsPerDip, glyphIndexes, 
        originAligned, advanceWidths, null, null, null, null, null, null);

      if (angle==0) {
        drawingContext.DrawGlyphRun(brush, glyphRun);
      } else {
        //rotation needed around origin, regardless of alignment
        drawingContext.PushTransform(new RotateTransform(angle, origin.X, origin.Y));
        drawingContext.DrawGlyphRun(brush, glyphRun);
        drawingContext.Pop();
      }
      
      //calculate where the next glyph should be painted after this string
      if (isRightAligned) {
        totalWidth = -totalWidth;
      }
      if (angle==0) {
        return new Point(origin.X + totalWidth, origin.Y);
      } else {
        var angleRad = angle * Math.PI / 180;
        return new Point(origin.X + totalWidth * Math.Cos(angleRad), origin.Y  + totalWidth * Math.Sin(angleRad));
      }
    }


    /// <summary>
    /// Returns the length of the text using the GlyphTypeface stored in the GlyphDrawer. 
    /// </summary>
    /// <param name="text"></param>
    /// <param name="size">same unit like FontSize: (em)</param>
    public double GetLength(string text, double size) {
      double length = 0;

      for (int charIndex = 0; charIndex<text.Length; charIndex++) {
        ushort glyphIndex = glyphTypeface.CharacterToGlyphMap[text[charIndex]];
        double width = glyphTypeface.AdvanceWidths[glyphIndex] * size;
        length += width;
      }
      return length;
    }


    /// <summary>
    /// calculates the width of each string in strings and returns the longest length.
    /// </summary>
    public double GetMaxLength(IEnumerable<string> strings, double size) {
      var maxLength = 0.0;
      foreach (var text in strings) {
        double length = 0;
        for (int charIndex = 0; charIndex<text.Length; charIndex++) {
          ushort glyphIndex = glyphTypeface.CharacterToGlyphMap[text[charIndex]];
          double width = glyphTypeface.AdvanceWidths[glyphIndex] * size;
          length += width;
        }
        maxLength = Math.Max(maxLength, length);
      }
      return maxLength;
    }


    /// <summary>
    /// Returns width and height of text
    /// </summary>
    public Size MeasureString(string text, double size) {
      var formattedText = new FormattedText(text, CultureInfo.CurrentUICulture,
                                              FlowDirection.LeftToRight,
                                              typeface,
                                              size,
                                              Brushes.Black,
                                              PixelsPerDip);

      return new Size(formattedText.Width, formattedText.Height);
    }
  }
}