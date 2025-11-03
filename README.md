# LabWeek12

How does each pathfinding algorithms calculate and prioritize paths?
-BFS pathfinding system creates a queue of all the nodes available to the user. Then the algorithm explores each of these one by one until they reach the desired goal.
-A* pathfinding systems creates a cost system where each path to the desired goal is weighed to eachother. Then whichever path is the cheapest, the algorithm selects that as the desired path.
-Dijkstra pathfinding system works similarly to the BFS pathfinding system. However here a priority queue is used to keep track of the unvisted nodes unlike BFS' regular queue.
  
What challenges arise when dynamically updating obstacles in real-time?
-The main challenge when dynamically updating obstacles is the amount of slow down that can happen, whether it is because there are lots of objects moving, lots of AI characters moving throughout a path, or if it is a large world in general. All of these can in turn have effects on performance which slows down how fast the AI is able to pathfind.

Which algorithm should you choose and how should you adapt it for larger grids or open-world settings?
-I believe using the A* system would be ideal for larger grids and open world settings. Because there are so many nodes in an open world game, to optimize performance, you need a system that won't indivdually check every single node to find the best path.

What would your approach be if you were to add weighted cells (e.g., "difficult terrain" areas)?
-We would approach this by still allowing the AI to navigate through these "difficult terrains" but have them at a lesser priority, such as only allowing it to go through these areas if there is no other possible way, or if the only other possible way is much longer. 
