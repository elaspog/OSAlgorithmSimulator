# OS Algorithm Simulator
This program simulates and demonstrates how the algorithms of operating systems work.

## Requirements
To run this apllication you will need:
* .NET 4.0 Framework installed

## Build and run the application
To demonstrate the algorithms listed below do the followings:
* Download the released binaries and examples or download the source files then build the application
* Start the application and load some input files
* Run some simulations

### Buld the project and run the application
1. Download the zip archive or clone this repository by using git command: `git clone https://github.com/prokhyon/OS_Algorithm_Simulator.git`
2. Open the **OS_SIMULATOR.sln** file (located in **OS_Simulator** folder in the repository) in Visual Studio 2013 or higher
3. Build the project
4. Start the application by pressing F5

### Run simulation(s)
0. Start the application (a main view will be visible)
1. Open one of the **\*.xml** examples (located in **inputs/examples** or **inputs/test** folder in the repository) by pressing **Load Simulation** button
2. Select the opened simulation and open the simulation's view by pressing **Open Simulation** button
3. Run the simulation by pressing **Next Step** or **Previous Step** buttons

## Algorithms
The following algorithms can be simulated and demonstrated by the program:
* Task Scheduler algorithms
    * First Come First Served (FCFS) / First Input First Output (FIFO)
    * Round Robin (RR)
    * Shortest Job First (SJF)
    * Shortest Remaining Time First (SRTF)
* Memory Allocation Algorithms
    * First Fit (FF)
    * Next Fit (NF)
    * Best Fit (BF)
    * Worst Fit (WF)
* Page Replacement Algorithms
    * Optimal (OPT)
    * First Input First output (FIFO)
    * Second Chance (SC)
    * Last Recently Used (LRU)
    * Least Frequently Used (LFU) / Not Frequently Used (NFU)
    * Not Recently Used (NRU)
* Virtual Address Mapping
