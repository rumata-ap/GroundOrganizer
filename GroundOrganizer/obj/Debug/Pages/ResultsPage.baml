   M S B A M L   `   `   `  ����J  FGroundOrganizer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:GroundOrganizerGroundOrganizer  5      !    GroundOrganizer.ResultsPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      /localclr-namespace:GroundOrganizer     5        N� Title$  ResultsPage��5	       � DataContext 5         GroundOrganizer.ViewModel  5   
   .��5      �  � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5      $
��auto �6   .+�5   
   &   System.Windows.Controls.DataGrid  -   5          Name$ ResultsDataGrid��5      $��0��6-      MinColumnWidth$ 40Q�5      $	��300��6)   $��3q�5      
�������   AutoGenerateColumns . $
��Left=�5   /      LayersDataGridStyle#��[  5        
rowsHeight   	RowHeight# �  5        DataGridHeaderStyle   ColumnHeaderStyle# �  5         MaxColumnWidth$	 150Q�5      ��5      �� *ResR+	 �� SelectedItem	 61   �� *ResRselected+5        5   
   -   5        $ DetailedResultsDataGrid��5      $��1��65   $��3q�5      
�������$	��500��5        . $
��Left=�5"   /   #��[  5#      # �  5$      # �  5%      
   SelectionUnit$

 Cell=�5&      ��5!      �� *
ResRdata+5'      5(      5)      