   M S B A M L   `   `   `  ����  MDPSimulator!    MDPSimulator.SimulatingPage   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Simulating Page��5   	   $
��1024��6!   $	��764��6.   .��5      � .+�5   
   � -   5         �Name$ mapGrid��5      ���#E6E6DA$	��600��65   $��10,70,556,0q�6B   $	��Top=�6W   5      �� 5   
   $��10,10,666,663q�6   $��Center=�6+   ���	#FF074AFD$	��340��6`   $��37.333(�6l   �� 6�   $� ��6�   �� 6�   $�	Simulator��6�   6�   �� 5	   
   -   5         ��Name$# RadioButtonGrid_TextBlockGrid��5	      $��Center=�6<   $��Center=�6W   $��500,70,316,525q�6t   $	��200��6�   $	��138��6�   .+�5
      �� $��Explore��6   $��
45,0,55,20q�6'   $��Center=�6;   ���	#FF074AFD$��18.667(�6o   $��27��6�   $	��100��6�   	� 5      -   5         	�Name$ FollowTheWallButton��5      $��5q�67    	� 	GroupName$ ExploreGroup��6B   $��Follow The Wall��6[   	� 5      -   5        $ 	DFSButton��5      $��5q�6-   $ ExploreGroup��68   $��Depth First Search��6Q   $��55��6n   5      �� 5   
   -   5        $( "RadioButtonGrid_TextBlockGrid_Copy��5      $��Center=�6A   $��Center=�6\   $��500,206,316,390q�6y   $	��200��6�   $	��135��6�   .+�5      �� $��	Fast Path��6   $��
50,0,40,20q�6)   $��Center=�6=   ���	#FF074AFD$��18.667(�6q   $	��110��6�   $��27��6�   	� 5      -   5        $ AStarButton��5      $��5q�6/   $	 Run��6:   $��A Star��6J   	� 5      -   5        $ 	BFSButton��5      $��5q�6-   $	 Run��68   $��BFS Algorithm��6H   5      �� 5   
   -   5         ��Name$ exploreButton��5      $��Explore��6)   $��665,127,286,572q�6;   ���	#FF074AFD��6p   Z�  J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5      5      �� 5   
   -	   5        $ 	runButton��5      $	��Run��6$   $��665,261,286,436q�62   ���	#FF074AFD��6c   Z�  5        5      5      �� 5   
   -
   5        $ 
loadButton��5      $��Load Map��6&   $��10,675,936,26q�69   ���	#FF074AFD��6e   Z�  5        5      5!      �� 5"   
   -   5        $ exportButton��5"      $��
Export Map��6'   $��110,675,816,26q�6<   ���	#FF074AFD��6o   Z�  5#        5$      5&      �� 5'   
   -   5        $ resetButton��5'      $��Reset��6&   $��390,675,556,26q�66   ���	#FF074AFD��6h   Z�  5(        5)      5+      � 5,   
   -   5         �Name$ CoverageProgressbar��5,       � Value$ 0Q�63   $��760,114,81,601q�6=   �� 5-   
   -   5        	 ��Name$	 coverageLabel��5-      $��0%��6'   $��Stretch=�64   $��940,105,10,579q�6Y   $��1q�6q   �� 5.   
   $��810,154,69,531q�6   $��Center=�6,   ���	#FF074AFD$��18.667(�6`   
 J� RenderTransformOrigin$
 	0.486,1.2&�6r   $	��137��6�   �� 6�   $� ��6�   �� 6�   $�Timer��6�   6�   �� 5/   
   $��803,70,76,601q�6   $��Center=�6+   ���	#FF074AFD$��18.667(�6_   $	��137��6q   �� 6~   $� ��6�   �� 6�   $�Coverage��6�   6�   � 50   
   -   5        $ TimerProgressbar��50      $	 100Q�60   $��760,184,81,531q�6<   �� 51   
   -   5        $	 	timeLabel��51      $��06:00��6#   $��Stretch=�63   $��940,175,10,495q�6X   $��1q�6p   �� 52   
   $��770,243,156,458q�6   $��Center=�6-   ���	#FF074AFD$��18.667(�6a   $
 	0.486,1.2&�6s   $��90��6�   �� 6�   $� ��6�   �� 6�   $�Location��6�   6�   �� 53   
   $��[��6   $��770,273,236,422q�6   $��Center=�66   ���	#FF074AFD$��18.667(�6j   $
 	0.486,1.2&�6|   $��10��6�   $
��Bold"�6�   55      �� 56   
   $��]��6   $��850,273,156,422q�6   $��Center=�66   ���	#FF074AFD$��18.667(�6j   $
 	0.486,1.2&�6|   $��10��6�   $
��Bold"�6�   58      �� 59   
   $��,��6   $��814,273,192,439q�6   $��Center=�66   ���	#FF074AFD$��18.667(�6j   $
 	0.486,1.2&�6|   $��10��6�   $
��Bold"�6�   5:      �� 5;   
   -   5        $	 xLabel��5;      $��1��6    $��785,271,192,407q�6,   $
��Bold"�6E   ���	#FFF50E0E$��16(�6n   �� 5<   
   -   5        $	 yLabel��5<      $��1��6    $��824,271,156,408q�6,   $
��Bold"�6E   ���	#FFF50E0E$��16(�6n   �� 5=   
   $��875,243,21,460q�6   $��Center=�6,   ���	#FF074AFD$��18.667(�6`   $
 	0.486,1.2&�6r   $	��120��6�   �� 6�   $�     ��6�   �� 6�   $�Speed��6�   6�   �� 5>   
   -   5        $	 
speedLabel��5>      $��1��6$   $��918,271,51,399q�60   ���	#FFF50E0E$
��Bold"�6_   $��16(�6q   �� 5?   
   -   5        $ settingsButton��5?      $��Setting��6)   $��905,20,36,668q�6;   ���	#FF074AFD���	#FF074AFD
�������6�   �� 5@   
   -   5         ��Name$ scrollViewer��5@      $��500,341,51,67q�6-   $��2q�6D   $
��Auto=�6X   $��Disabled=�6{   
���   ����	#FFF7F6F6��5A      �� 5B       �� EndPoint$ 0.5,1&�6&    �� 
StartPoint$ 0.5,0&�67   .��5C      �  � Color$ Black��6#    � Offset$ 0Q�61   � 5D      $ 	#FF393A39��6#   $ 1Q�65   5E      .��5I      �� -   5         ��Name$ consoleBlock��5I      $
��Wrap=�6.   $��MDP Group 24��6B   $	��Top=�6V    �� Padding$ 10q�6n   ���	#FFFBFBFB$	��260��6�   $
��Left=�6�   5J      �� 5K   
   -   5        $ connectButton��5K      $��Connect��6(   $��390,20,556,667q�6:   ���	#FF074AFD���	#FF074AFD
�������$
 0.727,0.833&�6�   5M      5N      