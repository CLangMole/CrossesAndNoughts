   M S B A M L   `   `   `  ���� L  HCrossesAndNoughts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:CrossesAndNoughts.ViewCrossesAndNoughts.View  I)clr-namespace:CrossesAndNoughts.ViewModelCrossesAndNoughts.ViewModel  O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem (    "CrossesAndNoughts.View.StartWindow   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     6local$clr-namespace:CrossesAndNoughts.View     ?	viewmodel)clr-namespace:CrossesAndNoughts.ViewModel     4s&clr-namespace:System;assembly=mscorlib       � Title$  Crosses and Noughts��$	��450��$	��800��
����ح�$	��470��$	��690�� � 	Resources    ButtonStyle �� Property �� Value��%r   &	        �� .���  � ��$ Impact)���  � ��
 ��d���  � ��
 ��=H���  � ��$ 30(�.��� -    �Name$ 
LayoutGrid�� � ColumnDefinitions �� $��* ��� $��2* ��� $��* � � RowDefinitions �� $
��0.5* ��� $��2* ��� $��75 �.+��� -    ��Name$ StartButton��$��1��$��2��$��10q�$��
Start Game��#��[   � Cursor$
 Hand������ *GoNextCommand+���� 	 �� ElementName$	 
LoginLabel���� -   $ 
QuitButton��$��0��$��2��$��10q�$
��Quit��#��[  $
 Hand������ *QuitCommand+�� -   $ RecordsButton��$��2��$��2��$��10q�$��Records��#��[  $
 Hand������ *GoNextCommand+���� $	 RecordsLabel��(� $��3��$��5q�$��0��.���� -   
 ��Name$
 Title��$��Crosses and Noughts��$��Impact)�
����=H� �� TextAlignment$ Center=��� -    ��Name$ 
TitleImage��$%��/Resources/Images/TicTacToe.png��$��1��$��1��$	��800��$	��800���� -    ��Name$ RecordsLabel��$y�	Collapsed=�
����ح�$��0��$��0��$��3��$��3��CollapsedAtStart����� $��Center=�$��Center=����� $	 RecordsLabel�� �� Path$ ActualHeight����� $	 RecordsLabel��$ ActualWidth� �� $��* ��� $��2* ��� $��* � �� $��112.5 ��� $��* ��� $��112.5 �.+�&   System.Windows.Controls.DataGrid  -     Name$ RecordsTable��$��1��$��1��   
IsReadOnly$
 True��
����·�
����=H�   RowBackground
 ��������� *	Records+�� -	   $ RecordsBackButton��$��1��$��2��$��100, 20, 100, 40q�$
��Back��#��[  $
 Hand������ *GoBackCommand+���� $	 RecordsLabel���� -
   $ 
LoginLabel��$y�	Collapsed=�
�������$��100, 75, 100, 75q�$��0��$��0��$��3��$��3��
����=H�$��2q�CollapsedAtStart��$	��450��$	��800����� $��Center=�$��Center=����� $	 
LoginLabel��$ ActualHeight����� $	 
LoginLabel��$ ActualWidth� �� $��* ��� $��* � �� $��80 ��� $��* ��� $��80 �.+��� -   $
 
LoginField��$��Enter your username:��$��Impact)�
����=H�$��2��$��Center=�$��Center=�$��40��$��40(�$ Center=�(� $��1��$��100, 30, 100, 30q�$��3��$	��150��$��44��$
��1000��.���� -    ��Name$ LoginTextBox1��$��1��$��30(�
����=H�$��2q� �� TextAlignment$ Center=�$��Center=�$ IBeam�� �� MaxLines$ 1�� �� 	MaxLength$ 20��$	��300��$��50��$��5q��� -   $ LoginBackButton��$��2��$��10q�$
��Back��#��[  $
 Hand������ *GoBackCommand+���� $	 
LoginLabel���� -   $ StartGameButton��$��2��$��1��$��10q�$��
Start Game��#��[  $
 Hand������ *StartGameCommand+