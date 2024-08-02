/**************************************************************************************

CustomControlBaseLib.DependencyObjectExtensions
===============================================

Contains the DependencyObject extensions FindVisualChild() and FindVisualChildren()

Written 2014 - 2024 by Jürgpeter Huber by Jürgpeter Huber 
Contact: PeterCode at Peterbox dot com

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see LICENSE file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;


namespace CustomControlBaseLib {


  /// <summary>
  /// Contains the DependencyObject extensions FindVisualChild() and FindVisualChildren()
  /// </summary>
  public static class DependencyObjectExtensions {

    /// <summary>
    /// Returns the children of type TChild in visualTree of parent
    /// </summary>
    public static IEnumerable<TChild> FindVisualChildren<TChild>(this DependencyObject? parent) where TChild : DependencyObject {
      if (parent!=null) {
        for (int i=0; i < VisualTreeHelper.GetChildrenCount(parent); i++) {
          DependencyObject child = VisualTreeHelper.GetChild(parent, i);
          if (child!=null) {
            if (child is TChild) {
              yield return (TChild)child;
            }
            foreach (TChild childOfChild in FindVisualChildren<TChild>(child)) {
              yield return childOfChild;
            }
          }
        }
      }
    }


    /// <summary>
    /// Returns the first child of type TChild in visualTree of parent
    /// </summary>
    public static TChild? FindVisualChild<TChild>(this DependencyObject parent) where TChild : DependencyObject {
      foreach (TChild child in FindVisualChildren<TChild>(parent)) {
        return child;
      }

      return null;
    }
  }
}
