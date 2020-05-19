# Megaman Clone Process Documentation

## Initializing Project
1) Download Unity Hub at https://store.unity.com/download
2) Create an account, and download Unity 2019.3.12 on your machine 
3) Create new GitHub repo, initialized with a Unity .gitignore file
4) In Unity Hub, select 2D as the preset, and Create a new project inside the repo
5) Move the .gitignore file into the Unity project root folder (Not the Repository root. Do not commit until the number of changed files is ~25, not several hundred.)

## Unity Basics
Unity utilizes a pseudo entity-component system. Entities are Objects (known as GameObjects), and Components are Objects (all inheriting from MonoBehaviour: the base component class).

### Development Overview
The development cycle for Unity is well-defined, and incredibly easy to work with once you get the hang of it. 

#### Project
A "Project" is simply a collection of Assets (seen in the Assets folder in the Project window). These Assets are simply a folder, containing a bunch of specialized files designed to make Game Development a breeze. 

#### Game
A "Game" is a collection of Scenes, each of which is a collection of GameObjects, each of which is a collection of MonoBehaviour components. All GameObjects are are Assets instantiated into a Scene. There are many ways to get GameObjects into a Scene, but the two most common ways are to simply find the asset in the Project window, and click and drag it into the Scene or Hierarchy windows. From there, you can see all the Components of that GameObject in the Inspector window.

#### Scenes
To create a new Scene, press Ctrl+N. Any time you hit Ctrl+S, what you are saving is the scene. To "save" a GameObject for reuse in other Scenes (for instance, for a player), you want to create a Prefab, which can be done by clicking and dragging the object from the Hierarchy window to the Assets folder in the Project window.

In order for a Scene to be added to the final build, you have to manually add it by pressing Ctrl+Shift+B, and adding it to the Scene list.

A common utility you need to use to change between scenes is UnityEngine.SceneManagement.SceneManager

#### Prefabs
The Prefab system itself is pretty complicated, but the basics of what you need to know are this: A Prefab is an Asset that exists as a template for an object. A Prefab can be instantiated in the editor simply by clicking and dragging it into the active scene. Any changes you make in the scene will not be reflected in the Prefab Asset unless you select Overrides in the top left of the Inspector window for the prefab instance and apply any changes you want all other copies to inherit. (It will automatically update them wherever they are instantiated, which makes Prefabs super useful!) 

Prefabs can be found in the Hierarchy of a Scene by the color of the GameObject's text. Black objects are one-off GameObjects, while blue ones are instances of a Prefab. When you select a prefab, values that are linked to the Prefab are normal, while values you have overriden appear in bold. To revert these changes, go to the top right of the Inspector, select Overrides->Revert All.

You can instantiate any prefab by a having a GameObject in the scene with some component that calls Object Instantiate(GameObject obj, Vector3 position = Vector3.zero, Quaternion rotation = Quaternion.Identity, Transform parent = null);.

## Assets
### Spritesheets and Tilesets
Megaman: http://www.sprites-inc.co.uk/sprite.php?local=/Classic/Megaman/MM8/
Tilesets: http://www.sprites-inc.co.uk/sprite.php?local=Classic/MM1/Tiles/
1) Ensure Texture Type is Sprite
2) Set Sprite Mode to Multiple
3) Set Pixels Per Unit to 16
4) Set Filter Mode to Point (no filter)
5) Hit Apply
6) Click Sprite Editor
7) In the Sprite Editor Window, select Slice, then Slice again (Automatic)
8) Hit apply
9) Profit

## Systems
### Animation
The Animations for any GameObject are handled by the AnimatorController script. At a fundamental level, all this is is a state machine. You provide animations with metadata (State), variables (Properties) to give the Controller information about the environment, and Transitions to mandate the flow between states.

I recommend first drawing a graph, identifying all the states (nodes) you need, as well as the transitions (edges) between the states, and properties (conditions) for which you would travel from one state to another. Once you have that, Simply add an AnimatorController component to your character GameObject and fill in the fields in the Inspector window, which is a serialized representation of the drawn graph structure.

### Collision
To give your object collisions, just add the CustomCollider2D component, and adjust the size and offset to match the dimensions of the Sprite. If you want it to register collisions but not apply collision resolution, set the Collider to a trigger.

Currently, the only valid collider is an axis-aligned box collider. Will soon likely add a Transform-alligned box collider, and maybe circle colliders.

### Physics
To give your object physics, add the CustomRigidbody2D component. You may adjust the gravity scale and any other parameters there. 

Currently, the only currently supported physics are gravity.

### Player
The Player class combines all the above systems together into one cohesive GameObject. Literally all it does is set the Animator properties (the Animator itself then takes that info and does its own thing), shoot, and lastly, it tells the rigidbody how to move based on a variety of factors (seen in the inspector).

PlayerInput has a reference to the Player class on the same GameObject, and plugs it into keyboard input.