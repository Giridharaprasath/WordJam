# WordJam

A Hyper-Casual Word Boggle Game made using Unity

The players can select words Horizontally, Vertically and diagonally to form words, but should be of 3 or more letters. Each Letter will have a random points from 1 to 3, which will be used to calculate the score when the player forms a correct word.
There are two modes available to try

## Game

- **[Download Latest Release](https://github.com/Giridharaprasath/WordJam/releases).**
- Download from Itch.io **[Word Jam Game Link](https://giridharaprasath.itch.io/word-jam)**

> ### Endless Mode
- Words found are removed and new words are added
- Find as many as you can
> ### Level Mode
- Reach the level objective to progress
- Select Bonus Letters to get extra points
- Clear out Blocks to reveal letters

## Gallery

- Level Mode
<img width="270" height="600" alt="Level Mode" src="https://github.com/user-attachments/assets/a844c20c-e14a-455c-b99a-379d9558c3df" />

- Bonus Points
<img width="270" height="600" alt="Bonus Points" src="https://github.com/user-attachments/assets/d9b34d43-8a2a-4961-baed-171dc2e5288e" />

- Blocked Tiles
<img width="270" height="600" alt="Block Tiles" src="https://github.com/user-attachments/assets/47d4fa7b-0e30-4c29-a31f-710410765fc1" />

- Endless Mode
<img width="270" height="600" alt="Endless Mode" src="https://github.com/user-attachments/assets/2b261ea7-bcad-4a5f-b916-a1c2cd7635e2" />

- Gameplay Video

[<img width="260" height="170" alt="Screenshot 2025-07-13 153901" src="https://github.com/user-attachments/assets/48c7b870-fb04-4ece-a663-ebad672ded8a" />](https://youtube.com/shorts/lUJg1P7RaLU "View Video on YouTube")

## Information

- The List of available words are taken from the **[Word File](Assets/Resources/AllWords.txt)**
- For Level Mode, the levels are constructed based on data entered here **[Level Data](Assets/Resources/LevelData.txt)**

## Data Structures Used

# Graph
- Graph class representing a collection of nodes and their adjacent nodes. This class provides methods to add nodes, check adjacency, retrieve adjacent nodes,
  and perform depth-first search (DFS) traversal. This class is used to manage the relationships between the tiles in the game. Each tile is represented as a node, and the connections between tiles are represented as edges.

# Linked List
- LinkedList class representing a singly linked list data structure. This class provides methods to add and search for elements in the list.
  This class is used to manage a collection of type integer which represents the index of the selected tiles in the game.
  Each node points to the next selected tile, allowing for efficient traversal and manipulation of the list.
  The linked list structure allows for dynamic resizing and efficient insertion and deletion of nodes.
  This class is useful for managing a sequence of selected tiles in the game, where each tile can be added or removed dynamically.
  The linked list can be traversed to retrieve the indices of the selected tiles, and it can be cleared or modified as needed.

# Trie
- Trie class representing a prefix tree data structure. This class provides methods to add words, check if a word exists, and retrieve words that start with a given prefix.
  This class is used to manage a collection of words in the game, allowing for efficient searching and retrieval of words based on prefixes.
  Each node in the trie represents a character, and the path from the root to a node represents a word.
  The trie structure allows for efficient prefix searching, making it suitable for word games where players need to find words quickly.

# Hash Set
- A set containing all the words that have been selected by the player. This set is used to ensure that each word can only be selected once, preventing duplicates.
  It allows for efficient checking of whether a word has already been selected.

# Weighted Random Custom Class
- Weighted Random class provides methods to add items with associated weights and retrieve a random item based on the weights.
  This class is used to select items randomly with a probability proportional to their weights. Each item is represented by an index, and the weights determine the likelihood of selecting each item.
