Game Design Document: Rush Delivery
•	Author: Chirag Atrey
•	Version: 1.0 (Completed Build
•	Date: September 4, 2025
1. High Concept
"Rush Delivery" is a fast-paced, arcade-style 3D driving game. Players race against a ticking clock to pick up and deliver packages in a vibrant, low-poly city. Each successful delivery adds precious seconds to the timer, challenging the player to see how many deliveries they can make before time runs out.
2. Core Gameplay
•	Genre: 3D Arcade Driving / Delivery Challenge
•	Platform: PC (Windows Standalone)
•	Target Audience: Casual gamers who enjoy score-attack challenges and driving games.
3. Core Gameplay Loop
The gameplay is built on a simple, addictive loop:
1.	SPAWN: The game begins with a package spawning at a random location in the city.
2.	PICK UP: The player drives to the package. The minimap provides a beacon to guide them.
3.	DELIVER: Upon collecting the package, a drop-off zone appears at a new random location.
4.	REWARD: When the player reaches the drop-off zone, the delivery is complete. They are rewarded with bonus time, their delivery score increases, and the loop repeats with a new package spawning.
5.	GAME OVER: The loop continues until the player's timer reaches zero.
4. Detailed Mechanics
4.1 Player Controls
•	Vehicle: The player controls a physics-based car using the asset's pre-built CarController.
•	Movement: WASD keys are used for acceleration, reversing, and steering.
•	Braking: The Space Bar is used for braking.
4.2 Game Systems
•	Countdown Timer: The central challenge. The game starts with 60 seconds. Each successful delivery adds a 15-second bonus. The game ends when the timer hits zero.
•	Scoring: A counter on the UI tracks the total number of successful deliveries made in a single run.
•	Objective Spawning: The game uses a predefined list of spawn points. To ensure variety, a package or drop-off zone will not spawn in the same location twice in a row.
4.3 User Interface (UI)
The game features a clean, image-based UI for a professional feel.
•	Main Menu: A dedicated scene that serves as the game's entry point. Features custom background art with "Play" and "Exit" buttons.
•	In-Game HUD:
o	Timer: A TextMeshPro element in the top-center of the screen displays the remaining time.
o	Delivery Counter: A TextMeshPro element in the top-left displays the current number of completed deliveries.
o	Minimap: A rotating minimap is fixed in the top-right corner. It features a static player icon and a dynamic objective icon that always points towards the current package or drop-off zone.
•	Pause Menu: Accessible by pressing the Escape key. It pauses all game action and sound, displaying a custom background with "Resume" and "Main Menu" buttons.
•	Game Over Screen: A full-screen panel that appears when the timer expires. It displays a custom background with a "Retry" button to restart the level.
4.4 Audio
•	Dynamic Engine Sound: The player's car has an AudioSource that plays an engine sound. The pitch of this sound increases with the car's speed, creating a realistic revving effect.
•	Audio Management: The engine sound is correctly managed by the GameManager, pausing when the pause menu is active and stopping completely on the Game Over screen.
5. Art and Visual Style
•	Graphics: The game uses a consistent, low-poly 3D art style for the city environment and all vehicles, giving it a clean and stylized look.
•	UI Design: The menus and game over screens use high-quality, pre-rendered images that match the game's aesthetic, providing a polished and cohesive user experience.

