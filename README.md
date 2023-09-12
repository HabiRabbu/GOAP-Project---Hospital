#GOAP Project - Hospital
Welcome to the GOAP Project - Hospital, a learning project designed to help junior software developers break into game programming. This project is a simulation of a hospital environment where agents (doctors, nurses, and patients) interact with each other using Goal Oriented Action Planning (GOAP).

#Getting Started
To get started with the project, follow these steps:

Clone the repository: git clone https://github.com/HabiRabbu/GOAP-Project---Hospital.git
Open the project in Unity (ensure you have the correct version installed).
Explore the pre-built scenes to understand the basic setup.
Dive into the scripts to understand how GOAP is implemented.

#Project Structure
Assets/Scripts/GOAP/: This folder contains the core scripts that implement the GOAP system. Here is a breakdown of some key scripts:
GAction.cs: Defines the abstract class for actions, which are the building blocks of the GOAP system. Each action has preconditions and effects that influence the world state.
GAgent.cs: Represents agents in the world, holding a list of actions they can perform and goals they want to achieve.
GPlanner.cs: The planner that finds the best series of actions to achieve a goal based on the current world state.
GWorld.cs: Manages the global state of the world and the resources queues.
WorldStates.cs: A class to manage the states of the world, allowing for the addition, modification, and removal of states.

#GOAP Explained
In this section, we delve deep into the GOAP system and explain the core concepts:

Actions: Defined in individual scripts, actions are the building blocks of the GOAP system. Each action has preconditions and effects.
Goals: Goals are the desired outcomes that the agents are trying to achieve through a series of actions.
Planner: The planner is the brain of the GOAP system. It finds the best series of actions to achieve a given goal based on the current state of the world.
