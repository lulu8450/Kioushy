using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class MazeGenerator : MonoBehaviour
{
    // Référence à la Tilemap pour les murs et les passages
    public Tilemap wallTilemap;
    public Tilemap pathTilemap;

    // Les tuiles à utiliser
    public TileBase wallTile;
    public TileBase pathTile;

    // Dimensions du labyrinthe
    public int mazeWidth = 31;
    public int mazeHeight = 21;

    private int[,] mazeGrid;

    void Start()
    {
        // On s'assure que les dimensions sont impaires pour la génération de murs
        // et qu'elles sont au minimum de 3
        if (mazeWidth % 2 == 0) mazeWidth++;
        if (mazeHeight % 2 == 0) mazeHeight++;
        if (mazeWidth < 3) mazeWidth = 3;
        if (mazeHeight < 3) mazeHeight = 3;

        GenerateMaze();
    }

    void GenerateMaze()
    {
        // Initialise la grille avec des murs partout
        mazeGrid = new int[mazeWidth, mazeHeight];
        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                mazeGrid[x, y] = 1; // 1 = Mur
            }
        }

        // On démarre l'algorithme de génération à un point de la grille
        // On commence au point (1, 1) car les bords sont des murs
        Generate(1, 1);

        // Dessine le labyrinthe sur la Tilemap
        DrawMaze();
    }

    // Algorithme de Recursive Backtracking
    void Generate(int x, int y)
    {
        // On marque la cellule actuelle comme un passage
        mazeGrid[x, y] = 0; // 0 = Passage

        // On crée une liste des directions possibles et on la mélange
        List<Vector2Int> directions = new List<Vector2Int>
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };
        Shuffle(directions);

        // Pour chaque direction, on "creuse" un passage si c'est possible
        foreach (var dir in directions)
        {
            int newX = x + dir.x * 2;
            int newY = y + dir.y * 2;

            // Vérifie si la nouvelle position est à l'intérieur de la grille
            if (newX >= 0 && newX < mazeWidth && newY >= 0 && newY < mazeHeight && mazeGrid[newX, newY] == 1)
            {
                // On creuse le mur entre les deux cellules
                mazeGrid[x + dir.x, y + dir.y] = 0;

                // On continue la génération à partir de la nouvelle cellule
                Generate(newX, newY);
            }
        }
    }

    // Mélange les éléments d'une liste
    void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    // Dessine la grille sur la Tilemap de Unity
    void DrawMaze()
    {
        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                if (mazeGrid[x, y] == 1) // Mur
                {
                    wallTilemap.SetTile(tilePosition, wallTile);
                }
                else // Passage
                {
                    pathTilemap.SetTile(tilePosition, pathTile);
                }
            }
        }
    }
}