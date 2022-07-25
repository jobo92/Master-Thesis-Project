# Master-Thesis-Project
The scripts I created and worked on for my master thesis project in collaboration with another person.

The **TileChecker script** is used to see if the tower was above grass or road tile to determine the tower's functions and visual appearance.

The **TowerSnapToPosition script** is used so the AR tower can only move in increments, from tile to tile, and thereby not smoothly around on the device screen.

The **WaveSpawn script** is used to spawn enemies in AR on the AR map that are aligned to the AR map on the screen and give them the path they need to follow.

The **RestClientExample script** makes everything ready for a request to be sent to Azure OCR Read API, making the results from Azure OCR Read API ready to be shown, and showing the results in the UI. 

The **RestWebClient script** handles the communication from Unity to Azure OCR Read API and back to Unity. 

The **AzureOCRResponse** is used to be able to deserialize JSON to objects that can be used in Unity. 

A flowchart showing how the three scripts are used and how they work can be seen below.

![FinalAzureOCRFlowchart](https://user-images.githubusercontent.com/32058431/180796693-85436077-9176-43d5-8e0f-bf7fc1b2684a.png)
