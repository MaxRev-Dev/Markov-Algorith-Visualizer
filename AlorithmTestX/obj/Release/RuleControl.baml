   M S B A M L   `   `   `  ���� T  PMarcovAlgorithmVisualizer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:MarcovAlgorithmVisualizerMarcovAlgorithmVisualizer  +    %MarcovAlgorithmVisualizer.RuleControl   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     9local'clr-namespace:MarcovAlgorithmVisualizer     -     >�Name$  userControl��$��Stretch=� � 	Resources     BooleanToVisibilityConverter%   MarcovAlgorithmVisualizer.NotEx 	 NotEx  LabelStyle1 �� 
TargetType �� Property �� Value �� 
TargetType �� Padding J� SnapsToDevicePixels �� Triggers	 P� Property
 P� Value��%>  &	        &	      &	 
     ��   �� " ��.���  � ��# � ����  � ��
 ���� ��  � ��$ 5q���  � ��$
 Left=���  � ��$	 Top=���  � �� �� " ��.R��� #��z��#��z��#��z��# z�� . ._��� #��z����.  # z #��z�� P� 	 � {�
 .  . ��� # � �� � ��.���  � DataContext ��  � ColumnDefinitions �� $��5* ��� $��2* ��� $��2* ��� $��2* �.+��� -    ��Name$	 lab��$��20(�$
��Left=�$��Center=�$��10q�#���  ���� *
ContentX+ �� ElementName$ userControl�� �� UpdateSourceTrigger$ PropertyChanged=�y��� *
EditMode+ �� 	Converter# [  $ userControl�� �� Mode$ OneWay=��� -    ��Name$ 
stackPanel�� �� Orientation$ 
Horizontal=�y��� *	IsVisible+# [ $	 lab��.+��� -    ��Name$ AV��$��Center=����� *A+�� $�� --> ��$��10q�$��Center=��� -   $ BV��$��Center=����� *B+�� -   $��Up��$��Center=�$��Center=�$��40��$��1��$��5q��� -   $
��Down��$��Center=�$��Center=�$��60��$��2��$��5q��� -   $	��Del��$��Center=�$��Center=�$��40��$��3��$��5q�