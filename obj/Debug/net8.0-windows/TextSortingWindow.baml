   M S B A M L   `   `   `  ����  DefSort#    SortingDemo.TextSortingWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$'  !Сортировка текста��5   	   $	��600��6#   $	��900��60   .��5      �  � ColumnDefinitions 5   
   �� 5      $��2* �6   �� 5      $��3* �6   .+�5   
   �� $��0��6   $��10q�6%   .+�5      �� $'��!Сортировка текста��6   $��20(�61   $
��Bold"�6?   $��0,0,0,20q�6Q   �� 5      $=��7Выберите алгоритм сортировки:��6   $��0,0,0,10q�6=   �� 5      -   5        $��SortMethodSelector��5      $��0,0,0,20q�61   .@�5      �� $��	QuickSort��6   �� 5      $��	RadixSort��6   5      �� 5      $-��'Выберите файл текста:��6   $��0,0,0,10q�65   �� 5      -   5        $��SelectInputFileButton��5      $��Выбрать файл��62   $��0,0,0,20q�6m   �� 5      -   5        $��InputFilePath��5      $ ��Файл не выбран��6-   $��12(�6C   
�������$��0,0,0,20q�6c   �� 5      $=��7Выберите результирующий файл:��6   $��0,0,0,10q�6=   �� 5      -   5        $��SelectOutputFileButton��5      $��Выбрать файл��63   $��0,0,0,20q�6o   �� 5      -   5        $��OutputFilePath��5      $ ��Файл не выбран��6.   $��12(�6D   
�������$��0,0,0,20q�6d   �� 5      -   5        $��StartSortButton��5      $'��!Начать сортировку��6,   6H   �� 5       -   5        $��BackToMainButton��5       $��
Назад��6-   $��0,20,0,0q�6\   �� 5!      -   5        $��SaveResultButton��5!      $)��#Записать результат��6-   $��0,20,0,0q�6i   �� 5"      -	   5        $��TimeComparisonButton��5"      $4��.Сравнить время обработки��61   $��0,20,0,0q�6}   5$      � 5'   
   $��1��6   $��10q�6    � RowDefinitions 5(      �� 5)      $��2* �6    �� 5*      $��1* �6    .+�5.      �� -
   5        $��SortedWordsLog��5.       �� 
IsReadOnly$
 True��6,    �� TextWrapping$
 Wrap=�6>   " �� VerticalScrollBarVisibility$
 Auto=�6R   $��0��6u   ���#f4f4f4$��0,0,0,10q�6�   �� 5/      -   5        $��WordCountLog��5/      $
 True��6*   $
 Wrap=�6<   $
 Auto=�6P   $��1��6s   ���#eaeaea6�   50      51      52      