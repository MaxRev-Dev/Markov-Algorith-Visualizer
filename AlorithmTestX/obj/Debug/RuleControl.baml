   M S B A M L   `   `   `  ����T  PMarcovAlgorithmVisualizer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:MarcovAlgorithmVisualizerMarcovAlgorithmVisualizer  5      +    %MarcovAlgorithmVisualizer.RuleControl   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      9local'clr-namespace:MarcovAlgorithmVisualizer     5      -   5          >�Name$  userControl��5      $��Stretch=�5   8    � 	Resources 5	          BooleanToVisibilityConverter%   MarcovAlgorithmVisualizer.NotEx 	 NotEx  LabelStyle1 �� 
TargetType �� Property �� Value �� 
TargetType �� Padding J� SnapsToDevicePixels �� Triggers	 P� Property
 P� Value��%P  &	        &	      &	 &     �� 5
   
   6'     5   
   6   �� 5   
   " ��6$   .�5      ��  � ��# � ��6+   �� 5       � ��
 ���� 6+   �� 5       � ��$ 5q�6(   �� 5       � ��$
 Left=�6;   �� 5       � ��$	 Top=�69   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� #��z��6!   #��z��6M   #��z��6�   # z��6�    . ._�5      �� #��z��6/   ��.  # z 6�   #��z��6�   5       5      P� 5      	 � {�
 .  . �5   "   �� # � ��6?    � ��5      5      5      5       5!      .��5"      �  � DataContext 6   ��  � ColumnDefinitions 5#   
   �� 5$      $��5* �6   �� 5%      $��2* �6   �� 5&      $��2* �6   �� 5'      $��2* �6   .+�5)   
   �� -   5         ��Name$	 lab��5+      $��20(�5)      $
��Left=�6w   $��Center=�5*      $��10q�6,   #���  5,   �   ��5)      �� *
ContentX+ �� ElementName$ userControl�� �� UpdateSourceTrigger$ PropertyChanged=�y�5,      �� *
EditMode+ �� 	Converter# [  $ userControl�� �� Mode$ OneWay=�6�   �� 5-   
   -   5         ��Name$ 
stackPanel��5-       �� Orientation$ 
Horizontal=�6}   y�6)   �� *	IsVisible+# [ $	 lab��.+�5.      �� -   5         ��Name$ AV��5.      $��Center=�6M   ��6"   �� *A+6M   �� 5/      $�� --> ��6   $��10q�6$   $��Center=�60   �� 50      -   5        $ BV��50      $��Center=�6N   ��6"   �� *B+6N   51      �� 52   
   -   5        $��Up��52      $��Center=�6   $��Center=�53      $��40��6,   $��1��69   $��5q�6I   6T   �� 54   
   -   5        $
��Down��54      $��Center=�6    $��Center=�55      $��60��6,   $��2��67   $��5q�6G   6S   �� 56   
   -   5        $	��Del��56      $��Center=�6   $��Center=�57      $��40��6,   $��3��67   $��5q�6G   6S   58      59      