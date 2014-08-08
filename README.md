#GRACE Satellite Data Analysis Tools
Analyzes the data from the [Gravity Recovery and Climate Experiment](http://www.csr.utexas.edu/grace/) to text and visual form (requires access to the data) and simulates data in a customizable format.

###Problems GS-DATs (currently) solves
#####Problem 1: GRACE Coverage
<img width=200px float=right align=right src="https://cloud.githubusercontent.com/assets/6364409/3851868/74c2e2f8-1e9d-11e4-9c6d-2098ca400050.JPG"/>As GRACE passes over the Earth, it intersects with more areas than others (shown in green).  To demonstrate this effect, `GRACE Frequency Map Analytic Tools (GF-MATS)` were born (shown right).  This tool reads groundtrack data and outputs images and videos showing the overall coverage for a particular month.  The results show inconsistencies for some months while evenness for others.

***

#####Problem 2: GRACE Data Precision
<img width=200px float=right align=right src="https://cloud.githubusercontent.com/assets/6364409/3852017/103700c2-1ea1-11e4-8054-1636ab32295d.JPG"/>GRACE (being only 1 satellite pair) cannot cross every point on the Earth with equal consistency.  In most cases, GRACE misses many critical data points, or curves in the change in mass.  Using two Earth mass models (`GLDAS` and `RL-05`), we can compare GRACE to more precise data points.  Thus, the `GRACE Frequency Chart Analytic Tools (GF-CATS)` were born.  Where is GRACE least precise?  Depends where you are.

***

#####Problem 3: Satellite Pairs Needed

![](https://cloud.githubusercontent.com/assets/6364409/3852061/4daf147a-1ea2-11e4-935b-4cf8534da5fc.JPG)

How many satellite pairs are needed to equalize GRACE's frequency distribution?  What is the optimal orbit?  How precise can a GRACE follow-on system be?  These questions are answered by the `GRACE Live Orbit and Groundtrack Simulation (G-LOGS)`, an online application to simulate an orbit in realtime.  Besides being used for answering the questions of GRACE, this app may serve as a template for learning rudimentary Orbital Mechanics (as it did for our research group) and for future, more complex orbital simulations.

Visit the simulation at [http://code.arthurpachachura.com/grace](http://code.arthurpachachura.com/grace).

###Installation
To run solutions 1 and 2, you will need Visual Studio 2013 or newer with C# Desktop app capability.  To modify solution 3, no software is required as it is entirely online.

###Contributing
Our internship has completed and we will no longer be making active edits to this repo, but we welcome **push requests and forks**!  You can also send us an email if you would like support using this product.

Thanks to UT/CSR staff, NASA, and JPL for this oppurtunity!
******
Arthur Pachachura (:bear: smo-key) and Naoki Ellis
