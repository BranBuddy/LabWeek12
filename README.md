# LabWeek12

How does each pathfinding algorithms calculate and prioritize paths?
  
What challenges arise when dynamically updating obstacles in real-time?
-The main challenge when dynamically updating obstacles is the amount of slow down that can happen, whether it is because there are lots of objects moving, lots of AI characters moving throughout a path, or if it is a large world in general. All of these can in turn have effects on performance which slows down how fast the AI is able to pathfind.

Which algorithm should you choose and how should you adapt it for larger grids or open-world settings?
-
What would your approach be if you were to add weighted cells (e.g., "difficult terrain" areas)?
-We would approach this by still allowing the AI to navigate through these "difficult terrains" but have them at a lesser priority, such as only allowing it to go through these areas if there is no other possible way, or if the only other possible way is much longer. 
