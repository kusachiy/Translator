   M S B A M L   `   `   `  ����N  JAssemblerTranslator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:AssemblerTranslatorAssemblerTranslator  5   	   $    AssemblerTranslator.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   3local!clr-namespace:AssemblerTranslator     5   	   U QMahApps.Metro, Version=1.5.0.23, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f;controls,http://metro.mahapps.com/winfx/xaml/controls  5   	     � WindowState$  	Maximized=�5
   	    � Title$ Translator (v 0.22)��5   	   $	��600��6%   $	��500��61    � DataContext 5      %   AssemblerTranslator.MainManager  5   
   6    � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $ /Resources/Icons.xaml?�6%   %    5      .��5      �  � RowDefinitions 5   
   �� 5      $��9* �6   6*   �� 5      $��4* �6   6*   .+�5   
   � $��Stretch=�6   $	��0,7q�6-    � ColumnDefinitions 5      �� 5      $��2* �6#   60   �� 5      $��3* �6#   60   .+�5       �� $	��5,0q�6   $��Исходный код��6$   $��0��6:   ��5!      �� 5"      .R�5#      � .+�5$      �� $��Center=�69   $��14(�6T   ��6(   �� 6d   �� 5%       �� Orientation$ 
Horizontal=�6)   $��Right=�6B   .+�5&   "   �� $��Center=�6)   	 � ToolTip$
	 Open��6D   $��5,0,0,0q�6�   ��6S   �� 
 �� RelativeSource
 ��  �� AncestorType$ controls:MetroWindowL� �� Path$! DataContext.FileOpenCommand�.��5'   &   �� $��15��60   $��15��6;   ��6G   �� 
 �� $ ButtonL�$ 
Foreground� J� OpacityMask 5(   *   !� 5)   .      appbar_edit_box !� Visual# [  6:   5+   '   5,   #   �� 5-   "   $��Center=�6)   $
	 Save��6D   $��5,0,0,0q�6�   ��6S   �� 
 �� $ controls:MetroWindowL�$! DataContext.FileSaveCommand�.��5.   &   �� $��15��60   $��15��6;   ��6G   �� 
 �� $ ButtonL�$ 
Foreground� 5/   *   !� 50   .     appbar_disk# [ 6:   52   '   53   #   �� 54   "   $��Center=�6)   $	 Compile��6D   $��5,0,0,0q�6�   ��6V   �� 
 �� $ controls:MetroWindowL�$  DataContext.CompileCommand�.��55   &   �� $��15��60   $��15��6;   ��6G   �� 
 �� $ ButtonL�$ 
Foreground� 56   *   !� 57   .     appbar_arrow_right# [ 6:   59   '   5:   #   5=      5>      5?      .��5A      �� .��5B      �� -   5        $��codebox��5B       �� AcceptsReturn . ��6-   �� $ CodeText� �� UpdateSourceTrigger$ PropertyChanged=�6�   5C      5D      �� 5E      $	��5,0q�6   $��Результат��6$   $��1��67   ��5F      �� 5G      .R�5H      � .+�5I      �� $��Center=�69   $��14(�6T   ��6(   �� 6d   �� 5J      $ 
Horizontal=�6)   $��Right=�6B   .+�5K   "   �� $��Center=�6)   $	 Save to .asm file��6D   $��5,0,0,0q�6]  {�6`   �� 
 �� $ controls:MetroWindowL�$# DataContext.EnabledASMButtons���6�   �� 
 �� $ controls:MetroWindowL�$* $DataContext.SaveAssemblerCodeCommand�.��5L   &   �� $��15��60   $��15��6;   ��6G   �� 
 �� $ ButtonL�$ 
Foreground� 5M   *   !� 5N   .     appbar_marvel_ironman# [ 6:   5P   '   5Q   #   �� 5R   "   $��Center=�6)   $	 Show .asm file directory��6D   $��5,0,0,0q�6�   ��6g   �� 
 �� $ controls:MetroWindowL�$' !DataContext.OpenWorkFolderCommand�.��5S   &   �� $��15��60   $��15��6;   ��6G   �� 
 �� $ ButtonL�$ 
Foreground� 5T   *   !� 5U   .     appbar_disk_download# [ 6:   5W   '   5X   #   5Z      5[      5\      .��5^      �� .��5_      �� {�.  ��60   �� *Answer+6J   5`      5a      5b      �� 5c   
   $��1��6   $-��'Состояние компиляции��6    .��5d      �� {�.  ��6   �� *Log+6?   5e      5f      5g      