   M S B A M L   `   `   `  ����A  =dbproj, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:dbprojdbproj  5      !    dbproj.AuthorDetailsControl   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      &localclr-namespace:dbproj     5      .��5	      � .+�5
   
      dbproj.NavBar  � 5   
   $
��0,60q�6     � RowDefinitions  5      �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    .+�5      ��  �� CornerRadius$ 25��6   $	��160��6'   $	��160��63   $��0��6@   $��Center=�6M    J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5         Color$ Black��6'      Opacity$
 0.35Q�66   ��5      �� 5       j� Stretch$
 Fill=�6!    �� ImageSource 60   ��  �� Path$ Picture�5      �� 5      $��1��6   $��24(�6D   $
��Bold"�6R   $��Center=�6d   ��6%   �� $ Nickname�6d   �� 5      $��2��6    �� Orientation$ 
Horizontal=�6&   $��Center=�6?   .+�5       �� $��18(�6<   ��6   �� $ 	FirstName�6<   �� 5!      $��18(�6;   $��5,0,0,0q�6I   ��6   �� $ LastName�6I   5"      �� 5$      $��3��6   $��16(�6f   $��Center=�6t   ��6%   �� $ 	Birthdate�	 �� StringFormat$	 {0:dd.MM.yyyy}��6t   �� 5&      $��4��6   $��16(�6G   $
��Wrap=�6U   $��0,10,0,0q�6i   $��Center=�6{   ��6%   �� $ Description�6{   5'      5(      5)      