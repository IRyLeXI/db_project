   M S B A M L   `   `   `  ����A  =dbproj, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:dbprojdbproj  5   	       dbproj.SongsWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   &localclr-namespace:dbproj     5   	     � Title$  Songs��5   	   $	��450��6   $	��800��6$   .��5	      �  � ColumnDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $��0��6   $��10q�6%   .+�5      �� $��0,0,0,10q�6   .+�5      �� -   5         ��Name$ 	txtSearch��5      $	��150��6-   $��0,0,5,0q�69   $��Center=�6J   �� 5      $��Search��6   $��75��6*   5      �� 5      -   5         ��Name$ cmbGenre��5      $	��150��6)   $��0,0,0,10q�65   .@�5      �� $��Select Genre��6   ${�False��66   �� 5      $	��Pop��6   �� 5      $
��Rock��6   �� 5      $��Hip Hop��6   5      �� 5!      -   5        $ 	cmbAuthor��5!      $	��150��6*   .@�5"      �� $��Select Author��6   ${�False��67   �� 5#      $��Artist 1��6   �� 5$      $��Artist 2��6   �� 5%      $��Artist 3��6   5'      5(      �� 5+   
   $��1��6   $��10q�6!   
�������$��1q�6E   5-      5.       � 	Resources 50       �� 
TargetType �� BasedOn �� Property �� Value	 �� Triggers
 P� Property P� Value��%(  '��       7���� 51   
   " ��6   8   .�52      ��  � ��
 �   �6+   	 53      P� 54      
 � ��$  ��6*   . �55      ��  � ��
 �����63   56      P� 57      
 �@J�IsKeyboardFocused$
 True��67   . �58      ��  � ��
 �   �63   59      5;      5<      5>      