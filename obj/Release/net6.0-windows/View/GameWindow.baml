   M S B A M L   `   `   `  ���� L  HCrossesAndNoughts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:CrossesAndNoughts.ViewCrossesAndNoughts.View  I)clr-namespace:CrossesAndNoughts.ViewModelCrossesAndNoughts.ViewModel  '    !CrossesAndNoughts.View.GameWindow   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     6local$clr-namespace:CrossesAndNoughts.View     ?	viewmodel)clr-namespace:CrossesAndNoughts.ViewModel     $	��450��$	��800��
�������$	��800��$	��450��.���   � ColumnDefinitions  �� $��2* ��� $��* � � RowDefinitions �� $��2* �.+�� -    �Name$ GameGrid��$��3��  �� $��2* ��� $��* � �� $��2* �.+��� -    ��Name$ SelectSymbolLabel��$y�Visible=�
����ح�$��0��$��0��$��2����� $��Center=�$��Center=�����  �� ElementName$ SelectSymbolLabel�� �� Path$ ActualHeight����� $ SelectSymbolLabel��$ ActualWidth�  �� $��* ��� $��* � �� $��* ��� $
��3.5* �.+�(� $��Center=�$��Center=�$��0��$��0��$��2��.���� $��Select your symbol�� �� TextAlignment$ Center=�$��50(�$��Impact)�
����=H�$��5q��� $��1��$��50q�
������� � Cursor$
 Hand��$	��600��$	��600������ *SelectSymbolCommand+���� *Cross+.���� $"��/Resources/Images/Cross3.png��$��10q�$	��450��$	��450���� $��1��$��1��$��50q�
�������$
 Hand��$	��600��$	��600������ *SelectSymbolCommand+���� *Nought+.���� $#��/Resources/Images/Nought5.png��$��10q�$	��450��$	��450��� -   $ GameUIContainer��$y�	Collapsed=�$��2��  �� $��2* ��� $��* � �� $��2* �.+�� -   $ Field��$��50q�$��0��$	��800��$
��1000��  �� $��* ��� $��* ��� $��* � �� $��* ��� $��* ��� $��* �.+��� $��0��$��0��
����=H�$��
0, 0, 2, 2q��� $��0��$��1��
����=H�$��
0, 0, 2, 2q��� $��0��$��2��
����=H�$��
0, 0, 2, 0q��� $��1��$��0��
����=H�$��
0, 0, 2, 2q��� $��1��$��1��
����=H�$��
0, 0, 2, 2q��� $��1��$��2��
����=H�$��
0, 0, 2, 0q��� $��2��$��0��
����=H�$��
0, 0, 0, 2q��� $��2��$��1��
����=H�$��
0, 0, 0, 2q��� $��2��$��2��
����=H�$��
0, 0, 0, 0q��� -    ��Name$	 One��	 J� Opacity$	 0Q�$��0��$��0������ *DrawSymbolCommand+���� $	 One���� -   $	 Two��$	 0Q�$��1��$��0������ *DrawSymbolCommand+���� $	 Two���� -   $ Three��$	 0Q�$��2��$��0������ *DrawSymbolCommand+���� $ Three���� -   $
 Four��$	 0Q�$��0��$��1������ *DrawSymbolCommand+���� $
 Four���� -	   $
 Five��$	 0Q�$��1��$��1������ *DrawSymbolCommand+���� $
 Five���� -
   $	 Six��$	 0Q�$��2��$��1������ *DrawSymbolCommand+���� $	 Six���� -   $ Seven��$	 0Q�$��0��$��2������ *DrawSymbolCommand+���� $ Seven���� -   $ Eight��$	 0Q�$��1��$��2������ *DrawSymbolCommand+���� $ Eight���� -   $
 Nine��$	 0Q�$��2��$��2������ *DrawSymbolCommand+���� $
 Nine��(� $��0��$��0��$��3��$��3��(� $��1��$��40q�.��� .+��� $��Score:��$ Center=�$��40(�$��Impact)�
����=H�$��0, 0, 0, 300q��� $��40(�$��Impact)�
������$ Center=�$��0, 50q����� *Points+�� $��
Difficulty��$ Center=�$��40(�$��Impact)�
����=H�$��0, 175q��� $ Center=�$��40(�$��Impact)�$��0, 225, 0, 175q����� *DifficultyName+���� *DifficultyColor+�� $
��Back��
����·�
����=H�$��Impact)�$��40(�$
 Hand��$��0, 375, 0, 0q�$	��200������ *GoBackToMenuCommand+�� -   $ GameOverLabel��$y�	Collapsed=�$��3��
����ح�
����=H�$��1q�CollapsedAtStart����� $��Center=�$��Center=����� $ GameOverLabel��$ ActualHeight����� $ GameOverLabel��$ ActualWidth� �� $��* ��� $��2* ��� $��* �.+��� -   $ MainMenuButton��$��	Main Menu��$��Impact)�$��50(�
����=H�
����d�$
 Hand��$��100, 10, 100, 10q�$��2������ *GoBackToMenuCommand+(� $��10q�.���� $��You lose with score:��$ Center=�$��Impact)�
����=H�(� $��1��$��30q�.���� $��50��$��Impact)�
������$ Center=�