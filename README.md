# Mots_Glisses
Ce projet répond au sujet de code imposée par MM Aline Ellul dans le cadre du module Algorithme et POO en deuxième année de l'Esilv. 
Voici l'arborescence de notre projet :
Mots_Glisses/
├──  Mots_Glisses/ # notre solution
│  ├──  Annexes/  # dossier regroupant l'ensemble des fichiers txt et csv
│  │  ├──  saved_boards # dossier regroupant les boards enregistrées par `save()`
│  │  │  ├── counter.txt # permet de générer des noms de board distincts
│  │  │  ├── board0.csv
│  │  │  ├── board1.csv
│  │  │  ...
│  ├──  Dictionnaire.cs
│  ├──  Jeu.cs
│  ├──  Joueur.cs
│  ├──  Plateau.cs
│  ├──  Programme.cs
│  ├──  Structures.cs
│  ├──  Tests.cs
│  ├──  Tools.cs # ensemble de fonctions nécessaires à l'ensemble des classes
├──  Mots_GlissesTests/  # notre projet de tests unitaires
│  │ ...

Nous avons constaté qu'un ensemble de fonctions était nécessaire dans l'ensemble des classes donc nous avons fait le choix de les centraliser dans une classe accessible `Tools` voici les fonctions principales :

 - `get_sorted_mots_français()` permet de trier le dictionnaire fourni
 - `merge_sort()` trie une liste de chaines de caractères dans l'ordre alphabétique
 - `is_alphabetically_ordered()` compare l'ordre alphabétique de deux mots
 - `print_mat()` permet d'afficher en console une matrice 2*2 quel que soit sont type
 - `is_file_alphabetically_ordered()` prends un fichier texte et retourne true s'il est trié dans l'ordre alphabétique

Dans la classe `Plateau` la recherche diagonale est désactivée par défaut.

 - `enable_diagonale_search()` permet d'activer la recherche diagonale
 - `search()` permet de dire si un mot donné est capable d'être écrit dans la board (selon les règles du jeu)
 - `Plateau()` le constructeur de la classe peut prendre en argument un fichier txt ou csv. Depuis un fichier csv il génère le plateau préétabli. Depuis un fichier txt il génère une board aléatoire en respectant les contraintes imposées.
