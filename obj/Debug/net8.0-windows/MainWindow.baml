   M S B A M L   `   `   `  ����  DefSort    SortingDemo.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$3  -Демонстрация сортировки��5   	   $	��600��6)   $	��900��66   .��5      �  � ColumnDefinitions 5   
   �� 5      $��2* �6   �� 5      $��3* �6   .+�5   
   �� $��0��6   $��10q�6%   .+�5      �� $)��#Сортировка массива��6   $��20(�62   $
��Bold"�6@   $��0,0,0,20q�6R   �� 5      $(��"Выберите алгоритм:��6   $��0,0,0,10q�62   �� 5      -   5        $��AlgorithmSelector��5      $��0,0,0,20q�60   .@�5      �� $��
SelectSort��6   �� 5      $��
BubbleSort��6   �� 5      $��	QuickSort��6   �� 5      $��HeapSort��6   5      �� 5      $<��6Введите массив (через пробел):��6   $��0,0,0,10q�6>   �� 5      -   5        $��
InputArray��5      $��0,0,0,20q�6(   �� 5      $��Задержка (мс):��6   $��0,0,0,10q�6.   �� 5      -   5        $��DelaySlider��5       � Minimum$	 100Q�6(    � Maximum$
 2000Q�66    � Value$	 500Q�6E    �� TickFrequency$	 100Q�6Q    �� IsSnapToTickEnabled$
 True��6e   $��0,0,0,20q�6�   �� 5      -   5        $��StartButton��5      $'��!Начать сортировку��6(   $��0,0,0,20q�6^   5      �� 5   
   -   5        $"��OpenExternalSortWindowButton��5      $;��5Перейти к внешней сортировке��65   $��50,495,50,32q�6�   �� 5    
   -   5        $!��OpenTextSortingWindowButton��5       $'��!Сортировка текста��64   $��50,418,50,110q�6z   � 5$   
   $��1��6   $��10q�6    � RowDefinitions 5%      �� 5&      $��3* �6    �� 5'      $��2* �6    .+�5+      �� -   5        $��SortingCanvas��5+      
�������$��0��6=   �� 5.      -   5        $��
SortingLog��5.       �� 
IsReadOnly$
 True��6(   	 �� TextWrapping$
	 Wrap=�6:   "
 �� VerticalScrollBarVisibility$

 Auto=�6N   $��1��6q   ���#f4f4f46~   5/      50      51      