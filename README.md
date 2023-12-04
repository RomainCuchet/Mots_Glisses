# Mots_Glissés
Ce projet répond au sujet de code imposé par *MM Aline Ellul* dans le cadre du module Algorithme et POO en deuxième année de l'Esilv.

L'ensemble du code a été réalisé par :

> **Romain Cuchet**
>  **Thomas Coste**

![arborescence de fichiers](https://i.ibb.co/dMhDSHg/Diagramme-vierge-1.png)

## Nos choix

Selon nous avoir un timer fixant la durée d'une partie n'avait pas de sens dans la mesure où un joueur pourrait se retrouver avantagé par rapport à un autre. La durée du jeu est donc définie par le temps d'un tour, le nombre de tours par joueur ainsi que le nombre de joueurs.

Nous avons crée une classe `Interface`afin de créer un menu navigable. Depuis ce menu on peut créer des joueurs, les supprimer, les afficher, lancer une nouvelle partie et afficher le règles.  Lorsqu'un joueur veut lancer une partie  il a la possibilité de choisir un plateau existant depuis une liste de plateaux développés spécialement pour le jeu ou des plateaux enregistrés par d'autres joueurs. Il peut aussi en générer aléatoirement. La recherche des plateaux existants est automatisée. Si on ajoute un plateau dans le dossier prévu à cet effet il sera automatiquement importé dans notre menu et accessible dans le jeu. Il en va de même pour les règles qui s'actualisent en consultant les fichiers et les paramètres. Si le joueur le souhaite il peut enregistrer un plateau généré aléatoirement en fin de partie et y accéder via le menu. Le nom des fichiers est généré automatiquement grâce à une compteur interne.

Les commandes du menu sont les suivantes :
 - **↑** sélection de l'élément au dessus
 - **↓** sélection de l'élément au dessous
 - **enter** valide la commande
 - **échap** quitte le programme depuis le menu principal et permet de retourner à la page précédente depuis les sous-menus
 -  **←** permet de retourner à la page précédente depuis les sous-menus

Nous avons constaté qu'un ensemble de fonctions était nécessaire dans l'ensemble des classes donc nous avons fait le choix de les centraliser dans une classe accessible `Tools` qui fait office d'api voici les fonctions principales :

 - `get_sorted_mots_français()` permet de trier le dictionnaire fourni
 - `merge_sort()` trie une liste de chaines de caractères dans l'ordre alphabétique
 - `is_alphabetically_ordered()` compare l'ordre alphabétique de deux mots
 - `print_mat()` permet d'afficher en console une matrice de dimension 2 quel que soit sont type
 - `is_file_alphabetically_ordered()` prends un fichier texte et retourne `true` s'il est trié dans l'ordre alphabétique

Dans la classe `Plateau` la recherche diagonale est activée par défaut.

 - `disable_diagonale_search()` permet de désactiver la recherche diagonale
 - `search()` permet de dire si un mot donné est capable d'être écrit dans la board (selon les règles du jeu)
 - `Plateau()` le constructeur de la classe peut prendre en argument un fichier txt ou csv. Depuis un fichier csv il génère le plateau préétabli. Depuis un fichier txt il génère un plateau aléatoire en respectant les contraintes imposées.
 - 
## Warnings
 - `Console.Clear()` vide la fenêtre courante. Cette dernière est distincte de la fenêtre qui s'affiche sur Windows pour une question d'espace mémoire. Si jamais le texte à afficher dépasse la taille de la fenêtre courante les données seront écrasées. Le texte sera toujours visible sur la fenêtre Windows mais il ne sera pas supprimé par console.Clear(). Il est donc conseillé de ne pas redimensionner la fenêtre en dessous d'une certaine valeur pour un meilleure affichage.
 - La structure de notre interface pourrait être optimisée et clarifiée. Etant fonctionnelle il ne nous a pas semblé primordiale de risquer la génération de bugs afin de clarifier un code qui n'a pas vocation à être modifié ni à être maintenu ou mis à jour dans le futur.

