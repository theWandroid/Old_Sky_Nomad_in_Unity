using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ImageSlicer {

    public static Texture2D[,] GetSlices(Texture2D image, int blocksPerLine) //devuelve una matriz bidimensional de textura y tomar la textura de la imagen para cortar
    {
        int imageSize = Mathf.Min(image.width, image.height); //imagen que sea de proporción cuadrada
        int blockSize = imageSize / blocksPerLine; //el tamaño del bloque es igual al tamaño de la imagen dividido por bloques por linea

        Texture2D[,] blocks = new Texture2D[blocksPerLine, blocksPerLine]; //establecerlo a una nueva matriz de tamaño de bloques por linea

        for (int y = 0; y < blocksPerLine; y++)
        {
            for (int x = 0; x < blocksPerLine; x++)
            {
                Texture2D block = new Texture2D(blockSize, blockSize);
                block.wrapMode = TextureWrapMode.Clamp;
                block.SetPixels(image.GetPixels(x * blockSize, y * blockSize, blockSize, blockSize));
                block.Apply();
                blocks[x, y] = block;
            }
        }

        return blocks;
    }
}
